using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialAdvisor.Data.Entities;

[Table("VerifiedAnswers")]
public class VerifiedAnswerEntity
{
    public Guid Id { get; set; }

    public Guid AnswerGroupId { get; set; }

    public virtual AnswerGroupEntity AnswerGroup { get; set; }

    public Guid AttemptId { get; set; }

    public virtual AttemptEntity Attempt { get; set; }

    [ForeignKey($"{nameof(AnswerGroupId)}, {nameof(AttemptId)}")]
    public virtual SubmittedAnswerEntity SubmittedAnswer { get; set; }

    public double Score { get; set; }

    public bool IsManual { get; set; }

    [MaxLength(255)]
    public string? Comment { get; set; }
}