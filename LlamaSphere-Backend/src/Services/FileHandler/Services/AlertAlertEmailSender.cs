using Azure;
using Azure.Communication.Email;
using FileHandler.Configurations;
using FileHandler.Models;
using Microsoft.Extensions.Logging;

namespace FileHandler.Services;
public class AlertAlertEmailSender : IAlertEmailSender
{
    private readonly EmailConfiguration _emailConfiguration;
    private readonly ILogger<AlertAlertEmailSender> _logger;

    public AlertAlertEmailSender(EmailConfiguration emailConfiguration,
        ILogger<AlertAlertEmailSender> logger)
    {
        _emailConfiguration = emailConfiguration;
        _logger = logger;
    }

    public async Task SendEmailAlertAsync(NewAlertEmailModel emailAlert)
    {
        var emailClient = new EmailClient(_emailConfiguration.ConnectionString);
        var emailMessage = FormatAlertEmail(emailAlert);

        try
        {
            await emailClient.SendAsync(WaitUntil.Completed, emailMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError("Could not send the email alert, marking it as failed. Exception: <{Exception}>", ex.Message);
        }
    }

    private EmailMessage FormatAlertEmail(NewAlertEmailModel emailAlert)
    {
        string alertType = string.Empty, name = string.Empty, recipient = string.Empty;

        switch (emailAlert)
        {
            case NewCvEmailModel cvEmailModel:
                alertType = cvEmailModel.AlertType;
                name = cvEmailModel.Name;
                recipient = _emailConfiguration.RecruiterEmail;
                break;
            case NewJobEmailModel jobEmailModel:
                alertType = jobEmailModel.AlertType;
                name = jobEmailModel.Name;
                recipient = _emailConfiguration.CandidateMail;
                break;
        }

        var emailContent = new EmailContent($"New {alertType}!")
        {
            PlainText = $"A new {alertType} was found! Head into the application for more details about {name}"
        };
        var emailMessage = new EmailMessage(_emailConfiguration.FromEmail, recipient, emailContent);

        return emailMessage;
    }
}
