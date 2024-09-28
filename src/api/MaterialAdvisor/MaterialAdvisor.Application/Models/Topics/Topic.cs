namespace MaterialAdvisor.Application.Models.Topics;

public class Topic
{
    public Guid Id { get; set; }

    public ushort Number { get; set; }

    public uint Version { get; set; }

    public IEnumerable<Question> Questions { get; set; } = [];

    public IEnumerable<LanguageText> Texts { get; set; } = [];
}
