namespace MaterialAdvisor.Application.Models.KnowledgeChecks;

public class VerifiedAnswer
{
    public Guid AnswerGroupId { get; set; }

    public Guid AttemptId { get; set; }

    public double Score { get; set; }
}