using MaterialAdvisor.AI.Configuration.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MaterialAdvisor.AI.Configuration;

public static class ConfigurationManager
{
    public static void ConfigureOpenAIAssistants(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenAIAssistantOptions>(configuration.GetSection("OpenAIAssistant"));

        services.AddScoped<IMaterialAdvisorAIAssistant, MaterialAdvisorAIAssistant>();
    }
}
