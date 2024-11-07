namespace MaterialAdvisor.Application.Quartz.Models;

public class VerifiedAIAnswer
{
    public Guid AnswerGroupId { get; set; }

    public Guid AttemptId { get; set; }

    public double Score { get; set; }

    public string? Comment { get; set; }
}
