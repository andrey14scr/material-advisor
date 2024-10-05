using MaterialAdvisor.QueueStorage.Handlers;
using MaterialAdvisor.QueueStorage.Messages;
using MaterialAdvisor.QueueStorage.QueueService;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MaterialAdvisor.QueueStorage.Configuration;

public static class ConfigurationManager
{
    public static void ConfigureQueueStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMessagesQueueService, InMemoryMessagesQueueService>();
        services.AddHostedService<QueueMessageProcessor>();

        services.AddTransient<IMessageHandler<GenerateTopicMessage>, GenerateTopicMessageHandler>();
    }
}
