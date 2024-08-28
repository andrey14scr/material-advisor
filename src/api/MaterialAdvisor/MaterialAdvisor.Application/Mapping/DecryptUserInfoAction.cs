using AutoMapper;

using MaterialAdvisor.Application.Models.Shared;
using MaterialAdvisor.Application.Services;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Mapping;

public class DecryptUserInfoAction(ISecurityService _securityService) : IMappingAction<UserEntity, UserInfo>
{
    public void Process(UserEntity source, UserInfo destination, ResolutionContext context)
    {
        destination.UserName = _securityService.Decrypt(source.Name);
        destination.UserEmail = _securityService.Decrypt(source.Email);
    }
}