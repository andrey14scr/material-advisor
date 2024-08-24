using MaterialAdvisor.Application.Models.Editable;
using MaterialAdvisor.Application.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;

[Authorize]
public class MaterialController(ITopicService topicService) : BaseApiController
{
    [HttpGet(Constants.Entities.Topic)]
    public async Task<ActionResult<EditableTopic>> GetByTopicIdAsync(Guid id)
    {
        var result = await topicService.Get(id);
        return Ok(result);
    }

    [HttpPost(Constants.Entities.Topic)]
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

    [HttpDelete(Constants.Entities.Topic)]
    public async Task<ActionResult<bool>> CreateTopic(Guid topicId)
    {
        var result = await topicService.Delete(topicId);
        return Ok(result);
    }
}
