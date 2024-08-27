using AutoMapper;

using MaterialAdvisor.Application.Models.Shared;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserEntity, UserInfo>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            .AfterMap<DecryptUserInfoAction>();
    }
}
