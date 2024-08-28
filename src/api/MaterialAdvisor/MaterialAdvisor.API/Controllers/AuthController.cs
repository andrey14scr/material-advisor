using MaterialAdvisor.API.Exceptions;
using MaterialAdvisor.API.Services;
using MaterialAdvisor.Application.Exceptions;
using MaterialAdvisor.Application.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace MaterialAdvisor.API.Controllers;

public class AuthController(TokensGenerator tokensGenerator, IUserService userService, IUserProvider userProvider) : BaseApiController
{
    [AllowAnonymous]
    [HttpPost(Constants.Actions.Register)]
    public async Task<IActionResult> Register(string userName, string email, string hash)
    {
        var userInfo = await userService.Create(userName, email, hash);
        var tokens = await tokensGenerator.Generate(userInfo);
        return Ok(tokens);
    }

    [AllowAnonymous]
    [HttpPost(Constants.Actions.Login)]
    public async Task<IActionResult> Login(string login, string hash)
    {
        try
        {
            var userInfo = await userService.Get(login, hash);
            var tokens = await tokensGenerator.Generate(userInfo);
            return Ok(tokens);
        }
        catch (NotFoundException)
        {
            return Unauthorized();
        }
        catch
        {
            throw;
        }
    }

    [Authorize]
    [HttpPost(Constants.Actions.Refresh)]
    public async Task<IActionResult> Refresh(string refreshToken)
    {
        var accessToken = Request.Headers[HeaderNames.Authorization]
            .ToString()
            .Replace("Bearer", string.Empty)
            .Trim();

        try
        {
            var userInfo = await userProvider.GetUser();
            var tokens = await tokensGenerator.Refresh(userInfo, refreshToken);
            return Ok(tokens);
        }
        catch (RefreshTokenExpiredException)
        {
            return Unauthorized();
        }
        catch (NotFoundException)
        {
            return Unauthorized();
        }
        catch
        {
            throw;
        }
    }
}
