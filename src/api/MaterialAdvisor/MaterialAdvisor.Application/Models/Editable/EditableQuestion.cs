using MaterialAdvisor.Application.Models.Shared;
using MaterialAdvisor.Data.Enums;

namespace MaterialAdvisor.Application.Models.Editable;

public class EditableQuestion
{
    public Guid Id { get; set; }

    public byte Number { get; set; }

    public byte Version { get; set; }

    public double Points { get; set; }

    public QuestionType Type { get; set; }

    public IEnumerable<LanguageText> Texts { get; set; } = null!;

    public IEnumerable<EditableAnswerGroup> AnswerGroups { get; set; } = [];
}