using InfluxDB.Client;
using InfluxDB.Client.Writes;
using Microsoft.Extensions.Options;

namespace BmsIngest.Services.InfluxDb;

/// <summary>
/// Service to write information into the InfluxDB database
/// </summary>
public class InfluxDbService : IInfluxDbService
{
    private readonly InfluxDbServiceOptions _options;
    private readonly InfluxDBClient _client;

    public InfluxDbService(IOptions<InfluxDbServiceOptions> options)
    {
        _options = options.Value;
        _client = new InfluxDBClient(_options.Url, _options.Token);
    }

    /// <summary>
    /// Write the given PointData to InfluxDb, in the default bucket/organization
    /// </summary>
    /// <param name="data"></param>
    public async Task WritePointData(PointData[] data)
    {
        WriteApiAsync? write = _client.GetWriteApiAsync();
        await write.WritePointsAsync(data, _options.Bucket, _options.Organization);
    }

    /// <summary>
    /// Write the given PointData to InfluxDb, in the default bucket/organization
    /// </summary>
    /// <param name="data"></param>
    public async Task WritePointData(PointData data)
    {
        WriteApiAsync? write = _client.GetWriteApiAsync();
        await write.WritePointAsync(data, _options.Bucket, _options.Organization);
    }

}