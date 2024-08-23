using MaterialAdvisor.Application.Models.Shared;

namespace MaterialAdvisor.Application.Services;

public interface IUserProvider
{
    Task<UserInfo> GetUser();
}
