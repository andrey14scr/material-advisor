using MaterialAdvisor.Data.Entities.Anstraction;
using MaterialAdvisor.Data.Enums;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialAdvisor.Data.Entities;

[Table("LanguageTexts")]
public class LanguageTextEntity : IEntity
{
    public Guid Id { get; set; }

    [MaxLength(500)]
    public string Text { get; set; } = null!;

    public Language LanguageId { get; set; }

    public virtual LanguageEntity Language { get; set; }

    public virtual Guid? QuestionId { get; set; }

    public virtual QuestionEntity? Question { get; set; }

    public virtual Guid? AnswerId { get; set; }

    public virtual AnswerEntity? Answer { get; set; }

    public virtual Guid? AnswerGroupId { get; set; }

    public virtual AnswerGroupEntity? AnswerGroup { get; set; }

    public virtual Guid? TopicId { get; set; }

    public virtual TopicEntity? Topic { get; set; }
}