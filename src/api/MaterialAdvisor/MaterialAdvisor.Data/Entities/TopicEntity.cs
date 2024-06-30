using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialAdvisor.Data.Entities;

[Table("Topics")]
public class TopicEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public virtual UserEntity User { get; set; }

    public virtual ICollection<LanguageTextEntity> Texts { get; set; }

    public virtual ICollection<QuestionEntity> Questions { get; set; }
}
