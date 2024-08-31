using MaterialAdvisor.Application.Models.Editable;
using MaterialAdvisor.Application.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;

[Authorize]
public class KnowledgeCheckController(IKnowledgeCheckService _knowledgeCheckService) : BaseApiController
{
    [HttpGet("{id}")]
    public async Task<ActionResult<EditableKnowledgeCheck>> GetById(Guid id)
    {
        var result = await _knowledgeCheckService.Get<EditableKnowledgeCheck>(id);
        return Ok(result);
    }

    [HttpGet()]
    public async Task<ActionResult<IList<EditableKnowledgeCheck>>> Get()
    {
        var result = await _knowledgeCheckService.Get<EditableKnowledgeCheck>();
        return Ok(result);
    }

    [HttpGet("group/{groupId}")]
    public async Task<ActionResult<IList<EditableKnowledgeCheck>>> GetByGroupId(Guid groupId)
    {
        var result = await _knowledgeCheckService.GetByGroup<EditableKnowledgeCheck>(groupId);
        return Ok(result);
    }

    [HttpPost()]
    public async Task<ActionResult<EditableKnowledgeCheck>> CreateOrUpdate(EditableKnowledgeCheck knowledgeCheck)
    {
        if (knowledgeCheck.Id == Guid.Empty)
        {
            var result = await _knowledgeCheckService.Create(knowledgeCheck);
            return Ok(result);
        }
        else
        {
            var result = await _knowledgeCheckService.Update(knowledgeCheck);
            return Ok(result);
        }
    }

    [HttpDelete()]
    public async Task<ActionResult<bool>> Delete(Guid id)
    {
        var result = await _knowledgeCheckService.Delete(id);
        return Ok(result);
    }
}