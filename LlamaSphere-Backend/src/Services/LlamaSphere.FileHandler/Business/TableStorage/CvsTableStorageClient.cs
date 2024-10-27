using Azure.Data.Tables;
using LlamaSphere.API.Configuration;
using LlamaSphere.API.Entities;
using Microsoft.Extensions.Options;

namespace LlamaSphere.API.Business.TableStorage;

public class CvsTableStorageClient : TableStorageClient<CvEntity>
{
    private readonly TableStorageConfiguration _configuration;

    public CvsTableStorageClient(IOptions<TableStorageConfiguration> configuration, ILogger<CvsTableStorageClient> logger) : base(logger)
    {
        _configuration = configuration.Value;
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
