using BmsIngest.Models;

namespace BmsIngest.Services.BmsRetrieval;

/// <summary>
/// Retrieves data from the BMS system
/// </summary>
public interface IBmsRetrievalService
{
    
    /// <summary>
    /// Get information from the BMS system and return in internal Information structure
    /// Returns null if there is an issue with retrieval or deserialization
    /// </summary>
    /// <returns></returns>
    Task<Information?> GetInformation();
}