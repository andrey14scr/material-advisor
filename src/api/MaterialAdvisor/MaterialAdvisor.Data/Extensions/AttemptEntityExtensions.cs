using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Data.Extensions;

public static class AttemptEntityExtensions
{
    public static bool IsFinished(this AttemptEntity attemptEntity)
    {
        return attemptEntity.IsSubmitted || 
            attemptEntity.KnowledgeCheck.EndDate.HasValue && attemptEntity.KnowledgeCheck.EndDate.Value < DateTime.UtcNow ||
            attemptEntity.KnowledgeCheck.Time.HasValue && attemptEntity.StartDate.AddSeconds(attemptEntity.KnowledgeCheck.Time.Value) < DateTime.UtcNow;
    }
}