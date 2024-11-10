using AutoMapper;

using MaterialAdvisor.Application.Exceptions;
using MaterialAdvisor.Application.Models.Shared;
using MaterialAdvisor.Application.Models.Users;
using MaterialAdvisor.Application.Services.Abstraction;
using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;

using Microsoft.EntityFrameworkCore;


namespace MaterialAdvisor.Application.Services;

public class UserService(MaterialAdvisorContext _dbContext, 
    ISecurityService _securityService,
    IUserProvider _userProvider,
    IMapper _mapper) : IUserService
{
    public async Task<TModel> Get<TModel>(string login, string password)
    {
        var searchLogin = _securityService.Encrypt(login);
        var searchHash = _securityService.GetHash(password);
        var user = await _dbContext.Users
            .SingleOrDefaultAsync(u => (u.Name == login || u.Email == searchLogin) && u.Hash == searchHash);
        
        if (user is null)
        {
            throw new NotFoundException();
        }

        return _mapper.Map<TModel>(user);
    }

    public async Task<TModel> Create<TModel>(string userName, string email, string password)
    {
        var userToCreate = new UserEntity
        {
            Email = _securityService.Encrypt(email),
            Name = userName,
            Hash = _securityService.GetHash(password)
        };

        var user = await _dbContext.Users.AddAsync(userToCreate);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<TModel>(user.Entity);
    }

    public async Task UpdateSettings(UserSettings userSettings)
    {
        var user = await _userProvider.GetUser();
        await _dbContext.Users
            .Where(u => u.Id == user.Id)
            .ExecuteUpdateAsync(u => u.SetProperty(p => p.CurrentLanguage, userSettings.CurrentLanguage));
    }

    public async Task<UserSettings> GetUserSettings()
    {
        var user = await _userProvider.GetUser();
        var userEntity = await _dbContext.Users.SingleAsync(u => u.Id == user.Id);
        var settings = _mapper.Map<UserSettings>(userEntity);
        return settings;
    }

    public async Task<IList<TModel>> Search<TModel>(string input)
    {
        var entities = await _dbContext.Users
            .Where(u => u.Name.ToLower().Contains(input.ToLower()))
            .OrderBy(u => u.Name)
            .AsNoTracking()
            .ToListAsync();
        var entitiesModel = _mapper.Map<IList<TModel>>(entities);
        return entitiesModel;
    }

    public async Task<IList<TModel>> Get<TModel>(Pagination pagination)
    {
        var entities = await _dbContext.Users
            .OrderBy(u => u.Name)
            .Skip(pagination.PageSize * (pagination.Page - 1))
            .Take(pagination.PageSize)
            .AsNoTracking()
            .ToListAsync();
        var entitiesModel = _mapper.Map<IList<TModel>>(entities);
        return entitiesModel;
    }
}