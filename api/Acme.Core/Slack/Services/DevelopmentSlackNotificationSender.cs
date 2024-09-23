namespace Acme.Core.Slack.Services;

public class DevelopmentSlackNotificationSender : ISlackNotificationSender
{
    public Task SendNotification(string recipient, string notificationMessage)
    {
        Console.WriteLine($"Slack notifications disabled. {Environment.NewLine}" + 
            $"Recipient: {recipient}{Environment.NewLine}" +
            $"Notification: {notificationMessage}{Environment.NewLine}");
        
        return Task.CompletedTask;
    }
}
