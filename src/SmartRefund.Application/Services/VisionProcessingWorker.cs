using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartRefund.Application.Interfaces;
using SmartRefund.Domain.Models.Enums;
using SmartRefund.Infra.Interfaces;

public class VisionProcessingWorker : BackgroundService
{
    private readonly ILogger<VisionProcessingWorker> _logger;

    public VisionProcessingWorker(IServiceProvider services,
        ILogger<VisionProcessingWorker> logger)
    {
        Services = services;
        _logger = logger;
    }

    public IServiceProvider Services { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Consume Scoped Service Hosted Service running.");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("while");

            using (var scope = Services.CreateScope())
            {
                var visionExecutorService = scope.ServiceProvider.GetRequiredService<IVisionExecutorService>();
                var visionTranslatorService = scope.ServiceProvider.GetRequiredService<IVisionTranslatorService>();

                var internalReceiptRepository = scope.ServiceProvider.GetRequiredService<IInternalReceiptRepository>();
                var rawVisionReceiptRepository = scope.ServiceProvider.GetRequiredService<IRawVisionReceiptRepository>();

                await ProcessInternalReceiptsWithStatusAsync(internalReceiptRepository, visionExecutorService, InternalReceiptStatusEnum.Unprocessed, stoppingToken);
                await ProcessInternalReceiptsWithStatusAsync(internalReceiptRepository, visionExecutorService, InternalReceiptStatusEnum.FailedOnce, stoppingToken);
                await ProcessInternalReceiptsWithStatusAsync(internalReceiptRepository, visionExecutorService, InternalReceiptStatusEnum.FailedMoreThanOnce, stoppingToken);
                await TranslateRawVisionReceiptAsync(rawVisionReceiptRepository, visionTranslatorService, stoppingToken);
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task ProcessInternalReceiptsWithStatusAsync(IInternalReceiptRepository internalReceiptRepository, IVisionExecutorService visionExecutorService, InternalReceiptStatusEnum status, CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            $"ProcessInternalReceiptsWithStatusAsync is working with status: {status}");


        var internalReceipts = await internalReceiptRepository.GetByStatusAsync(status);

        foreach (var receipt in internalReceipts)
        {
            if (stoppingToken.IsCancellationRequested)
                break;

            try
            {
                _logger.LogInformation($"Processing InternalReceipt with ID: {receipt.Id} and Status: {receipt.Status.ToString()}");
                await visionExecutorService.ExecuteRequestAsync(receipt);
                _logger.LogInformation($"Processed InternalReceipt with ID: {receipt.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing InternalReceipt with ID: {receipt.Id}");
            }
        }
    }

    private async Task TranslateRawVisionReceiptAsync(IRawVisionReceiptRepository rawVisionReceiptRepository, IVisionTranslatorService visionTranslatorService, CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "TranslateRawVisionReceiptAsync Scoped Service Hosted Service is working.");


        var rawReceipts = await rawVisionReceiptRepository.GetByIsTranslatedFalseAsync();

        foreach (var receipt in rawReceipts)
        {
            if (stoppingToken.IsCancellationRequested)
                break;

            try
            {
                _logger.LogInformation($"Processing RawReceipt with ID: {receipt.Id}");
                await visionTranslatorService.GetTranslatedVisionReceipt(receipt);
                _logger.LogInformation($"Processed RawReceipt with ID: {receipt.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing InternalReceipt with ID: {receipt.Id}");
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