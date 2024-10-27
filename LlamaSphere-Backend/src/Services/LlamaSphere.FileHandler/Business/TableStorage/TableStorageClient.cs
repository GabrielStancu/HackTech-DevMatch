using Azure.Data.Tables;
using LlamaSphere.API.Entities;

namespace LlamaSphere.API.Business.TableStorage;

public abstract class TableStorageClient<T> : ITableStorageClient<T> where T : MatchTableEntity
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

    public async Task<List<T>> GetEntitiesByKeywords(Dictionary<string, decimal> weightedKeywords)
    {
        await InitTableClientAsync();

        var entities = new Dictionary<T, decimal>();
        var searchedEntities = await TableClient.QueryAsync<T>().ToListAsync();

        foreach(var entity in searchedEntities.Where(e => weightedKeywords.Keys.Any(k => e.JsonContent.Contains(k))))//e => weightedKeywords.Keys.Any(k => e.JsonContent.Contains(k))))
        {
            decimal weightedSum = 0;

            foreach (var kvp in weightedKeywords)
            {
                var containsKeyword = entity.Content.Contains(kvp.Key);
                if (!containsKeyword)
                    continue;

                var weightedApp = kvp.Value * CountSubstringOccurrences(entity.Content, kvp.Key);
                weightedSum += weightedApp;
            }

            entities.Add(entity, weightedSum);
        }

        return entities
            .OrderByDescending(e => e.Value)
            .Take(5)
            .Select(kvp => kvp.Key)
            .ToList();
    }

    public async Task UpsertEntityAsync(T entity)
    {
        await InitTableClientAsync();

        await TableClient.UpsertEntityAsync(entity);
    }

    protected abstract Task InitTableClientAsync();

    private static int CountSubstringOccurrences(string source, string substring)
    {
        if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(substring))
        {
            return 0;
        }

        int count = 0;
        int index = 0;

        while ((index = source.IndexOf(substring, index, StringComparison.Ordinal)) != -1)
        {
            count++;
            index += substring.Length;
        }

        return count;
    }
}
