namespace MaterialAdvisor.Application.Models.Users;

public class Group
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public IEnumerable<User> Users { get; set; } = [];
}
