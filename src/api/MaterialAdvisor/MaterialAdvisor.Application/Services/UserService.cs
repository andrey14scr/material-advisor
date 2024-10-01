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
    public async Task<User> Get(string login, string password)
    {
        var searchLogin = _securityService.Encrypt(login);
        var searchHash = _securityService.GetHash(password);
        var user = await _dbContext.Users
            .SingleOrDefaultAsync(u => (u.Name == searchLogin || u.Email == searchLogin) && u.Hash == searchHash);
        
        if (user is null)
        {
            throw new NotFoundException();
        }

        return _mapper.Map<User>(user);
    }

    public async Task<User> Create(string userName, string email, string password)
    {
        var userToCreate = new UserEntity
        {
            Email = _securityService.Encrypt(email),
            Name = _securityService.Encrypt(userName),
            Hash = _securityService.GetHash(password)
        };

        var user = await _dbContext.Users.AddAsync(userToCreate);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<User>(user.Entity);
    }

    public async Task UpdateSettings(UserSettings userSettings)
    {
        var user = await _userProvider.GetUser();

        await _dbContext.Users
            .Where(u => u.Id == user.Id)
            .ExecuteUpdateAsync(u => u.SetProperty(p => p.CurrentLanguage, userSettings.CurrentLanguage)
                .SetProperty(p => p.FirstName, userSettings.FirstName)
                .SetProperty(p => p.SecondName, userSettings.SecondName));
    }
}
