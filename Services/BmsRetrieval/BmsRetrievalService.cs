using System.Net.Http.Headers;
using System.Text.Json;
using BmsIngest.Models;
using Microsoft.Extensions.Options;

namespace BmsIngest.Services.BmsRetrieval;

/// <summary>
/// Retrieves data from the BMS system
/// </summary>
public class BmsRetrievalService : IBmsRetrievalService
{
    private readonly BmsRetrievalServiceOptions _options;
    private readonly HttpClient _httpClient;

    public BmsRetrievalService(IOptions<BmsRetrievalServiceOptions> options, HttpClient httpClient)
    {
        _options = options.Value;
        _httpClient = httpClient;
    }

    /// <summary>
    /// Get information from the BMS system and return in internal Information structure
    /// Returns null if there is an issue with retrieval or deserialization
    /// Todo: Better error handling/logging
    /// </summary>
    /// <returns></returns>
    public async Task<Information?> GetInformation()
    {
        UriBuilder uri = new()
        {
            Host = _options.Hostname,
            Path = _options.UrlPath,
            Scheme = "http"
        };
            
        

        HttpRequestMessage request = new(HttpMethod.Get, uri.Uri);

        request.Headers.Authorization = GetAuthenticationHeader();

        HttpResponseMessage response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode == false)
            return null;
            
        string data = await response.Content.ReadAsStringAsync();

        BmsJsonResponse? apiResponse = JsonSerializer.Deserialize<BmsJsonResponse>(data);

        if (apiResponse == null)
            return null;

        return apiResponse.ToTempInformation();
    }

    /// <summary>
    /// Gets the authentication header for the HTTP request
    /// Uses Basic Auth, with username/password from the options.
    /// </summary>
    /// <returns></returns>
    private AuthenticationHeaderValue GetAuthenticationHeader()
    {
        var authenticationString = $"{_options.Username}:{_options.Password}";
        var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(authenticationString));
        
        return new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
    }
    
}