using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialAdvisor.Data.Entities;

[Table("KnowledgeChecks")]
public class KnowledgeCheckEntity
{
    public Guid Id { get; set; }

    public Guid TopicId { get; set; }

    public virtual TopicEntity Topic { get; set; }

    public short Number { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(300)]
    public string Description { get; set; } = null!;
}
