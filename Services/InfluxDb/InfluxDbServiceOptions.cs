using JetBrains.Annotations;

namespace BmsIngest.Services.InfluxDb;

public class InfluxDbServiceOptions
{
    /// <summary>
    /// URL of the InfluxDb service
    /// </summary>
    [PublicAPI]
    public string? Url { get; set; }
        
    /// <summary>
    /// API Token for authentication
    /// </summary>
    [PublicAPI]
    public string? Token { get; set; }
        
    /// <summary>
    /// Default organization to use
    /// </summary>
    [PublicAPI]
    public string? Organization { get; set; }
        
    /// <summary>
    /// Default bucket to use
    /// </summary>
    [PublicAPI]
    public string? Bucket { get; set; }
}