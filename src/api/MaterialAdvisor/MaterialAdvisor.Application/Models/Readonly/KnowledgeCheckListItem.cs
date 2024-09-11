namespace MaterialAdvisor.Application.Models.Readonly;

public class KnowledgeCheckListItem
{
    public Guid Id { get; set; }

    public short Number { get; set; }

    public string Name { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public ushort? Time { get; set; }

    public byte? MaxAttempts { get; set; }

    public byte? UsedAttempts { get; set; }
}