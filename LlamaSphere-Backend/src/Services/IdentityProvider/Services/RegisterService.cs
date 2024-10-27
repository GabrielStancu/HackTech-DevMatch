using AutoMapper;
using IdentityProvider.Dtos;
using IdentityProvider.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityProvider.Services;

public interface IRegisterService
{
    public Task<(UserDto User, string Message)> RegisterAsync(RegisterDto registerDto);
}

public class RegisterService : IRegisterService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public RegisterService(
        UserManager<AppUser> userManager,
        ITokenService tokenService,
        IMapper mapper)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<(UserDto User, string Message)> RegisterAsync(RegisterDto registerDto)
    {
        if(await UserNameAlreadyInUseAsync(registerDto.UserName))
            return (null, "UserName is in use.");

        // Register the user
        var user = _mapper.Map<AppUser>(registerDto);
        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
            return (null, string.Join(';', result.Errors));

        // Create return object
        var registeredUser = new UserDto
        {
            ExternalId = registerDto.ExternalId,
            UserName = registerDto.UserName,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Role = registerDto.Role,
            Token = _tokenService.CreateToken(user)
        };

        return (registeredUser, string.Empty);
    }

    private async Task<bool> UserNameAlreadyInUseAsync(string userName)
        => await _userManager.FindByNameAsync(userName) != null;
}