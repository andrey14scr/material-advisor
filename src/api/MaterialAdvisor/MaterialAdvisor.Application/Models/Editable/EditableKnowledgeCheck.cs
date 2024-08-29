namespace MaterialAdvisor.Application.Models.Editable;

public class EditableKnowledgeCheck
{
    public Guid Id { get; set; }

    public Guid TopicId { get; set; }

    public short Number { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public ushort? Time { get; set; }

    public byte? MaxAttempts { get; set; }

    public IEnumerable<Guid> GroupIds { get; set; } = [];
}
