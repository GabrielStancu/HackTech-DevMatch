using System.Text.Json.Serialization;

namespace FileHandler.DTOs;

public class ParseJobRequest
{
    [JsonPropertyName("text_job")]
    public string JobText { get; set; }
}
