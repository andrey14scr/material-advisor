using MaterialAdvisor.Application.Models.Shared;

namespace MaterialAdvisor.Application.Services.Abstraction;

public interface IUserProvider
{
    Task<User> GetUser();
}
