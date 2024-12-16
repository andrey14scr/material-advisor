namespace MaterialAdvisor.QueueStorage;

public interface IMessageHandler<TMessage> where TMessage : QueueMessage
{
    Task HandleAsync(TMessage message);
}
