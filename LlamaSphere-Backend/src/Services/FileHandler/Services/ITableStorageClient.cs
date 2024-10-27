using Azure.Data.Tables;

namespace FileHandler.Services;

public interface ITableStorageClient<T> where T : class, ITableEntity
{
    Task<T> GetEntityAsync(string partitionKey, string rowKey);
    Task<List<T>> GetEntitiesAsync();
    Task UpsertEntityAsync(T entity);
}