using AutoMapper;

using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace MaterialAdvisor.Application.Services;

public class TopicService(MaterialAdvisorContext _dbContext, IUserProvider _tenantService, IMapper _mapper) : ITopicService
{
    public async Task<TModel> Create<TModel>(TModel model)
    {
        var entityToCreate = await MapToEntity(model);
        var createdEntity = await CreateAndSave(entityToCreate);
        var createdModel = await MapToModel<TModel>(createdEntity);
        return createdModel;
    }

    public async Task<bool> Delete(Guid id)
    {
        var entityToDelete = await GetFullEntity().SingleAsync(t => t.Id == id);
        var deleted = await DeleteAndSave(entityToDelete);
        return deleted != 0;
    }

    public async Task<TModel> Get<TModel>(Guid id)
    {
        var entity = await GetFullEntity().AsNoTracking().SingleAsync(t => t.Id == id);
        var model = await MapToModel<TModel>(entity);
        return model;
    }

    public async Task<IList<TModel>> Get<TModel>()
    {
        var entities = await GetFullEntity().AsNoTracking().ToListAsync();
        var entitiesModel = _mapper.Map<IList<TModel>>(entities);
        return entitiesModel;
    }

    public async Task<TModel> Update<TModel>(TModel model)
    {
        var entityToUpdate = await MapToEntity(model);
        var existingEntity = await GetFullEntity().SingleAsync(t => t.Id == entityToUpdate.Id);

        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            await DeleteAndSave(existingEntity);
            var createdEntity = await CreateAndSave(entityToUpdate);
            return await MapToModel<TModel>(createdEntity);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task<int> DeleteAndSave(TopicEntity entity)
    {
        _dbContext.Topics.Remove(entity);
        RemoveUnusedLanguageTexts();
        var deleted = await _dbContext.SaveChangesAsync();
        return deleted;
    }

    private async Task<TopicEntity> CreateAndSave(TopicEntity entity)
    {
        var createdTopic = await _dbContext.Topics.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return createdTopic.Entity;
    }

    private async Task<TopicEntity> MapToEntity<TModel>(TModel model)
    {
        var topicEntity = _mapper.Map<TopicEntity>(model);
        var user = await _tenantService.GetUser();
        topicEntity.OwnerId = user.UserId;
        return topicEntity;
    }

    private async Task<TModel> MapToModel<TModel>(TopicEntity entity)
    {
        var topicsModel = _mapper.Map<TModel>(entity);
        return await Task.FromResult(topicsModel);
    }

    private IQueryable<TopicEntity> GetFullEntity()
    {
        return _dbContext.Topics
                    .Include(t => t.Questions).ThenInclude(q => q.AnswerGroups).ThenInclude(ag => ag.Answers).ThenInclude(a => a.Texts)
                    .Include(t => t.Questions).ThenInclude(q => q.AnswerGroups).ThenInclude(a => a.Texts)
                    .Include(t => t.Questions).ThenInclude(a => a.Texts)
                    .Include(a => a.Texts);
    }

    private void RemoveUnusedLanguageTexts()
    {
        var textsToDelete = _dbContext.ChangeTracker.Entries<LanguageTextEntity>()
            .Where(lt => lt.State == EntityState.Modified
                && !lt.Entity.AnswerGroupId.HasValue
                && !lt.Entity.AnswerId.HasValue
                && !lt.Entity.QuestionId.HasValue
            && !lt.Entity.TopicId.HasValue)
            .Select(lt => lt.Entity)
            .ToList();

        _dbContext.LanguageTexts.RemoveRange(textsToDelete);
    }
}
