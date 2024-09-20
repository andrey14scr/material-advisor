using MaterialAdvisor.API.Exceptions;
using MaterialAdvisor.API.Models;
using MaterialAdvisor.API.Options;
using MaterialAdvisor.Application.Models.Shared;
using MaterialAdvisor.Application.Services;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace MaterialAdvisor.API.Services;

public class TokensGenerator(IOptions<AuthOptions> _authOptions,
    IHttpContextAccessor _httpContextAccessor, 
    IRefreshTokenService _refreshTokenService,
    IUserService _userService)
{
    public async Task<TokensResult> Generate(User user)
    {
        var (expireIn, refreshExpireIn) = GetExpirationTimes();
        var claims = await CreateClaims(user, expireIn);
        var creds = GetSigningCredentials();
        var issuer = _authOptions.Value.Issuer;

        var identity = new ClaimsIdentity(claims);
        _httpContextAccessor.HttpContext!.User = new ClaimsPrincipal(identity);

        var token = new JwtSecurityToken(issuer: issuer, claims: claims, expires: expireIn, signingCredentials: creds);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        
        var refreshToken = Guid.NewGuid().ToString();
        var userId = user.Id;
        await _refreshTokenService.Create(userId, refreshToken, refreshExpireIn);

        return new TokensResult 
        {
            AccessToken = accessToken, 
            RefreshToken = refreshToken
        };
    }

    public async Task<TokensResult> Refresh(User user, string token)
    {
        var refreshToken = await _refreshTokenService.Get(user.Id, token);

        if (refreshToken?.ExpireAt > DateTime.UtcNow)
        {
            return await Generate(user);
        }

        throw new RefreshTokenExpiredException();
    }

    private async Task<IEnumerable<Claim>> CreateClaims(User user, DateTime expireIn)
    {
        var issuer = _authOptions.Value.Issuer;
        var iat = EpochTime.GetIntDate(expireIn).ToString(CultureInfo.InvariantCulture);

        var roles = await _userService.GetRoles(user.Id);
        var rolesDictionary = roles.ToDictionary(k => (int)k.RoleId, e => e.GroupIds);
        var rolesClaim = JsonSerializer.Serialize(rolesDictionary);

        return 
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Iss, issuer),
            new Claim(JwtRegisteredClaimNames.Iat, iat, ClaimValueTypes.Integer64),
            new Claim(Constants.Claims.RolesGroups, rolesClaim)
        ];
    }

    private (DateTime ExpireIn, DateTime RefreshExtraTime) GetExpirationTimes()
    {
        var now = DateTime.UtcNow;
        var expireIn = now.AddMinutes(_authOptions.Value.ExpireIn);
        var refreshExtraTime = expireIn.AddMinutes(_authOptions.Value.RefreshTime);

        return (expireIn, refreshExtraTime);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var secret = _authOptions.Value.Key;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        return creds;
    }
}
