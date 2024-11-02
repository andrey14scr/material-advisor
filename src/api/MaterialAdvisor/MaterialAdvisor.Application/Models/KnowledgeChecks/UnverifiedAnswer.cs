using MaterialAdvisor.Application.Models.Topics;
using MaterialAdvisor.Application.Models.Users;

namespace MaterialAdvisor.Application.Models.KnowledgeChecks;

public class UnverifiedAnswer : SubmittedAnswer
{
    public User User { get; set; } = null!;

    public KnowledgeCheckTopicListItem KnowledgeCheck { get; set; } = null!;

    public Topic Topic { get; set; } = null!;
}