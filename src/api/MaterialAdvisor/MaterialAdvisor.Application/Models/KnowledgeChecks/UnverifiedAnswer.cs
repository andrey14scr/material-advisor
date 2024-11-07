using MaterialAdvisor.Application.Models.Topics;
using MaterialAdvisor.Application.Models.Users;

namespace MaterialAdvisor.Application.Models.KnowledgeChecks;

public class UnverifiedAnswer
{
    public SubmittedAnswer SubmittedAnswer { get; set; } = null!;

    public User User { get; set; } = null!;

    public KnowledgeCheckTopicListItem KnowledgeCheck { get; set; } = null!;

    public Topic Topic { get; set; } = null!;

    public ICollection<VerifiedAnswer> VerifiedAnswers { get; set; } = [];
}