using MaterialAdvisor.Application.Quartz.Configuration.Options;
using MaterialAdvisor.Application.Quartz.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Quartz;

namespace MaterialAdvisor.Application.Quartz.Configuration;

public static class ConfigurationManager
{
    public static void ConfigureQuartz(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<VerifyKnowledgeCheckJobOptions>(configuration.GetSection("VerifyKnowledgeCheckJobOptions"));

        var options = configuration.GetSection("VerifyKnowledgeCheckJobOptions").Get<VerifyKnowledgeCheckJobOptions>()!;

        services.AddQuartz(q =>
        {
            var jobKey = new JobKey(nameof(VerifyKnowledgeCheckJob));
            q.AddJob<VerifyKnowledgeCheckJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity($"{nameof(VerifyKnowledgeCheckJob)}-trigger")
                .WithCronSchedule(options.Cron)
                .StartNow()
            );
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }
}
