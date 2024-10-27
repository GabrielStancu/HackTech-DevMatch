namespace LlamaSphere.Project.DTOs.Projects;

public record GetProjectByIdRequest(Guid Id);

public record GetProjectByIdResponse(Models.Project Project);
