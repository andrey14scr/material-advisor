namespace MaterialAdvisor.Application.Models.KnowledgeChecks;

public class StartedAttempt
{
    public Guid Id { get; set; }

    public DateTime StartDate { get; set; }

    public Guid KnowledgeCheckId { get; set; }

    public IEnumerable<SubmittedAnswer> SubmittedAnswers { get; set; } = [];
}