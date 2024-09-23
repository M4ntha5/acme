using System.Net.Mail;

namespace Acme.Core.Emails.Services;

public class DevelopmentEmailSender : IEmailsSender
{
    public Task SendEmail(MailMessage mailMessage)
    {
        Console.WriteLine($"Email sending disabled. {Environment.NewLine}" + 
            $"To: {mailMessage.To}{Environment.NewLine}" + 
            $"Subjet: {mailMessage.Subject}{Environment.NewLine}" + 
            $"Body: {mailMessage.Body}{Environment.NewLine}");
        
        return Task.CompletedTask;
    }
}
