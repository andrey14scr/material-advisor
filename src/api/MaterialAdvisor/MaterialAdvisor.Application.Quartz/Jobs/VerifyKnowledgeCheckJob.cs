using MaterialAdvisor.AI;
using MaterialAdvisor.Application.Quartz.Configuration.Options;
using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;
using MaterialAdvisor.SignalR.Hubs;

using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Quartz;

using System.Text.Json;
using System.Text;
using MaterialAdvisor.SignalR;
using MaterialAdvisor.AI.Configuration.Options;
using MaterialAdvisor.Data.Enums;
using MaterialAdvisor.Data.Extensions;
using MaterialAdvisor.Application.Quartz.Properties;
using MaterialAdvisor.Application.Quartz.Models;

namespace MaterialAdvisor.Application.Quartz.Jobs;

public class VerifyKnowledgeCheckJob(IOptions<VerifyKnowledgeCheckJobOptions> _verifyKnowledgeCheckJobOptions,
    IOptions<OpenAIAssistantOptions> _openAIAssistantOptions,
    IHubContext<AnswerVerificationHub> _answerVerificationHubContext,
    ILogger<VerifyKnowledgeCheckJob> _logger,
    IMaterialAdvisorAIAssistant _materialAdvisorAIAssistant,
    MaterialAdvisorContext _dbContext) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogDebug($"Started job {nameof(VerifyKnowledgeCheckJob)}");

            var answersInBulk = _verifyKnowledgeCheckJobOptions.Value.AnswersInBulk;
            var typesToVerify = Constants.QuestionTypesRequiredVerification;

            var submittedAnswers = await _dbContext.SubmittedAnswers
                .Include(sa => sa.AnswerGroup).ThenInclude(ag => ag.Question).ThenInclude(ag => ag.Content)
                .Include(sa => sa.AnswerGroup).ThenInclude(ag => ag.Question).ThenInclude(ag => ag.Topic)
                .Include(sa => sa.AnswerGroup).ThenInclude(ag => ag.Answers).ThenInclude(ag => ag.Content)
                .Include(sa => sa.Attempt).ThenInclude(a => a.User)
                .Include(sa => sa.VerifiedAnswers)
                .Where(sa => !sa.VerifiedAnswers.Any() &&
                    sa.Value != null && sa.Value.Length != 0 &&
                    typesToVerify.Contains(sa.AnswerGroup.Question.Type) &&
                    sa.AnswerGroup.Question.Topic.File != null)
                .OrderBy(sa => sa.Attempt.StartDate)
                .Take(answersInBulk)
                .ToListAsync();

            if (submittedAnswers.Count == 0)
            {
                _logger.LogDebug($"No submitted answers to verify by AI");
                return;
            }

            var smallestUnverifiedKnowledgeCheck = submittedAnswers
                .GroupBy(sa => sa.Attempt.KnowledgeCheckId)
                .OrderBy(g => g.Count())
                .First();

            var file = smallestUnverifiedKnowledgeCheck.First().AnswerGroup.Question.Topic.File!;
            var user = smallestUnverifiedKnowledgeCheck.First().Attempt.User.Name;

            var maxAnswersSize = _verifyKnowledgeCheckJobOptions.Value.MaxAnswersSize;
            var smallestUnverifiedKnowledgeCheckTruncated = new List<SubmittedAnswerEntity>();
            var counter = 0;

            foreach (var submittedAnswer in smallestUnverifiedKnowledgeCheck)
            {
                if (counter == 0 || counter + submittedAnswer.Value!.Length <= maxAnswersSize)
                {
                    counter += submittedAnswer.Value!.Length;
                    smallestUnverifiedKnowledgeCheckTruncated.Add(submittedAnswer);
                }
                else
                {
                    break;
                }
            }

            var answers = smallestUnverifiedKnowledgeCheckTruncated.Select(g => new
            {
                g.AnswerGroupId,
                g.AttemptId,
                g.Value,
                g.AnswerGroup.Question.Points,
                Question = g.AnswerGroup.Question.Content.Select(c => new
                {
                    c.LanguageId,
                    c.Text,
                }),
                Answer = g.AnswerGroup.Answers.FirstOrDefault()?.Content.FirstOrDefault()?.Text,
            });

            var submittedAnswersJson = await SerializeJson(answers);
            var json = await Verify(file, submittedAnswersJson); 
            var verifiedAnswers = await ParseVerifiedAnswers(json);

            await AddVerifiedAnswers(verifiedAnswers);

            var verifiedAIAnswers = verifiedAnswers.Select(va => new VerifiedAIAnswer
            {
                AnswerGroupId = va.AnswerGroupId,
                AttemptId = va.AttemptId,
                Comment = va.Comment,
                Score = va.Score,
            }).ToList();

            await _answerVerificationHubContext.Clients
                .User(user)
                .SendAsync(SignalRConstants.Messages.AnswersVerifiedMessage, verifiedAIAnswers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing job");
            throw;
        }
    }

    private static async Task<string> SerializeJson(object obj)
    {
        string json = string.Empty;
        using (var stream = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(stream, obj, obj.GetType());
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }
    }

    private async Task<string> Verify(string file, string submittedAnswersJson)
    {
        var enumsPrompt = GetEnumsPrompt();
        var answersPrompt = GetPrompt(Resources.AnswersPrompt, submittedAnswersJson); ;
        var prompt = $"{enumsPrompt} {answersPrompt}";
        _logger.LogDebug($"Prompt: {prompt}");

        var json = await _materialAdvisorAIAssistant.CallAssistant(file, prompt, _openAIAssistantOptions.Value.VerifyAnswersAssistantId);
        return json;
    }

    private static string GetEnumsPrompt()
    {
        var languagesDictionary = EnumConverter.ToDictionary<Language>().Select(x => $"{x.Key} - {x.Value.ToString()}");
        var languages = string.Join(", ", languagesDictionary);

        return GetPrompt(Resources.LanguagesPrompt, languages);
    }

    private static string GetPrompt(string prompt, params object[] args)
    {
        return string.Format(prompt, args);
    }

    private async Task<VerifiedAnswerEntity[]> ParseVerifiedAnswers(string json)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        var verifiedAnswers = await JsonSerializer.DeserializeAsync<VerifiedAnswerEntity[]>(stream);

        if (verifiedAnswers is null)
        {
            throw new ArgumentNullException(nameof(verifiedAnswers), "Verified answers were null");
        }

        foreach (var verifiedAnswer in verifiedAnswers)
        {
            verifiedAnswer.IsManual = false;
            if (verifiedAnswer.Comment?.Length > 255)
            {
                verifiedAnswer.Comment = verifiedAnswer.Comment.Substring(0, 255);
            }
        }

        return verifiedAnswers ?? [];
    }

    private async Task AddVerifiedAnswers(IList<VerifiedAnswerEntity> verifiedAnswers)
    {
        await _dbContext.VerifiedAnswers.AddRangeAsync(verifiedAnswers);
        await _dbContext.SaveChangesAsync();
    }
}
