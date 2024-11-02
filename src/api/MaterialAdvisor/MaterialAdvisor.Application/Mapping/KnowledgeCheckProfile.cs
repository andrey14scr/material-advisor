using AutoMapper;

using MaterialAdvisor.Application.Models.KnowledgeChecks;
using MaterialAdvisor.Data.Entities;
using MaterialAdvisor.Data.Extensions;

namespace MaterialAdvisor.Application.Mapping;

public class KnowledgeCheckProfile : Profile
{
    public KnowledgeCheckProfile()
    {
        CreateMap<KnowledgeCheck, KnowledgeCheckEntity>()
            .ForMember(dest => dest.Groups, opt => opt.MapFrom(src => src.GroupIds.Select(g => new GroupEntity { Id = g }).ToList()));

        CreateMap<KnowledgeCheckEntity, KnowledgeCheckTopicListItem>()
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.StartDate, DateTimeKind.Utc)))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.HasValue 
                ? DateTime.SpecifyKind(src.EndDate.Value, DateTimeKind.Utc) 
                : (DateTime?)null));

        CreateMap<KnowledgeCheckEntity, KnowledgeCheckListItem>()
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.StartDate, DateTimeKind.Utc)))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.HasValue
                ? DateTime.SpecifyKind(src.EndDate.Value, DateTimeKind.Utc)
                : (DateTime?)null))
            .ForMember(dest => dest.UsedAttempts, opt => opt.MapFrom(src => src.Attempts.Count))
            .ForMember(dest => dest.IsSubmitted, opt => opt.MapFrom(src => src.Attempts.Any(a => a.IsFinished())));

        CreateMap<KnowledgeCheckEntity, KnowledgeCheck>()
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.StartDate, DateTimeKind.Utc)))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.HasValue 
                ? DateTime.SpecifyKind(src.EndDate.Value, DateTimeKind.Utc) 
                : (DateTime?)null))
            .ForMember(dest => dest.GroupIds, opt => opt.MapFrom(src => src.Groups.Select(g => g.Id).ToList()))
            .ForMember(dest => dest.UsedAttempts, opt => opt.MapFrom(src => src.Attempts.Count));
    }
}
