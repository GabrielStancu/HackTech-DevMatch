namespace IdentityProvider.Dtos;

public class UserDto
{
    public string Token { get; set; }
    public string UserName { get; set; }
    public string ExternalId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
}
