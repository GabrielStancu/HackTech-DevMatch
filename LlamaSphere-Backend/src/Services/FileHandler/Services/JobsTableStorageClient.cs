using Azure.Data.Tables;
using FileHandler.Configurations;
using FileHandler.Entities;
using Microsoft.Extensions.Logging;

namespace FileHandler.Services;
public class JobsTableStorageClient : TableStorageClient<JobEntity>
{
    private readonly TableStorageConfiguration _configuration;

    public JobsTableStorageClient(TableStorageConfiguration configuration,
        ILogger<TableStorageClient<JobEntity>> logger) : base(logger)
    {
        _configuration = configuration;
    }

    protected override async Task InitTableClientAsync()
    {
        if (TableClient != null)
            return;

        var serviceClient = new TableServiceClient(_configuration.ConnectionString);
        TableClient = serviceClient.GetTableClient(_configuration.JobsTableName);
        await TableClient.CreateIfNotExistsAsync();
    }
}
