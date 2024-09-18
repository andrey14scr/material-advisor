using System.ComponentModel.DataAnnotations.Schema;
using MaterialAdvisor.Data.Entities.Anstraction;

namespace MaterialAdvisor.Data.Entities;

[Table("Attempts")]
public class AttemptEntity : IEntity
{
    public Guid Id { get; set; }

    public ushort Number { get; set; }

    public DateTime StartDate { get; set; }

    public Guid KnowledgeCheckId { get; set; }

    public virtual KnowledgeCheckEntity KnowledgeCheck { get; set; }

    public Guid UserId { get; set; }

    public virtual UserEntity User { get; set; }

    public bool IsSubmitted { get; set; }

    public ICollection<SubmittedAnswerEntity> SubmittedAnswers { get; set; } = [];
}