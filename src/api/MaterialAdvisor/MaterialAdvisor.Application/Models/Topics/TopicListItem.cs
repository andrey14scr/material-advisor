using MaterialAdvisor.Application.Models.KnowledgeChecks;

namespace MaterialAdvisor.Application.Models.Topics;

public class TopicListItem
{
    public Guid Id { get; set; }

    public string Owner { get; set; }

    public ushort Number { get; set; }

    public IEnumerable<LanguageText> Texts { get; set; } = [];

    public IEnumerable<KnowledgeCheckListItem> KnowledgeChecks { get; set; } = [];
}
