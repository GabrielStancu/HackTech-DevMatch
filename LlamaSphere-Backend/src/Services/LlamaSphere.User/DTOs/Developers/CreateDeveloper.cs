namespace LlamaSphere.AppUser.DTOs.Developers;

public record CreateDeveloperRequest(string FirstName, string LastName);

public record CreateDeveloperResponse(Guid Id);