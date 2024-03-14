using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartRefund.Application.Interfaces;
using SmartRefund.Domain.Models.Enums;

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

                await ProcessInternalReceiptsWithStatusAsync(visionExecutorService, InternalReceiptStatusEnum.Unprocessed, stoppingToken);
                await ProcessInternalReceiptsWithStatusAsync(visionExecutorService, InternalReceiptStatusEnum.FailedOnce, stoppingToken);
                await ProcessInternalReceiptsWithStatusAsync(visionExecutorService, InternalReceiptStatusEnum.FailedMoreThanOnce, stoppingToken);
                await TranslateRawVisionReceiptAsync(visionExecutorService, stoppingToken);
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task ProcessInternalReceiptsWithStatusAsync(IVisionExecutorService visionExecutorService, InternalReceiptStatusEnum status, CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            $"ProcessInternalReceiptsWithStatusAsync is working with status: {status}");


        var internalReceipts = await visionExecutorService.GetInternalReceiptsWithStatusAsync(status);

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

    private async Task TranslateRawVisionReceiptAsync(IVisionExecutorService visionExecutorService, CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "TranslateRawVisionReceiptAsync Scoped Service Hosted Service is working.");


        var rawReceipts = await visionExecutorService.GetRawReceiptsWithIsTranslatedFalseAsync();

        foreach (var receipt in rawReceipts)
        {
            if (stoppingToken.IsCancellationRequested)
                break;

            try
            {
                _logger.LogInformation($"Processing RawReceipt with ID: {receipt.Id}");
                //await visionExecutorService.ExecuteRequestAsync(receipt);
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