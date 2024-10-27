using System.Text.Json.Serialization;

namespace LlamaSphere.API.DTOs;

public class CvMatchingJobs
{
    [JsonPropertyName("text_cv")]
    public string Cv { get; set; }

    [JsonPropertyName("job_list")]
    public IEnumerable<string> Jobs { get; set; }
}
