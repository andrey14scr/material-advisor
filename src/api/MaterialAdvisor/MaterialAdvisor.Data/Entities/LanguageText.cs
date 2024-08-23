using MaterialAdvisor.Data.Enums;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialAdvisor.Data.Entities;

[Table("LanguageTexts")]
public class LanguageTextEntity
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(500)]
    public string Text { get; set; } = null!;

    public Language LanguageId { get; set; }

    public virtual LanguageEntity Language { get; set; }

    public virtual ICollection<QuestionEntity> Questions { get; set; } = [];

    public virtual ICollection<AnswerEntity> Answers { get; set; } = [];

    public virtual ICollection<AnswerGroupEntity> AnswerGroups { get; set; } = [];

    public virtual ICollection<TopicEntity> Topics { get; set; } = [];
}