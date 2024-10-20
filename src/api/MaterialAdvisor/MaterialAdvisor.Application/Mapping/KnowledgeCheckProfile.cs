using AutoMapper;

using MaterialAdvisor.Application.Models.KnowledgeChecks;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Mapping;

public class KnowledgeCheckProfile : Profile
{
    public KnowledgeCheckProfile()
    {
        CreateMap<KnowledgeCheck, KnowledgeCheckEntity>()
            .ForMember(dest => dest.Groups, opt => opt.MapFrom(src => src.GroupIds.Select(g => new GroupEntity { Id = g }).ToList()));

        CreateMap<KnowledgeCheckEntity, KnowledgeCheck>()
            .ForMember(dest => dest.GroupIds, opt => opt.MapFrom(src => src.Groups.Select(g => g.Id).ToList()));

        CreateMap<KnowledgeCheckEntity, KnowledgeCheckListItem>()
            .ForMember(dest => dest.UsedAttempts, opt => opt.MapFrom(src => src.Attempts.Count));
    }
}
