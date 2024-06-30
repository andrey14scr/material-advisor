using MaterialAdvisor.Data.Enums;

namespace MaterialAdvisor.API.Models;

public class Question
{
    public byte Number { get; set; }

    public byte Version { get; set; }

    public double Points { get; set; }

    public QuestionType Type { get; set; }

    public IEnumerable<LanguageText> Texts { get; set; } = null!;

    public IEnumerable<AnswerGroup> AnswerGroups { get; set; } = [];
}
