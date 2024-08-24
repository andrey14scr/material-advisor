using System.Text;

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
            context.Request.EnableBuffering();

            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
            {
                var body = await reader.ReadToEndAsync();
                logger.LogInformation($"{url} was called with body:{Environment.NewLine}{body}");
                context.Request.Body.Position = 0; 
                reader.DiscardBufferedData();
            }
        }
        
        await next(context);
    }
}
