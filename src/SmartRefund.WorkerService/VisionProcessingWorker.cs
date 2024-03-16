using SmartRefund.Application.Interfaces;
using SmartRefund.Domain.Enums;
using SmartRefund.Domain.Models;
using SmartRefund.Domain.Models.Enums;
using SmartRefund.Infra.Interfaces;
using System.Diagnostics.CodeAnalysis;

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
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

            _logger.LogInformation(
            "Worker Background Service is now running.");

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
                InternalReceiptStatusEnum.FailedMoreThanOnce
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
                    await visionExecutorService.ExecuteRequestAsync(receipt);
                    var successEvent = new Event(eventSource.UniqueHash, EventSourceStatusEnum.VisionExecutorSuccessful, DateTime.Now, "Internal Receipt created with success");
                    await eventSourceRepository.AddEvent(eventSource.UniqueHash, successEvent);
                    _logger.LogInformation($"InternalReceipt with ID: {receipt.Id} processed with success. | {successEvent}");
                }   
                catch (Exception ex)
                {
                    Event eventToAdd = null;

                    if (receipt.Status.Equals(InternalReceiptStatusEnum.Unsuccessful))
                    {
                        eventToAdd = new Event(eventSource.UniqueHash, EventSourceStatusEnum.VisionExecutorUnsuccessful, DateTime.Now, "Vision Executor process failed. A person in charge will process this receipt"); ;
                        await eventSourceRepository.AddEvent(eventSource.UniqueHash, eventToAdd);
                    }
                    else
                    {
                        eventToAdd = new Event(eventSource.UniqueHash, EventSourceStatusEnum.VisionExecutorFailed, DateTime.Now, "Vision Executor process failed, trying again...");
                        await eventSourceRepository.AddEvent(eventSource.UniqueHash, eventToAdd);
                    }

                    _logger.LogError(ex, $"Error processing InternalReceipt with ID: {receipt.Id} | {eventToAdd}");
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
                        var successEvent = new Event(eventSource.UniqueHash, EventSourceStatusEnum.FileTranslated, DateTime.Now, "The translation process succed. Ready to be analized.");
                        await eventSourceRepository.AddEvent(eventSource.UniqueHash, successEvent);
                        _logger.LogInformation($"RawReceipt with ID: {receipt.Id} translated with success. | {successEvent}");
                    }
                    catch (Exception ex)
                    {
                        var eventToAdd = new Event(eventSource.UniqueHash, EventSourceStatusEnum.FileTranslationFailed, DateTime.Now, "The translation process failed. A person in charge will process this receipt");
                        await eventSourceRepository.AddEvent(eventSource.UniqueHash, eventToAdd);
                        _logger.LogError(ex, $"Error processing InternalReceipt with ID: {receipt.Id} | {eventToAdd}");
                    }
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
