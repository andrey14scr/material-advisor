using MaterialAdvisor.Application.Configuration.Options;
using MaterialAdvisor.Application.Models.Shared;
using MaterialAdvisor.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using System.Security.Claims;

namespace MaterialAdvisor.Application.Services;

public class UserProvider(MaterialAdvisorContext dbContext, 
    IOptions<CachingOptions> cachingOptions, 
    IHttpContextAccessor httpContextAccessor, 
    IMemoryCache cache) : IUserProvider
{
    public async Task<UserInfo> GetUser()
    {
        var email = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)!.Value;
        var username = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)!.Value;
        var cacheKey = $"user_{username}_{email}";

        if (!cache.TryGetValue(cacheKey, out UserInfo? user) || user is null)
        {
            var userEntity = await dbContext.Users.SingleAsync(u => u.Name == username && u.Email == email);
            user = new UserInfo() { UserId = userEntity.Id, UserEmail = userEntity.Email, UserName = userEntity.Name };
            
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(cachingOptions.Value.ExpirationTime));
            cache.Set(cacheKey, user, cacheOptions);
        }

        return user;
    }
}
