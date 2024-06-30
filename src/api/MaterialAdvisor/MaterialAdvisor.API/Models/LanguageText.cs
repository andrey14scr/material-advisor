using MaterialAdvisor.Data.Enums;

namespace MaterialAdvisor.API.Models;

public class LanguageText
{
    public LanguageType Language { get; set; }

    public string Text { get; set; } = null!;
}
