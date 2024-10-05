using Microsoft.AspNetCore.SignalR;

using System.IdentityModel.Tokens.Jwt;

namespace MaterialAdvisor.SignalR.Providers;

public class UserIdProvider : IUserIdProvider
{
    public virtual string GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst(JwtRegisteredClaimNames.Name)?.Value!;
    }
}