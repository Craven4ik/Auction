using Auction.Domain.Interfaces.Services;
using Auction.Presentation.Contracts.Users;
using Microsoft.AspNetCore.Mvc;

namespace Auction.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService, ILogger<UserController> logger) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly ILogger<UserController> _logger = logger;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        _logger.LogInformation("Получен запрос на регистрацию: {@request.UserName}, {@request.Email}, {@request.Password}",
            request.UserName, request.Email, request.Password);

        await _userService.Register(request.UserName, request.Email, request.Password);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        _logger.LogInformation("Получен запрос на регистрацию: {@request.Email}, {@request.Password}",
            request.Email, request.Password);

        var token = await _userService.Login(request.Email, request.Password);

        HttpContext.Response.Cookies.Append("some-cookies", token);

        return Ok();
    }
}
