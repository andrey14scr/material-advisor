using MaterialAdvisor.Application.QueueStorage.Messages;

namespace MaterialAdvisor.Application.QueueStorage.QueueService;

public interface IMessagesQueueService
{
    void SendMessage(QueueMessage message);

    bool TryDequeue(out QueueMessage message);

    bool IsEmpty { get; }
}
