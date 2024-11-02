using MaterialAdvisor.Application.Models.KnowledgeChecks;
using MaterialAdvisor.Application.Services.Abstraction;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;

[Authorize]
public class VerifyController(ISubmittedAnswerService _submittedAnswerService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<Attempt?>> GetAnswersToVerify(Guid knowledgeCheckId)
    {
        var result = await _submittedAnswerService.GetUnverifiedAnswers<UnverifiedAnswer>(knowledgeCheckId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Attempt?>> VerifyAnswer(VerifiedAnswer verifiedAnswer)
    {
        var result = await _submittedAnswerService.VerifyAnswer(verifiedAnswer);
        return Ok(result);
    }
}