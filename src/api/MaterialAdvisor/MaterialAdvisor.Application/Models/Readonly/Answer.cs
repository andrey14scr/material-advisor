using MaterialAdvisor.Application.Models.Shared;

namespace MaterialAdvisor.Application.Models.Readonly;

public class Answer
{
    public byte Number { get; set; }

    public double Points { get; set; }

    public IEnumerable<LanguageText> Texts { get; set; } = [];
}
