using AutoMapper;

using MaterialAdvisor.Application.Models.Users;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Mapping;

public class GroupProfile : Profile
{
    public GroupProfile()
    {
        CreateMap<GroupEntity, Group>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users));

        CreateMap<Group, GroupEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users));
    }
}
