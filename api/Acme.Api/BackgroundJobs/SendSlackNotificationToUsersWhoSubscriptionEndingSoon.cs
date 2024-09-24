using Acme.Core.Slack.Services;
using Acme.Data.Contexts;
using Acme.Utilities.BackgrouondJobs;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Acme.Api.BackgroundJobs;

public class SendSlackNotificationToUsersWhoSubscriptionEndingSoon(ApplicationDbContext applicationDbContext, ISlackNotificationSender slackNotificationSender, ILogger<SendEmailNotificationToUsersWhoSubscriptionEndingSoon> logger) : BackgroundJob(logger)
{
    protected override async Task ExecuteJob(IJobExecutionContext context)
    {
        var oneMonthTillExpiration = DateOnly.FromDateTime(DateTime.Now.AddMonths(1));
        
        var subscribersToNotify = await applicationDbContext.Subscribers
            .Where(s => s.ExpirationDate <= oneMonthTillExpiration)
            .Take(2)
            .ToListAsync();
        
        if (!subscribersToNotify.Any())
        {
            return;
        }

        foreach (var subscriber in subscribersToNotify)
        {
            await slackNotificationSender.SendNotification(subscriber.Email, $"Your subscription ending in one month. {subscriber.ExpirationDate}");
        }
        
    }

    public override ITrigger Trigger()
    {
        var triggerKey = new TriggerKey($"{GetType()}Trigger");
        return TriggerBuilder.Create()
            .ForJob(JobKey)
            .WithSimpleSchedule(s => s.RepeatForever().WithInterval(TimeSpan.FromMinutes(3)))
            .WithIdentity(triggerKey)
            .Build();
    }
}
