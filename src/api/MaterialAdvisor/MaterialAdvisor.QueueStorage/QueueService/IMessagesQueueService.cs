using MaterialAdvisor.QueueStorage.Messages;

namespace MaterialAdvisor.QueueStorage.QueueService;

public interface IMessagesQueueService
{
    void SendMessage(QueueMessage message);

    bool IsEmpty { get; }
}
