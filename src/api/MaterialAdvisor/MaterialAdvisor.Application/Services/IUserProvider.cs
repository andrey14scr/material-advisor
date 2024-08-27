using MaterialAdvisor.Application.Models.Shared;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Services;

public interface IUserProvider
{
    Task<UserInfo> GetUser();

    UserInfo AddUser(UserEntity user);
}
