using MaterialAdvisor.Application.Models.Readonly;

namespace MaterialAdvisor.Application.Models.Editable;

public class EditableAnswer : Answer
{
    public Guid Id { get; set; }
}