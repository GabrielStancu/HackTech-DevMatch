using LlamaSphere.API.DTOs;

namespace LlamaSphere.API.Services;

public interface ICvMatchingJobsService
{
    Task<CvMatchingJobs> GetMatchingCvsForJobAsync(Guid cvId);
}