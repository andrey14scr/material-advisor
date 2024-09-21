using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MaterialAdvisor.Data.Entities.Anstraction;

namespace MaterialAdvisor.Data.Entities;

[Table("Groups")]
public class GroupEntity : IEntity
{
    public Guid Id { get; set; }

    public Guid OwnerId { get; set; }

    public virtual UserEntity Owner { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = null!;

    public virtual ICollection<KnowledgeCheckEntity> KnowledgeChecks { get; set; } = [];

    public virtual ICollection<UserEntity> Users { get; set; } = [];
}