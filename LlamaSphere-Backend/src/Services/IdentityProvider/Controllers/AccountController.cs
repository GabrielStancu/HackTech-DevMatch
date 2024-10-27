using IdentityProvider.Dtos;
using IdentityProvider.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProvider.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILoginService _loginService;
    private readonly IRegisterService _authenticationService;

    public AccountController(ILoginService loginService, IRegisterService authenticationService)
    {
        _loginService = loginService;
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        Console.WriteLine($"--> Authenticating user {loginDto.UserName}...");

        var user = await _loginService.LoginAsync(loginDto);

        return user ?? (ActionResult<UserDto>)Unauthorized();
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        Console.WriteLine($"--> Registering user {registerDto.UserName}...");

        var registerResult = await _authenticationService.RegisterAsync(registerDto);

        return registerResult.User ?? (ActionResult<UserDto>)BadRequest(registerResult.Message);
    }
}