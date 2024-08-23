namespace MaterialAdvisor.API.Middleware;

public class EndpointLogMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ILogger<EndpointLogMiddleware> logger)
    {
        var request = context.Request;
        var url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";

        if (string.Equals(request.Method, HttpMethod.Get.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            logger.LogInformation($"{url} was called");
        }
        else
        {
            using (var reader = new StreamReader(context.Request.Body))
            {
                var body = reader.ReadToEnd();
                logger.LogInformation($"{url} was called with body:{Environment.NewLine}{body}");
            }
        }
        
        await next(context);
    }
}
