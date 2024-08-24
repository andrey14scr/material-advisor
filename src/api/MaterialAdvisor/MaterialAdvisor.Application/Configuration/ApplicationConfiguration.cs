using MaterialAdvisor.Application.Configuration.Options;
using MaterialAdvisor.Application.Mapping;
using MaterialAdvisor.Application.Services;
using MaterialAdvisor.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MaterialAdvisor.Application.Configuration;

public static class ApplicationConfiguration
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.AddAutoMapper(typeof(TopicProfile));

        services.AddScoped<IUserProvider, UserProvider>();
        services.AddScoped<ITopicService, TopicService>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.Configure<CachingOptions>(configuration.GetSection("Caching"));

        services.AddDbContext<MaterialAdvisorContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }
}
