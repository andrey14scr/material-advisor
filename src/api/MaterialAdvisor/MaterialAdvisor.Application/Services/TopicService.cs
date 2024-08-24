using AutoMapper;

using MaterialAdvisor.Application.Models.Editable;
using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace MaterialAdvisor.Application.Services;

public class TopicService(MaterialAdvisorContext dbContext, IUserProvider tenantService, IMapper mapper) : ITopicService
{
    public async Task<EditableTopic> Create(EditableTopic topicModel)
    {
        var topicEntity = mapper.Map<TopicEntity>(topicModel);
        await EnrichWithUserId(topicEntity);

        var createdTopic = await dbContext.Topics.AddAsync(topicEntity);
        await dbContext.SaveChangesAsync();

        var createdTopicModel = mapper.Map<EditableTopic>(createdTopic.Entity);
        return createdTopicModel;
    }

    public async Task<bool> Delete(Guid topicId)
    {
        var topicToDelete = await GetFullTopic(dbContext).SingleOrDefaultAsync(t => t.Id == topicId);

        dbContext.Topics.Remove(topicToDelete!);

        var textsToDelete = dbContext.ChangeTracker.Entries<LanguageTextEntity>()
            .Where(lt => !lt.Entity.AnswerGroupId.HasValue
                && !lt.Entity.AnswerId.HasValue
                && !lt.Entity.QuestionId.HasValue
                && !lt.Entity.TopicId.HasValue)
            .Select(lt => lt.Entity)
            .ToList();

        dbContext.LanguageTexts.RemoveRange(textsToDelete);

        var deleted = await dbContext.SaveChangesAsync();

        return deleted != 0;
    }

    public async Task<EditableTopic> Get(Guid topicId)
    {
        var topic = await GetFullTopic(dbContext).AsNoTracking().SingleOrDefaultAsync(t => t.Id == topicId);
        var topicModel = mapper.Map<EditableTopic>(topic);
        return topicModel;
    }

    public async Task<EditableTopic> Update(EditableTopic topicModel)
    {
        var topicEntity = mapper.Map<TopicEntity>(topicModel);
        await EnrichWithUserId(topicEntity);

        var updatedTopic = dbContext.Topics.Update(topicEntity);
        await dbContext.SaveChangesAsync();

        var updatedTopicModel = mapper.Map<EditableTopic>(updatedTopic);
        return updatedTopicModel;
    }

    private static IIncludableQueryable<TopicEntity, ICollection<LanguageTextEntity>> GetFullTopic(MaterialAdvisorContext dbContext)
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
