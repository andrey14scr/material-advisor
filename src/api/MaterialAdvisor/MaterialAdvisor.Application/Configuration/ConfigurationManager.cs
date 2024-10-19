﻿using MaterialAdvisor.Application.AI;
using MaterialAdvisor.Application.AI.Configuration.Options;
using MaterialAdvisor.Application.Configuration.Options;
using MaterialAdvisor.Application.Mapping;
using MaterialAdvisor.Application.Services;
using MaterialAdvisor.Application.Services.Abstraction;
using MaterialAdvisor.Application.Storage;
using MaterialAdvisor.Application.Storage.Configuration.Options;
using MaterialAdvisor.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MaterialAdvisor.Application.Configuration;

public static class ConfigurationManager
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.AddAutoMapper(typeof(TopicProfile));

        // Services registering:
        services.AddScoped<IUserProvider, UserProvider>();
        services.AddScoped<ITopicService, TopicService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<ISecurityService, SecurityService>();
        services.AddScoped<IKnowledgeCheckService, KnowledgeCheckService>();
        services.AddScoped<ISubmittedAnswerService, SubmittedAnswerService>();
        services.AddScoped<IAttemptService, AttemptService>();
        services.AddScoped<IMaterialAdvisorAIAssistant, MaterialAdvisorAIAssistant>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        //services.AddSingleton<IStorageService, LocalStorageService>();
        services.AddSingleton<IStorageService, BlobStorageService>();

        // Options registering:
        services.Configure<CachingOptions>(configuration.GetSection("Caching"));
        services.Configure<SecurityOptions>(configuration.GetSection("Security"));
        services.Configure<StorageOptions>(configuration.GetSection("Storage"));
        services.Configure<AzureOptions>(configuration.GetSection("Azure"));
        services.Configure<OpenAIAssistantOptions>(configuration.GetSection("OpenAIAssistant"));

        // Database registering:
        services.AddDbContext<MaterialAdvisorContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }
}
