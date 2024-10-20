using AutoMapper;

using MaterialAdvisor.Application.Models.Topics;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Mapping;

public class TopicProfile : Profile
{
    public TopicProfile()
    {
        CreateMap<Topic, TopicEntity>().ReverseMap();
        CreateMap<LanguageText, LanguageTextEntity>().ReverseMap();
        CreateMap<Question, QuestionEntity>().ReverseMap();
        CreateMap<Answer, AnswerEntity>().ReverseMap();
        CreateMap<AnswerGroup, AnswerGroupEntity>().ReverseMap();

        CreateMap<TopicEntity, TopicListItem>()
            .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner.Name));
    }
}
