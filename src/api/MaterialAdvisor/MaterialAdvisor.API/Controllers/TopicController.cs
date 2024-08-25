using MaterialAdvisor.Application.Models.Editable;
using MaterialAdvisor.Application.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TopicController(ITopicService topicService) : BaseApiController
{
    [HttpGet()]
    public async Task<ActionResult<IList<EditableTopic>>> GetTopics()
    {
        var result = await topicService.Get<EditableTopic>();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EditableTopic>> GetByTopicIdAsync(Guid id)
    {
        var result = await topicService.Get<EditableTopic>(id);
        return Ok(result);
    }

    [HttpPost()]
    public async Task<ActionResult<EditableTopic>> CreateTopic(EditableTopic topic)
    {
        if (topic.Id == Guid.Empty)
        {
            var result = await topicService.Create(topic);
            return Ok(result);
        }
        else
        {
            var result = await topicService.Update(topic);
            return Ok(result);
        }
    }

    [HttpDelete()]
    public async Task<ActionResult<bool>> DeleteTopic(Guid topicId)
    {
        var result = await topicService.Delete(topicId);
        return Ok(result);
    }
}