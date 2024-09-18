using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MaterialAdvisor.Data.Entities.Anstraction;

namespace MaterialAdvisor.Data.Entities;

[Table("AnswerGroups")]
public class AnswerGroupEntity : IEntity
{
    public Guid Id { get; set; }

    public Guid QuestionId { get; set; }

    public virtual QuestionEntity Question { get; set; }

    [Range(1, 250)]
    public byte Number { get; set; }

    public virtual ICollection<LanguageTextEntity> Texts { get; set; } = [];

    public virtual ICollection<AnswerEntity> Answers { get; set; } = [];
}
