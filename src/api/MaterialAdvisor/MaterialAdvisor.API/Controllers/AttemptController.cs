using MaterialAdvisor.Application.Models.KnowledgeChecks;
using MaterialAdvisor.Application.Services.Abstraction;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;

[Authorize]
public class AttemptController(ISubmittedAnswerService _submittedAnswerService,
    IAttemptService _attemptService) : BaseApiController
{
    [HttpGet()]
    public async Task<ActionResult<Attempt?>> GetLastAttempt(Guid knowledgeCheckId)
    {
        var result = await _attemptService.GetLast<StartedAttempt>(knowledgeCheckId);
        return Ok(result);
    }

    [HttpPost("start")]
    public async Task<ActionResult<Attempt>> CreateAttempt(CreateAttempt model)
    {
        var result = await _attemptService.Create<Attempt>(model);
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