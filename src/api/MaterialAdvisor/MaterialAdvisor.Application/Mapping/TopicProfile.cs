using AutoMapper;

using MaterialAdvisor.Application.Models.Topics;
using MaterialAdvisor.Application.Services;
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
