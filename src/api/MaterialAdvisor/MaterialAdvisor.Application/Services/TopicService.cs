using AutoMapper;

using MaterialAdvisor.Application.Models.Editable;
using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Services;

public class TopicService(MaterialAdvisorContext dbContext, IUserProvider tenantService, IMapper mapper) : ITopicService
{
    public async Task<EditableTopic> Create(EditableTopic topicModel)
    {
        var topicEntity = mapper.Map<TopicEntity>(topicModel);
        await EnrichWithUserId(topicEntity);

        var createdTopic = await dbContext.AddAsync(topicEntity);
        await dbContext.SaveChangesAsync();

        var createdTopicModel = mapper.Map<EditableTopic>(topicModel);
        return createdTopicModel;
    }

    public async Task<EditableTopic> Update(EditableTopic topicModel)
    {
        var topicEntity = mapper.Map<TopicEntity>(topicModel);
        await EnrichWithUserId(topicEntity);

        var updatedTopic = dbContext.Update(topicEntity);
        await dbContext.SaveChangesAsync();

        var updatedTopicModel = mapper.Map<EditableTopic>(updatedTopic);
        return updatedTopicModel;
    }

    private async Task EnrichWithUserId(TopicEntity topic)
    {
        var userTenant = await tenantService.GetUser();
        topic.UserId = userTenant.UserId;
    }
}
