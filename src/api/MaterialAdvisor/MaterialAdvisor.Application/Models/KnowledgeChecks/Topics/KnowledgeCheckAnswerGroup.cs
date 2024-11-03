namespace MaterialAdvisor.Application.Models.Topics;

public class KnowledgeCheckAnswerGroup
{
    public Guid Id { get; set; }

    public byte Number { get; set; }

    public bool IsTechnical { get; set; }

    public IEnumerable<LanguageText>? Content { get; set; }

    public IEnumerable<KnowledgeCheckAnswer> Answers { get; set; } = [];
}
