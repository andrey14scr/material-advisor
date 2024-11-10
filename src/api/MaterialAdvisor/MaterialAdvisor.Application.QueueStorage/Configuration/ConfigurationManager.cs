using MaterialAdvisor.Application.QueueStorage.Handlers;
using MaterialAdvisor.Application.QueueStorage.Messages;
using MaterialAdvisor.Application.QueueStorage.QueueService;
using MaterialAdvisor.Application.TableGenerator.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MaterialAdvisor.Application.QueueStorage.Configuration;

public static class ConfigurationManager
{
    public static void ConfigureQueueStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureTableGenerator(configuration);

        services.AddSingleton<IMessagesQueueService, InMemoryMessagesQueueService>();
        services.AddHostedService<QueueMessageProcessor>();

        services.AddTransient<IMessageHandler<GenerateTopicMessage>, GenerateTopicMessageHandler>();
        services.AddTransient<IMessageHandler<GenerateTablesMessage>, GenerateTablesMessageHandler>();
    }
}
