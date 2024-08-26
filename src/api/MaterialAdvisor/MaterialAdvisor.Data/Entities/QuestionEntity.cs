using MaterialAdvisor.Data.Enums;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialAdvisor.Data.Entities;

[Table("Questions")]
public class QuestionEntity
{
    public Guid Id { get; set; }

    public Guid TopicId { get; set; }

    public virtual TopicEntity Topic { get; set; }

    [Range(1, 250)]
    public byte Number { get; set; }

    [Range(1, 250)]
    public byte Version { get; set; }

    public double Points { get; set; }

    public QuestionType Type { get; set; }

    public virtual ICollection<LanguageTextEntity> Texts { get; set; } = [];

    public virtual ICollection<AnswerGroupEntity> AnswerGroups { get; set; } = [];
}
