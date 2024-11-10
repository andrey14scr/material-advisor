using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MaterialAdvisor.Application.TableGenerator.Configuration;

public static class ConfigurationManager
{
    public static void ConfigureTableGenerator(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITableGenerator, TableGenerator>();
    }
}
