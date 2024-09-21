using AutoMapper;

using MaterialAdvisor.Application.Models.Editable;
using MaterialAdvisor.Application.Models.Readonly;
using MaterialAdvisor.Application.Models.Shared;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Mapping;

public class TopicProfile : Profile
{
    public TopicProfile()
    {
        CreateMap<EditableTopic, TopicEntity>().ReverseMap();
        CreateMap<LanguageText, LanguageTextEntity>().ReverseMap();
        CreateMap<EditableQuestion, QuestionEntity>().ReverseMap();
        CreateMap<EditableAnswer, AnswerEntity>().ReverseMap();
        CreateMap<EditableAnswerGroup, AnswerGroupEntity>().ReverseMap();

        CreateMap<TopicEntity, TopicListItem>()
            .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner.Name));
    }
}
