using System.Text.Json;
using LlamaSphere.API.Business;
using LlamaSphere.API.Business.TableStorage;
using LlamaSphere.API.Configuration;
using LlamaSphere.API.DTOs;
using LlamaSphere.API.Entities;
using Microsoft.Extensions.Options;

namespace LlamaSphere.API.Services;

public class JobMatchingCvsService : IJobMatchingCvsService
{
    private readonly ITableStorageClient<JobEntity> _jobTableStorageClient;
    private readonly ITableStorageClient<CvEntity> _cvTableStorageClient;
    private readonly MatchingPerformanceConfiguration _matchingPerformanceConfiguration;
    private readonly HttpClient _httpClient;

    public JobMatchingCvsService([FromKeyedServices("job")] ITableStorageClient<JobEntity> jobTableStorageClient,
        [FromKeyedServices("cv")] ITableStorageClient<CvEntity> cvTableStorageClient,
        IHttpClientFactory factory,
        IOptions<MatchingPerformanceConfiguration> matchingPerformanceConfiguration)
    {
        _jobTableStorageClient = jobTableStorageClient;
        _cvTableStorageClient = cvTableStorageClient;
        _matchingPerformanceConfiguration = matchingPerformanceConfiguration.Value;
        _httpClient = factory.CreateClient("reasoning");
    }

    public async Task<List<ReasoningResponse>> GetMatchingCvsForJobAsync(FindDevMatches findDevMatches)
    {
        var job = await _jobTableStorageClient.GetEntityAsync(findDevMatches.ProjectId, findDevMatches.ProjectId);
        int loops = 20;

        while (string.IsNullOrWhiteSpace(job.JsonContent) && loops > 0)
        {
            await Task.Delay(1000);
            job = await _jobTableStorageClient.GetEntityAsync(findDevMatches.ProjectId, findDevMatches.ProjectId);
            loops--;
        }

        var matchingCvs = await _cvTableStorageClient.GetEntitiesAsync();
        var responses = new List<ReasoningResponse>();
        var scoreMatches = new Dictionary<ReasoningRequest, decimal>();

        foreach (var matchingCv in matchingCvs)
        {
            var structuredJob = JsonSerializer.Deserialize<ParsedJob>(job.JsonContent!).StructuredJob;
            var structuredCv = JsonSerializer.Deserialize<ParsedCv>(matchingCv.JsonContent).StructuredCv;
            var score = ScoreMatcher.CalculateMatchScore(structuredJob, structuredCv, findDevMatches.Keywords);
            var reasoningRequest = new ReasoningRequest
            {
                StructuredJob = structuredJob,
                StructuredCv = structuredCv
            };
            reasoningRequest.StructuredJob.HrRequirements = new List<HrRequirement>();

            foreach (var hrRequirement in findDevMatches.Keywords)
            {
                reasoningRequest.StructuredJob.HrRequirements.Add(new HrRequirement
                {
                    Skill = hrRequirement.Key,
                    Weight = hrRequirement.Value
                });
            }

            scoreMatches.Add(reasoningRequest, score);
        }

        var topMatches = scoreMatches
            .OrderByDescending(sm => sm.Value)
            .Take(_matchingPerformanceConfiguration.TopMatchesCount);

        foreach (var scoreMatch in topMatches)
        {
            var response = await _httpClient.PostAsJsonAsync("match", scoreMatch.Key);
            var responseContent = await response.Content.ReadAsStringAsync();
            var reasonResponse = JsonSerializer.Deserialize<ReasoningResponse>(responseContent);

            responses.Add(reasonResponse);
        }

        return responses.OrderByDescending(r => r.Score).Take(5).ToList();
    }

    public async Task<List<ReasoningResponse>> GetMatchingJobsForCvAsync(FindJobMatches findJobMatches)
    {
        var cv = await _cvTableStorageClient.GetEntityAsync(findJobMatches.CvId, findJobMatches.CvId);

        int loops = 10;

        while (string.IsNullOrWhiteSpace(cv.JsonContent) && loops > 0)
        {
            await Task.Delay(1000);
            cv = await _cvTableStorageClient.GetEntityAsync(findJobMatches.CvId, findJobMatches.CvId);
            loops--;
        }

        var matchingJobs = await _jobTableStorageClient.GetEntitiesAsync();
        var responses = new List<ReasoningResponse>();
        var scoreMatches = new Dictionary<ReasoningRequest, decimal>();

        foreach (var matchingJob in matchingJobs)
        {
            var structuredJob = JsonSerializer.Deserialize<ParsedJob>(matchingJob.JsonContent).StructuredJob;
            var structuredCv = JsonSerializer.Deserialize<ParsedCv>(cv.JsonContent!).StructuredCv;
            var reasoningRequest = new ReasoningRequest
            {
                StructuredJob = structuredJob,
                StructuredCv = structuredCv
            };
            reasoningRequest.StructuredJob.HrRequirements = new List<HrRequirement>();

            var score = ScoreMatcher.CalculateMatchScore(structuredJob, structuredCv,
                new Dictionary<string, decimal>());

            scoreMatches.Add(reasoningRequest, score);
        }

        var topMatches = scoreMatches
            .OrderByDescending(sm => sm.Value)
            .Take(_matchingPerformanceConfiguration.TopMatchesCount);

        foreach (var scoreMatch in topMatches)
        {
            var response = await _httpClient.PostAsJsonAsync("match", scoreMatch.Key);
            var responseContent = await response.Content.ReadAsStringAsync();
            var reasonResponse = JsonSerializer.Deserialize<ReasoningResponse>(responseContent);

            responses.Add(reasonResponse);
        }

        return responses.OrderByDescending(r => r.Score).Take(1).ToList();
    }
}
