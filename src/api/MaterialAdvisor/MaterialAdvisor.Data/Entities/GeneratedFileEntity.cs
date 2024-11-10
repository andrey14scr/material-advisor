using System.ComponentModel.DataAnnotations.Schema;

using MaterialAdvisor.Data.Entities.Anstraction;

namespace MaterialAdvisor.Data.Entities;

[Table("GeneratedFiles")]
public class GeneratedFileEntity : IEntity
{
    public Guid Id { get; set; }

    public string? File { get; set; }

    public string? FileName { get; set; }

    public DateTime GeneratedAt { get; set; }

    public Guid OwnerId { get; set; }

    public UserEntity Owner { get; set; }

    public virtual ICollection<GeneratedFilesKnowldgeChecks> GeneratedFilesKnowldgeChecks { get; set; } = [];
}