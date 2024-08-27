using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace MaterialAdvisor.Application.Services;

public class RefreshTokenService(MaterialAdvisorContext dbContext, IUserProvider userProvider) : IRefreshTokenService
{
    public async Task Create(Guid userId, string token, DateTime expireAt)
    {
        var refreshToken = new RefreshTokenEntity
        {
            Value = token,
            ExpireAt = expireAt,
            UserId = userId
        };

        await dbContext.RefreshTokens.AddAsync(refreshToken);
        await dbContext.SaveChangesAsync();
    }

    public async Task<RefreshTokenEntity?> Get(Guid userId, string token)
    {
        var refreshToken = await dbContext.RefreshTokens
            .OrderByDescending(rt => rt.ExpireAt)
            .FirstOrDefaultAsync(rt => !rt.IsRevoked && rt.UserId == userId && rt.Value == token);

        return refreshToken;
    }
}