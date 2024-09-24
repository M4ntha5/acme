using Acme.Core.PhoneMessages.Services;
using Acme.Data.Contexts;
using Acme.Utilities.BackgrouondJobs;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Acme.Api.BackgroundJobs;

public class SendPhoneMessageNotificationToUsersWhoSubscriptionEndingSoon(ApplicationDbContext applicationDbContext, IPhoneMessagesSender phoneMessagesSender, ILogger<SendEmailNotificationToUsersWhoSubscriptionEndingSoon> logger) : BackgroundJob(logger)
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
            await phoneMessagesSender.SendPhoneMessage(subscriber.Email, $"Your subscription ending in one month. {subscriber.ExpirationDate}");
        }
    }

    public override ITrigger Trigger()
    {
        var triggerKey = new TriggerKey($"{GetType()}Trigger");
        return TriggerBuilder.Create()
            .ForJob(JobKey)
            .WithSimpleSchedule(s => s.RepeatForever().WithInterval(TimeSpan.FromMinutes(2)))
            .WithIdentity(triggerKey)
            .Build();
    }
}
