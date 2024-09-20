namespace MaterialAdvisor.Application.Models.Shared;

public class User
{
    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public Guid Id { get; set; }
}
