using MaterialAdvisor.Data.Enums;

using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;
namespace MaterialAdvisor.Data.Entities;

[Table("GroupRoles")]
[PrimaryKey(nameof(UserId), nameof(GroupId), nameof(RoleId))]
public class GroupRoleEntity
{
    public Guid UserId { get; set; }

    public virtual UserEntity User { get; set; }

    public Guid GroupId { get; set; }

    public virtual GroupEntity Group { get; set; }

    public RoleType RoleId { get; set; }

    public virtual RoleEntity Role { get; set; }
}
