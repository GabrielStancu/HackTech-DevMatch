using LlamaSphere.AppUser.Models;

namespace LlamaSphere.AppUser.DTOs.Managers;

public record GetManagersRequest(int? PageNumber = 1, int? PageSize = 10);

public record GetManagersResponse(IEnumerable<Manager> Managers);