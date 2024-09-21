using AutoMapper;

using MaterialAdvisor.Application.Exceptions;
using MaterialAdvisor.Application.Models.Shared;
using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;
using MaterialAdvisor.Data.Enums;

using Microsoft.EntityFrameworkCore;

namespace MaterialAdvisor.Application.Services;

public class UserService(MaterialAdvisorContext _dbContext, 
    ISecurityService _securityService,
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

        var initialGroup = new GroupEntity()
        {
            Name = $"{userName}'s group",
        };

        var group = await _dbContext.Groups.AddAsync(initialGroup);

        var initialGroupRole = new GroupRoleEntity
        {
            UserId = user.Entity.Id,
            RoleId = RoleType.Admin,
            GroupId = group.Entity.Id
        };

        await _dbContext.GroupRoles.AddAsync(initialGroupRole);

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<User>(user.Entity);
    }

    public async Task<IEnumerable<Role>> GetRoles(Guid userId)
    {
        var roles = await _dbContext.Roles
            .Where(r => r.GroupRoles.Any(gr => gr.UserId == userId))
            .Include(r => r.GroupRoles.Where(gr => gr.UserId == userId))
            .Include(r => r.Permissions)
            .ToListAsync();

        return _mapper.Map<IEnumerable<Role>>(roles);
    }
}
