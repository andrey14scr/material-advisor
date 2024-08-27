using AutoMapper;

using MaterialAdvisor.Application.Exceptions;
using MaterialAdvisor.Application.Models.Shared;
using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace MaterialAdvisor.Application.Services;

public class UserService(MaterialAdvisorContext dbContext, 
    ISecurityService securityService, 
    IUserProvider userProvider, 
    IMapper mapper) : IUserService
{
    public async Task<UserInfo> Get(string login, string hash)
    {
        var searchLogin = securityService.Encrypt(login);
        var searchHash = securityService.GetHash(hash);
        var user = await dbContext.Users.SingleOrDefaultAsync(u => (u.Name == searchLogin || u.Email == searchLogin) && u.Hash == searchHash);
        
        if (user is null)
        {
            throw new NotFoundException();
        }

        return mapper.Map<UserInfo>(user);
    }

    public async Task<UserInfo> Create(string username, string email, string hash)
    {
        var initialGroup = new List<GroupEntity>
        { 
            new GroupEntity()
            {
                Name = securityService.Encrypt($"{username} group"),
            }
        };

        var userToCreate = new UserEntity
        {
            Email = securityService.Encrypt(email),
            Name = securityService.Encrypt(username),
            Hash = securityService.GetHash(hash),
            Groups = initialGroup,
            CreatedGroups = initialGroup
        };

        var user = await dbContext.Users.AddAsync(userToCreate);
        await dbContext.SaveChangesAsync();

        return userProvider.AddUser(user.Entity);
    }
}
