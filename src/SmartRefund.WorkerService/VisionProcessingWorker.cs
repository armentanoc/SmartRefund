using SmartRefund.Application.Interfaces;
using SmartRefund.Domain.Enums;
using SmartRefund.Domain.Models;
using SmartRefund.Domain.Models.Enums;
using SmartRefund.Infra.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Security.Authentication;

namespace SmartRefund.WorkerService
{
    [ExcludeFromCodeCoverage]
    public class VisionProcessingWorker : BackgroundService
    {
        private readonly ILogger<VisionProcessingWorker> _logger;
        public IServiceProvider Services { get; }
        public VisionProcessingWorker(IServiceProvider services,
        ILogger<VisionProcessingWorker> logger)
        {
            Services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(40), stoppingToken);

            _logger.LogInformation("[BACKGROUND SERVICE STARTED] Worker Background Service is now running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessChangesAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        private async Task ProcessChangesAsync(CancellationToken stoppingToken)
        {
            using (var scope = Services.CreateScope())
            {
                var visionExecutorService = scope.ServiceProvider.GetRequiredService<IVisionExecutorService>();
                var visionTranslatorService = scope.ServiceProvider.GetRequiredService<IVisionTranslatorService>();

                var internalReceiptRepository = scope.ServiceProvider.GetRequiredService<IInternalReceiptRepository>();
                var rawVisionReceiptRepository = scope.ServiceProvider.GetRequiredService<IRawVisionReceiptRepository>();

                var eventSourceRepository = scope.ServiceProvider.GetRequiredService<IEventSourceRepository>();

                await ProcessInternalReceiptsWithStatusAsync(internalReceiptRepository, visionExecutorService, eventSourceRepository, stoppingToken);
                await TranslateRawVisionReceiptAsync(rawVisionReceiptRepository, visionTranslatorService, eventSourceRepository, stoppingToken);
            }
        }

        private async Task ProcessInternalReceiptsWithStatusAsync(IInternalReceiptRepository internalReceiptRepository, IVisionExecutorService visionExecutorService, IEventSourceRepository eventSourceRepository, CancellationToken stoppingToken)
        {
            var statusesToProcess = new List<InternalReceiptStatusEnum>
            {
                InternalReceiptStatusEnum.Unprocessed,
                InternalReceiptStatusEnum.FailedOnce,
                InternalReceiptStatusEnum.FailedMoreThanOnce,
                InternalReceiptStatusEnum.VisionAuthenticationFailed
            };

            var internalReceipts = await internalReceiptRepository.GetByStatusAsync(statusesToProcess);
            var eventSources = await eventSourceRepository.GetAllByHashAsync(internalReceipts);

            foreach (var receipt in internalReceipts)
            {
                if (stoppingToken.IsCancellationRequested)
                    break;

                var eventSource = eventSources.FirstOrDefault(e => e.UniqueHash.Equals(receipt.UniqueHash));
                try
                {
                    var rawVisionReceipt = await visionExecutorService.ExecuteRequestAsync(receipt);
                    eventSource.SetRawVisionReceipt(rawVisionReceipt);

                    var message = $"InternalReceipt with ID: {receipt.Id} processed with succes | RawVisionReceipt with ID: {rawVisionReceipt.Id} was created | Details: {rawVisionReceipt}";
                    var successEvent = new Event(eventSource.UniqueHash, EventSourceStatusEnum.VisionExecutorSuccessful, DateTime.Now, message);
                    await eventSourceRepository.AddEvent(eventSource, eventSource.UniqueHash, successEvent);

                    _logger.LogInformation($"[VISION EXECUTOR SUCCESS] InternalReceipt with ID: {receipt.Id} processed with succes " +
                        $"\n       RawVisionReceipt with ID: {rawVisionReceipt.Id} was created " +
                        $"\n       Details: {rawVisionReceipt}");
                }
                catch (AuthenticationException authEx)
                {
                    var message = $"Failed to process InternalReceipt with ID: {receipt.Id} through GPT Vision  | Error {authEx.GetBaseException().GetType()} | Details: {authEx.Message}";

                    var eventToAdd = new Event(eventSource.UniqueHash, EventSourceStatusEnum.WaitingForAuthenticationSolution, DateTime.Now, message);
                    await eventSourceRepository.AddEvent(eventSource, eventSource.UniqueHash, eventToAdd);

                    _logger.LogError($"[VISION EXECUTOR ERROR] Failed to process InternalReceipt with ID: {receipt.Id} through GPT Vision " +
                        $"\n      Error {authEx.GetBaseException().GetType()}" +
                        $"\n      Details: {authEx.Message}");
                }
                catch (Exception ex)
                {
                    Event eventToAdd = null;

                    if (receipt.Status.Equals(InternalReceiptStatusEnum.Unsuccessful))
                    {
                        eventToAdd = new Event(eventSource.UniqueHash, EventSourceStatusEnum.VisionExecutorUnsuccessful, DateTime.Now, $"Vision Executor process failed. A person in charge will process this receipt. | Error {ex.GetBaseException().GetType()} | Details: {ex.Message}"); ;
                        await eventSourceRepository.AddEvent(eventSource, eventSource.UniqueHash, eventToAdd);
                    }
                    else
                    {
                        eventToAdd = new Event(eventSource.UniqueHash, EventSourceStatusEnum.VisionExecutorFailed, DateTime.Now, $"Vision Executor process failed, trying again... | Error {ex.GetBaseException().GetType()} | Details: {ex.Message}");
                        await eventSourceRepository.AddEvent(eventSource, eventSource.UniqueHash, eventToAdd);
                    }

                    _logger.LogError($"[VISION EXECUTOR ERROR] Failed to process InternalReceipt with ID: {receipt.Id} through GPT Vision" +
                        $"\n      Error {ex.GetBaseException().GetType()}" +
                        $"\n      Details: {ex.Message}");
                }
            }
        }

        private async Task TranslateRawVisionReceiptAsync(IRawVisionReceiptRepository rawVisionReceiptRepository, IVisionTranslatorService visionTranslatorService, IEventSourceRepository eventSourceRepository, CancellationToken stoppingToken)
        {
            var rawReceipts = await rawVisionReceiptRepository.GetByIsTranslatedFalseAsync();
            var eventSources = await eventSourceRepository.GetAllByHashAsync(rawReceipts);

            foreach (var receipt in rawReceipts)
            {
                if (stoppingToken.IsCancellationRequested)
                    break;

                var eventSource = eventSources.FirstOrDefault(e => e.UniqueHash.Equals(receipt.UniqueHash));
                if(!eventSource.CurrentStatus.Equals(EventSourceStatusEnum.FileTranslationFailed))
                {
                    try
                    {
                        var updatedReceipt = await visionTranslatorService.GetTranslatedVisionReceipt(receipt);
                        eventSource.SetTranslatedVisionReceipt(updatedReceipt);
                        var message = $"The translation process succed. Ready to be analized. | RawVisionReceipt with ID: {receipt.Id} translated with succes | TranslatedVisionReceipt with ID: {updatedReceipt.Id} was created | Details: {updatedReceipt}";
                        var successEvent = new Event(eventSource.UniqueHash, EventSourceStatusEnum.FileTranslated, DateTime.Now, message);
                        await eventSourceRepository.AddEvent(eventSource, eventSource.UniqueHash, successEvent);

                        _logger.LogInformation($"[TRANSLATION SUCCESS] The translation process succed. Ready to be analized." +
                            $"\n      RawVisionReceipt with ID: {receipt.Id} translated with succes" +
                            $"\n      TranslatedVisionReceipt with ID: {updatedReceipt.Id} was created" +
                            $"\n      Details: {updatedReceipt}");
                    }
                    catch (Exception ex)
                    {
                        var eventToAdd = new Event(eventSource.UniqueHash, EventSourceStatusEnum.FileTranslationFailed, DateTime.Now, $"The translation process failed. A person in charge will process this receipt | Error {ex.GetBaseException().GetType()} | Details: {ex.Message}");
                        await eventSourceRepository.AddEvent(eventSource, eventSource.UniqueHash, eventToAdd);

                        _logger.LogError($"[TRANSLATION ERROR] Failed to process RawVisionReceipt with ID: {receipt.Id} through Translation Service" +
                            $"\n      Error {ex.GetBaseException().GetType()}" +
                            $"\n      Details: {ex.Message}");
                    }
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("[BACKGROUND SERVICE STOPPED] Consume Scoped Service Hosted Service is stopping.");
            await base.StopAsync(stoppingToken);
        }
    }
}
