using MaterialAdvisor.Application.Models.Shared;
using MaterialAdvisor.Application.Models.Users;

namespace MaterialAdvisor.Application.Services.Abstraction;

public interface IUserService : ISearchService
{
    Task<TModel> Get<TModel>(string login, string password);

    Task<TModel> Create<TModel>(string userName, string email, string password);

    Task UpdateSettings(UserSettings userSettings);

    Task<string?> CetCurrentLanguage();

    Task<IList<TModel>> Get<TModel>(Pagination pagination);
}