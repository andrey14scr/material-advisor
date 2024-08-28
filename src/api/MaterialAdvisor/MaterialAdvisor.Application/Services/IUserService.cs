using MaterialAdvisor.Application.Models.Shared;

namespace MaterialAdvisor.Application.Services;

public interface IUserService
{
    Task<UserInfo> Get(string login, string hash);

    Task<UserInfo> Create(string userName, string email, string hash);
}