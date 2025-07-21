using BmsIngest.Scheduler.Jobs;
using Quartz;
using Quartz.AspNetCore;

namespace BmsIngest.Scheduler;

/// <summary>
/// Convenience class for setting up the scheduler, jobs, and triggers
/// </summary>
public static class SchedulerInfrastructure
{
    /// <summary>
    /// Adds the quartz scheduler to the service collection
    /// Adds necessary jobs and triggers
    /// </summary>
    /// <param name="extends"></param>
    public static void AddSchedulerService(this IServiceCollection extends)
    {
        extends.AddQuartz(options =>
        {
            options.AddJob<InsertInformationJob>(c =>
            {
                c.WithIdentity(InsertInformationJob.Key);
                c.StoreDurably();
            }).AddTrigger(trigger =>
                trigger.ForJob(InsertInformationJob.Key)
                       .WithSimpleSchedule(schedule => schedule.WithIntervalInMinutes(1).RepeatForever()));
        });

        extends.AddQuartzServer(options => { options.WaitForJobsToComplete = true; });
    }
}