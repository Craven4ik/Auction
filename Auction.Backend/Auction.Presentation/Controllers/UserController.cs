using Auction.Domain.Interfaces.Services;
using Auction.Presentation.Contracts.Users;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Auction.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService, ILogger<UserController> logger, IMapper mapper) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly ILogger<UserController> _logger = logger;
    private readonly IMapper _mapper = mapper;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        _logger.LogInformation("Получен запрос на регистрацию: {@request.UserName}, {@request.Email}, {@request.Password}",
            request.UserName, request.Email, request.Password);

        if (await _userService.IsFreeEmail(request.Email))
            return BadRequest("Email not free");

        await _userService.Register(request.UserName, request.Email, request.Password);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        _logger.LogInformation("Получен запрос на авторизацию: {@request.Email}, {@request.Password}",
            request.Email, request.Password);

        try
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddHours(12)
            };

            var token = await _userService.Login(request.Email, request.Password);

            Response.Cookies.Delete("some-cookies");
            Response.Cookies.Append("some-cookies", token, cookieOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }

        return Ok();
    }

    [HttpGet("getUserInfo")]
    public async Task<IActionResult> GetUserInfo()
    {
        _logger.LogInformation("Поступил запрос на получение информации пользователя");

        var jwtToken = Request.Cookies["some-cookies"];

        if (jwtToken is null)
            return Unauthorized();

        var user = await _userService.GetById(jwtToken);

        var freeBalance = await _userService.GetFreeUserBalance(jwtToken);

        if (user is null)
            return BadRequest();

        var response = _mapper.Map<GetUserInfoResponse>(user);

        response.FreeBalance = freeBalance;

        return Ok(response);
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        _logger.LogInformation("Получен запрос на выход из аккаунта");

        Response.Cookies.Delete("some-cookies");

        return Ok();
    }

    [HttpPut("updateBalance")]
    public async Task<IActionResult> UpdateUserBalance([FromBody] UpdateBalanceRequest request)
    {
        _logger.LogInformation("Поступил запрос на пополнение баланса пользователя");

        var jwtToken = Request.Cookies["some-cookies"];

        if (jwtToken is null)
            return Unauthorized();

        try
        {
            await _userService.UpdateBalance(jwtToken, request.Amount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }

        return Ok();
    }

    [HttpGet("checkToken")]
    public IActionResult CheackToken()
    {
        _logger.LogInformation("Получен запрос на проверку токена");

        var jwtToken = Request.Cookies["some-cookies"];

        if (jwtToken is not null && _userService.CheckToken(jwtToken))
            return Ok(jwtToken);

        return Unauthorized();
    }

    [HttpGet("getUserId")]
    public IActionResult GetUserId()
    {
        _logger.LogInformation("Получен запрос на получение идентификатора пользователя");

        var jwtToken = Request.Cookies["some-cookies"];

        if (jwtToken is null)
            return BadRequest();

        var userId = _userService.GetUserIdFromToken(jwtToken);

        return Ok(userId);
    }

    [HttpGet("checkOwner/{id}")]
    public IActionResult CheckProductOwner([FromRoute] Guid id)
    {
        _logger.LogInformation("Получен запрос на проверку владения товаром");

        var jwtToken = Request.Cookies["some-cookies"];

        if (jwtToken is not null && _userService.CheckProductOwner(id, jwtToken))
            return Ok(true);

        return Ok(false);
    }

    [HttpGet("getBalance")]
    public async Task<IActionResult> GetUserBalance()
    {
        _logger.LogInformation("Получен запрос на получение баланса пользователя");

        var jwtToken = Request.Cookies["some-cookies"];

        if (jwtToken is null)
            return Unauthorized();

        return Ok(await _userService.GetUserBalance(jwtToken));
    }

    [HttpGet("getFreeBalance")]
    public async Task<IActionResult> GetFreeUserBalance()
    {
        _logger.LogInformation("Получен запрос на получение свободного баланса пользователя");

        var jwtToken = Request.Cookies["some-cookies"];

        if (jwtToken is null)
            return Unauthorized();

        return Ok(await _userService.GetFreeUserBalance(jwtToken));
    }

    [HttpGet("getUsername/{id}")]
    public async Task<IActionResult> GetUserName([FromRoute] Guid id)
    {
        _logger.LogInformation("Получен запрос на получение имени пользователя");

        var username = await _userService.GetUserName(id);

        if (string.IsNullOrEmpty(username))
            return BadRequest();

        return Ok(username);
    }
}
