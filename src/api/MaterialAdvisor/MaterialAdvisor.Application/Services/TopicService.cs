using AutoMapper;

using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace MaterialAdvisor.Application.Services;

public class TopicService(MaterialAdvisorContext dbContext, IUserProvider tenantService, IMapper mapper) : ITopicService
{
    public async Task<TModel> Create<TModel>(TModel topicModel)
    {
        var topicEntity = await MapToEntity(topicModel);
        var createdTopic = await CreateTopic(topicEntity);
        return MapToEditableModel<TModel>(createdTopic);
    }

    public async Task<bool> Delete(Guid topicId)
    {
        var topicToDelete = await GetFullTopic().SingleAsync(t => t.Id == topicId);
        var deleted = await DeleteTopic(topicToDelete);
        return deleted != 0;
    }

    public async Task<TModel> Get<TModel>(Guid topicId)
    {
        var topic = await GetFullTopic().AsNoTracking().SingleAsync(t => t.Id == topicId);
        return MapToEditableModel<TModel>(topic);
    }

    public async Task<IList<TModel>> Get<TModel>()
    {
        var topic = await GetFullTopic().AsNoTracking().ToListAsync();
        var topicsModel = mapper.Map<IList<TModel>>(topic);
        return topicsModel;
    }

    public async Task<TModel> Update<TModel>(TModel topicModel)
    {
        var topicEntity = await MapToEntity(topicModel);
        var existingTopic = await GetFullTopic().SingleAsync(t => t.Id == topicEntity.Id);

        using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            await DeleteTopic(existingTopic);
            var createdTopic = await CreateTopic(topicEntity);
            return MapToEditableModel<TModel>(createdTopic);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task<TopicEntity> MapToEntity<TModel>(TModel topic)
    {
        var topicEntity = mapper.Map<TopicEntity>(topic);
        await EnrichWithUserId(topicEntity);
        return topicEntity;
    }

    private TModel MapToEditableModel<TModel>(TopicEntity topic)
    {
        var topicsModel = mapper.Map<TModel>(topic);
        return topicsModel;
    }

    private async Task<int> DeleteTopic(TopicEntity topic)
    {
        dbContext.Topics.Remove(topic);
        RemoveUnusedLanguageTexts();
        var deleted = await dbContext.SaveChangesAsync();
        return deleted;
    }

    private async Task<TopicEntity> CreateTopic(TopicEntity topic)
    {
        var createdTopic = await dbContext.Topics.AddAsync(topic);
        await dbContext.SaveChangesAsync();
        return createdTopic.Entity;
    }

    private void RemoveUnusedLanguageTexts()
    {
        var textsToDelete = dbContext.ChangeTracker.Entries<LanguageTextEntity>()
            .Where(lt => !lt.Entity.AnswerGroupId.HasValue
                && !lt.Entity.AnswerId.HasValue
                && !lt.Entity.QuestionId.HasValue
                && !lt.Entity.TopicId.HasValue)
            .Select(lt => lt.Entity)
            .ToList();

        dbContext.LanguageTexts.RemoveRange(textsToDelete);
    }

    private IIncludableQueryable<TopicEntity, ICollection<LanguageTextEntity>> GetFullTopic()
    {
        return dbContext.Topics
                    .Include(t => t.Questions).ThenInclude(q => q.AnswerGroups).ThenInclude(ag => ag.Answers).ThenInclude(a => a.Texts)
                    .Include(t => t.Questions).ThenInclude(q => q.AnswerGroups).ThenInclude(a => a.Texts)
                    .Include(t => t.Questions).ThenInclude(a => a.Texts)
                    .Include(a => a.Texts);
    }

    private async Task EnrichWithUserId(TopicEntity topic)
    {
        var userTenant = await tenantService.GetUser();
        topic.UserId = userTenant.UserId;
    }
}
