using FluentValidation;

using MaterialAdvisor.Data.Enums;

namespace MaterialAdvisor.API.Validators;

public class QuestionTypeEnumValidator : AbstractValidator<QuestionType>
{
    public QuestionTypeEnumValidator()
    {
        RuleFor(x => x)
            .IsInEnum()
            .WithMessage("Question type should be enum value");
    }
}