namespace MaterialAdvisor.Application.Models.Topics;

public class Topic
{
    public Guid Id { get; set; }

    public uint Version { get; set; }

    public string? FilePath { get; set; }

    public IEnumerable<Question> Questions { get; set; } = [];

    public IEnumerable<LanguageText> Name { get; set; } = [];
}
