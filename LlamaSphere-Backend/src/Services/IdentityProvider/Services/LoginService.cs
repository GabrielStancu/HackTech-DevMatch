using AutoMapper;
using IdentityProvider.Dtos;
using IdentityProvider.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityProvider.Services;

public interface ILoginService
{
    public Task<UserDto> LoginAsync(LoginDto loginDto);
}

public class LoginService : ILoginService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public LoginService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<UserDto> LoginAsync(LoginDto loginDto)
    {
        // Check the user exists
        var user = await _userManager.FindByEmailAsync(loginDto.UserName);
        if (user == null)
        {
            user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user is null)
                return null;
        }

        // Check the password
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!result.Succeeded)
            return null;

        // Create the return user object
        var loggedUser = _mapper.Map<UserDto>(user);
        loggedUser.Token = _tokenService.CreateToken(user);

        return loggedUser;
    }
}
