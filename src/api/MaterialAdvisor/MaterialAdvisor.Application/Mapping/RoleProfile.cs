using AutoMapper;

using MaterialAdvisor.Application.Models.Shared;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Mapping;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<RoleEntity, Role>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Permissions.Select(p => p.Id)))
            .ForMember(dest => dest.GroupIds, opt => opt.MapFrom(src => src.GroupRoles.Select(gr => gr.GroupId)));
    }
}