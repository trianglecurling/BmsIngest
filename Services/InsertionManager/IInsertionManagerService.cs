namespace BmsIngest.Services.InsertionManager;

/// <summary>
/// Service to get the data from the BMS system and insert it into InfluxDB
/// </summary>
public interface IInsertionManagerService
{
    /// <summary>
    /// Get the current measurements from the BMS System and insert into InfluxDB
    /// </summary>
    /// <returns></returns>
    Task InsertMeasurements();
}