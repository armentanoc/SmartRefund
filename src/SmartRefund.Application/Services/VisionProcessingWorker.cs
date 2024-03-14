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

        _logger.LogInformation("Vision processing worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessInternalReceiptsWithStatusAsync(InternalReceiptStatusEnum.Unprocessed, stoppingToken);
                //await ProcessInternalReceiptsWithStatusAsync(InternalReceiptStatusEnum.FailedOnce, stoppingToken);
                //await ProcessInternalReceiptsWithStatusAsync(InternalReceiptStatusEnum.FailedMoreThanOnce, stoppingToken);
                _logger.LogInformation("VisionProcessingWorker try");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing InternalReceipts.");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task ProcessInternalReceiptsWithStatusAsync(InternalReceiptStatusEnum status, CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Consume Scoped Service Hosted Service is working.");

        using (var scope = Services.CreateScope())
        {
            var scopedProcessingService =
                scope.ServiceProvider
                    .GetRequiredService<IVisionExecutorService>();

            var internalReceipts = await scopedProcessingService.GetInternalReceiptsWithStatusAsync(InternalReceiptStatusEnum.Unprocessed);

            foreach (var receipt in internalReceipts)
            {
                if (stoppingToken.IsCancellationRequested)
                    break;

                try
                {
                    _logger.LogInformation($"Processing InternalReceipt with ID: {receipt.Id} and Status: {receipt.Status.ToString()}");

                    await scopedProcessingService.ExecuteRequestAsync(receipt);

                    _logger.LogInformation($"Processed InternalReceipt with ID: {receipt.Id}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error processing InternalReceipt with ID: {receipt.Id}");
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