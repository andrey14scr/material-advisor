using AutoMapper;

using MaterialAdvisor.Application.Exceptions;
using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace MaterialAdvisor.Application.Services;

public class SubmittedAnswerService(MaterialAdvisorContext _dbContext, IUserProvider _tenantService, IMapper _mapper) : ISubmittedAnswerService
{
    public async Task<TModel> CreateOrUpdate<TModel>(TModel model)
    {
        var user = await _tenantService.GetUser();
        var entity = MapToEntity(model);
        var existingAttempt = await _dbContext.Attempts
            .Include(a => a.SubmittedAnswers)
            .Where(a => a.Id == entity.AttemptId && a.SubmittedAnswers.Any(sa => sa.QuestionId == entity.QuestionId))
            .AsNoTracking()
            .SingleOrDefaultAsync();

        if (existingAttempt?.IsSubmitted == true)
        {
            throw new ActionNotSupportedException(ErrorCode.CannotChangeSubmittedAttempt);
        }

        if (existingAttempt?.UserId != user.UserId)
        {
            throw new ActionNotAllowedException(ErrorCode.CannotAnswerForAnotherUser);
        }

        if (!existingAttempt.SubmittedAnswers.Any())
        {
            await _dbContext.SubmittedAnswers.AddAsync(entity);
        }
        else
        {
            _dbContext.SubmittedAnswers.Update(entity);
        }
        await _dbContext.SaveChangesAsync();

        return model;
    }

    private SubmittedAnswerEntity MapToEntity<TModel>(TModel model)
    {
        var topicEntity = _mapper.Map<SubmittedAnswerEntity>(model);
        return topicEntity;
    }
}
