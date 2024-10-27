namespace LlamaSphere.AppUser.DTOs.Managers;

public record UpdateManagerRequest(Guid Id, string FirstName, string LastName);

public record UpdateManagerResponse(bool Success);