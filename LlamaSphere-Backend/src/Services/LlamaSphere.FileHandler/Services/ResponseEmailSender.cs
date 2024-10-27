using Azure;
using LlamaSphere.API.Configuration;
using LlamaSphere.API.DTOs;
using Microsoft.Extensions.Options;
using Azure.Communication.Email;

namespace LlamaSphere.API.Services;

public class ResponseEmailSender : IResponseEmailSender
{
    private readonly ILogger<ResponseEmailSender> _logger;
    private readonly ResponseEmailConfiguration _configuration;

    public ResponseEmailSender(IOptions<ResponseEmailConfiguration> configuration,
        ILogger<ResponseEmailSender> logger)
    {
        _logger = logger;
        _configuration = configuration.Value;
    }

    public async Task SendResponseEmailAsync(EmailResponse emailResponse)
    {
        var emailClient = new EmailClient(_configuration.ConnectionString);
        var emailMessage = FormatEmail(emailResponse);

        try
        {
            await emailClient.SendAsync(WaitUntil.Completed, emailMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError("Could not send the email alert, marking it as failed. Exception: <{Exception}>", ex.Message);
        }
    }

    private EmailMessage FormatEmail(EmailResponse emailResponse)
    {
        var emailContent = new EmailContent(emailResponse.Subject)
        {
            PlainText = emailResponse.Content
        };
        var fromEmail = string.IsNullOrWhiteSpace(emailResponse.FromEmail)
            ? _configuration.FromEmail
            : emailResponse.FromEmail;
        var toEmail = string.IsNullOrWhiteSpace(emailResponse.ToEmail)
            ? _configuration.ToEmail
            : emailResponse.ToEmail;
        var emailMessage = new EmailMessage(fromEmail, toEmail, emailContent);

        return emailMessage;
    }
}
