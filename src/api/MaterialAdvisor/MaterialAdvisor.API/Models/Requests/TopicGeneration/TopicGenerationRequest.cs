using MaterialAdvisor.Application.Models.Topics;
using MaterialAdvisor.Data.Enums;
using MaterialAdvisor.QueueStorage.Messages;

namespace MaterialAdvisor.API.Models.Requests.TopicGeneration;

public class TopicGenerationRequest
{
    public List<LanguageText> TopicName { get; set; } = [];

    public List<Language> Languages { get; set; } = [];

    public string CultureContext { get; set; } = null!;

    public List<QuestionsSection>? QuestionsStructure { get; set; }

    public byte? MaxQuestionsCount { get; set; }

    public bool DoesComplexityIncrease { get; set; }

    public byte? DefaultAnswersCount { get; set; }

    public IFormFile File { get; set; } = null!;
}
