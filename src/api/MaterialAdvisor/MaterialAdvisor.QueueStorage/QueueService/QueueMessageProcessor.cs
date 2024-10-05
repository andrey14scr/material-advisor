using MaterialAdvisor.QueueStorage.Handlers;
using MaterialAdvisor.QueueStorage.Messages;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MaterialAdvisor.QueueStorage.QueueService;

public class QueueMessageProcessor : BackgroundService
{
    private readonly IMessagesQueueService _queueService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<QueueMessageProcessor> _logger;

    public QueueMessageProcessor(IMessagesQueueService queueService, IServiceProvider serviceProvider, ILogger<QueueMessageProcessor> logger)
    {
        _queueService = queueService;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

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

        var handler = _serviceProvider.GetService(handlerType);
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