using AutoMapper;

using MaterialAdvisor.Application.Exceptions;
using MaterialAdvisor.Application.Services.Abstraction;
using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Extensions;
using MaterialAdvisor.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace MaterialAdvisor.Application.Services;

public class SubmittedAnswerService(MaterialAdvisorContext _dbContext, IUserProvider _tenantService, IMapper _mapper) : ISubmittedAnswerService
{
    public async Task<TModel> CreateOrUpdate<TModel>(TModel model)
    {
        var user = await _tenantService.GetUser();
        var entity = MapToEntity(model);

        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var existingAttempt = await _dbContext.Attempts
                .Include(a => a.KnowledgeCheck)
                .AsNoTracking()
                .SingleAsync(a => a.Id == entity.AttemptId);

            if (existingAttempt?.IsFinished() == true)
            {
                throw new ActionNotSupportedException(ErrorCode.CannotChangeSubmittedAttempt);
            }

            if (existingAttempt?.UserId != user.Id)
            {
                throw new ActionNotAllowedException(ErrorCode.CannotAnswerForAnotherUser);
            }

            var existingSubmittedAnswer = await _dbContext.SubmittedAnswers
                .AsNoTracking()
                .SingleOrDefaultAsync(sa => sa.AnswerGroupId == entity.AnswerGroupId && sa.AttemptId == entity.AttemptId);

            if (existingSubmittedAnswer is null)
            {
                await _dbContext.SubmittedAnswers.AddAsync(entity);
            }
            else
            {
                _dbContext.SubmittedAnswers.Update(entity);
            }
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return model;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IList<TModel>> GetUnverifiedAnswers<TModel>(Guid knowledgeCheckId)
    {
        var entities = await _dbContext.SubmittedAnswers
            .Include(sa => sa.AnswerGroup).ThenInclude(ag => ag.Answers).ThenInclude(ag => ag.Content)
            .Include(sa => sa.AnswerGroup).ThenInclude(ag => ag.Question).ThenInclude(q => q.Topic).ThenInclude(t => t.Name)
            .Include(sa => sa.AnswerGroup).ThenInclude(ag => ag.Question).ThenInclude(ag => ag.Content)
            .Include(sa => sa.AnswerGroup).ThenInclude(ag => ag.Content)
            .Include(sa => sa.Attempt).ThenInclude(a => a.User)
            .Include(sa => sa.Attempt).ThenInclude(a => a.KnowledgeCheck)
            .Where(sa => sa.AnswerGroup.Question.Type == Data.Enums.QuestionType.OpenText &&
                sa.Attempt.KnowledgeCheckId == knowledgeCheckId &&
                sa.Value != null && sa.Value.Length != 0 &&
                !sa.VerifiedAnswers.Any(va => va.IsManual))
            .OrderBy(sa => sa.Attempt.KnowledgeCheck.StartDate)
            .ThenBy(sa => sa.Attempt.StartDate)
            .ToListAsync();

        var models = _mapper.Map<IList<TModel>>(entities);
        return models;
    }

    public async Task<bool> VerifyAnswer<TModel>(TModel verifiedAnswer)
    {
        var entity = _mapper.Map<VerifiedAnswerEntity>(verifiedAnswer);
        entity.IsManual = true;

        await _dbContext.VerifiedAnswers.AddAsync(entity);
        var created = await _dbContext.SaveChangesAsync();

        return created != 0;
    }

    private SubmittedAnswerEntity MapToEntity<TModel>(TModel model)
    {
        var entity = _mapper.Map<SubmittedAnswerEntity>(model);
        return entity;
    }
}
