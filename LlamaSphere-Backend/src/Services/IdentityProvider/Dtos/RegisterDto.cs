using System.ComponentModel.DataAnnotations;

namespace IdentityProvider.Dtos;

public class RegisterDto
{
    [Required]
    public string UserName { get; set; }

    [Required]
    //[RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$",
    //ErrorMessage = "Password must have 1 Uppercase, 1 Lowercase, 1 number and 1 non alpha-numeric and at least 6 characters")]
    public string Password { get; set; } = null!;

    [Required]
    public string ExternalId { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string Role { get; set; }
}