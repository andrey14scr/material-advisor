using AutoMapper;

using MaterialAdvisor.Application.Models.Shared;
using MaterialAdvisor.Application.Services;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Mapping;

public class DecryptUserInfoAction(ISecurityService securityService) : IMappingAction<UserEntity, UserInfo>
{
    public void Process(UserEntity source, UserInfo destination, ResolutionContext context)
    {
        destination.UserName = securityService.Decrypt(source.Name);
        destination.UserEmail = securityService.Decrypt(source.Email);
    }
}