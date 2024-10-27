namespace LlamaSphere.Project.DTOs.Projects;

public record UpdateProjectRequest(Guid Id, string Name);

public record UpdateProjectResponse(bool Success);