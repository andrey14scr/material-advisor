namespace MaterialAdvisor.Application.Models.Shared;

public class UserInfo
{
    public string UserName { get; set; } = null!;

    public string UserEmail { get; set; } = null!;

    public Guid UserId { get; set; }
}
