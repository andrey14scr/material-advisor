using Microsoft.AspNetCore.SignalR;

namespace MaterialAdvisor.SignalR.Hubs;

public class TopicGenerationHub : Hub
{
    public async Task TopicGeneratedMessage(string user, Guid generatedTopicId)
    {
        await Clients.User(user).SendAsync(SignalRConstants.Messages.TopicGeneratedMessage, generatedTopicId);
    }
}
