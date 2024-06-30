using MaterialAdvisor.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpGet]
    public IActionResult Get(string name, string email)
    {
        return Ok(authService.GenerateJwtToken(name, email));
    }
}
