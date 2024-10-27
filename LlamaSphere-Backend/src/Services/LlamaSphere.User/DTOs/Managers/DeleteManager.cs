namespace LlamaSphere.AppUser.DTOs.Managers;

public record DeleteManagerRequest(Guid Id);

public record DeleteManagerResponse(bool Success);