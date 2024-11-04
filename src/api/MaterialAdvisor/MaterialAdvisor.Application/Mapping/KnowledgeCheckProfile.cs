using AutoMapper;

using MaterialAdvisor.Application.Models.KnowledgeChecks;
using MaterialAdvisor.Data.Entities;
using MaterialAdvisor.Data.Extensions;

namespace MaterialAdvisor.Application.Mapping;

public class KnowledgeCheckProfile : Profile
{
    public KnowledgeCheckProfile()
    {
        CreateMap<KnowledgeCheck, KnowledgeCheckEntity>()
            .ForMember(dest => dest.Groups, opt => opt.MapFrom(src => src.GroupIds.Select(g => new GroupEntity { Id = g }).ToList()));

        CreateMap<KnowledgeCheckEntity, KnowledgeCheckTopicListItem>()
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.StartDate, DateTimeKind.Utc)))
            .ForMember(dest => dest.HasAnswersToVerify, opt => opt.MapFrom(src => src.Attempts.Any()))
            .ForMember(dest => dest.EndDate,
                opt =>
                {
                    opt.PreCondition(src => src.EndDate.HasValue);
                    opt.MapFrom(src => DateTime.SpecifyKind(src.EndDate!.Value, DateTimeKind.Utc));
                });

        CreateMap<KnowledgeCheckEntity, KnowledgeCheckListItem>()
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.StartDate, DateTimeKind.Utc)))
            .ForMember(dest => dest.EndDate,
                opt =>
                {
                    opt.PreCondition(src => src.EndDate.HasValue);
                    opt.MapFrom(src => DateTime.SpecifyKind(src.EndDate!.Value, DateTimeKind.Utc));
                })
            .ForMember(dest => dest.UsedAttempts, opt => opt.MapFrom(src => src.Attempts.Count))
            .ForMember(dest => dest.MaxScore, opt => opt.MapFrom(src => src.Topic.Questions.Sum(q => q.Points)))
            .ForMember(dest => dest.IsSubmitted,
                opt =>
                {
                    opt.PreCondition(src => src.Attempts.Any());
                    opt.MapFrom(src => src.Attempts.OrderByDescending(a => a.StartDate).First().IsFinished());
                })
            .ForMember(dest => dest.HasAnswersToVerify,
                opt => opt.MapFrom(src => src.Attempts
                    .Any(a => a.SubmittedAnswers.Any(sa => !sa.VerifiedAnswers.Any(va => va.IsManual)))))
            .ForMember(dest => dest.IsVerified,
                opt =>
                {
                    opt.PreCondition(src => src.Attempts.Any(a => a.IsFinished()));
                    opt.MapFrom(src => IsVerified(src));
                })
            .ForMember(dest => dest.Score,
                opt =>
                {
                    opt.PreCondition(src => src.Attempts.Any(a => a.IsFinished()));
                    opt.MapFrom(src => CalculateScore(src));
                });

        CreateMap<KnowledgeCheckEntity, KnowledgeCheck>()
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.StartDate, DateTimeKind.Utc)))
            .ForMember(dest => dest.EndDate,
                opt => 
                {
                    opt.PreCondition(src => src.EndDate.HasValue);
                    opt.MapFrom(src => DateTime.SpecifyKind(src.EndDate!.Value, DateTimeKind.Utc));
                })
            .ForMember(dest => dest.GroupIds, opt => opt.MapFrom(src => src.Groups.Select(g => g.Id).ToList()))
            .ForMember(dest => dest.UsedAttempts, opt => opt.MapFrom(src => src.Attempts.Count));
    }

    private static double? CalculateScore(KnowledgeCheckEntity src)
    {
        var lastAttempt = src.Attempts.OrderByDescending(a => a.StartDate).First();

        if (!lastAttempt.IsFinished())
        {
            return null;
        }

        var typesToVerify = Constants.QuestionTypesRequiredVerification;

        var openQuestionsSum = lastAttempt.SubmittedAnswers
            .Where(sa => !string.IsNullOrEmpty(sa.Value) && typesToVerify.Contains(sa.AnswerGroup.Question.Type))
            .Sum(sa => sa.VerifiedAnswers.Where(va => va.IsManual).Sum(va => va.Score));

        var closedQuestionsSum = lastAttempt.SubmittedAnswers
            .Where(sa => !string.IsNullOrEmpty(sa.Value) && typesToVerify.Contains(sa.AnswerGroup.Question.Type))
            .Sum(sa => sa.AnswerGroup.Answers
                .Where(a => SplitValue(sa.Value!).Contains(a.Id.ToString()))
                .Sum(a => a.Points));

        string[] SplitValue(string str)
        {
            return str.Split(Data.Constants.ListDelimeter, StringSplitOptions.RemoveEmptyEntries);
        }

        return openQuestionsSum + closedQuestionsSum;
    }

    private static bool IsVerified(KnowledgeCheckEntity src)
    {
        var lastAttempt = src.Attempts.OrderByDescending(a => a.StartDate).First();
        var typesToVerify = Constants.QuestionTypesRequiredVerification;

        return lastAttempt.IsFinished() && lastAttempt.SubmittedAnswers
            .Where(sa => typesToVerify.Contains(sa.AnswerGroup.Question.Type))
            .All(sa => sa.VerifiedAnswers.Any(va => va.IsManual));
    }
}
