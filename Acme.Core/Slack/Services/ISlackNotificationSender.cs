namespace Acme.Core.Slack.Services;

public interface ISlackNotificationSender
{
    Task SendNotification(string recipient, string notification);
}
