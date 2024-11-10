using MaterialAdvisor.Application.Models.Topics;
using MaterialAdvisor.Application.QueueStorage.Messages;
using MaterialAdvisor.Data.Enums;

namespace MaterialAdvisor.API.Models.Requests.TopicGeneration;

public class ReportGenerationRequest
{
    public List<LanguageText> TopicName { get; set; } = [];

    public List<Language> Languages { get; set; } = [];

    public string CultureContext { get; set; } = null!;

    public List<QuestionsSection> QuestionsStructure { get; set; } = [];

    public bool DoesComplexityIncrease { get; set; }

    public byte? DefaultAnswersCount { get; set; }

    public IFormFile File { get; set; } = null!;
}
