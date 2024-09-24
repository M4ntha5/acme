using Quartz;

namespace Acme.Utilities.BackgroundJobs;

public interface IBackgroundJob
{
    ITrigger Trigger();
    Task ScheduleJob(IScheduler scheduler);
}