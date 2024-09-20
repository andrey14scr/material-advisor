using MaterialAdvisor.Data.Enums;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MaterialAdvisor.Data.Entities;

[Table("Roles")]
public class RoleEntity
{
    public RoleType Id { get; set; }

    [Required]
    [MaxLength(25)]
    public string Name { get; set; } = null!;

    public virtual ICollection<PermissionEntity> Permissions { get; set; } = [];

    public virtual ICollection<GroupRoleEntity> GroupRoles { get; set; } = [];
}
