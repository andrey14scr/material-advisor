using MaterialAdvisor.Data.Enums;
using MaterialAdvisor.Data.Extensions;

using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;

public class DictionaryController : BaseApiController
{
    [HttpGet("languages")]
    public IActionResult GetLanguages()
    {
        return Ok(EnumConverter.ToDictionary<Language>());
    }

    [HttpGet("question-types")]
    public IActionResult GetQuestionTypes()
    {
        return Ok(EnumConverter.ToDictionary<QuestionType>());
    }
}