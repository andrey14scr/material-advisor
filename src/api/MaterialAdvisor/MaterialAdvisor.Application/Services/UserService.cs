using AutoMapper;

using MaterialAdvisor.Application.Exceptions;
using MaterialAdvisor.Application.Models.Shared;
using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace MaterialAdvisor.Application.Services;

public class UserService(MaterialAdvisorContext _dbContext, 
    ISecurityService _securityService, 
    IUserProvider _userProvider, 
    IMapper _mapper) : IUserService
{
    public async Task<UserInfo> Get(string login, string hash)
    {
        var searchLogin = _securityService.Encrypt(login);
        var searchHash = _securityService.GetHash(hash);
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => (u.Name == searchLogin || u.Email == searchLogin) && u.Hash == searchHash);
        
        if (user is null)
        {
            throw new NotFoundException();
        }

        return _mapper.Map<UserInfo>(user);
    }

    public async Task<UserInfo> Create(string userName, string email, string hash)
    {
        var initialGroup = new List<GroupEntity>
        { 
            new GroupEntity()
            {
                Name = _securityService.Encrypt($"{userName} group"),
            }
        };

        var userToCreate = new UserEntity
        {
            Email = _securityService.Encrypt(email),
            Name = _securityService.Encrypt(userName),
            Hash = _securityService.GetHash(hash),
            Groups = initialGroup,
            CreatedGroups = initialGroup
        };

        var user = await _dbContext.Users.AddAsync(userToCreate);
        await _dbContext.SaveChangesAsync();

        return _userProvider.AddUser(user.Entity);
    }
}
