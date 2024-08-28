using AutoMapper;

using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace MaterialAdvisor.Application.Services;

public class KnowledgeCheckService(MaterialAdvisorContext _dbContext, IUserProvider _tenantService, IMapper _mapper) 
    : BaseService<KnowledgeCheckEntity>, IKnowledgeCheckService
{
    public async Task<IList<TModel>> Get<TModel>()
    {
        var entities = await GetFullEntity().AsNoTracking().ToListAsync();
        var entitiesModel = _mapper.Map<IList<TModel>>(entities);
        return entitiesModel;
    }

    public async Task<IList<TModel>> GetByGroup<TModel>(Guid groupId)
    {
        var entities = await GetFullEntity().Where(kc => kc.GroupId == groupId).AsNoTracking().ToListAsync();
        var entitiesModel = _mapper.Map<IList<TModel>>(entities);
        return entitiesModel;
    }

    protected override async Task<KnowledgeCheckEntity> MapToEntity<TModel>(TModel model)
    {
        var topicEntity = _mapper.Map<KnowledgeCheckEntity>(model);
        var user = await _tenantService.GetUser();
        topicEntity.OwnerId = user.UserId;
        return await Task.FromResult(topicEntity);
    }

    protected override async Task<TModel> MapToModel<TModel>(KnowledgeCheckEntity entity)
    {
        var topicsModel = _mapper.Map<TModel>(entity);
        return await Task.FromResult(topicsModel);
    }

    protected override IQueryable<KnowledgeCheckEntity> GetFullEntity() => _dbContext.KnowledgeChecks;

    protected override IQueryable<KnowledgeCheckEntity> GetFullEntityForDelete() => _dbContext.KnowledgeChecks.Include(kc => kc.SubmittedAnswers);

    protected override MaterialAdvisorContext GetDbContext() => _dbContext;

    protected override DbSet<KnowledgeCheckEntity> GetDbSet() => _dbContext.KnowledgeChecks;
}