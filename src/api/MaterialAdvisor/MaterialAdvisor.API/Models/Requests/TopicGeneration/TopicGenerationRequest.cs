using MaterialAdvisor.Application.Models.Topics;
using MaterialAdvisor.Data.Enums;

namespace MaterialAdvisor.API.Models.Requests.TopicGeneration;

public class TopicGenerationRequest
{
    public IEnumerable<LanguageText> TopicName { get; set; } = null!;

    public ushort TopicNumber { get; set; }

    public ushort? MaxQuestionsCount { get; set; }

    public IEnumerable<Language> Languages { get; set; } = [];

    public IFormFile File { get; set; } = null!;
}
