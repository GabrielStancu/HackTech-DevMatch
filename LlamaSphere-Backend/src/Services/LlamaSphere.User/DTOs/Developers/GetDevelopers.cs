using LlamaSphere.AppUser.Models;

namespace LlamaSphere.AppUser.DTOs.Developers;

public record GetDevelopersRequest(int? PageNumber = 1, int? PageSize = 10);

public record GetDevelopersResponse(IEnumerable<Developer> Developers);