using MaterialAdvisor.Application.Models.Shared;
using MaterialAdvisor.Application.Services;

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
}