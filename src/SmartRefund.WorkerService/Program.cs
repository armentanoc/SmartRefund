using SmartRefund.WorkerService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<VisionProcessingWorker>();

var host = builder.Build();
host.Run();
