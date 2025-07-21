using BmsIngest.Services.InsertionManager;
using Quartz;

namespace BmsIngest.Scheduler.Jobs;

/// <summary>
/// Job to insert information from the BMS system into InfluxDb
/// </summary>
public class InsertInformationJob : IJob
{

    /// <summary>
    /// Job key, used to uniquely identify job by scheduler
    /// Added for convenience when referencing
    /// </summary>
    public static JobKey Key { get; } = new("InsertTempsJob");
    
    private readonly IInsertionManagerService _insertionManager;

    public InsertInformationJob(IInsertionManagerService insertionManager)
    {
        _insertionManager = insertionManager;
    }
    
    /// <summary>
    /// Called by the <see cref="T:Quartz.IScheduler" /> when a <see cref="T:Quartz.ITrigger" />
    /// fires that is associated with the <see cref="T:Quartz.IJob" />.
    /// </summary>
    /// <remarks>
    /// The implementation may wish to set a  result object on the
    /// JobExecutionContext before this method exits.  The result itself
    /// is meaningless to Quartz, but may be informative to
    /// <see cref="T:Quartz.IJobListener" />s or
    /// <see cref="T:Quartz.ITriggerListener" />s that are watching the job's
    /// execution.
    /// </remarks>
    /// <param name="context">The execution context.</param>
    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"Running Insert Measurements - {DateTime.UtcNow}");
        await _insertionManager.InsertMeasurements();
    }
}