namespace LlamaSphere.AppUser.DTOs.Managers;

public record CreateManagerRequest(string FirstName, string LastName);

public record CreateManagerResponse(Guid Id);
