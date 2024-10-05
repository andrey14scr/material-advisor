using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Services.Abstraction;

public interface IRefreshTokenService
{
    Task Create(Guid userId, string token, DateTime expireAt);

    Task<RefreshTokenEntity?> Get(Guid userId, string token);
}
