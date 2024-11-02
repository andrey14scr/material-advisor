namespace MaterialAdvisor.Application.Models.KnowledgeChecks;

public class StartedAttempt
{
    public Guid Id { get; set; }

    public DateTime StartDate { get; set; }

    public Guid KnowledgeCheckId { get; set; }

    public ICollection<SubmittedAnswer> SubmittedAnswers { get; set; } = [];
}