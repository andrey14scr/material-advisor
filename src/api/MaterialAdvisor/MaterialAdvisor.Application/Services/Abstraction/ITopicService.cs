namespace MaterialAdvisor.Application.Services.Abstraction;

public interface ITopicService
{
    Task<TModel> Create<TModel>(TModel model);

    Task<bool> Delete(Guid id);

    Task<TModel> Get<TModel>(Guid id);

    Task<IList<TModel>> Get<TModel>(bool isOwner);

    Task<TModel> Update<TModel>(TModel model);
}