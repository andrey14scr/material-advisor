using AutoMapper;

using MaterialAdvisor.Application.Exceptions;
using MaterialAdvisor.Application.Services.Abstraction;
using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Extensions;
using MaterialAdvisor.Data.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

            return MapToModel<TModel>(entity);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IList<TModel>> GetUnverifiedAnswers<TModel>(Guid knowledgeCheckId)
    {
        var typesToVerify = Data.Constants.QuestionTypesRequiredVerification;

        var entities = await _dbContext.SubmittedAnswers
            .Include(sa => sa.AnswerGroup).ThenInclude(ag => ag.Answers).ThenInclude(ag => ag.Content)
            .Include(sa => sa.AnswerGroup).ThenInclude(ag => ag.Question).ThenInclude(q => q.Topic).ThenInclude(t => t.Name)
            .Include(sa => sa.AnswerGroup).ThenInclude(ag => ag.Question).ThenInclude(ag => ag.Content)
            .Include(sa => sa.AnswerGroup).ThenInclude(ag => ag.Content)
            .Include(sa => sa.Attempt).ThenInclude(a => a.User)
            .Include(sa => sa.Attempt).ThenInclude(a => a.KnowledgeCheck)
            .Include(sa => sa.VerifiedAnswers.Where(sa => !sa.IsManual))
            .Where(sa => typesToVerify.Contains(sa.AnswerGroup.Question.Type) &&
                sa.Attempt.KnowledgeCheckId == knowledgeCheckId &&
                !sa.Attempt.IsCanceled &&
                sa.Value != null && sa.Value.Length != 0 &&
                !sa.VerifiedAnswers.Any(va => va.IsManual))
            .OrderByDescending(sa => sa.Attempt.StartDate)
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

    private TModel MapToModel<TModel>(SubmittedAnswerEntity entity)
    {
        var model = _mapper.Map<TModel>(entity);
        return model;
    }
}
