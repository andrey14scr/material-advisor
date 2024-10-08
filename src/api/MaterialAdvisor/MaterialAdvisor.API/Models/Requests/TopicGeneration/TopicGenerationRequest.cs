using MaterialAdvisor.Application.Models.Topics;

namespace MaterialAdvisor.API.Models.Requests.TopicGeneration;

public class TopicGenerationRequest
{
    public List<LanguageText> TopicName { get; set; } = [];

    public ushort? MaxQuestionsCount { get; set; }

    public IFormFile File { get; set; } = null!;
}
