using AutoMapper;

using MaterialAdvisor.Application.Models.KnowledgeChecks;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Mapping;

public class SubmittedAnswerProfile : Profile
{
    public SubmittedAnswerProfile()
    {
        CreateMap<SubmittedAnswerEntity, SubmittedAnswer>()
            .ForMember(dest => dest.Values, 
                opt => 
                {
                    opt.PreCondition(src => !string.IsNullOrEmpty(src.Value));
                    opt.MapFrom(src => src.Value!.Split(Data.Constants.ListDelimeter, StringSplitOptions.RemoveEmptyEntries));
                });

        CreateMap<SubmittedAnswer, SubmittedAnswerEntity>()
            .ForMember(dest => dest.Value, 
                opt => 
                {
                    opt.PreCondition(src => src.Values.Any());
                    opt.MapFrom(src => string.Join(Data.Constants.ListDelimeter, src.Values.Where(v => !string.IsNullOrEmpty(v))));
                });

        CreateMap<SubmittedAnswerEntity, UnverifiedAnswer>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.Attempt.User))
            .ForMember(dest => dest.KnowledgeCheck, opt => opt.MapFrom(src => src.Attempt.KnowledgeCheck))
            .ForMember(dest => dest.Topic, opt => opt.MapFrom(src => src.AnswerGroup.Question.Topic))
            .ForMember(dest => dest.Values,
                opt =>
                {
                    opt.PreCondition(src => !string.IsNullOrEmpty(src.Value));
                    opt.MapFrom(src => src.Value!.Split(Data.Constants.ListDelimeter, StringSplitOptions.RemoveEmptyEntries));
                });
    }
}