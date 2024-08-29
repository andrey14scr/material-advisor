namespace MaterialAdvisor.Application.Services;

public interface ITopicService
{
    Task<TModel> Create<TModel>(TModel model);

    Task<bool> Delete(Guid id);

    Task<TModel> Get<TModel>(Guid id);

    Task<IList<TModel>> Get<TModel>();

    Task<TModel> Update<TModel>(TModel model);
}