using MaterialAdvisor.QueueStorage.Messages;

namespace MaterialAdvisor.QueueStorage.Handlers;

public interface IMessageHandler<TMessage> where TMessage : QueueMessage
{
    Task HandleAsync(TMessage message);
}
