using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.QueueStorage.Models;

public class TopicQuestions
{
    public List<QuestionEntity> Questions { get; set; } = [];

    public List<LanguageTextEntity> TopicName { get; set; } = [];
}