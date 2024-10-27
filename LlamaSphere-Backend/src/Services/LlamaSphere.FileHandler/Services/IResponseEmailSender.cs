using LlamaSphere.API.DTOs;

namespace LlamaSphere.API.Services;

public interface IResponseEmailSender
{
    Task SendResponseEmailAsync(EmailResponse emailResponse);
}