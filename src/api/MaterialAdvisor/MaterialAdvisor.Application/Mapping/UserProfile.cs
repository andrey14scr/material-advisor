using AutoMapper;

using MaterialAdvisor.Application.Models.Users;
using MaterialAdvisor.Application.Services.Abstraction;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserEntity, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .AfterMap<DecryptUserInfoAction>();

        CreateMap<UserEntity, UserSettings>();

        CreateMap<User, UserEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .AfterMap<EncryptUserInfoAction>();
    }

    public class DecryptUserInfoAction(ISecurityService _securityService) : IMappingAction<UserEntity, User>
    {
        public void Process(UserEntity source, User destination, ResolutionContext context)
        {
            destination.Email = _securityService.Decrypt(source.Email);
        }
    }

    public class EncryptUserInfoAction(ISecurityService _securityService) : IMappingAction<User, UserEntity>
    {
        public void Process(User source, UserEntity destination, ResolutionContext context)
        {
            destination.Email = _securityService.Encrypt(source.Email);
        }
    }
}