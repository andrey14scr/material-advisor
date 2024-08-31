using AutoMapper;

using MaterialAdvisor.Application.Exceptions;
using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace MaterialAdvisor.Application.Services;

public class SubmittedAnswerService(MaterialAdvisorContext _dbContext, IMapper _mapper) : ISubmittedAnswerService
{
    public async Task<TModel> CreateOrUpdate<TModel>(TModel model)
    {
        var entity = MapToEntity(model);
        var existingSubmittedAnswer = await _dbContext.SubmittedAnswers
            .Include(sa => sa.Attempt)
            .AsNoTracking()
            .SingleOrDefaultAsync(sa => sa.AttemptId == entity.AttemptId && sa.QuestionId == entity.QuestionId);

        if (existingSubmittedAnswer?.Attempt.IsSubmitted == true)
        {
            throw new ActionNotSupportedException(ErrorCode.CannotChangeSubmittedAttempt);
        }

        if (existingSubmittedAnswer is null)
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
