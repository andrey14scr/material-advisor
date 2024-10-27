using MaterialAdvisor.Application.Models.KnowledgeChecks;

namespace MaterialAdvisor.Application.Models.Topics;

public class TopicListItem
{
    public Guid Id { get; set; }

    public string Owner { get; set; }

    public ushort Number { get; set; }

    public uint Version { get; set; }

    public IEnumerable<LanguageText> Name { get; set; } = [];

    public IEnumerable<KnowledgeCheckListItem> KnowledgeChecks { get; set; } = [];
}
