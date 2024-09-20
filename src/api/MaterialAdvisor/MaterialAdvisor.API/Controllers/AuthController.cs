using MaterialAdvisor.API.Exceptions;
using MaterialAdvisor.API.Models.Requests;
using MaterialAdvisor.API.Services;
using MaterialAdvisor.Application.Exceptions;
using MaterialAdvisor.Application.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace MaterialAdvisor.API.Controllers;

public class AuthController(TokensGenerator _tokensGenerator, IUserService _userService, IUserProvider _userProvider) : BaseApiController
{
    [AllowAnonymous]
    [HttpPost(Constants.Actions.Register)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = await _userService.Create(request.UserName, request.Email, request.Password);
        var tokens = await _tokensGenerator.Generate(user);
        return Ok(tokens);
    }

    [AllowAnonymous]
    [HttpPost(Constants.Actions.Login)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var userInfo = await _userService.Get(request.Login, request.Password);
            var tokens = await _tokensGenerator.Generate(userInfo);
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
    public async Task<IActionResult> Refresh([FromBody] string refreshToken)
    {
        var accessToken = Request.Headers[HeaderNames.Authorization]
            .ToString()
            .Replace("Bearer", string.Empty)
            .Trim();

        try
        {
            var userInfo = await _userProvider.GetUser();
            var tokens = await _tokensGenerator.Refresh(userInfo, refreshToken);
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
