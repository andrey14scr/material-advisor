using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MaterialAdvisor.Data.Entities.Anstraction;

using Microsoft.EntityFrameworkCore;

namespace MaterialAdvisor.Data.Entities;

[Table("KnowledgeChecks")]
[Index(nameof(Name))]
public class KnowledgeCheckEntity : IEntity
{
    public Guid Id { get; set; }

    public Guid TopicId { get; set; }

    public virtual TopicEntity Topic { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(300)]
    public string Description { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Range(1, 57600)]
    public ushort? Time { get; set; }

    [Range(1, 250)]
    public byte? MaxAttempts { get; set; }

    public double PassScore { get; set; }

    public bool IsAttemptOverrided { get; set; }

    public bool IsManualOnlyVerification { get; set; }

    public virtual ICollection<GroupEntity> Groups { get; set; } = [];

    public virtual ICollection<AttemptEntity> Attempts { get; set; } = [];

    public virtual ICollection<GeneratedFilesKnowldgeChecks> GeneratedFilesKnowldgeChecks { get; set; } = [];
}
