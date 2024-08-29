using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialAdvisor.Data.Entities;

[Table("Attempts")]
public class AttemptEntity : IEntity
{
    public Guid Id { get; set; }

    public short Number { get; set; }

    public DateTime StartDate { get; set; }

    public Guid KnowledgeCheckId { get; set; }

    public virtual KnowledgeCheckEntity KnowledgeCheck { get; set; }

    public Guid UserId { get; set; }

    public virtual UserEntity User { get; set; }

    public ICollection<SubmittedAnswerEntity> SubmittedAnswers { get; set; } = [];
}