namespace LlamaSphere.API.Configuration;

public class BlobStorageConfiguration
{
    public string ConnectionString { get; set; }
    public string CvsContainerName { get; set; }
    public string JobsContainerName { get; set; }
}
