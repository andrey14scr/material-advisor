using AutoMapper;

using MaterialAdvisor.Application.Exceptions;
using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace MaterialAdvisor.Application.Services;

public class TopicService(MaterialAdvisorContext _dbContext, IUserProvider _userService, IMapper _mapper) : ITopicService
{
    public async Task<TModel> Create<TModel>(TModel model)
    {
        var entityToCreate = await MapToEntity(model);
        entityToCreate.Version = 1;
        var createdEntity = await CreateAndSave(entityToCreate);
        var createdModel = MapToModel<TModel>(createdEntity);
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
        var model = MapToModel<TModel>(entity);
        return model;
    }

    public async Task<IList<TModel>> Get<TModel>()
    {
        var user = await _userService.GetUser();
        var entities = await _dbContext.Topics
            .Where(t => t.OwnerId == user.Id || t.KnowledgeChecks.Any(kc => kc.Groups.Any(g => g.Users.Any(u => u.Id == user.Id))))
            .Include(t => t.Owner)
            .Include(t => t.Texts)
            .Include(t => t.KnowledgeChecks).ThenInclude(kc => kc.Attempts.Where(a => a.UserId == user.Id))
            .Include(t => t.KnowledgeChecks).ThenInclude(kc => kc.Groups).ThenInclude(kc => kc.Users)
            .AsNoTracking()
            .ToListAsync();
        var entitiesModel = _mapper.Map<IList<TModel>>(entities);
        return entitiesModel;
    }

    public async Task<TModel> Update<TModel>(TModel model)
    {
        var entityToUpdate = await MapToEntity(model);
        var existingEntity = await GetFullEntity().SingleAsync(t => t.Id == entityToUpdate.Id);

        var maxVersion = await _dbContext.Topics
            .Where(t => t.PersistentId == existingEntity.PersistentId)
            .MaxAsync(t => t.Version);

        if (entityToUpdate.Version != maxVersion)
        {
            throw new ActionNotAllowedException(ErrorCode.TopicVersionIsOutdated);
        }
        entityToUpdate.PersistentId = existingEntity.PersistentId;

        var now = DateTime.UtcNow;
        var hasStartedKnowledgeCheck = await _dbContext.KnowledgeChecks
            .Where(kc => kc.TopicId == entityToUpdate.Id && kc.StartDate <= now)
            .AnyAsync();

        if (hasStartedKnowledgeCheck)
        {
            entityToUpdate.Id = Guid.NewGuid();
            entityToUpdate.Version++;
            var createdEntity = await CreateAndSave(entityToUpdate);
            return MapToModel<TModel>(createdEntity);
        }
        else 
        {
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
        var isNewEntity = entity.Id == default;
        var createdTopic = await _dbContext.Topics.AddAsync(entity);
        if (isNewEntity) 
        {
            createdTopic.Entity.PersistentId = createdTopic.Entity.Id;
        }
        await _dbContext.SaveChangesAsync();
        return createdTopic.Entity;
    }

    private async Task<TopicEntity> MapToEntity<TModel>(TModel model)
    {
        var topicEntity = _mapper.Map<TopicEntity>(model);
        var user = await _userService.GetUser();
        topicEntity.OwnerId = user.Id;
        return topicEntity;
    }

    private TModel MapToModel<TModel>(TopicEntity entity)
    {
        var topicsModel = _mapper.Map<TModel>(entity);
        return topicsModel;
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
