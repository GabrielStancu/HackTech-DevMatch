namespace LlamaSphere.Project.DTOs.Projects;

public record DeleteProjectRequest(Guid Id);

public record DeleteProjectResponse(bool Success);
