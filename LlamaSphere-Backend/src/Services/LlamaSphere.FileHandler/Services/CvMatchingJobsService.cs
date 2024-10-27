using LlamaSphere.API.Business.TableStorage;
using LlamaSphere.API.DTOs;
using LlamaSphere.API.Entities;

namespace LlamaSphere.API.Services;

public class CvMatchingJobsService : ICvMatchingJobsService
{
    private readonly ITableStorageClient<JobEntity> _jobTableStorageClient;
    private readonly ITableStorageClient<CvEntity> _cvTableStorageClient;

    public CvMatchingJobsService([FromKeyedServices("job")] ITableStorageClient<JobEntity> jobTableStorageClient,
        [FromKeyedServices("cv")] ITableStorageClient<CvEntity> cvTableStorageClient)
    {
        _jobTableStorageClient = jobTableStorageClient;
        _cvTableStorageClient = cvTableStorageClient;
    }

    public async Task<CvMatchingJobs> GetMatchingCvsForJobAsync(Guid cvId)
    {
        var cv = await _cvTableStorageClient.GetEntityAsync(cvId.ToString(), cvId.ToString());
        var matchingJobs = await _jobTableStorageClient.GetEntitiesAsync();
        var cvMatchingJobs = new CvMatchingJobs
        {
            Cv = cv.Content,
            Jobs = matchingJobs.Select(j => j.Content)
        };

        return cvMatchingJobs;
    }
}
