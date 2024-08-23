using MaterialAdvisor.Data.Enums;

namespace MaterialAdvisor.Application.Models.Shared;

public class LanguageText
{
    public Guid Id { get; set; }

    public Language LanguageId { get; set; }

    public string Text { get; set; } = null!;
}
