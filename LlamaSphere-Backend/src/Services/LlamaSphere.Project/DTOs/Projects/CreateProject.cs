namespace LlamaSphere.Project.DTOs.Projects;

public record CreateProjectRequest(string Name);

public record CreateProjectResponse(Guid Id);
