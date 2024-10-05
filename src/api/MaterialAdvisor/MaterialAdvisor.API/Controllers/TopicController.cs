using MaterialAdvisor.Application.Models.Topics;
using MaterialAdvisor.Application.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;

[Authorize]
public class TopicController(ITopicService _topicService) : BaseApiController
{
    [HttpGet()]
    public async Task<ActionResult<IList<TopicListItem>>> Get()
    {
        var result = await _topicService.Get<TopicListItem>();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Topic>> GetById(Guid id)
    {
        var result = await _topicService.Get<Topic>(id);
        return Ok(result);
    }

    [HttpPost()]
    public async Task<ActionResult<Topic>> CreateOrUpdate(Topic topic)
    {
        if (topic.Id == Guid.Empty)
        {
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
