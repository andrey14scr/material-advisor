using AutoMapper;

using MaterialAdvisor.Application.Configuration.Options;
using MaterialAdvisor.Application.Exceptions;
using MaterialAdvisor.Application.Models.Shared;
using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using System.IdentityModel.Tokens.Jwt;

namespace MaterialAdvisor.Application.Services;

public class UserProvider(MaterialAdvisorContext dbContext, 
    IOptions<CachingOptions> cachingOptions, 
    IHttpContextAccessor httpContextAccessor, 
    ISecurityService securityService,
    IMemoryCache cache,
    IMapper mapper) : IUserProvider
{
    public UserInfo AddUser(UserEntity user)
    {
        return AddToCache(user);
    }

    public async Task<UserInfo> GetUser()
    {
        var username = httpContextAccessor.HttpContext.User.FindFirst(JwtRegisteredClaimNames.Name)!.Value;
        var cacheKey = GetKey(username);

        if (!cache.TryGetValue(cacheKey, out UserInfo? userInfo) || userInfo is null)
        {
            var searchName = securityService.Encrypt(username);

            var userEntity = await dbContext.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Name == searchName);
            if (userEntity is null)
            {
                throw new NotFoundException();
            }

            userInfo = AddToCache(userEntity);
        }

        return userInfo;
    }

    private UserInfo AddToCache(UserEntity userEntity)
    {
        var userInfo = mapper.Map<UserInfo>(userEntity);

        var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(cachingOptions.Value.ExpirationTime));
        var cacheKey = GetKey(userInfo.UserName);
        cache.Set(cacheKey, userInfo, cacheOptions);

        return userInfo;
    }

    private static string GetKey(string userName) => $"user_{userName}";
}
