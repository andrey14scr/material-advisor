using MaterialAdvisor.Application.Models.Shared;

namespace MaterialAdvisor.Application.Models.Editable;

public class EditableAnswerGroup
{
    public Guid Id { get; set; }

    public byte Number { get; set; }

    public IEnumerable<LanguageText>? Texts { get; set; }

    public IEnumerable<EditableAnswer> Answers { get; set; } = [];
}