using MaterialAdvisor.API.Exceptions;
using MaterialAdvisor.API.Extentions;
using MaterialAdvisor.API.Models;

using System.Net.Mime;
using System.Text.Json;

namespace MaterialAdvisor.API.Middleware;

public class ErrorHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ILogger<EndpointLogMiddleware> logger)
    {
        try
        {
            await next(context);
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

        var statusCode = exception switch
        {
            //RefreshTokenExpiredException => StatusCodes.Status401Unauthorized,
            //KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.StatusCode = statusCode;

        var result = JsonSerializer.Serialize(new ErrorModel
        {
            Message = exception.Message,
            CorrelationId = context.GetCorrelationId()
        });

        return context.Response.WriteAsync(result);
    }
}