
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartRefund.CustomExceptions;
using System.Diagnostics.CodeAnalysis;
using SmartRefund.Application.Interfaces;
using SmartRefund.Infra.Repositories;

[ExcludeFromCodeCoverage]
public class ExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnExceptionAsync(ExceptionContext context)
    {
        context.ExceptionHandled = false;
        var ex = context.Exception;
        int statusCode;

        var response = context.HttpContext.Response;
        response.ContentType = "application/json";

        switch (ex)
        {
            case EntityAlreadyExistsException _:
                statusCode = StatusCodes.Status409Conflict;
                break;

            case EntityNotFoundException _:
                statusCode = StatusCodes.Status404NotFound;
                break;

            case InvalidOperationException _:
                statusCode = StatusCodes.Status400BadRequest;
                break;

            default:
                statusCode = StatusCodes.Status500InternalServerError;
                break;
        }

        var objectResponse = new
        {
            Error = new
            {
                message = context.Exception.Message,
                statusCode = statusCode
            }
        };

        context.Result = new ObjectResult(objectResponse)
        {
            StatusCode = statusCode
        };

        _logger.LogError($"Erro no Sistema" +
            $" Mensagem: {objectResponse.Error.message}" +
            $" StatusCode: {objectResponse.Error.statusCode}");

        await Task.CompletedTask;
    }
}