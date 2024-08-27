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

namespace MaterialAdvisor.API.Services;

public class TokensGenerator(IOptions<AuthOptions> authOptions,
    IHttpContextAccessor httpContextAccessor, 
    IRefreshTokenService refreshTokenService)
{
    public async Task<TokensResult> Generate(UserInfo userInfo)
    {
        var (expireIn, refreshExpireIn) = GetExpirationTimes();
        var claims = CreateClaims(userInfo, expireIn);
        var creds = GetSigningCredentials();
        var issuer = authOptions.Value.Issuer;

        var identity = new ClaimsIdentity(claims);
        httpContextAccessor.HttpContext!.User = new ClaimsPrincipal(identity);

        var token = new JwtSecurityToken(issuer: issuer, claims: claims, expires: expireIn, signingCredentials: creds);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        
        var refreshToken = Guid.NewGuid().ToString();
        var userId = userInfo.UserId;
        await refreshTokenService.Create(userId, refreshToken, refreshExpireIn);

        return new TokensResult 
        {
            AccessToken = accessToken, 
            RefreshToken = refreshToken
        };
    }

    public async Task<TokensResult> Refresh(UserInfo userInfo, string token)
    {
        var refreshToken = await refreshTokenService.Get(userInfo.UserId, token);

        if (refreshToken?.ExpireAt > DateTime.UtcNow)
        {
            return await Generate(userInfo);
        }

        throw new RefreshTokenExpiredException();
    }

    private Claim[] CreateClaims(UserInfo userInfo, DateTime expireIn)
    {
        var issuer = authOptions.Value.Issuer;

        return
        [
            new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
            new Claim(JwtRegisteredClaimNames.Name, userInfo.UserName),
            new Claim(JwtRegisteredClaimNames.Email, userInfo.UserEmail),
            new Claim(JwtRegisteredClaimNames.Iss, issuer),
            new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(expireIn).ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64)
        ];
    }

    private (DateTime ExpireIn, DateTime RefreshExtraTime) GetExpirationTimes()
    {
        var now = DateTime.UtcNow;
        var expireIn = now.AddMinutes(authOptions.Value.ExpireIn);
        var refreshExtraTime = expireIn.AddMinutes(authOptions.Value.RefreshTime);

        return (expireIn, refreshExtraTime);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var secret = authOptions.Value.Key;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        return creds;
    }
}
