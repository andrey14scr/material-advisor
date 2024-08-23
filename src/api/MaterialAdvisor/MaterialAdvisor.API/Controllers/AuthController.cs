using MaterialAdvisor.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;

public class AuthController(AuthService authService, ILogger<MaterialController> logger) : BaseApiController
{
    [HttpGet]
    public IActionResult Get(string username, string email)
    {
        return Ok(authService.GenerateJwtToken(username, email));
    }
}
