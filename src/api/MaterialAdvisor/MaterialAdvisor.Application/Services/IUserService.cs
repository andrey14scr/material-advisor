using MaterialAdvisor.Application.Models.Shared;

namespace MaterialAdvisor.Application.Services;

public interface IUserService
{
    Task<UserInfo> Get(string login, string password);

    Task<UserInfo> Create(string userName, string email, string password);
}