using AutoMapper;

using MaterialAdvisor.Application.Exceptions;
using MaterialAdvisor.Application.Models.KnowledgeChecks;
using MaterialAdvisor.Application.Services.Abstraction;
using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;
using MaterialAdvisor.Data.Extensions;

using Microsoft.EntityFrameworkCore;

namespace MaterialAdvisor.Application.Services;

public class AttemptService(MaterialAdvisorContext _dbContext, IUserProvider _tenantService, IMapper _mapper) : IAttemptService
{
    public async Task<TModel?> GetLast<TModel>(Guid knowledgeCheckId)
    {
        var user = await _tenantService.GetUser();
        var attempt = await _dbContext.Attempts
            .Include(a => a.KnowledgeCheck)
            .Include(a => a.SubmittedAnswers)
            .OrderByDescending(a => a.StartDate)
            .FirstOrDefaultAsync(a => a.UserId == user.Id && a.KnowledgeCheckId == knowledgeCheckId);

        if (attempt is null || attempt.IsFinished())
        {
            throw new NotFoundException();
        }

        return MapToModel<TModel>(attempt);
    }

    public async Task<TModel> Create<TModel>(CreateAttempt model)
    {
        var user = await _tenantService.GetUser();

        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var knowledgeCheck = await _dbContext.KnowledgeChecks
                .Where(kc => kc.Id == model.KnowledgeCheckId)
                .Include(kc => kc.Attempts.Where(a => a.UserId == user.Id))
                .SingleAsync();

            if (knowledgeCheck.Attempts.Count >= knowledgeCheck.MaxAttempts)
            {
                throw new ActionNotAllowedException(ErrorCode.CannotCreateMoreThanMaxAttempts);
            }

            var now = DateTime.UtcNow;
            if (knowledgeCheck.EndDate.HasValue && knowledgeCheck.EndDate.Value <= now)
            {
                throw new ActionNotAllowedException(ErrorCode.CannotAnswerAfterEndDate);
            }

            var entityToCreate = new AttemptEntity
            {
                IsSubmitted = false,
                KnowledgeCheckId = model.KnowledgeCheckId,
                StartDate = now,
                UserId = user.Id,
            };

            var createdEntity = await _dbContext.Attempts.AddAsync(entityToCreate);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return MapToModel<TModel>(createdEntity.Entity);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
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
        var entity = _mapper.Map<AttemptEntity>(model);
        var user = await _tenantService.GetUser();
        entity.UserId = user.Id;
        return entity;
    }

    private TModel MapToModel<TModel>(AttemptEntity entity)
    {
        var model = _mapper.Map<TModel>(entity);
        return model;
    }
}