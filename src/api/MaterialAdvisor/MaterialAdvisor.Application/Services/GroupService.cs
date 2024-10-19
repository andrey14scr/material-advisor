using AutoMapper;

using MaterialAdvisor.Application.Exceptions;
using MaterialAdvisor.Application.Models.Users;
using MaterialAdvisor.Application.Services.Abstraction;
using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;

using Microsoft.EntityFrameworkCore;

using System.Linq;

namespace MaterialAdvisor.Application.Services;

public class GroupService(MaterialAdvisorContext _dbContext, IUserProvider _userProvider, IMapper _mapper) : IGroupService
{
    public async Task<TModel> Create<TModel>(TModel model)
    {
        var user = await _userProvider.GetUser();
        var entity = _mapper.Map<GroupEntity>(model);
        entity.OwnerId = user.Id;

        var createdEntity = await _dbContext.Groups.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        var createdModel = _mapper.Map<TModel>(createdEntity.Entity);
        return createdModel;
    }

    public async Task<bool> Delete(Guid id)
    {
        var deleted = await _dbContext.Groups.Where(g => g.Id == id).ExecuteDeleteAsync();
        return deleted != 0;
    }

    public async Task<TModel> Get<TModel>(Guid id)
    {
        var entity = await _dbContext.Groups.Include(g => g.Users).AsNoTracking().SingleAsync(g => g.Id == id);
        var model = _mapper.Map<TModel>(entity);
        return model;
    }

    public async Task<IList<TModel>> GetAsMember<TModel>()
    {
        var user = await _userProvider.GetUser();
        var entities = await _dbContext.Groups
            .Include(g => g.Users)
            .Where(g => g.Users.Any(u => u.Id == user.Id))
            .AsNoTracking()
            .ToListAsync();

        var entitiesModel = _mapper.Map<IList<TModel>>(entities);
        return entitiesModel;
    }

    public async Task<IList<TModel>> GetAsOwner<TModel>()
    {
        var user = await _userProvider.GetUser();
        var entities = await _dbContext.Groups.AsNoTracking().Where(g => g.OwnerId == user.Id).ToListAsync();
        var entitiesModel = _mapper.Map<IList<TModel>>(entities);
        return entitiesModel;
    }

    public async Task<IList<TModel>> Search<TModel>(string input)
    {
        var entities = await _dbContext.Groups
            .AsNoTracking()
            .Where(g => g.Name.ToLower().Contains(input.ToLower()))
            .OrderBy(g => g.Name)
            .ToListAsync();
        var entitiesModel = _mapper.Map<IList<TModel>>(entities);
        return entitiesModel;
    }

    public async Task<TModel> Update<TModel>(TModel model)
    {
        var entity = _mapper.Map<GroupEntity>(model);
        var existingEntity = await _dbContext.Groups.AsNoTracking().SingleAsync(g => g.Id == entity.Id);

        var user = await _userProvider.GetUser();
        if (existingEntity.OwnerId != user.Id)
        {
            throw new ActionNotAllowedException(ErrorCode.CannotChangeNotOwnedEntity);
        }
        entity.OwnerId = user.Id;

        var updatedEntity = _dbContext.Groups.Update(entity);
        await _dbContext.SaveChangesAsync();

        var updatedModel = _mapper.Map<TModel>(updatedEntity.Entity);
        return updatedModel;
    }
}