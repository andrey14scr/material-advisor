namespace MaterialAdvisor.Application.Services;

public interface IKnowledgeCheckService : IBaseService
{
    Task<IList<TModel>> Get<TModel>();

    Task<IList<TModel>> GetByGroup<TModel>(Guid groupId);
}