using MaterialAdvisor.Application.QueueStorage.Messages;

namespace MaterialAdvisor.Application.QueueStorage.Handlers;

public interface IMessageHandler<TMessage> where TMessage : QueueMessage
{
    Task HandleAsync(TMessage message);
}
