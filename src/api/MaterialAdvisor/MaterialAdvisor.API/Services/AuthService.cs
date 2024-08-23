using MaterialAdvisor.API.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Text;

namespace MaterialAdvisor.API.Services;

public class AuthService(IConfiguration configuration, IOptions<JwtOptions> jwtOptions)
{
    public string GenerateJwtToken(string username, string email)
    {
        var issuer = jwtOptions.Value.Issuer;
        var secret = jwtOptions.Value.Key;

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Name, username),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Iss, issuer),
            new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.Now).ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(issuer: issuer, claims: claims, expires: DateTime.Now.AddMinutes(2), signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
