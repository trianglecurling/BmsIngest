using InfluxDB.Client.Writes;

namespace BmsIngest.Services.InfluxDb;

public interface IInfluxDbService
{

    /// <summary>
    /// Write the given PointData to InfluxDb, in the default bucket/organization
    /// </summary>
    /// <param name="data"></param>
    Task WritePointData(PointData[] data);

    /// <summary>
    /// Write the given PointData to InfluxDb, in the default bucket/organization
    /// </summary>
    /// <param name="data"></param>
    Task WritePointData(PointData data);
}