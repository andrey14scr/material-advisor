namespace MaterialAdvisor.Application.Models.Shared;

public abstract class AbstractTopic<TQuestion>
{
    public Guid Id { get; set; }

    public IEnumerable<TQuestion> Questions { get; set; } = [];

    public IEnumerable<LanguageText> Texts { get; set; } = [];
}
