namespace MaterialAdvisor.Application.Services;

public interface ITopicService
{
    Task<TModel> Create<TModel>(TModel topicModel);

    Task<bool> Delete(Guid topicId);

    Task<IList<TModel>> Get<TModel>();

    Task<TModel> Get<TModel>(Guid topicId);

    Task<TModel> Update<TModel>(TModel topicModel);
}
