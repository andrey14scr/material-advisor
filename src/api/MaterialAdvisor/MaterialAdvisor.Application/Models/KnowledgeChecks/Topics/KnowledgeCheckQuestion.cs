using MaterialAdvisor.Data.Enums;

namespace MaterialAdvisor.Application.Models.Topics;

public class KnowledgeCheckQuestion
{
    public Guid Id { get; set; }

    public byte Number { get; set; }

    public double Points { get; set; }

    public QuestionType Type { get; set; }

    public IEnumerable<LanguageText> Content { get; set; } = null!;

    public IEnumerable<KnowledgeCheckAnswerGroup> AnswerGroups { get; set; } = [];
}
