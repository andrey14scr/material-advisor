namespace MaterialAdvisor.Application.Models.Topics;

public class TopicListItem<TKnowledgeChecks>
{
    public Guid Id { get; set; }

    public uint Version { get; set; }

    public IEnumerable<LanguageText> Name { get; set; } = [];

    public IEnumerable<TKnowledgeChecks> KnowledgeChecks { get; set; } = [];
}