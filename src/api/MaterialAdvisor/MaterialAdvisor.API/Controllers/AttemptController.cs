using MaterialAdvisor.Application.Models.KnowledgeChecks;
using MaterialAdvisor.Application.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;

[Authorize]
public class AttemptController(ISubmittedAnswerService _submittedAnswerService, IAttemptService _attemptService) : BaseApiController
{
    [HttpPost("start")]
    public async Task<ActionResult<Attempt>> CreateAttempt(Attempt model)
    {
        var result = await _attemptService.Create(model);
        return Ok(result);
    }

    [HttpPost("submit-answer")]
    public async Task<ActionResult<SubmittedAnswer>> SubmitAnswer(SubmittedAnswer model)
    {
        var result = await _submittedAnswerService.CreateOrUpdate(model);
        return Ok(result);
    }

    [HttpPost("submit")]
    public async Task<ActionResult<bool>> SubmitAnswer(Guid id)
    {
        var result = await _attemptService.SetIsSubmit(id);
        return Ok(result);
    }
}