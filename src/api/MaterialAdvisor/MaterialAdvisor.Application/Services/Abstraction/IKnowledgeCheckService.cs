namespace MaterialAdvisor.Application.Services.Abstraction;

public interface IKnowledgeCheckService
{
    Task<TModel> Create<TModel>(TModel model);

    Task<bool> Delete(Guid id);

    Task<TModel> Get<TModel>(Guid id);

    Task<IList<TModel>> Get<TModel>();

    Task<IList<TModel>> GetByGroup<TModel>(Guid groupId);

    Task<IList<TModel>> GetByTopic<TModel>(Guid topicId);

    Task<TModel> Update<TModel>(TModel model);

    Task<IDictionary<Guid, int>> GetAttemptsCount(IEnumerable<Guid> topicIds);

    Task<bool> HasVerifiedAttemts(Guid id);
}
