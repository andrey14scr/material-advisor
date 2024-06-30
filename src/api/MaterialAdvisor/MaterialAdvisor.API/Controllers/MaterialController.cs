using MaterialAdvisor.API.Models;
using MaterialAdvisor.Data.Enums;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class MaterialController : ControllerBase
{
    [HttpGet("topic")]
    public Topic GetByTopicId(Guid id)
    {
        return new Topic
        {
            Id = id,
            Texts = CreateText("topic text"),
            Questions =
            [
                new Question
                {
                    Number = 1,
                    Points = 10,
                    Type = QuestionType.Select,
                    Texts = CreateText("question 1 text"),
                    Version = 1,
                    AnswerGroups =
                    [
                        new AnswerGroup
                        {
                            Number = 1,
                            Texts = CreateText("Answer group 1 text"),
                            Answers = [ CreateAnswer(1, "Answer 1 text"), CreateAnswer(2, "Answer 2 text")]
                        }
                    ]
                },
                new Question
                {
                    Number = 2,
                    Points = 10,
                    Type = QuestionType.Select,
                    Texts = CreateText("question 2 text"),
                    Version = 1,
                    AnswerGroups =
                    [
                        new AnswerGroup
                        {
                            Number = 1,
                            Texts = CreateText("Answer group 1 text"),
                            Answers = [ CreateAnswer(1, "Answer 1 text"), CreateAnswer(2, "Answer 2 text")]
                        }
                    ]
                },
                new Question
                {
                    Number = 3,
                    Points = 10,
                    Type = QuestionType.Select,
                    Texts = CreateText("question 3 text"),
                    Version = 1,
                    AnswerGroups =
                    [
                        new AnswerGroup
                        {
                            Number = 1,
                            Texts = CreateText("Answer group 1 text"),
                            Answers = [ CreateAnswer(1, "Answer 1 text"), CreateAnswer(2, "Answer 2 text")]
                        },
                        new AnswerGroup
                        {
                            Number = 2,
                            Texts = CreateText("Answer group 2 text"),
                            Answers = [ CreateAnswer(1, "Answer 1 text"), CreateAnswer(2, "Answer 2 text")]
                        }
                    ]
                }
            ]
        };
    }

    [HttpPost("topic")]
    public Topic CreateTopicQuestions(Guid id)
    {
        return new Topic
        {
            Id = id,
            Texts = CreateText("topic text"),
            Questions =
            [
                new Question
                {
                    Number = 1,
                    Points = 10,
                    Type = QuestionType.Select,
                    Texts = CreateText("question 1 text"),
                    Version = 1,
                    AnswerGroups =
                    [
                        new AnswerGroup
                        {
                            Number = 1,
                            Texts = CreateText("Answer group 1 text"),
                            Answers = [ CreateAnswer(1, "Answer 1 text"), CreateAnswer(2, "Answer 2 text")]
                        }
                    ]
                },
                new Question
                {
                    Number = 2,
                    Points = 10,
                    Type = QuestionType.Select,
                    Texts = CreateText("question 2 text"),
                    Version = 1,
                    AnswerGroups =
                    [
                        new AnswerGroup
                        {
                            Number = 1,
                            Texts = CreateText("Answer group 1 text"),
                            Answers = [ CreateAnswer(1, "Answer 1 text"), CreateAnswer(2, "Answer 2 text")]
                        }
                    ]
                },
                new Question
                {
                    Number = 3,
                    Points = 10,
                    Type = QuestionType.Select,
                    Texts = CreateText("question 3 text"),
                    Version = 1,
                    AnswerGroups =
                    [
                        new AnswerGroup
                        {
                            Number = 1,
                            Texts = CreateText("Answer group 1 text"),
                            Answers = [ CreateAnswer(1, "Answer 1 text"), CreateAnswer(2, "Answer 2 text")]
                        },
                        new AnswerGroup
                        {
                            Number = 2,
                            Texts = CreateText("Answer group 2 text"),
                            Answers = [ CreateAnswer(1, "Answer 1 text"), CreateAnswer(2, "Answer 2 text")]
                        }
                    ]
                }
            ]
        };
    }

    private static Answer CreateAnswer(byte number, string text)
    {
        return new Answer
        {
            Number = number,
            Points = 2,
            Texts = CreateText(text)
        };
    }

    private static List<LanguageText> CreateText(string text)
    {
        return new List<LanguageText> { new LanguageText { Text = text, Language = LanguageType.English } };
    }
}
