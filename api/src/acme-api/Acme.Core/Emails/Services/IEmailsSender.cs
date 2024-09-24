using System.Net.Mail;

namespace Acme.Core.Emails.Services;

public interface IEmailsSender
{
    Task SendEmail(MailMessage mailMessage);
}
