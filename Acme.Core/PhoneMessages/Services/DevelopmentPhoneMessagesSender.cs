using System.Net.Mail;

namespace Acme.Core.PhoneMessages.Services;

public class DevelopmentPhoneMessagesSender : IPhoneMessagesSender
{
    public Task SendPhoneMessage(string recipient, string message)
    {
        Console.WriteLine($"Phone messages disabled. {Environment.NewLine}" + 
            $"Recipient: {recipient}{Environment.NewLine}" +
            $"Notification: {message}{Environment.NewLine}");
        
        return Task.CompletedTask;
    }
}
