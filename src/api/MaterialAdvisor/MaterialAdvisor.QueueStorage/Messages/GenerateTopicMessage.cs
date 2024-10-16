namespace MaterialAdvisor.QueueStorage.Messages;

public class GenerateTopicMessage : QueueMessage
{
    public Guid TopicId { get; set; }

    public List<QuestionsSection>? QuestionsStructure { get; set; }

    public ushort? MaxQuestionsCount { get; set; }

    public byte? AnswersCount { get; set; }

    public bool DoesComplexityIncrease { get; set; }
}
