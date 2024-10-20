using MaterialAdvisor.Application.Models.Shared;
using MaterialAdvisor.Application.Models.Users;
using MaterialAdvisor.Application.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;

[Authorize]
public class UserController(IUserService _userService) : BaseApiController
{
    [HttpPatch("settings")]
    public async Task<ActionResult> UpdateCurrentLanguage(UserSettings userSettings)
    {
        await _userService.UpdateSettings(userSettings);
        return Ok();
    }

    [HttpGet("language")]
    public async Task<ActionResult> GetCurrentLanguage()
    {
        var currentLanguage = await _userService.CetCurrentLanguage();
        return Ok(currentLanguage);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IList<User>>> Search(string input)
    {
        var result = await _userService.Search<Group>(input);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IList<User>>> Get(int page = 1, int pageSize = 100)
    {
        var pagination = new Pagination { Page = page, PageSize = pageSize };
        var result = await _userService.Get<User>(pagination);
        return Ok(result);
    }
}