namespace MaterialAdvisor.Application.Models.KnowledgeChecks;

public class KnowledgeCheckTopicListItem
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool HasAnswersToVerify { get; set; }
}