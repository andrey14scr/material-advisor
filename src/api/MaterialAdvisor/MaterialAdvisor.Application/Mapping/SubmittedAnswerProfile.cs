using AutoMapper;

using MaterialAdvisor.Application.Models.KnowledgeChecks;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Mapping;

public class SubmittedAnswerProfile : Profile
{
    public SubmittedAnswerProfile()
    {
        CreateMap<SubmittedAnswerEntity, SubmittedAnswer>().ReverseMap();

        CreateMap<SubmittedAnswerEntity, UnverifiedAnswer>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.Attempt.User))
            .ForMember(dest => dest.KnowledgeCheck, opt => opt.MapFrom(src => src.Attempt.KnowledgeCheck))
            .ForMember(dest => dest.Topic, opt => opt.MapFrom(src => src.AnswerGroup.Question.Topic));
    }
}