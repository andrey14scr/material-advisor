using MaterialAdvisor.SignalR.Hubs;
using MaterialAdvisor.SignalR.Providers;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MaterialAdvisor.SignalR.Configuration;

public static class ConfigurationManager
{
    public static void ConfigureSignalR(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IUserIdProvider, UserIdProvider>();
        services.AddSignalR();
    }

    public static void ConfigureHubs(this WebApplication app)
    {
        app.MapHub<TopicGenerationHub>(SignalRConstants.TopicGenerationHubName);
        app.MapHub<AnswerVerificationHub>(SignalRConstants.AnswerVerificationHubName);
        app.MapHub<TableGenerationHub>(SignalRConstants.ReportGenerationHubName);
    }
}
