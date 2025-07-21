using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;

namespace BmsIngest.Models;

/// <summary>
/// Handy extensions for Information
/// </summary>
public static class InformationExtensions
{
    /// <summary>
    /// Adds timestamp to the PointData using the provided information
    /// Uses 1-Second precision
    /// </summary>
    /// <param name="extends"></param>
    /// <param name="information"></param>
    /// <returns></returns>
    public static PointData AddTimestamp(this PointData extends, Information information)
    {
        return extends.Timestamp(information.Timestamp, WritePrecision.S);
    }
}