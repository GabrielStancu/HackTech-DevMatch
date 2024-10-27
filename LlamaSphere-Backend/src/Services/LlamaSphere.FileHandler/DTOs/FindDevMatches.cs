namespace LlamaSphere.API.DTOs;

// received from frontend
public class FindDevMatches
{
    public string ProjectId { get; set; }
    public Dictionary<string, decimal> Keywords { get; set; }
}
