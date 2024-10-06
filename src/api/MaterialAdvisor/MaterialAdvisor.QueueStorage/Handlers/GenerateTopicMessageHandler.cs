using MaterialAdvisor.Data;
using MaterialAdvisor.QueueStorage.Messages;
using MaterialAdvisor.SignalR;
using MaterialAdvisor.SignalR.Hubs;

using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaterialAdvisor.QueueStorage.Handlers;

public class GenerateTopicMessageHandler(IHubContext<TopicGenerationHub> _topicGenerationHubContext, 
    ILogger<GenerateTopicMessageHandler> _logger, 
    MaterialAdvisorContext _dbContext) : IMessageHandler<GenerateTopicMessage>
{
    public async Task HandleAsync(GenerateTopicMessage message)
    {
        try
        {
            var topic = await _dbContext.Topics.SingleAsync(t => t.Id == message.TopicId);

            // ...

            await _topicGenerationHubContext.Clients
                .User(message.UserName)
                .SendAsync(SignalRConstants.Messages.TopicGeneratedMessage, message.TopicId, TopicGenerationStatuses.Generated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing handler. Removing old topic...");

            await _topicGenerationHubContext.Clients
                .User(message.UserName)
                .SendAsync(SignalRConstants.Messages.TopicGeneratedMessage, message.TopicId, TopicGenerationStatuses.Failed);

            await _dbContext.Topics.Where(t => t.Id == message.TopicId).ExecuteDeleteAsync();

            throw;
        }
    }
}
