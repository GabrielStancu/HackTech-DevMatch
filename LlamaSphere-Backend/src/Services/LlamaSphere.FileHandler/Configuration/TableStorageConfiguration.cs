namespace LlamaSphere.API.Configuration;

public class TableStorageConfiguration
{
    public string ConnectionString { get; set; }
    public string CvsTableName { get; set; }
    public string JobsTableName { get; set; }
}
