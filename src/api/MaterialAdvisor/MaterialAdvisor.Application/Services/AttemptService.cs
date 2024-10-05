using AutoMapper;

using MaterialAdvisor.Application.Exceptions;
using MaterialAdvisor.Application.Services.Abstraction;
using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace MaterialAdvisor.Application.Services;

public class AttemptService(MaterialAdvisorContext _dbContext, IUserProvider _tenantService, IMapper _mapper) : IAttemptService
{
    public async Task<TModel> Create<TModel>(TModel model)
    {
        var entityToCreate = await MapToEntity(model);
        entityToCreate.IsSubmitted = false;

        var knowledgeCheck = await _dbContext.KnowledgeChecks
            .Where(kc => kc.Id == entityToCreate.KnowledgeCheckId)
            .Include(kc => kc.Attempts.Where(a => a.UserId == entityToCreate.UserId))
            .SingleAsync();

        if (knowledgeCheck.Attempts.Count >= knowledgeCheck.MaxAttempts)
        {
            throw new ActionNotAllowedException(ErrorCode.CannotCreateMoreThanMaxAttempts);
        }

        if (knowledgeCheck.EndDate.HasValue && knowledgeCheck.EndDate.Value <= entityToCreate.StartDate)
        {
            throw new ActionNotAllowedException(ErrorCode.CannotAnswerAfterEndDate);
        }

        var createdEntity = await _dbContext.Attempts.AddAsync(entityToCreate);
        await _dbContext.SaveChangesAsync();
        return MapToModel<TModel>(createdEntity.Entity);
    }

    public async Task<bool> SetIsSubmit(Guid id)
    {
        var updated = await _dbContext.Attempts
            .Where(a => a.Id == id)
            .ExecuteUpdateAsync(a => a.SetProperty(p => p.IsSubmitted, true));

        return updated != 0;
    }

    private async Task<AttemptEntity> MapToEntity<TModel>(TModel model)
    {
        var topicEntity = _mapper.Map<AttemptEntity>(model);
        var user = await _tenantService.GetUser();
        topicEntity.UserId = user.Id;
        return topicEntity;
    }

    private TModel MapToModel<TModel>(AttemptEntity entity)
    {
        var topicsModel = _mapper.Map<TModel>(entity);
        return topicsModel;
    }
}