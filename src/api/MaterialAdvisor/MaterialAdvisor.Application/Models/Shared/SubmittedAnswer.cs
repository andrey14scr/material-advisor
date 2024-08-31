namespace MaterialAdvisor.Application.Models.Shared;

public class SubmittedAnswer
{
    public Guid QuestionId { get; set; }

    public Guid AttemptId { get; set; }

    public string? Value { get; set; }
}
