using System.Collections.Concurrent;

namespace MaterialAdvisor.QueueStorage;

public class InMemoryMessagesQueueService : IMessagesQueueService
{
    private readonly ConcurrentQueue<QueueMessage> _queue = new ConcurrentQueue<QueueMessage>();

    public void SendMessage(QueueMessage message)
    {
        _queue.Enqueue(message);
    }

    public bool TryDequeue(out QueueMessage message)
    {
        return _queue.TryDequeue(out message);
    }

    public bool IsEmpty => _queue.IsEmpty;
}
