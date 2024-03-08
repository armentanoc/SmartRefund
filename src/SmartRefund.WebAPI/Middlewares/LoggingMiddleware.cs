
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.WebAPI.Middlewares
{
    [ExcludeFromCodeCoverage]
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestDetails = $"{context.Request.Method} {context.Request.Path}";
            _logger.LogInformation($"BEGIN REQUEST: {requestDetails}");

            var stopwatch = Stopwatch.StartNew();
            await _next(context);
            stopwatch.Stop();

            _logger.LogInformation($"END REQUEST: {requestDetails} - Status: {context.Response.StatusCode} - Elapsed Time: {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}