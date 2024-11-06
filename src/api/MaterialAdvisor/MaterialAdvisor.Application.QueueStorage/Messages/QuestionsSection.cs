using MaterialAdvisor.Data.Enums;

namespace MaterialAdvisor.Application.QueueStorage.Messages;

public class QuestionsSection
{
    public byte Count { get; set; }

    public QuestionType QuestionType { get; set; }

    public byte? AnswersCount { get; set; }
}