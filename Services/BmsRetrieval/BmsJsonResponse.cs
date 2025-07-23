using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using BmsIngest.Enums;
using BmsIngest.Models;
using JetBrains.Annotations;

namespace BmsIngest.Services.BmsRetrieval;

/// <summary>
/// Data we get back from the BMS System
/// Structure based on BMS json format,
/// for easy deserialization
/// </summary>
public class BmsJsonResponse
{
    [JsonPropertyName("timestamp")]
    [JsonConverter(typeof(BmsDateTimeConverter))]
    public DateTime Timestamp { get; init; }
        
    [JsonPropertyName("sheet_A_NearHogLine")]
    public DoubleData SheetANearHogLine { get; init; }
        
    [JsonPropertyName("sheet_D_NearHogLine")]
    public DoubleData SheetDNearHogLine { get; init; }
        
    [JsonPropertyName("sheet_A_FarHogLine")]
    public DoubleData SheetAFarHogLine { get; init; }
        
    [JsonPropertyName("sheet_D_FarHogLine")]
    public DoubleData SheetDFarHogLine { get; init; }
        
    [JsonPropertyName("iceShedAirTemp")]
    public DoubleData IceShedAirTemp { get; init; }
        
    [JsonPropertyName("iceShedRH")]
    public DoubleData IceShedRelativeHumidity { get; init; }
        
    [JsonPropertyName("iceShedDewPoint")]
    public DoubleData IceShedDewPoint { get; init; }
    
    
    [JsonPropertyName("%ChillerRunning")]
    public DoubleData ChillerRunning { get; init; }
    
    [JsonPropertyName("chillerGlycolEnterTemp")]
    public DoubleData ChillerGlycolEnterTemp { get; init; }
    
    
    [JsonPropertyName("chillerGlycolExitTemp")]
    public DoubleData ChillerGlycolExitTemp { get; init; }
    
    [JsonPropertyName("chillerSetPoint")]
    public DoubleData ChillerSetPoint { get; init; }
    
    [JsonPropertyName("outdoorTemp")]
    public DoubleData OutdoorTemp { get; init; }

    /// <summary>
    /// Convert to internal Information structure
    /// </summary>
    /// <returns></returns>
    public Information ToTempInformation()
    {
        return new Information
        {
            Timestamp = Timestamp,
            IceShedAirSensor = new AirSensorInformation()
            {
                AirTemperature = IceShedAirTemp.ToTemperatureInformation(),
                DewPoint = IceShedDewPoint.ToTemperatureInformation(),
                RelativeHumidity = IceShedRelativeHumidity.Data
            },
            SheetANearHogLineIceSensor = SheetANearHogLine.ToIceSensorInformation(),
            SheetAFarHogLineIceSensor = SheetAFarHogLine.ToIceSensorInformation(),
            SheetDNearHogLineIceSensor = SheetDNearHogLine.ToIceSensorInformation(),
            SheetDFarHogLineIceSensor = SheetDFarHogLine.ToIceSensorInformation(),
            ChillerInformation = new ChillerInformation()
            {
                ChillerLoad = ChillerRunning.Data,
                ChillerSetPoint = ChillerSetPoint.ToTemperatureInformation(),
                ChillerGlycolEnterTemp = ChillerGlycolEnterTemp.ToTemperatureInformation(),
                ChillerGlycolExitTemp = ChillerGlycolExitTemp.ToTemperatureInformation()
            },
            OutdoorTemperature = OutdoorTemp.ToTemperatureInformation()
        };
    }
}

/// <summary>
/// Data from the BMS system, stored as a double
/// </summary>
public readonly struct DoubleData
{
    [JsonPropertyName("out")]
    [UsedImplicitly]
    public double Data { get; init; }

    /// <summary>
    /// Converts data to IceSensorInformation, with the data as the temperature (in Fahrenheit)
    /// </summary>
    /// <returns></returns>
    public IceSensorInformation ToIceSensorInformation()
    {
        return new IceSensorInformation()
        {
            Temperature = ToTemperatureInformation()
        };
    }

    /// <summary>
    /// Converts data to TemperatureInformation, with the temperature in Fahrenheit
    /// </summary>
    /// <returns></returns>
    public TemperatureInformation ToTemperatureInformation()
    {
        return new TemperatureInformation(Data, TemperatureUnit.Fahrenheit);
    }
}

/// <summary>
/// Custom JSON converter for the BMS DateTime format, which is not quite ISO-8601
/// </summary>
public class BmsDateTimeConverter
    : JsonConverter<DateTime>
{
    /// <summary>
    /// Data format
    /// Sample Data:
    /// 2025-07-22 11:15:00.127-0400
    /// </summary>
    private const string FORMAT = "yyyy-MM-dd HH:mm:ss.fffzzzz";

    public override DateTime Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var dateString = reader.GetString();
        if (DateTimeOffset.TryParseExact(
                dateString,
                FORMAT,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var result
            ))
        {
            return result.DateTime.ToUniversalTime();
        }

        throw new JsonException(
            $"Unable to parse \"{dateString}\" into a DateTimeOffset."
        );
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateTime value,
        JsonSerializerOptions options
    )
    {
        writer.WriteStringValue(
            value.ToString(FORMAT, CultureInfo.InvariantCulture)
        );
    }
}