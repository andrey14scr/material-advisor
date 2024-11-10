using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace MaterialAdvisor.Data.Entities;

[Table("GeneratedFilesKnowldgeChecks")]
[PrimaryKey(nameof(GeneratedFileId), nameof(KnowledgeCheckId))]
[Index(nameof(GeneratedFileId), IsUnique = true)]
public class GeneratedFilesKnowldgeChecks
{
    public Guid GeneratedFileId { get; set; }

    public Guid KnowledgeCheckId { get; set; }

    public virtual KnowledgeCheckEntity KnowledgeCheck { get; set; }

    public virtual GeneratedFileEntity GeneratedFile { get; set; }
}