using MaterialAdvisor.Application.QueueStorage.Messages;
using MaterialAdvisor.Data;

namespace MaterialAdvisor.Application.QueueStorage.Handlers;

public interface IMessageHandler<TMessage> where TMessage : QueueMessage
{
    Task HandleAsync(TMessage message);
}
