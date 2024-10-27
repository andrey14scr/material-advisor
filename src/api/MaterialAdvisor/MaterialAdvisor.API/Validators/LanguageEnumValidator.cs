using FluentValidation;

using MaterialAdvisor.Data.Enums;

namespace MaterialAdvisor.API.Validators;

public class LanguageEnumValidator : AbstractValidator<Language>
{
    public LanguageEnumValidator()
    {
        RuleFor(x => x)
            .IsInEnum()
            .WithMessage("Language id should be enum value");
    }
}
