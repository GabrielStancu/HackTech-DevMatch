namespace LlamaSphere.API.DTOs;

public class EmailResponse
{
    public string Subject { get; set; }
    public string Content { get; set; }
    public string ToEmail { get; set; }
    public string FromEmail { get; set; }
}
