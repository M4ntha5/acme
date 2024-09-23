using Quartz;
using Quartz.Impl.Matchers;

namespace Acme.Utilities.BackgrouondJobs;

public class BackgroundJobScheduler(ISchedulerFactory schedulerFactory, IEnumerable<IBackgroundJob> backgroundJobs)
{
    public async Task ScheduleBackgroundJobs()
    {
        var scheduler = await schedulerFactory.GetScheduler();
        var triggerKeys = backgroundJobs.Select(j => j.Trigger().Key);

        await RemoveLegacyTriggers(scheduler, triggerKeys);

        foreach (var backgroundJob in backgroundJobs)
        {
            await backgroundJob.ScheduleJob(scheduler);
        }
    }

    private static async Task RemoveLegacyTriggers(IScheduler scheduler, IEnumerable<TriggerKey> triggerKeys)
    {
        var currentTriggerKeys = await scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup());
        var triggerKeysToDelete = currentTriggerKeys.Where(k => !triggerKeys.Contains(k)).ToList();

        await scheduler.UnscheduleJobs(triggerKeysToDelete);
    }
}