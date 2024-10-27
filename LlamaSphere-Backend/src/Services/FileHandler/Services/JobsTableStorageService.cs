using FileHandler.DTOs;
using FileHandler.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace FileHandler.Services;

public class JobsTableStorageService : IJobsTableStorageService
{
    private readonly ITableStorageClient<JobEntity> _tableStorageClient;

    public JobsTableStorageService([FromKeyedServices("jobs")] ITableStorageClient<JobEntity> tableStorageClient)
    {
        _tableStorageClient = tableStorageClient;
    }

    public async Task UpdateTableStorageContent(ParsedJob parsedJob)
    {
        var oldEntity = await _tableStorageClient.GetEntityAsync(parsedJob.StructuredJob.Id, parsedJob.StructuredJob.Id);

        await _tableStorageClient.UpsertEntityAsync(new JobEntity
        {
            PartitionKey = parsedJob.StructuredJob.Id,
            RowKey = parsedJob.StructuredJob.Id,
            Content = oldEntity.Content,
            JsonContent = JsonSerializer.Serialize(parsedJob)
        });
    }
}
