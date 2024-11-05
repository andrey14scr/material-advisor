using MaterialAdvisor.Application.Models.KnowledgeChecks;
using MaterialAdvisor.Application.Models.Topics;

namespace MaterialAdvisor.Application.Mapping;

public static class MappingExtensions
{
    public static void EnrichAttemptsCount(this IEnumerable<TopicListItem<KnowledgeCheckTopicListItem>> topics, IDictionary<Guid, int> dictionary)
    {
        foreach (var topic in topics)
        {
            foreach (var knowledgeCheck in topic.KnowledgeChecks)
            {
                knowledgeCheck.DataCount = dictionary[knowledgeCheck.Id];
            }
        }
    }
}
