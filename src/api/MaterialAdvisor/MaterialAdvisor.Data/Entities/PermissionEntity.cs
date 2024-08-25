using MaterialAdvisor.Data.Enums;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MaterialAdvisor.Data.Entities;

[Table("Permissions")]
public class PermissionEntity
{
    public PermissionType Id { get; set; }

    [Required]
    [MaxLength(25)]
    public string Name { get; set; } = null!;

    public virtual ICollection<RoleEntity> Roles { get; set; } = [];
}