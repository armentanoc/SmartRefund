using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartRefund.Application.Interfaces;

namespace SmartRefund.Application.Services
{
    public class VisionProcessingWorker : BackgroundService
    {
        private readonly ILogger<VisionProcessingWorker> _logger;
        //private readonly IVisionExecutorService _visionExecutorService;

        public VisionProcessingWorker(ILogger<VisionProcessingWorker> logger, IVisionExecutorService visionExecutorService)
        {
            _logger = logger;
            //_visionExecutorService = visionExecutorService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Vision processing worker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    //await ProcessInternalReceiptsWithStatusAsync(InternalReceiptStatusEnum.Unprocessed, stoppingToken);
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

        //private async Task ProcessInternalReceiptsWithStatusAsync(InternalReceiptStatusEnum status, CancellationToken stoppingToken)
        //{
        //    var internalReceipts = await _visionExecutorService.GetInternalReceiptsWithStatusAsync(status);

        //    foreach (var receipt in internalReceipts)
        //    {
        //        if (stoppingToken.IsCancellationRequested)
        //            break;

        //        try
        //        {
        //            _logger.LogInformation($"Processing InternalReceipt with ID: {receipt.Id}");

        //            await _visionExecutorService.ExecuteRequestAsync(receipt);

        //            _logger.LogInformation($"Processed InternalReceipt with ID: {receipt.Id}");
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, $"Error processing InternalReceipt with ID: {receipt.Id}");
        //        }
        //    }
        //}
    }
}
