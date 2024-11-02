using AutoMapper;

using MaterialAdvisor.Application.Models.KnowledgeChecks;
using MaterialAdvisor.Application.Models.Topics;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Mapping;

public class TopicProfile : Profile
{
    public TopicProfile()
    {
        CreateMap<LanguageText, LanguageTextEntity>().ReverseMap();

        CreateMap<Topic, TopicEntity>().ReverseMap();
        CreateMap<Question, QuestionEntity>().ReverseMap();
        CreateMap<AnswerGroup, AnswerGroupEntity>().ReverseMap();
        CreateMap<Answer, AnswerEntity>().ReverseMap();

        CreateMap<KnowledgeCheckTopic, TopicEntity>().ReverseMap();
        CreateMap<KnowledgeCheckQuestion, QuestionEntity>().ReverseMap();
        CreateMap<KnowledgeCheckAnswerGroup, AnswerGroupEntity>().ReverseMap();
        CreateMap<KnowledgeCheckAnswer, AnswerEntity>().ReverseMap();

        CreateMap<TopicListItem<KnowledgeCheckListItem>, TopicEntity>().ReverseMap();
        CreateMap<TopicListItem<KnowledgeCheckTopicListItem>, TopicEntity>().ReverseMap();
    }
}