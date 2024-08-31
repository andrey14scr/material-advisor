namespace MaterialAdvisor.Application.Models.Shared;

public class Attempt
{
    public Guid Id { get; set; }

    public ushort Number { get; set; }

    public DateTime StartDate { get; set; }

    public Guid KnowledgeCheckId { get; set; }
}