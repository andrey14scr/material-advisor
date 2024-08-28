using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialAdvisor.Data.Entities;

[Table("Topics")]
public class TopicEntity : IEntity
{
    public Guid Id { get; set; }

    public Guid OwnerId { get; set; }

    public virtual UserEntity Owner { get; set; }

    public virtual ICollection<LanguageTextEntity> Texts { get; set; } = [];

    public virtual ICollection<QuestionEntity> Questions { get; set; } = [];
}
