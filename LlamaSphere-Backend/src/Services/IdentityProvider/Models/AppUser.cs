using Microsoft.AspNetCore.Identity;

namespace IdentityProvider.Models;

public class AppUser : IdentityUser
{
    public Guid ExternalId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
}