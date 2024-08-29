using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialAdvisor.Data.Entities;

[Table("SubmittedAnswers")]
[PrimaryKey(nameof(QuestionId), nameof(AttemptId))]
public class SubmittedAnswerEntity
{
    public Guid QuestionId { get; set; }

    public virtual QuestionEntity Question { get; set; }

    public Guid AttemptId { get; set; }

    public virtual AttemptEntity Attempt { get; set; }

    public string? Value { get; set; }
}
