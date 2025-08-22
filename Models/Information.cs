using System.Text.Json.Serialization;
using BmsIngest.Enums;
using JetBrains.Annotations;

namespace BmsIngest.Models;

/// <summary>
/// Contains all the information from the BMS System, in a more organized format
/// </summary>
public readonly struct Information
{
    
    /// <summary>
    /// Timestamp when data was collected
    /// </summary>
    [PublicAPI]
    public DateTime Timestamp { get; init; }

    /// <summary>
    /// Temp/Humidity air sensor in the ice shed
    /// East wall, near Sheet A Far Hog
    /// </summary>
    [PublicAPI]
    public AirSensorInformation IceShedAirSensor { get; init; }

    /// <summary>
    /// Temp sensor in the ice
    /// Sheet A Far Hog
    /// </summary>
    [PublicAPI]
    public IceSensorInformation SheetANearHogLineIceSensor { get; init; }

    /// <summary>
    /// Temp sensor in the ice
    /// Sheet D Far Hog
    /// </summary>
    [PublicAPI]
    public IceSensorInformation SheetDNearHogLineIceSensor { get; init; }

    /// <summary>
    /// Temp sensor in the ice
    /// Sheet A Far Hog
    /// </summary>
    [PublicAPI]
    public IceSensorInformation SheetAFarHogLineIceSensor { get; init; }

    /// <summary>
    /// Temp sensor in the ice
    /// Sheet D Far Hog
    /// </summary>
    [PublicAPI]
    public IceSensorInformation SheetDFarHogLineIceSensor { get; init; }

    /// <summary>
    /// Information about the status of the chiller
    /// </summary>
    [PublicAPI]
    public ChillerInformation ChillerInformation { get; init; }

    /// <summary>
    /// Outdoor Temp Sensor
    /// Near the chiller
    /// </summary>
    [PublicAPI]
    public TemperatureInformation OutdoorTemperature { get; init; }
}

/// <summary>
/// Represents information from the ice sensors
/// Currently only temperature data
/// </summary>
public struct IceSensorInformation
{
    public TemperatureInformation Temperature { get; init; }
}

/// <summary>
/// Represents temperature information
/// Stores and converts between different units
/// </summary>
public readonly struct TemperatureInformation
{
    
    /// <summary>
    /// Native stored temperature
    /// </summary>
    [PublicAPI]
    public double Value { get; }
    
    /// <summary>
    /// Unit of native stored temperature
    /// </summary>
    [PublicAPI]
    public TemperatureUnit Unit { get; }
    
    public TemperatureInformation(double value, TemperatureUnit unit)
    {
        Value = value;
        Unit = unit;
    }

    /// <summary>
    /// Gets the temperature value in Celsius
    /// </summary>
    /// <exception cref="InvalidOperationException">If the Unit is out of range</exception>
    [JsonIgnore]
    [PublicAPI]
    public double ValueCelsius
    {
        get
        {
            switch (Unit)
            {
                case TemperatureUnit.Celsius:
                    return Value;
                case TemperatureUnit.Fahrenheit:
                    return FahrenheitToCelsius(Value);
                case TemperatureUnit.Kelvin:
                    return KelvinToCelsius(Value);
                default:
                    throw new InvalidOperationException("Unknown Unit");
            }
        }
    }

    /// <summary>
    /// Gets the temperature value in Fahrenheit
    /// </summary>
    /// <exception cref="InvalidOperationException">If the Unit is out of range</exception>
    [JsonIgnore]
    [PublicAPI]
    public double ValueFahrenheit
    {
        get
        {
            return Unit == TemperatureUnit.Fahrenheit ? Value : CelsiusToFahrenheit(ValueCelsius);
        }
    }

    /// <summary>
    /// Gets the temperature value in Kelvin
    /// </summary>
    /// <exception cref="InvalidOperationException">If the Unit is out of range</exception>
    [JsonIgnore]
    [PublicAPI]
    public double ValueKelvin
    {
        get
        {
            return Unit == TemperatureUnit.Kelvin ? Value : CelsiusToKelvin(ValueCelsius);
        }
    }

    /// <summary>
    /// Converts Fahrenheit temperature to Celsius.
    /// </summary>
    /// <param name="value">The temperature value in Fahrenheit.</param>
    /// <returns>The equivalent temperature value in Celsius.</returns>
    [PublicAPI]
    public static double FahrenheitToCelsius(double value)
    {
        return (value - 32) * 5 / 9;
    }

    /// <summary>
    /// Converts Celsius temperature to Fahrenheit.
    /// </summary>
    /// <param name="value">The temperature value in Celsius.</param>
    /// <returns>The equivalent temperature value in Fahrenheit.</returns>
    [PublicAPI]
    public static double CelsiusToFahrenheit(double value)
    {
        return (value * 9 / 5) + 32;
    }

    /// <summary>
    /// Converts Celsius temperature to Kelvin.
    /// </summary>
    /// <param name="value">The temperature value in Celsius.</param>
    /// <returns>The equivalent temperature value in Kelvin.</returns>
    [PublicAPI]
    public static double CelsiusToKelvin(double value)
    {
        return value + 273.15;
    }

    /// <summary>
    /// Converts Kelvin temperature to Celsius.
    /// </summary>
    /// <param name="value">The temperature value in Kelvin.</param>
    /// <returns>The equivalent temperature value in Celsius.</returns>
    [PublicAPI]
    public static double KelvinToCelsius(double value)
    {
        return value - 273.15;
    }
}

/// <summary>
/// Contains information from the air sensor
/// Includes air temperature, relative humidity, and dew point.
/// </summary>
public struct AirSensorInformation
{
    [PublicAPI]
    public TemperatureInformation AirTemperature { get; init; }
        
    /// <summary>
    /// Relative Humidity
    /// Range: 0 - 100
    /// </summary>
    [PublicAPI]
    public double RelativeHumidity { get; init; }
    
    [PublicAPI]
    public TemperatureInformation DewPoint { get; init; }
}

/// <summary>
/// Information about the current status of the chiller
/// </summary>
public struct ChillerInformation
{
    /// <summary>
    /// What load the chiller is running at - expressed as a percentage, 0-100
    /// </summary>
    [PublicAPI]
    public double Load { get; init; }
    
    /// <summary>
    /// Set point of the chiller
    /// </summary>
    [PublicAPI]
    public TemperatureInformation SetPoint { get; init; }
    
    /// <summary>
    /// Temperature of the glycol at the chiller inlet
    /// </summary>
    [PublicAPI]
    public TemperatureInformation GlycolEnterTemp { get; init; }
    
    /// <summary>
    /// Temperature of the glycol at the chiller outlet
    /// </summary>
    [PublicAPI]
    public TemperatureInformation GlycolExitTemp { get; init; }
    
    
    /// <summary>
    /// Process value the chiller is using for control
    /// This is the ice temp the chiller is currently using
    /// Selectable between min/mean/max on the chiller side
    /// </summary>
    [PublicAPI]
    public TemperatureInformation ProcessValue { get; init; }

}