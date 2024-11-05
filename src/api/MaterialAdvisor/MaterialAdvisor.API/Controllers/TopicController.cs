using MaterialAdvisor.Application.Mapping;
using MaterialAdvisor.Application.Models.KnowledgeChecks;
using MaterialAdvisor.Application.Models.Topics;
using MaterialAdvisor.Application.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;

[Authorize]
public class TopicController(ITopicService _topicService, IKnowledgeCheckService _knowledgeCheckService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IList<TopicListItem<KnowledgeCheckTopicListItem>>>> Get()
    {
        var result = await _topicService.Get<TopicListItem<KnowledgeCheckTopicListItem>>();
        var dictionary = await _knowledgeCheckService.GetAttemptsCount(result.Select(t => t.Id).ToList());
        result.EnrichAttemptsCount(dictionary);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Topic>> GetById(Guid id)
    {
        var result = await _topicService.Get<Topic>(id);
        return Ok(result);
    }

    [HttpGet("list-item/{id}")]
    public async Task<ActionResult<Topic>> GetListItemById(Guid id)
    {
        var result = await _topicService.Get<TopicListItem<KnowledgeCheckListItem>>(id);
        return Ok(result);
    }

    [HttpGet("{id}/knowledge-check-topic")]
    public async Task<ActionResult<Attempt>> GetKnowledgeCheckTopic(Guid id)
    {
        var result = await _topicService.Get<KnowledgeCheckTopic>(id);
        return Ok(result);
    }

    [HttpPost()]
    public async Task<ActionResult<Topic>> CreateOrUpdate(Topic topic)
    {
        if (topic.Id == Guid.Empty)
        {
            topic.Version = 1;
            var result = await _topicService.Create(topic);
            return Ok(result);
        }
        else
        {
            var result = await _topicService.Update(topic);
            return Ok(result);
        }
    }

    [HttpDelete()]
    public async Task<ActionResult<bool>> Delete(Guid id)
    {
        var result = await _topicService.Delete(id);
        return Ok(result);
    }
}
