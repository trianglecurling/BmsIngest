using JetBrains.Annotations;

namespace BmsIngest.Services.BmsRetrieval;

public class BmsRetrievalServiceOptions
{
    /// <summary>
    /// Base Hostname/IP of the BMS controller
    /// </summary>
    [PublicAPI]
    public string? Hostname { get; set; }
    
    /// <summary>
    /// Path component of the information retrieval API endpoint
    /// </summary>
    [PublicAPI]
    public string? UrlPath { get; set; }
    
    /// <summary>
    /// Username for the API
    /// </summary>
    [PublicAPI]
    public string? Username { get; set; }
    
    /// <summary>
    /// Password for the API
    /// </summary>
    [PublicAPI]
    public string? Password { get; set; }
}