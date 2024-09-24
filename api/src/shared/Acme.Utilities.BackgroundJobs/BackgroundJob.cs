using Microsoft.Extensions.Logging;
using Quartz;

namespace Acme.Utilities.BackgroundJobs;

public abstract class BackgroundJob : IJob, IBackgroundJob
{
    private readonly ILogger _logger;
    public readonly JobKey JobKey;

    protected BackgroundJob(ILogger logger, JobKey? jobKey = null)
    {
        _logger = logger;
        JobKey = new JobKey(GetType().ToString());
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await ExecuteJob(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "{Type} failed", GetType());
        }
    }

    protected abstract Task ExecuteJob(IJobExecutionContext context);
    public abstract ITrigger Trigger();

    public async Task ScheduleJob(IScheduler scheduler)
    {
        var trigger = Trigger();
        if (await scheduler.GetTrigger(trigger.Key) != null)
            return;

        var job = JobBuilder.Create(GetType())
            .WithIdentity(JobKey)
            .DisallowConcurrentExecution()
            .Build();

        await scheduler.ScheduleJob(job, trigger);
    }
}