using FluentValidation;

using MaterialAdvisor.Application.Models.Topics;

namespace MaterialAdvisor.API.Validators;

public class LanguageTextValidator : AbstractValidator<LanguageText>
{
    public LanguageTextValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .WithMessage("Text cannot be empty");
    }
}