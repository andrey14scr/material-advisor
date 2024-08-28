namespace MaterialAdvisor.Application.Services;

public interface IBaseService
{
    Task<TModel> Create<TModel>(TModel model);

    Task<bool> Delete(Guid id);

    Task<TModel> Get<TModel>(Guid id);

    Task<TModel> HardUpdate<TModel>(TModel model);

    Task<TModel> SoftUpdate<TModel>(TModel model);
}
