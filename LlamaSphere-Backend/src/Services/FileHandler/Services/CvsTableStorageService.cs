using System.Text.Json;
using FileHandler.DTOs;
using FileHandler.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace FileHandler.Services;

public class CvsTableStorageService : ICvTableStorageService
{
    private readonly ITableStorageClient<CvEntity> _tableStorageClient;

    public CvsTableStorageService([FromKeyedServices("cvs")] ITableStorageClient<CvEntity> tableStorageClient)
    {
        _tableStorageClient = tableStorageClient;
    }

    public async Task UpdateTableStorageContent(ParsedCv parsedCv)
    {
        var oldEntity = await _tableStorageClient.GetEntityAsync(parsedCv.StructuredCv.Id, parsedCv.StructuredCv.Id);

        await _tableStorageClient.UpsertEntityAsync(new CvEntity
        {
            PartitionKey = parsedCv.StructuredCv.Id,
            RowKey = parsedCv.StructuredCv.Id,
            Content = oldEntity.Content,
            JsonContent = JsonSerializer.Serialize(parsedCv)
        });
    }
}
