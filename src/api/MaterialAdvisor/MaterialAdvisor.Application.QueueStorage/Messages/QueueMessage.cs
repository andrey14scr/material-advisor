namespace MaterialAdvisor.Application.QueueStorage.Messages;

public abstract class QueueMessage
{
    public string UserName { get; set; } = null!;

    public string Metadata { get; set; } = "{}";
}
