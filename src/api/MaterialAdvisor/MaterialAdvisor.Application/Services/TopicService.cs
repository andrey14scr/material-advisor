using AutoMapper;

using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace MaterialAdvisor.Application.Services;

public class TopicService(MaterialAdvisorContext _dbContext, IUserProvider _tenantService, IMapper _mapper) : BaseService<TopicEntity>, ITopicService
{
    public async Task<IList<TModel>> Get<TModel>()
    {
        var entities = await GetFullEntity().AsNoTracking().ToListAsync();
        var entitiesModel = _mapper.Map<IList<TModel>>(entities);
        return entitiesModel;
    }

    protected override IQueryable<TopicEntity> GetFullEntity()
    {
        return _dbContext.Topics
                    .Include(t => t.Questions).ThenInclude(q => q.AnswerGroups).ThenInclude(ag => ag.Answers).ThenInclude(a => a.Texts)
                    .Include(t => t.Questions).ThenInclude(q => q.AnswerGroups).ThenInclude(a => a.Texts)
                    .Include(t => t.Questions).ThenInclude(a => a.Texts)
                    .Include(a => a.Texts);
    }

    protected override async Task<TopicEntity> MapToEntity<TModel>(TModel model)
    {
        var topicEntity = _mapper.Map<TopicEntity>(model);
        var user = await _tenantService.GetUser();
        topicEntity.OwnerId = user.UserId;
        return topicEntity;
    }

    protected override async Task<TModel> MapToModel<TModel>(TopicEntity entity)
    {
        var topicsModel = _mapper.Map<TModel>(entity);
        return await Task.FromResult(topicsModel);
    }

    protected override DbSet<TopicEntity> GetDbSet() => _dbContext.Topics;

    protected override MaterialAdvisorContext GetDbContext() => _dbContext;
}
