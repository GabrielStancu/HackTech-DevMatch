using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using FileHandler.Configurations;
using FileHandler.Entities;

namespace FileHandler.Services;
public class CvsTableStorageClient : TableStorageClient<CvEntity>
{
    private readonly TableStorageConfiguration _configuration;

    public CvsTableStorageClient(TableStorageConfiguration configuration, ILogger<CvsTableStorageClient> logger) : base(logger)
    {
        _configuration = configuration;
    }

    protected override async Task InitTableClientAsync()
    {
        if (TableClient != null)
            return;

        var serviceClient = new TableServiceClient(_configuration.ConnectionString);
        TableClient = serviceClient.GetTableClient(_configuration.CvsTableName);
        await TableClient.CreateIfNotExistsAsync();
    }
}