using MaterialAdvisor.Application.Models.Users;
using MaterialAdvisor.Application.Services.Abstraction;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;

[Authorize]
public class GroupController(IGroupService _groupService) : BaseApiController
{
    [HttpGet("{id}")]
    public async Task<ActionResult<Group>> GetById(Guid id)
    {
        var result = await _groupService.Get<Group>(id);
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IList<Group>>> Search(string input)
    {
        var result = await _groupService.Search<Group>(input);
        return Ok(result);
    }

    [HttpGet("owner")]
    public async Task<ActionResult<IList<Group>>> GetAsOwner()
    {
        var result = await _groupService.GetAsOwner<Group>();
        return Ok(result);
    }

    [HttpGet("member")]
    public async Task<ActionResult<IList<Group>>> GetAsMember()
    {
        var result = await _groupService.GetAsMember<Group>();
        return Ok(result);
    }

    [HttpPost()]
    public async Task<ActionResult<Group>> CreateOrUpdate(Group group)
    {
        if (group.Id == Guid.Empty)
        {
            var result = await _groupService.Create(group);
            return Ok(result);
        }
        else
        {
            var result = await _groupService.Update(group);
            return Ok(result);
        }
    }

    [HttpDelete()]
    public async Task<ActionResult<bool>> Delete(Guid id)
    {
        var result = await _groupService.Delete(id);
        return Ok(result);
    }
}