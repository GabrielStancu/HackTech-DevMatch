using Microsoft.Extensions.Logging;
using Azure.Data.Tables;

namespace FileHandler.Services;
public abstract class TableStorageClient<T> : ITableStorageClient<T> where T : class, ITableEntity
{
    private readonly ILogger _logger;
    protected TableClient TableClient;

    protected TableStorageClient(ILogger<TableStorageClient<T>> logger)
    {
        _logger = logger;
    }

    public async Task<T> GetEntityAsync(string partitionKey, string rowKey)
    {
        await InitTableClientAsync();

        try
        {
            return await TableClient.GetEntityAsync<T>(partitionKey, rowKey);
        }
        catch (Exception ex)
        {
            _logger.LogError("Could not find entity with partition key {NotFoundPartitionKey} and row key {NotFoundRowKey}", partitionKey, rowKey);
            _logger.LogError("Exception while querying table storage: {TableStorageException} @ {TableStorageExceptionStackTrace}", ex.Message, ex.StackTrace);

            return null;
        }
    }

    public async Task<List<T>> GetEntitiesAsync()
    {
        await InitTableClientAsync();

        return await TableClient.QueryAsync<T>().ToListAsync();
    }

    public async Task UpsertEntityAsync(T entity)
    {
        await InitTableClientAsync();

        await TableClient.UpsertEntityAsync(entity);
    }

    protected abstract Task InitTableClientAsync();
}
