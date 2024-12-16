namespace MaterialAdvisor.QueueStorage;

public interface IMessagesQueueService
{
    void SendMessage(QueueMessage message);

    bool TryDequeue(out QueueMessage message);

    bool IsEmpty { get; }
}
