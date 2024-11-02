namespace MaterialAdvisor.Application.Models.Topics;

public class KnowledgeCheckTopic
{
    public Guid Id { get; set; }

    public IEnumerable<KnowledgeCheckQuestion> Questions { get; set; } = [];

    public IEnumerable<LanguageText> Name { get; set; } = [];
}
