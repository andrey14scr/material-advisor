using MaterialAdvisor.Application.Models.Users;

namespace MaterialAdvisor.Application.Services.Abstraction;

public interface IUserService : ISearchService
{
    Task<User> Get(string login, string password);

    Task<User> Create(string userName, string email, string password);

    Task UpdateSettings(UserSettings userSettings);

    Task<string?> CetCurrentLanguage();
}