using AutoMapper;

using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace MaterialAdvisor.Application.Services;

public class KnowledgeCheckService(MaterialAdvisorContext _dbContext, IUserProvider _tenantService, IMapper _mapper) 
    : IKnowledgeCheckService
{
    public async Task<TModel> Create<TModel>(TModel model)
    {
        var entityToCreate = await MapToEntity(model);
        var createdEntity = await CreateAndSave(entityToCreate);
        var createdModel = MapToModel<TModel>(createdEntity);
        return createdModel;
    }

    public async Task<bool> Delete(Guid id)
    {
        var entityToDelete = await GetFullEntityToDelete().SingleAsync(t => t.Id == id);
        var deleted = await DeleteAndSave(entityToDelete);
        return deleted != 0;
    }

    public async Task<TModel> Get<TModel>(Guid id)
    {
        var entity = await GetFullEntity().AsNoTracking().SingleAsync(t => t.Id == id);
        var model = MapToModel<TModel>(entity);
        return model;
    }

    public async Task<IList<TModel>> Get<TModel>()
    {
        var entities = await GetFullEntity().AsNoTracking().ToListAsync();
        var entitiesModel = _mapper.Map<IList<TModel>>(entities);
        return entitiesModel;
    }

    public async Task<IList<TModel>> GetByGroup<TModel>(Guid groupId)
    {
        var entities = await GetFullEntity().Where(kc => kc.Groups.Any(g => g.Id == groupId)).AsNoTracking().ToListAsync();
        var entitiesModel = _mapper.Map<IList<TModel>>(entities);
        return entitiesModel;
    }

    public async Task<TModel> Update<TModel>(TModel model)
    {
        var entityToUpdate = await MapToEntity(model);
        var existingEntity = await GetFullEntityToDelete().SingleAsync(t => t.Id == entityToUpdate.Id);

        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            await DeleteAndSave(existingEntity);
            var createdEntity = await CreateAndSave(entityToUpdate);
            await transaction.CommitAsync();
            return MapToModel<TModel>(createdEntity);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task<KnowledgeCheckEntity> MapToEntity<TModel>(TModel model)
    {
        var topicEntity = _mapper.Map<KnowledgeCheckEntity>(model);
        var user = await _tenantService.GetUser();
        topicEntity.OwnerId = user.UserId;
        return topicEntity;
    }

    private TModel MapToModel<TModel>(KnowledgeCheckEntity entity)
    {
        var topicsModel = _mapper.Map<TModel>(entity);
        return topicsModel;
    }

    private async Task<int> DeleteAndSave(KnowledgeCheckEntity entity)
    {
        _dbContext.KnowledgeChecks.Remove(entity);
        var deleted = await _dbContext.SaveChangesAsync();
        return deleted;
    }

    private async Task<KnowledgeCheckEntity> CreateAndSave(KnowledgeCheckEntity entity)
    {
        var createdTopic = await _dbContext.KnowledgeChecks.AddAsync(entity);
        IgnoreGroups();
        await _dbContext.SaveChangesAsync();
        return createdTopic.Entity;
    }

    private IQueryable<KnowledgeCheckEntity> GetFullEntity()
    {
        return _dbContext.KnowledgeChecks.Include(kc => kc.Groups);
    }

    private IQueryable<KnowledgeCheckEntity> GetFullEntityToDelete()
    {
        return GetFullEntity()
            .Include(kc => kc.Attempts)
            .ThenInclude(a => a.SubmittedAnswers);
    }

    private void IgnoreGroups()
    {
        var groupsToIgnore = _dbContext.ChangeTracker.Entries<GroupEntity>().ToList();

        foreach (var groupToIgnore in groupsToIgnore)
        {
            groupToIgnore.State = EntityState.Unchanged;
        }
    }
}