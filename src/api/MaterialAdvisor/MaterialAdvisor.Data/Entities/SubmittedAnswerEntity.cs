using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialAdvisor.Data.Entities;

[Table("SubmittedAnswers")]
[PrimaryKey(nameof(UserId), nameof(QuestionId), nameof(KnowledgeCheckId))]
public class SubmittedAnswerEntity
{
    public Guid UserId { get; set; }

    public virtual UserEntity User { get; set; }

    public Guid QuestionId { get; set; }

    public virtual QuestionEntity Question { get; set; }

    public Guid KnowledgeCheckId { get; set; }

    public virtual KnowledgeCheckEntity KnowledgeCheck { get; set; }

    [Range(1, 250)]
    public byte Attempt { get; set; }

    public string? Value { get; set; }
}
