using System.Text.Json.Serialization;

namespace FileHandler.DTOs;

public class ParsedCv
{
    [JsonPropertyName("structured_cv")]
    public StructuredCv StructuredCv { get; set; }
}

public class StructuredCv
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string LastName { get; set; }

    [JsonPropertyName("technical_skills")]
    public List<TechnicalSkill> TechnicalSkills { get; set; }

    [JsonPropertyName("soft_skills")]
    public List<string> SoftSkills { get; set; }

    [JsonPropertyName("education")]
    public List<Education> Education { get; set; }

    [JsonPropertyName("work_experience")]
    public List<WorkExperience> WorkExperience { get; set; }

    [JsonPropertyName("projects")]
    public List<string> Projects { get; set; }

    [JsonPropertyName("contests")]
    public List<string> Contests { get; set; }

    [JsonPropertyName("certifications")]
    public List<string> Certifications { get; set; }

    [JsonPropertyName("foreign_languages")]
    public List<string> ForeignLanguages { get; set; }

    [JsonPropertyName("volunteering")]
    public List<string> Volunteering { get; set; }
}

public class TechnicalSkill
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("strength")]
    public int Strength { get; set; } // from 1 to 10
}

public class Education
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("duration")]
    public float Duration { get; set; } // in years
}

public class WorkExperience
{
    [JsonPropertyName("function")]
    public string Function { get; set; }

    [JsonPropertyName("duration")]
    public float Duration { get; set; } // in years
}
