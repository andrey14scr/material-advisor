using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialAdvisor.Data.Entities;

[Table("SubmittedAnswers")]
[PrimaryKey(nameof(AnswerGroupId), nameof(AttemptId))]
public class SubmittedAnswerEntity
{
    public Guid AnswerGroupId { get; set; }

    public virtual AnswerGroupEntity AnswerGroup { get; set; }

    public Guid AttemptId { get; set; }

    public virtual AttemptEntity Attempt { get; set; }

    public string? Value { get; set; }

    public virtual ICollection<VerifiedAnswerEntity> VerifiedAnswers { get; set; } = [];
}