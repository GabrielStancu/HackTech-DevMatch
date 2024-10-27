namespace LlamaSphere.Project.DTOs.Projects;

public record GetProjectsRequest(int? PageNumber = 1, int? PageSize = 10);

public record GetProjectsResponse(IEnumerable<Models.Project> Projects);
