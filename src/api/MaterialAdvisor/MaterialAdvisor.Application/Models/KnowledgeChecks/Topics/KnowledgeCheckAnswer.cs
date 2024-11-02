namespace MaterialAdvisor.Application.Models.Topics;

public class KnowledgeCheckAnswer
{
    public Guid Id { get; set; }

    public byte Number { get; set; }

    public double Points { get; set; }

    public IEnumerable<LanguageText> Content { get; set; } = [];
}