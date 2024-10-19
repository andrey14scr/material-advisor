namespace MaterialAdvisor.Application.Models.Users;

public class User
{
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public Guid Id { get; set; }
}
