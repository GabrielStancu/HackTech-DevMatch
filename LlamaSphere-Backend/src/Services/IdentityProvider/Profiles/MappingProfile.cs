using AutoMapper;
using IdentityProvider.Dtos;
using IdentityProvider.Models;

namespace IdentityProvider.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AppUser, UserDto>();
        CreateMap<RegisterDto, AppUser>();
    }
}