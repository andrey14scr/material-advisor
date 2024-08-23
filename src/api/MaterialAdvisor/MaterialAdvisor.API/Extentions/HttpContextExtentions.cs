using System.Text;

namespace MaterialAdvisor.API.Extentions;

public static class HttpContextExtentions
{
    public static async Task<string> ReadBody(this HttpRequest request)
    {
        request.EnableBuffering();
        request.Body.Position = 0;

        using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true))
        {
            string body = await reader.ReadToEndAsync();
            request.Body.Position = 0;
            return body;
        }
    }

    public static string GetCorrelationId(this HttpContext context)
    {
        return context.Items[Constants.Headers.CorrelationIdHeader]?.ToString() ?? string.Empty;
    }
}
