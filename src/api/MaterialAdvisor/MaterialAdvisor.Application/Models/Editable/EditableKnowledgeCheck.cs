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

    public byte Attempts { get; set; }

    public Guid GroupId { get; set; }
}
