namespace LlamaSphere.AppUser.DTOs.Developers;

public record UpdateDeveloperRequest(Guid Id, string FirstName, string LastName);

public record UpdateDeveloperResponse(bool Success);