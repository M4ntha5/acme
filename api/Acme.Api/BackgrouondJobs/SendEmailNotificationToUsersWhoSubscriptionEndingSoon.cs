using System.Net.Mail;
using Acme.Core.Emails.Services;
using Acme.Data.Contexts;
using Acme.Utilities.BackgrouondJobs;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Acme.Api.BackgrouondJobs;

public class SendEmailNotificationToUsersWhoSubscriptionEndingSoon(ApplicationDbContext applicationDbContext, IEmailsSender emailsSender, ILogger<SendEmailNotificationToUsersWhoSubscriptionEndingSoon> logger) : BackgroundJob(logger)
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
            await emailsSender.SendEmail(new MailMessage("noreply@acme.com", subscriber.Email, "Subscription ending soon", $"Your subscription ending in one month. {subscriber.ExpirationDate}"));
        }
        
    }

    public override ITrigger Trigger()
    {
        var triggerKey = new TriggerKey($"{GetType()}Trigger");
        return TriggerBuilder.Create()
            .ForJob(JobKey)
            .WithSimpleSchedule(s => s.RepeatForever().WithInterval(TimeSpan.FromMinutes(1)))
            .WithIdentity(triggerKey)
            .Build();
    }
}
