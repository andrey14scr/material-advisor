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
}