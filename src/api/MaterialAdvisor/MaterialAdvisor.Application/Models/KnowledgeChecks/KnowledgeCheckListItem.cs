namespace MaterialAdvisor.Application.Models.KnowledgeChecks;

public class KnowledgeCheckListItem
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public ushort? Time { get; set; }

    public byte? MaxAttempts { get; set; }

    public double? Score { get; set; }

    public double PassScore { get; set; }

    public double MaxScore { get; set; }

    public byte? UsedAttempts { get; set; }

    public bool IsSubmitted { get; set; }

    public bool IsVerified { get; set; }

    public bool IsAttemptOverrided { get; set; }

    public bool IsManualOnlyVerification { get; set; }

    public bool HasAnswersToVerify { get; set; }
}