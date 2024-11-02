using MaterialAdvisor.Application.Models.KnowledgeChecks;

namespace MaterialAdvisor.Application.Services.Abstraction;

public interface IAttemptService
{
    Task<TModel> Create<TModel>(CreateAttempt model);

    Task<bool> SetIsSubmit(Guid id);

    Task<TModel?> GetLast<TModel>(Guid knowledgeCheckId);
}