using MaterialAdvisor.Application.Models.Editable;
using MaterialAdvisor.Application.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;

[Authorize]
public class KnowledgeCheckController(IKnowledgeCheckService knowledgeCheckService) : BaseApiController
{
    [HttpGet("{id}")]
    public async Task<ActionResult<EditableKnowledgeCheck>> GetByKnowledgeCheckId(Guid id)
    {
        var result = await knowledgeCheckService.Get<EditableKnowledgeCheck>(id);
        return Ok(result);
    }

    [HttpGet()]
    public async Task<ActionResult<IList<EditableKnowledgeCheck>>> GetKnowledgeChecks()
    {
        var result = await knowledgeCheckService.Get<EditableKnowledgeCheck>();
        return Ok(result);
    }

    [HttpGet("group/{groupId}")]
    public async Task<ActionResult<IList<EditableKnowledgeCheck>>> GetByGroupId(Guid groupId)
    {
        var result = await knowledgeCheckService.GetByGroup<EditableKnowledgeCheck>(groupId);
        return Ok(result);
    }

    [HttpPost()]
    public async Task<ActionResult<EditableKnowledgeCheck>> CreateKnowledgeCheck(EditableKnowledgeCheck knowledgeCheck)
    {
        if (knowledgeCheck.Id == Guid.Empty)
        {
            var result = await knowledgeCheckService.Create(knowledgeCheck);
            return Ok(result);
        }
        else
        {
            var result = await knowledgeCheckService.SoftUpdate(knowledgeCheck);
            return Ok(result);
        }
    }

    [HttpDelete()]
    public async Task<ActionResult<bool>> DeleteKnowledgeCheck(Guid topicId)
    {
        var result = await knowledgeCheckService.Delete(topicId);
        return Ok(result);
    }
}