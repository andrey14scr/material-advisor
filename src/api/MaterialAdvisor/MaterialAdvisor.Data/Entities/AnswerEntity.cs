using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialAdvisor.Data.Entities;

[Table("Answers")]
public class AnswerEntity
{
    public Guid Id { get; set; }

    public Guid AnswerGroupId { get; set; }

    public virtual AnswerGroupEntity AnswerGroup { get; set; }

    public byte Number { get; set; }

    public double Points { get; set; }

    public bool IsRight { get; set; }

    public virtual ICollection<LanguageTextEntity> Texts { get; set; } = [];
}
