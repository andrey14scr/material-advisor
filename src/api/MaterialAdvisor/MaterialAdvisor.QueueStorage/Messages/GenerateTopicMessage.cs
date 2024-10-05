namespace MaterialAdvisor.QueueStorage.Messages;

public class GenerateTopicMessage : QueueMessage
{
    public Guid TopicId { get; set; }

    public string UserName { get; set; }
}
