using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialAdvisor.Data.Entities;

[Table("VerifiedAnswers")]
public class VerifiedAnswerEntity
{
    public Guid Id { get; set; }

    public Guid AnswerGroupId { get; set; }

    public Guid AttemptId { get; set; }

    [ForeignKey($"{nameof(AnswerGroupId)}, {nameof(AttemptId)}")]
    public virtual SubmittedAnswerEntity SubmittedAnswer { get; set; }

    public double Score { get; set; }

    public bool IsManual { get; set; }
}