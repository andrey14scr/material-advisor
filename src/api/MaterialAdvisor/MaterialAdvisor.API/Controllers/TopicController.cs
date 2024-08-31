using MaterialAdvisor.Application.Models.Editable;
using MaterialAdvisor.Application.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;

[Authorize]
public class TopicController(ITopicService _topicService) : BaseApiController
{
    [HttpGet()]
    public async Task<ActionResult<IList<EditableTopic>>> Get()
    {
        var result = await _topicService.Get<EditableTopic>();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EditableTopic>> GetById(Guid id)
    {
        var result = await _topicService.Get<EditableTopic>(id);
        return Ok(result);
    }

    [HttpPost()]
    public async Task<ActionResult<EditableTopic>> CreateOrUpdate(EditableTopic topic)
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
