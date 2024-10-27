using System.Text.Json.Serialization;

namespace LlamaSphere.API.DTOs;

public class JobMatchingCvs
{
    [JsonPropertyName("text_job")]
    public string Job { get; set; }

    [JsonPropertyName("devs_list")]
    public IEnumerable<string> Cvs { get; set; }
}
