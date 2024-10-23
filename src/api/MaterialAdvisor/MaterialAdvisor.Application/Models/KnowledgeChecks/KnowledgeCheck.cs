namespace MaterialAdvisor.Application.Models.KnowledgeChecks;

public class KnowledgeCheck
{
    public Guid Id { get; set; }

    public Guid TopicId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public ushort? Time { get; set; }

    public byte? MaxAttempts { get; set; }

    public byte? UsedAttempts { get; set; }

    public IEnumerable<Guid> GroupIds { get; set; } = [];
}
