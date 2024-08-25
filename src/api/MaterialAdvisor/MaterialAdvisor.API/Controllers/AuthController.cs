using MaterialAdvisor.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;

public class AuthController(AuthService authService, ILogger<TopicController> logger) : BaseApiController
{
    [HttpGet]
    public IActionResult Get(string username, string email)
    {
        return Ok("Bearer " + authService.GenerateJwtToken(username, email));
    }
}
