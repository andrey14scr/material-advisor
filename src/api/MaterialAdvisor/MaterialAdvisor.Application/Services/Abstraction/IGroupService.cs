namespace MaterialAdvisor.Application.Services.Abstraction;

public interface IGroupService : ISearchService
{
    Task<TModel> Get<TModel>(Guid id);

    Task<IList<TModel>> GetAsOwner<TModel>();

    Task<IList<TModel>> GetAsMember<TModel>();

    Task<TModel> Create<TModel>(TModel model);

    Task<bool> Delete(Guid id);

    Task<TModel> Update<TModel>(TModel model);
}