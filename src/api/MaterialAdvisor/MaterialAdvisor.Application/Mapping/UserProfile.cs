using AutoMapper;

using MaterialAdvisor.Application.Models.Shared;
using MaterialAdvisor.Application.Services;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserEntity, UserInfo>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            .AfterMap<DecryptUserInfoAction>();

        CreateMap<UserInfo, UserEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
            .AfterMap<EncryptUserInfoAction>();
    }

    public class DecryptUserInfoAction(ISecurityService _securityService) : IMappingAction<UserEntity, UserInfo>
    {
        public void Process(UserEntity source, UserInfo destination, ResolutionContext context)
        {
            destination.UserName = _securityService.Decrypt(source.Name);
            destination.UserEmail = _securityService.Decrypt(source.Email);
        }
    }

    public class EncryptUserInfoAction(ISecurityService _securityService) : IMappingAction<UserInfo, UserEntity>
    {
        public void Process(UserInfo source, UserEntity destination, ResolutionContext context)
        {
            destination.Name = _securityService.Encrypt(source.UserName);
            destination.Email = _securityService.Encrypt(source.UserEmail);
        }
    }
}
