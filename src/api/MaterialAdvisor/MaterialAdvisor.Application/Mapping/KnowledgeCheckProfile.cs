using AutoMapper;

using MaterialAdvisor.Application.Models.Editable;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Mapping;

public class KnowledgeCheckProfile : Profile
{
    public KnowledgeCheckProfile()
    {
        CreateMap<EditableKnowledgeCheck, KnowledgeCheckEntity>()
            .ForMember(dest => dest.Groups, opt => opt.MapFrom(src => src.GroupIds.Select(g => new GroupEntity { Id = g }).ToList()));

        CreateMap<KnowledgeCheckEntity, EditableKnowledgeCheck>()
            .ForMember(dest => dest.GroupIds, opt => opt.MapFrom(src => src.Groups.Select(g => g.Id).ToList()));
    }
}
