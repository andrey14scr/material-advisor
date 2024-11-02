using MaterialAdvisor.Application.Models.KnowledgeChecks;

namespace MaterialAdvisor.Application.Services.Abstraction;

public interface ISubmittedAnswerService
{
    Task<TModel> CreateOrUpdate<TModel>(TModel model);

    Task<IList<TModel>> GetUnverifiedAnswers<TModel>(Guid knowledgeCheckId);

    Task<bool> VerifyAnswer<TModel>(TModel verifiedAnswer);
}
