namespace MaterialAdvisor.API.Middleware;

public class CorrelationIdMiddleware(RequestDelegate _next)
{
    public async Task InvokeAsync(HttpContext context, ILogger<EndpointLogMiddleware> logger)
    {
        var correlationIdHeaader = Constants.Headers.CorrelationIdHeader;

        var correlationId = context.Request.Headers.ContainsKey(correlationIdHeaader)
            ? context.Request.Headers[correlationIdHeaader].First()
            : Guid.NewGuid().ToString();

        context.Response.OnStarting(() =>
        {
            context.Response.Headers[correlationIdHeaader] = correlationId;
            return Task.CompletedTask;
        });

        context.Items[correlationIdHeaader] = correlationId;

        await _next(context);
    }
}
