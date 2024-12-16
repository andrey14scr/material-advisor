using AutoMapper;

using MaterialAdvisor.Application.Services.Abstraction;
using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace MaterialAdvisor.Application.Services;

public class KnowledgeCheckService(MaterialAdvisorContext _dbContext, IUserProvider _userProvider, IMapper _mapper) 
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

    public async Task<bool> HasVerifiedAttemts(Guid id)
    {
        var verifictionTypes = Data.Constants.QuestionTypesRequiredVerification;
        var hasVerifiedAttemts = await _dbContext.KnowledgeChecks
            .AnyAsync(kc => kc.Id == id && !kc.Attempts.Any(a => 
                a.SubmittedAnswers.Any(sa => verifictionTypes.Contains(sa.AnswerGroup.Question.Type) && 
                    !sa.VerifiedAnswers.Any(va => va.IsManual))));
        return hasVerifiedAttemts;
    }

    public async Task<TModel> Get<TModel>(Guid id)
    {
        var entity = await GetFullEntity().AsNoTracking().SingleAsync(t => t.Id == id);
        var model = MapToModel<TModel>(entity);
        return model;
    }

    public async Task<IList<TModel>> Get<TModel>()
    {
        var user = await _userProvider.GetUser();

        var entities = await _dbContext.Topics
            .Where(t => t.KnowledgeChecks.Any(kc => kc.Groups.Any(g => g.Users.Any(u => u.Id == user.Id))))
            .Include(t => t.Name)
            .Include(t => t.Questions)
            .Include(t => t.KnowledgeChecks)
                .ThenInclude(kc => kc.Attempts.Where(a => !a.IsCanceled && a.UserId == user.Id))
                .ThenInclude(a => a.SubmittedAnswers)
                .ThenInclude(a => a.VerifiedAnswers)
            .Include(t => t.KnowledgeChecks)
                .ThenInclude(kc => kc.Attempts.Where(a => !a.IsCanceled && a.UserId == user.Id))
                .ThenInclude(a => a.SubmittedAnswers)
                .ThenInclude(sa => sa.AnswerGroup)
                .ThenInclude(sa => sa.Question)
            .Include(t => t.KnowledgeChecks)
                .ThenInclude(kc => kc.Attempts.Where(a => !a.IsCanceled && a.UserId == user.Id))
                .ThenInclude(a => a.SubmittedAnswers)
                .ThenInclude(sa => sa.AnswerGroup)
                .ThenInclude(sa => sa.Answers)
            .Include(t => t.KnowledgeChecks)
                .ThenInclude(kc => kc.Groups)
                .ThenInclude(kc => kc.Users)
            .AsNoTracking()
            .ToListAsync();

        var entitiesModel = _mapper.Map<IList<TModel>>(entities);
        return entitiesModel;
    }

    public async Task<IList<TModel>> GetByGroup<TModel>(Guid groupId)
    {
        var entities = await GetFullEntity().Where(kc => kc.Groups.Any(g => g.Id == groupId)).AsNoTracking().ToListAsync();
        var entitiesModel = _mapper.Map<IList<TModel>>(entities);
        return entitiesModel;
    }

    public async Task<IList<TModel>> GetByTopic<TModel>(Guid topicId)
    {
        var entities = await GetFullEntity().Where(kc => kc.TopicId == topicId).AsNoTracking().ToListAsync();
        var entitiesModel = _mapper.Map<IList<TModel>>(entities);
        return entitiesModel;
    }

    public async Task<IDictionary<Guid, int>> GetAttemptsCount(IEnumerable<Guid> topicIds)
    {
        var entities = await _dbContext.KnowledgeChecks
            .Where(kc => topicIds.Contains(kc.TopicId))
            .Include(kc => kc.Attempts)
            .ToDictionaryAsync(ks => ks.Id, es => es.Attempts.Count);

        return entities;
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
        var entity = _mapper.Map<KnowledgeCheckEntity>(model);
        var user = await _userProvider.GetUser();
        return entity;
    }

    private TModel MapToModel<TModel>(KnowledgeCheckEntity entity)
    {
        var model = _mapper.Map<TModel>(entity);
        return model;
    }

    private async Task<int> DeleteAndSave(KnowledgeCheckEntity entity)
    {
        _dbContext.KnowledgeChecks.Remove(entity);
        var deleted = await _dbContext.SaveChangesAsync();
        return deleted;
    }

    private async Task<KnowledgeCheckEntity> CreateAndSave(KnowledgeCheckEntity entity)
    {
        var created = await _dbContext.KnowledgeChecks.AddAsync(entity);
        IgnoreGroups();
        await _dbContext.SaveChangesAsync();
        return created.Entity;
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