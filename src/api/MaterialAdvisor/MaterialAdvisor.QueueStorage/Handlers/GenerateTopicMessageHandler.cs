using MaterialAdvisor.Application.Storage;
using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;
using MaterialAdvisor.QueueStorage.Messages;
using MaterialAdvisor.QueueStorage.Properties;
using MaterialAdvisor.SignalR;
using MaterialAdvisor.SignalR.Hubs;

using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaterialAdvisor.QueueStorage.Handlers;

public class GenerateTopicMessageHandler(IHubContext<TopicGenerationHub> _topicGenerationHubContext, 
    ILogger<GenerateTopicMessageHandler> _logger,
    IStorageService _storageService,
    MaterialAdvisorContext _dbContext) : IMessageHandler<GenerateTopicMessage>
{
    public async Task HandleAsync(GenerateTopicMessage message)
    {
        try
        {
            var topic = await _dbContext.Topics.Include(t => t.Name).SingleAsync(t => t.Id == message.TopicId);

            if (topic.File is null)
            {
                throw new ArgumentNullException(nameof(topic.File), $"File path of topic {topic.Id} was null");
            }

            var file = await _storageService.GetFile(topic.File);

            var languagesPrompt = GetLanguagesPrompt(topic);
            var questionsStructurePrompt = GetQuestionsStructurePrompt(message);

            var prompt = string.Join(" ", languagesPrompt, questionsStructurePrompt);

            // ...
            await Task.Delay(3000);

            await _topicGenerationHubContext.Clients
                .User(message.UserName)
                .SendAsync(SignalRConstants.Messages.TopicGeneratedMessage, message.TopicId, TopicGenerationStatuses.Generated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing handler. Removing old topic...");

            await _topicGenerationHubContext.Clients
                .User(message.UserName)
                .SendAsync(SignalRConstants.Messages.TopicGeneratedMessage, message.TopicId, TopicGenerationStatuses.Failed);

            await _dbContext.Topics.Where(t => t.Id == message.TopicId).ExecuteDeleteAsync();

            throw;
        }
    }

    private static string GetLanguagesPrompt(TopicEntity topic)
    {
        var languages = topic.Name.Select(n => n.LanguageId.ToString()).ToList();
        var languagesPrompt = GetPrompt(Resources.LanguagesPrompt, string.Join(", ", languages));
        return languagesPrompt;
    }

    private static string GetQuestionsStructurePrompt(GenerateTopicMessage message)
    {
        if (message.QuestionsStructure is null)
        {
            var complexityPrompt = message.DoesComplexityIncrease
                ? GetPrompt(Resources.IncreasingComplexityPrompt)
                : GetPrompt(Resources.EqualComplexityPrompt);

            var maxQuestionsCountPrompt = message.MaxQuestionsCount is not null
                ? GetPrompt(Resources.MaxQuestionsCountPrompt, message.MaxQuestionsCount)
                : GetPrompt(Resources.UseOptimalQuestionsCountPrompt);

            var answersCountPrompt = message.AnswersCount is not null
                ? GetPrompt(Resources.AnswersCountPrompt, message.AnswersCount)
                : GetPrompt(Resources.UseOptimalAnswersCountPrompt);

            return string.Join(" ", complexityPrompt, maxQuestionsCountPrompt, answersCountPrompt);
        }
        else
        {
            var defaultAnswersCount = message.AnswersCount.HasValue ? message.AnswersCount.Value.ToString() : "optimal count";
            string GetAnswersCount(QuestionsSection qs)
            {
                return qs.AnswersCount.HasValue ? qs.AnswersCount.Value.ToString() : defaultAnswersCount;
            }

            var questionsStructure = message.QuestionsStructure
                .Select(x => GetPrompt(Resources.QuestionsSectionPrompt, x.Count, x.QuestionType.ToString(), GetAnswersCount(x)))
                .ToList();
            return GetPrompt(Resources.QuestionsStructurePrompt, string.Join("; ", questionsStructure));
        }
    }

    private static string GetPrompt(string prompt, params object[] args)
    {
        return string.Format(prompt, args);
    }
}
