namespace MaterialAdvisor.API.Models;

public class TokensResult
{
    public string AccessToken { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;
}
