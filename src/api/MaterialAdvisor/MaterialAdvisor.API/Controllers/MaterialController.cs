using AutoMapper;

using MaterialAdvisor.Application.Models.Editable;
using MaterialAdvisor.Application.Models.Shared;
using MaterialAdvisor.Application.Services;
using MaterialAdvisor.Data.Enums;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;

[Authorize]
public class MaterialController(ITopicService topicService, IMapper mapper, ILogger<MaterialController> logger) : BaseApiController
{
    [HttpGet(Constants.Entities.Topic)]
    public async Task<EditableTopic> GetByTopicIdAsync(Guid id, Language? language = null)
    {
        return new EditableTopic
        {
            Id = id,
            Texts = CreateText("topic text"),
            Questions =
            [
                new EditableQuestion
                {
                    Id = Guid.NewGuid(),
                    Number = 1,
                    Points = 10,
                    Type = QuestionType.Select,
                    Texts = CreateText("question 1 text"),
                    Version = 1,
                    AnswerGroups =
                    [
                        new EditableAnswerGroup
                        {
                            Id = Guid.NewGuid(),
                            Number = 1,
                            Texts = CreateText("Answer group 1 text"),
                            Answers = [ CreateAnswer(1, "Answer 1 text"), CreateAnswer(2, "Answer 2 text")]
                        }
                    ]
                },
                new EditableQuestion
                {
                    Id = Guid.NewGuid(),
                    Number = 2,
                    Points = 10,
                    Type = QuestionType.Select,
                    Texts = CreateText("question 2 text"),
                    Version = 1,
                    AnswerGroups =
                    [
                        new EditableAnswerGroup
                        {
                            Id = Guid.NewGuid(),
                            Number = 1,
                            Texts = CreateText("Answer group 1 text"),
                            Answers = [ CreateAnswer(1, "Answer 1 text"), CreateAnswer(2, "Answer 2 text")]
                        }
                    ]
                },
                new EditableQuestion
                {
                    Id = Guid.NewGuid(),
                    Number = 3,
                    Points = 10,
                    Type = QuestionType.Select,
                    Texts = CreateText("question 3 text"),
                    Version = 1,
                    AnswerGroups =
                    [
                        new EditableAnswerGroup
                        {
                            Id = Guid.NewGuid(),
                            Number = 1,
                            Texts = CreateText("Answer group 1 text"),
                            Answers = [ CreateAnswer(1, "Answer 1 text"), CreateAnswer(2, "Answer 2 text")]
                        },
                        new EditableAnswerGroup
                        {
                            Id = Guid.NewGuid(),
                            Number = 2,
                            Texts = CreateText("Answer group 2 text"),
                            Answers = [ CreateAnswer(1, "Answer 1 text"), CreateAnswer(2, "Answer 2 text")]
                        }
                    ]
                }
            ]
        };
    }

    [HttpPost(Constants.Entities.Topic)]
    public async Task<ActionResult<EditableTopic>> CreateTopic(EditableTopic topic)
    {
        var result = await topicService.Create(topic);
        return Ok(result);
    }

    [HttpPut(Constants.Entities.Topic)]
    public async Task<ActionResult<EditableTopic>> UpdateTopic(EditableTopic topic)
    {
        var result = await topicService.Update(topic);
        return Ok(result);
    }

    private static EditableAnswer CreateAnswer(byte number, string text)
    {
        return new EditableAnswer
        {
            Id = Guid.NewGuid(),
            Number = number,
            Points = 2,
            Texts = CreateText(text)
        };
    }

    private static List<LanguageText> CreateText(string text)
    {
        return new List<LanguageText> { new LanguageText { Id = Guid.NewGuid(), Text = text, LanguageId = Language.English } };
    }
}
