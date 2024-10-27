using FileHandler.Models;

namespace FileHandler.Services;

public interface IAlertEmailSender
{
    Task SendEmailAlertAsync(NewAlertEmailModel emailAlert);
}