using MaterialAdvisor.Application.AI;
using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;
using MaterialAdvisor.Data.Enums;
using MaterialAdvisor.Data.Extensions;
using MaterialAdvisor.QueueStorage.Messages;
using MaterialAdvisor.QueueStorage.Models;
using MaterialAdvisor.QueueStorage.Properties;
using MaterialAdvisor.SignalR;
using MaterialAdvisor.SignalR.Hubs;

using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System.Text;
using System.Text.Json;

namespace MaterialAdvisor.QueueStorage.Handlers;

public class GenerateTopicMessageHandler(IHubContext<TopicGenerationHub> _topicGenerationHubContext, 
    ILogger<GenerateTopicMessageHandler> _logger,
    IMaterialAdvisorAIAssistant _materialAdvisorAIAssistant,
    MaterialAdvisorContext _dbContext) : IMessageHandler<GenerateTopicMessage>
{
    public async Task HandleAsync(GenerateTopicMessage message)
    {
        try
        {
            var topic = await _dbContext.Topics.Include(t => t.Name).AsNoTracking().SingleAsync(t => t.Id == message.TopicId);
            ValidateTopic(topic);

            await Task.Delay(5000);
            //var json = await Generate(topic, message);
            var topicQuestions = new TopicQuestions(); //await ParseTopicQuestions(json);

            await UpdateTopic(topic, topicQuestions);

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

    private void ValidateTopic(TopicEntity topic)
    {
        if (topic.Version != 0)
        {
            throw new ArgumentException(nameof(topic), $"Topic {topic.Id} was not of initialized(0) version");
        }

        if (topic.File is null)
        {
            throw new ArgumentNullException(nameof(topic.File), $"File path of topic {topic.Id} was null");
        }
    }

    private async Task<string> Generate(TopicEntity topic, GenerateTopicMessage message)
    {
        var additionalInfoPrompt = GetEnumsPrompt();
        var languagesPrompt = GetLanguagesPrompt(topic, message.Languages);
        var culturePrompt = GetPrompt(Resources.CultureContextPrompt, message.CultureContext);
        var questionsStructurePrompt = GetQuestionsStructurePrompt(message);

        var prompt = string.Join(" ", additionalInfoPrompt, languagesPrompt, culturePrompt, questionsStructurePrompt);
        _logger.LogError($"Prompt: {prompt}");

        var json = await _materialAdvisorAIAssistant.GenerateQuestions(topic.File!, prompt);
        return json;
    }

    private async Task<TopicQuestions> ParseTopicQuestions(string json)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        var topicQuestions = await JsonSerializer.DeserializeAsync<TopicQuestions>(stream);

        if (topicQuestions is null)
        {
            throw new ArgumentNullException(nameof(topicQuestions), "Generated questions were null");
        }

        return topicQuestions;
    }

    private async Task UpdateTopic(TopicEntity topic, TopicQuestions topicQuestions)
    {
        topic.Version = 1;
        topic.Questions = topicQuestions.Questions;

        foreach (var name in topicQuestions.TopicName)
        {
            name.TopicId = topic.Id;
        }

        await _dbContext.LanguageTexts.AddRangeAsync(topicQuestions.TopicName);
        _dbContext.Topics.Update(topic);
        await _dbContext.SaveChangesAsync();
    }

    private static string GetLanguagesPrompt(TopicEntity topic, IList<Language> languages)
    {
        var allLanguages = topic.Name.Select(n => n.LanguageId).Concat(languages).Distinct().Select(l => l.ToString()).ToList();

        var languagesPrompt = GetPrompt(Resources.LanguagesPrompt, string.Join(", ", allLanguages));

        var topicNamePrompt = string.Empty;
        if (languages.Any())
        {
            var topicLanguages = topic.Name.Select(n => $"\"{n.Text}\" in {n.LanguageId.ToString()}").ToList();
            var questionsLanguages = languages.Select(l => l.ToString()).ToList();

            topicNamePrompt = GetPrompt(Resources.TopicNamePrompt, 
                string.Join(", ", topicLanguages), 
                string.Join(", ", questionsLanguages));
        }
        else
        {
            topicNamePrompt = GetPrompt(Resources.NoTopicNamePrompt);
        }

        return string.Join(" ", topicNamePrompt, languagesPrompt);
    }

    private static string GetEnumsPrompt()
    {
        var languagesDictionary = EnumConverter.ToDictionary<Language>().Select(x => $"{x.Key} - {x.Value.ToString()}");
        var languages = string.Join(", ", languagesDictionary);

        var questionTypesDictionary = EnumConverter.ToDictionary<QuestionType>().Select(x => $"{x.Key} - {x.Value.ToString()}");
        var questionTypes = string.Join(", ", questionTypesDictionary);

        return GetPrompt(Resources.EnumsPrompt, languages, questionTypes);
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

            var answersCountPrompt = message.DefaultAnswersCount is not null
                ? GetPrompt(Resources.AnswersCountPrompt, message.DefaultAnswersCount)
                : GetPrompt(Resources.UseOptimalAnswersCountPrompt);

            return string.Join(" ", complexityPrompt, maxQuestionsCountPrompt, answersCountPrompt);
        }
        else
        {
            var defaultAnswersCount = message.DefaultAnswersCount.HasValue ? message.DefaultAnswersCount.Value.ToString() : "optimal count";
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
