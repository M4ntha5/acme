using System.Net.Mail;

namespace Acme.Core.Emails.Services;

public class EmailSender : IEmailsSender
{
    public Task SendEmail(MailMessage mailMessage)
    {
        throw new NotImplementedException();
    }
}
