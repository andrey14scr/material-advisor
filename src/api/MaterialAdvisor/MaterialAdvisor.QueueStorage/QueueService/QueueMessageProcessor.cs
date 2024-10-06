using MaterialAdvisor.Data;
using MaterialAdvisor.QueueStorage.Handlers;
using MaterialAdvisor.QueueStorage.Messages;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MaterialAdvisor.QueueStorage.QueueService;

public class QueueMessageProcessor(IMessagesQueueService _queueService, 
    IServiceProvider _serviceProvider, 
    ILogger<QueueMessageProcessor> _logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Messages Queue processing started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            if (_queueService.TryDequeue(out var message))
            {
                try
                {
                    await HandleMessageAsync(message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message.");
                }
            }
            else
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        _logger.LogInformation("Messages Queue processing stopped.");
    }

    private async Task HandleMessageAsync(QueueMessage message)
    {
        var messageType = message.GetType();
        var handlerType = typeof(IMessageHandler<>).MakeGenericType(messageType);
        using var scope = _serviceProvider.CreateScope();
        
        //var _emailRepository = scope.ServiceProvider.GetRequiredService<MaterialAdvisorContext>();
        
        //var dbContext = _serviceProvider.GetRequiredService<MaterialAdvisorContext>();
        
        var handler = scope.ServiceProvider.GetService(handlerType);
        if (handler == null)
        {
            throw new ArgumentNullException(nameof(handler), "Appropriate handler was not found");
        }

        var handleMethod = handlerType.GetMethod("HandleAsync");
        if (handleMethod is null)
        {
            throw new ArgumentNullException(nameof(handleMethod), "Appropriate handler method was not found");
        }

        _logger.LogInformation($"Message {messageType.Name} is handled by {handlerType.Name}");
        var task = (Task)handleMethod.Invoke(handler, [message])!;
        await task;
    }
}