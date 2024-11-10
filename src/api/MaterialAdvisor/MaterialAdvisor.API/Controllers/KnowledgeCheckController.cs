using MaterialAdvisor.Application.Models.KnowledgeChecks;
using MaterialAdvisor.Application.Models.Topics;
using MaterialAdvisor.Application.Services.Abstraction;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;

[Authorize]
public class KnowledgeCheckController(IKnowledgeCheckService _knowledgeCheckService) : BaseApiController
{
    [HttpGet("{id}")]
    public async Task<ActionResult<KnowledgeCheck>> GetById(Guid id)
    {
        var result = await _knowledgeCheckService.Get<KnowledgeCheck>(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IList<KnowledgeCheck>>> Get()
    {
        var result = await _knowledgeCheckService.Get<TopicListItem<KnowledgeCheckListItem>>();
        return Ok(result);
    }

    [HttpGet("group/{groupId}")]
    public async Task<ActionResult<IList<KnowledgeCheck>>> GetByGroupId(Guid groupId)
    {
        var result = await _knowledgeCheckService.GetByGroup<KnowledgeCheck>(groupId);
        return Ok(result);
    }

    [HttpGet("topic/{topicId}")]
    public async Task<ActionResult<IList<KnowledgeCheck>>> GetByTopicId(Guid topicId)
    {
        var result = await _knowledgeCheckService.GetByTopic<KnowledgeCheck>(topicId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<KnowledgeCheck>> CreateOrUpdate(KnowledgeCheck knowledgeCheck)
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

    [HttpDelete]
    public async Task<ActionResult<bool>> Delete(Guid id)
    {
        var result = await _knowledgeCheckService.Delete(id);
        return Ok(result);
    }
}