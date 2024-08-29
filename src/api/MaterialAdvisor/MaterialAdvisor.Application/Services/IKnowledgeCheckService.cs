namespace MaterialAdvisor.Application.Services;

public interface IKnowledgeCheckService
{
    Task<TModel> Create<TModel>(TModel model);

    Task<bool> Delete(Guid id);

    Task<TModel> Get<TModel>(Guid id);

    Task<IList<TModel>> Get<TModel>();

    Task<IList<TModel>> GetByGroup<TModel>(Guid groupId);

    Task<TModel> Update<TModel>(TModel model);
}