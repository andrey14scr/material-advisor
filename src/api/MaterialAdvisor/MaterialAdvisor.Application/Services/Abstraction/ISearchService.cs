namespace MaterialAdvisor.Application.Services.Abstraction;

public interface ISearchService
{
    Task<IList<TModel>> Search<TModel>(string input);
}