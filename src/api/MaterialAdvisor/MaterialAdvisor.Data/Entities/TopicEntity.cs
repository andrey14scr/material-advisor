using System.ComponentModel.DataAnnotations.Schema;
using MaterialAdvisor.Data.Entities.Anstraction;

namespace MaterialAdvisor.Data.Entities;

[Table("Topics")]
public class TopicEntity : IEntity, IVersion
{
    public Guid Id { get; set; }

    public Guid PersistentId { get; set; }

    public ushort Number { get; set; }

    public uint Version { get; set; }

    public Guid OwnerId { get; set; }

    public virtual UserEntity Owner { get; set; }

    public virtual ICollection<LanguageTextEntity> Texts { get; set; } = [];

    public virtual ICollection<QuestionEntity> Questions { get; set; } = [];

    public virtual ICollection<KnowledgeCheckEntity> KnowledgeChecks { get; set; } = [];
}
