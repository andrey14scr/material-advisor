using AutoMapper;

using MaterialAdvisor.Application.Configuration.Options;
using MaterialAdvisor.Application.Exceptions;
using MaterialAdvisor.Application.Models.Users;
using MaterialAdvisor.Application.Services.Abstraction;
using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using System.IdentityModel.Tokens.Jwt;

namespace MaterialAdvisor.Application.Services;

public class UserProvider(MaterialAdvisorContext _dbContext, 
    IOptions<CachingOptions> _cachingOptions, 
    IHttpContextAccessor _httpContextAccessor, 
    ISecurityService _securityService,
    IMemoryCache _cache,
    IMapper _mapper) : IUserProvider
{
    public async Task<User> GetUser()
    {
        var userName = _httpContextAccessor.HttpContext.User.FindFirst(JwtRegisteredClaimNames.Name)!.Value;
        var cacheKey = GetKey(userName);

        if (!_cache.TryGetValue(cacheKey, out User? userInfo) || userInfo is null)
        {
            var userEntity = await _dbContext.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Name == userName);
            if (userEntity is null)
            {
                throw new NotFoundException();
            }

            userInfo = AddToCache(userEntity);
        }

        return userInfo;
    }

    private User AddToCache(UserEntity userEntity)
    {
        var userInfo = _mapper.Map<User>(userEntity);

        var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(_cachingOptions.Value.ExpirationTime));
        var cacheKey = GetKey(userInfo.Name);
        _cache.Set(cacheKey, userInfo, cacheOptions);

        return userInfo;
    }

    private static string GetKey(string userName) => $"user_{userName}";
}
