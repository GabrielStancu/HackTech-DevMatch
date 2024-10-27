using Azure;
using Azure.Data.Tables;

namespace FileHandler.Entities;

public class JobEntity : ITableEntity
{
    public string Content { get; set; }
    public string JsonContent { get; set; }
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
