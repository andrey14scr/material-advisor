using AutoMapper;

using MaterialAdvisor.Application.Models.Shared;
using MaterialAdvisor.Application.Services;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserEntity, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .AfterMap<DecryptUserInfoAction>();

        CreateMap<User, UserEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .AfterMap<EncryptUserInfoAction>();
    }

    public class DecryptUserInfoAction(ISecurityService _securityService) : IMappingAction<UserEntity, User>
    {
        public void Process(UserEntity source, User destination, ResolutionContext context)
        {
            destination.UserName = _securityService.Decrypt(source.Name);
            destination.Email = _securityService.Decrypt(source.Email);
        }
    }

    public class EncryptUserInfoAction(ISecurityService _securityService) : IMappingAction<User, UserEntity>
    {
        public void Process(User source, UserEntity destination, ResolutionContext context)
        {
            destination.Name = _securityService.Encrypt(source.UserName);
            destination.Email = _securityService.Encrypt(source.Email);
        }
    }
}
