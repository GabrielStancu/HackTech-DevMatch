using System.Text.Json.Serialization;

namespace FileHandler.DTOs;

public class ParseCvRequest
{
    [JsonPropertyName("text_cv")]
    public string CvText { get; set; }
}
