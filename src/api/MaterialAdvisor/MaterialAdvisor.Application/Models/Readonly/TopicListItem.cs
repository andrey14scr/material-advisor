using MaterialAdvisor.Application.Models.Shared;

namespace MaterialAdvisor.Application.Models.Readonly;

public class TopicListItem
{
    public Guid Id { get; set; }

    public IEnumerable<LanguageText> Texts { get; set; } = [];

    public IEnumerable<KnowledgeCheckListItem> KnowledgeChecks { get; set; } = [];
}
