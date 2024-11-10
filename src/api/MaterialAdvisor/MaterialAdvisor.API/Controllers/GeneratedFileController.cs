using MaterialAdvisor.Application.Models.KnowledgeChecks;
using MaterialAdvisor.Application.Services.Abstraction;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;

[Authorize]
public class GeneratedFileController(IGeneratedFileService _generatedFileService) : BaseApiController
{
    [HttpGet("knowledge-check/{knowledgeCheckId}")]
    public async Task<IActionResult> GetByKnowledgeCheckId(Guid knowledgeCheckId)
    {
        var reponse = await _generatedFileService.GetByKnowledgeCheckId<GeneratedFile>(knowledgeCheckId);
        return Ok(reponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var reponse = await _generatedFileService.Get<GeneratedFile>(id);
        return Ok(reponse);
    }
}