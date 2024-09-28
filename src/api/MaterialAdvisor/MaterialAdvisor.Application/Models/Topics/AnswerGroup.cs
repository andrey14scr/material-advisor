namespace MaterialAdvisor.Application.Models.Topics;

public class AnswerGroup
{
    public byte Number { get; set; }

    public IEnumerable<LanguageText>? Texts { get; set; }

    public IEnumerable<Answer> Answers { get; set; } = [];
}
