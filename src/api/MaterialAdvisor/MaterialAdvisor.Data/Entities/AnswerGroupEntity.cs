using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialAdvisor.Data.Entities;

[Table("AnswerGroups")]
public class AnswerGroupEntity
{
    public Guid Id { get; set; }

    public Guid QuestionId { get; set; }

    public virtual QuestionEntity Question { get; set; }

    public byte Number { get; set; }

    public virtual ICollection<LanguageTextEntity> Texts { get; set; }

    public virtual ICollection<AnswerEntity> Answers { get; set; }
}
