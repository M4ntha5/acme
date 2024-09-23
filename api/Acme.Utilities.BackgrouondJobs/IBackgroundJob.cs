using Quartz;

namespace Acme.Utilities.BackgrouondJobs;

public interface IBackgroundJob
{
    ITrigger Trigger();
    Task ScheduleJob(IScheduler scheduler);
}