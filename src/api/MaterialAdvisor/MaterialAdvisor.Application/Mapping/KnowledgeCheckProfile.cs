using AutoMapper;

using MaterialAdvisor.Application.Models.Editable;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Mapping;

public class KnowledgeCheckProfile : Profile
{
    public KnowledgeCheckProfile()
    {
        CreateMap<EditableKnowledgeCheck, KnowledgeCheckEntity>().ReverseMap();
    }
}
