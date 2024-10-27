using System.Text.Json.Serialization;

namespace FileHandler.DTOs;

public class ParsedJob
{
    [JsonPropertyName("structured_job")]
    public StructuredJob StructuredJob { get; set; }
}

public class StructuredJob
{
    [JsonPropertyName("benefits")]
    public List<string> Benefits { get; set; }

    [JsonPropertyName("company_overview")]
    public string CompanyOverview { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("job_title")]
    public string JobTitle { get; set; }

    [JsonPropertyName("key_responsibilities")]
    public List<string> KeyResponsibilities { get; set; }

    [JsonPropertyName("preferred_skills")]
    public List<string> PreferredSkills { get; set; }

    [JsonPropertyName("required_qualifications")]
    public List<string> RequiredQualifications { get; set; }
}
