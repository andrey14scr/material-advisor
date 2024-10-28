using FluentValidation;

using MaterialAdvisor.API.Models.Requests.TopicGeneration;

namespace MaterialAdvisor.API.Validators;

public class TopicGenerationRequestValidator : AbstractValidator<TopicGenerationRequest>
{
    public TopicGenerationRequestValidator()
    {
        RuleFor(x => x.TopicName)
            .NotEmpty()
            .WithMessage("At least 1 topic name is required");

        RuleFor(x => x)
            .Must(x => !x.TopicName.Select(tn => tn.LanguageId).Intersect(x.Languages).Any())
            .WithMessage("Languages should not contain language ids from topic name");

        RuleFor(x => x.QuestionsStructure)
            .NotEmpty()
            .WithMessage("Questions structure acnnot be empty");

        RuleForEach(x => x.QuestionsStructure).ChildRules(tn =>
        {
            tn.RuleFor(x => x.Count)
                .LessThanOrEqualTo((byte)100)
                .WithMessage("Questions count should be less than 100");
        });

        RuleFor(x => x.DefaultAnswersCount)
            .LessThanOrEqualTo((byte)8)
            .WithMessage("Default answers count cannot be more than 8");

        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("File cannot be null");
    }
}