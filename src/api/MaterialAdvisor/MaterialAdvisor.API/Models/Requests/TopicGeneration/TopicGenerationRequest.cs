using MaterialAdvisor.Application.Models.Topics;
using MaterialAdvisor.Data.Enums;

namespace MaterialAdvisor.API.Models.Requests.TopicGeneration;

public class TopicGenerationRequest
{
    public List<LanguageText> TopicName { get; set; } = [];

    public ushort? MaxQuestionsCount { get; set; }

    public List<Language> Languages { get; set; } = [];

    public IFormFile File { get; set; } = null!;
}
