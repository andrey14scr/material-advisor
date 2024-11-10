using MaterialAdvisor.Data;
using MaterialAdvisor.Data.Entities;
using MaterialAdvisor.Data.Enums;
using MaterialAdvisor.Storage;

using Microsoft.EntityFrameworkCore;

using System.Collections.Immutable;
using System.Text;

namespace MaterialAdvisor.Application.TableGenerator;

public class TableGenerator(IStorageService _storageService, MaterialAdvisorContext _materialAdvisorContext) : ITableGenerator
{
    public async Task<IEnumerable<TableGenerationResponse>> GenerateTable(IEnumerable<TableGenerationParameter> tableGenerationParameters)
    {
        var tasks = tableGenerationParameters.Select(Generate).ToArray();
        var results = await Task.WhenAll(tasks);
        return results?.ToList() ?? [];
    }

    private async Task<TableGenerationResponse> Generate(TableGenerationParameter tableGenerationParameter)
    {
        var (csv, knowledgeCheckName) = await GetCsv(tableGenerationParameter.KnowledgeCheckId);
        if (csv is null || knowledgeCheckName is null)
        {
            return new TableGenerationResponse
            {
                KnowledgeCheckId = tableGenerationParameter.KnowledgeCheckId,
                PreGeneratedFileId = tableGenerationParameter.PreGeneratedFileId
            };
        }
        var byteArray = Encoding.UTF8.GetBytes(csv);
        using var memoryStream = new MemoryStream(byteArray);
        var fileName = $"{knowledgeCheckName}.csv";
        var file = await _storageService.SaveFile(memoryStream, fileName);
        return new TableGenerationResponse
        {
            File = file,
            FileName = fileName,
            KnowledgeCheckId = tableGenerationParameter.KnowledgeCheckId,
            PreGeneratedFileId = tableGenerationParameter.PreGeneratedFileId
        };
    }

    private async Task<(string? Csv, string? KnowledgeCheckName)> GetCsv(Guid knowledgeCheckId)
    {
        var knowledgeCheck = await GetKnowledgeCheck(knowledgeCheckId);
        if (knowledgeCheck is null)
        {
            return (null, null);
        }
        var csv = BuildCsv(knowledgeCheck);
        var knowledgeCheckName = $"[{knowledgeCheck.StartDate.ToString("yyyy-MM-dd HH:mm")}] {knowledgeCheck.Name}";
        return (csv, knowledgeCheckName);
    }

    private async Task<KnowledgeCheckEntity?> GetKnowledgeCheck(Guid knowledgeCheckId)
    {
        var knowledgeCheck = await _materialAdvisorContext.KnowledgeChecks
            .Include(kc => kc.Topic).ThenInclude(t => t.Questions).ThenInclude(t => t.AnswerGroups)
            .Include(kc => kc.Attempts).ThenInclude(a => a.User)
            .Include(kc => kc.Attempts)
                .ThenInclude(a => a.SubmittedAnswers.Where(sa => sa.Value != null && sa.Value.Length != 0))
                .ThenInclude(sa => sa.AnswerGroup).ThenInclude(t => t.Question)
            .Include(kc => kc.Attempts)
                .ThenInclude(a => a.SubmittedAnswers.Where(sa => sa.Value != null && sa.Value.Length != 0))
                .ThenInclude(sa => sa.AnswerGroup).ThenInclude(t => t.Answers)
            .Include(kc => kc.Attempts)
                .ThenInclude(a => a.SubmittedAnswers.Where(sa => sa.Value != null && sa.Value.Length != 0))
                .ThenInclude(sa => sa.VerifiedAnswers)
            .SingleOrDefaultAsync(kc => kc.Id == knowledgeCheckId && !kc.Attempts.Any(a => !a.VerifiedAnswers.Any(va => va.IsManual)));
        return knowledgeCheck;
    }

    private string BuildCsv(KnowledgeCheckEntity knowledgeCheck)
    {
        var csvBuilder = new StringBuilder();

        csvBuilder.Append("User,Attempt,Attempt Start At,Attempt State,");

        foreach (var question in knowledgeCheck.Topic.Questions)
        {
            foreach (var answerGroup in question.AnswerGroups.OrderBy(ag => ag.Question.Number).ThenBy(ag => ag.Number))
            {
                var number = answerGroup.IsTechnical ? question.Number.ToString() : $"{question.Number}.{answerGroup.Number}";
                csvBuilder.Append($"{number}. Answer,{number}. Right Answer,{number}. Score,{number}. Max Score,");
            }
        }

        csvBuilder.Append("Total,Max Total,Percentage");

        var attempts = knowledgeCheck.Attempts.OrderBy(a => a.StartDate).ToImmutableList();

        foreach (var attempt in knowledgeCheck.Attempts.OrderBy(a => a.StartDate))
        {
            csvBuilder.AppendLine();

            csvBuilder.Append(attempt.User.Name);
            csvBuilder.Append(",");
            csvBuilder.Append(attempts.IndexOf(attempt) + 1);
            csvBuilder.Append(",");
            csvBuilder.Append(attempt.StartDate);
            csvBuilder.Append(",");
            csvBuilder.Append(attempt.IsCanceled ? "Canceled" : "Passed");
            csvBuilder.Append(",");

            var total = 0d;

            foreach (var submittedAnswer in attempt.SubmittedAnswers
                .OrderBy(sa => sa.AnswerGroup.Question.Number)
                .ThenBy(sa => sa.AnswerGroup.Number))
            {
                double score;

                switch (submittedAnswer.AnswerGroup.Question.Type)
                {
                    case QuestionType.Select:
                        var userSelectAnswer = submittedAnswer.AnswerGroup.Answers.Single(a => a.Id.ToString() == submittedAnswer.Value);
                        csvBuilder.Append(userSelectAnswer.Number);
                        csvBuilder.Append(",");
                        csvBuilder.Append(submittedAnswer.AnswerGroup.Answers
                            .Single(a => a.IsCorrect)
                            .Number);
                        csvBuilder.Append(",");
                        score = userSelectAnswer.Points;
                        total += score;
                        csvBuilder.Append(score);
                        csvBuilder.Append(",");
                        break;
                    case QuestionType.MultiSelect:
                        var userMultiSelectAnswer = submittedAnswer.AnswerGroup.Answers
                            .Where(a => submittedAnswer.Value!.Split(Constants.ListDelimeter).Contains(a.Id.ToString()));
                        csvBuilder.Append(string.Join(";", userMultiSelectAnswer.Select(a => a.Number)));
                        csvBuilder.Append(",");
                        csvBuilder.Append(string.Join(";", submittedAnswer.AnswerGroup.Answers
                            .Where(a => a.IsCorrect)
                            .Select(a => a.Number)));
                        csvBuilder.Append(",");
                        score = userMultiSelectAnswer.Sum(a => a.Points);
                        total += score;
                        csvBuilder.Append(score);
                        csvBuilder.Append(",");
                        break;
                    case QuestionType.OpenText:
                        csvBuilder.Append("<answer>");
                        csvBuilder.Append(",");
                        csvBuilder.Append(string.Empty);
                        csvBuilder.Append(",");
                        score = submittedAnswer.VerifiedAnswers.Single(va => va.IsManual).Score;
                        total += score;
                        csvBuilder.Append(score);
                        csvBuilder.Append(",");
                        break;
                }
                
                csvBuilder.Append(submittedAnswer.AnswerGroup.Question.Points);
                csvBuilder.Append(",");
            }

            csvBuilder.Append(total);
            csvBuilder.Append(",");
            var maxTotal = attempt.KnowledgeCheck.Topic.Questions.Sum(q => q.Points);
            csvBuilder.Append(maxTotal);
            csvBuilder.Append(",");
            csvBuilder.Append($"{(total * 100 / maxTotal):0.##}%");
            csvBuilder.Append(",");
        }

        return csvBuilder.ToString();
    }
}