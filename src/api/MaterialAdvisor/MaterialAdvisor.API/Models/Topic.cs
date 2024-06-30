namespace MaterialAdvisor.API.Models;

public class Topic
{
    public Guid Id { get; set; }

    public IEnumerable<Question> Questions { get; set; } = [];

    public IEnumerable<LanguageText> Texts { get; set; } = [];
}