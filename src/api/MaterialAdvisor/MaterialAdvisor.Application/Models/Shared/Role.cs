using MaterialAdvisor.Data.Enums;

namespace MaterialAdvisor.Application.Models.Shared;

public class Role
{
    public RoleType RoleId { get; set; }

    public IEnumerable<PermissionType> Permissions { get; set; } = [];

    public IEnumerable<Guid> GroupIds { get; set; } = [];
}