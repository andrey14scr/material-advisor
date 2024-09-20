using MaterialAdvisor.Data.Entities.Anstraction;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MaterialAdvisor.Data.Entities;

[Table("Users")]
[Index(nameof(Name), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class UserEntity : IEntity
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(150)]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(150)]
    public string Hash { get; set; } = null!;

    public virtual ICollection<GroupEntity> CreatedGroups { get; set; } = [];

    public virtual ICollection<GroupRoleEntity> GroupRoles { get; set; } = [];
}
