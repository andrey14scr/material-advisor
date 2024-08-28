namespace MaterialAdvisor.Application.Services;

public interface ITopicService : IBaseService
{
    Task<IList<TModel>> Get<TModel>();
}