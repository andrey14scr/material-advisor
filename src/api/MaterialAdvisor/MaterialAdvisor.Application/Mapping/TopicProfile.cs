using AutoMapper;

using MaterialAdvisor.Application.Models.Editable;
using MaterialAdvisor.Application.Models.Readonly;
using MaterialAdvisor.Application.Models.Shared;
using MaterialAdvisor.Application.Services;
using MaterialAdvisor.Data.Entities;

using static MaterialAdvisor.Application.Mapping.UserProfile;

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
            .AfterMap<DecryptUserNameAction>();
    }

    public class DecryptUserNameAction(ISecurityService _securityService) : IMappingAction<TopicEntity, TopicListItem>
    {
        public void Process(TopicEntity source, TopicListItem destination, ResolutionContext context)
        {
            destination.Owner = _securityService.Decrypt(source.Owner.Name);
        }
    }
}
