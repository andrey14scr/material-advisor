using MaterialAdvisor.Application.Models.Users;

namespace MaterialAdvisor.Application.Services.Abstraction;

public interface IUserProvider
{
    Task<User> GetUser();
}
