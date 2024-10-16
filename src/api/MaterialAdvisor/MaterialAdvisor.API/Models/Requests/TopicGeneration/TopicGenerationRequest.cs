using MaterialAdvisor.Application.Models.Topics;
using MaterialAdvisor.QueueStorage.Messages;

namespace MaterialAdvisor.API.Models.Requests.TopicGeneration;

public class TopicGenerationRequest
{
    public List<LanguageText> TopicName { get; set; } = [];

    public List<QuestionsSection>? QuestionsStructure { get; set; }

    public ushort? MaxQuestionsCount { get; set; }

    public bool DoesComplexityIncrease { get; set; }

    public byte? AnswersCount { get; set; }

    public IFormFile File { get; set; } = null!;
}
