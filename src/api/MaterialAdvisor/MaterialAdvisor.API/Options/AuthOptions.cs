namespace MaterialAdvisor.API.Options;

public class AuthOptions
{
    public string Issuer { get; set; } = null!;

    public string Audience { get; set; } = null!;

    public string Key { get; set; } = null!;

    public int ExpireIn { get; set; }

    public int RefreshTime { get; set; }
}
