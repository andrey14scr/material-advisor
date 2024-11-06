using MaterialAdvisor.Data.Enums;

namespace MaterialAdvisor.Application.QueueStorage.Messages;

public class GenerateTopicMessage : QueueMessage
{
    public Guid TopicId { get; set; }

    public List<Language> Languages { get; set; } = [];

    public string CultureContext { get; set; } = null!;

    public List<QuestionsSection>? QuestionsStructure { get; set; }

    public byte? MaxQuestionsCount { get; set; }

    public bool DoesComplexityIncrease { get; set; }

    public byte? DefaultAnswersCount { get; set; }
}
