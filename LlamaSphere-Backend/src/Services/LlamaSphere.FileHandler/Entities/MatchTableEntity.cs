using Azure;
using Azure.Data.Tables;

namespace LlamaSphere.API.Entities;

public class MatchTableEntity : ITableEntity
{
    public string Content { get; set; }
    public string JsonContent { get; set; }
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
