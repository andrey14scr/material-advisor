using AutoMapper;

using MaterialAdvisor.Application.Models.Editable;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Mapping;

public class TopicProfile : Profile
{
    public TopicProfile()
    {
        CreateMap<EditableTopic, TopicEntity>().ReverseMap();
    }
}
