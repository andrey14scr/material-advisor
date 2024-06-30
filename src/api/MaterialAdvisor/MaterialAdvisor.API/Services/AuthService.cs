using Microsoft.IdentityModel.Tokens;

using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Text;

namespace MaterialAdvisor.API.Services;

public class AuthService(IConfiguration configuration)
{
    public string GenerateJwtToken(string name, string email)
    {
        var issuer = configuration.GetSection("Jwt")["Issuer"]!;
        var secret = configuration.GetSection("Jwt")["Key"]!;

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, name),
            new Claim(JwtRegisteredClaimNames.Name, name),
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
