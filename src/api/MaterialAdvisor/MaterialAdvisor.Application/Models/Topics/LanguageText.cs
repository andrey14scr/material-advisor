using MaterialAdvisor.Data.Enums;

namespace MaterialAdvisor.Application.Models.Topics;

public class LanguageText
{
    public Language LanguageId { get; set; }

    public string Text { get; set; } = null!;
}
