using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.WorkerService
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<VisionProcessingWorker>();

            var host = builder.Build();
            host.Run();

        }
    }
}
