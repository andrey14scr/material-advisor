using MaterialAdvisor.API.Extentions;
using MaterialAdvisor.API.Models;
using MaterialAdvisor.Application.Exceptions;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MaterialAdvisor.API.Middleware;

public class ErrorHandlingMiddleware(RequestDelegate _next)
{
    public async Task InvokeAsync(HttpContext context, ILogger<EndpointLogMiddleware> logger)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;

        var (statusCode, errorCodes) = exception switch
        {
            //RefreshTokenExpiredException => StatusCodes.Status401Unauthorized,
            //KeyNotFoundException => StatusCodes.Status404NotFound,
            ActionNotSupportedException => (StatusCodes.Status400BadRequest, (exception as ActionNotSupportedException)!.ErrorCodes),
            _ => (StatusCodes.Status500InternalServerError, [])
        };

        context.Response.StatusCode = statusCode;

        var result = JsonSerializer.Serialize(new ErrorModel
        {
            Message = exception.Message,
            Codes = errorCodes,
            CorrelationId = context.GetCorrelationId()
        });

        return context.Response.WriteAsync(result);
    }
}
