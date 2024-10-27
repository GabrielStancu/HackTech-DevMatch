namespace LlamaSphere.AppUser.DTOs.Developers;

public record DeleteDeveloperRequest(Guid Id);

public record DeleteDeveloperResponse(bool Success);