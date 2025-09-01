using BmsIngest.Models;
using BmsIngest.Services.BmsRetrieval;
using BmsIngest.Services.InfluxDb;
using InfluxDB.Client.Writes;

namespace BmsIngest.Services.InsertionManager;

/// <summary>
/// Service to get the data from the BMS system and insert it into InfluxDB
/// Uses IBmsRetrievalService to get data
/// Uses IInfluxDbService to insert data
/// </summary>
public class InsertionManagerService : IInsertionManagerService
{

    #region Influx Consts
    
    #region Measurements
    private const string MEASUREMENT_ICE_SENSOR = "IceSensor";
    private const string MEASUREMENT_AIR_SENSOR = "AirSensor";
    private const string MEASUREMENT_TEMPERATURE_SENSOR = "TemperatureSensor";
    private const string MEASUREMENT_CHILLER_INFORMATION = "ChillerInformation";
    private const string MEASUREMENT_DEHUMIDIFIER_INFORMATION = "DehumidifierInformation";
    #endregion
    
    #region Tags
    private const string TAG_KEY_AREA = "Area";
    private const string TAG_AREA_ICE_SHED = "IceShed";
    private const string TAG_AREA_OUTDOOR = "Outdoor";

    private const string TAG_KEY_LOCATION = "Location";
    private const string TAG_LOCATION_NEAR_HOG = "NearHog";
    private const string TAG_LOCATION_FAR_HOG = "FarHog";
    private const string TAG_LOCATION_NEAR_CHILLER = "NearChiller";
    
    private const string TAG_KEY_SHEET = "Sheet";
    private const string TAG_SHEET_A = "A";
    private const string TAG_SHEET_D = "D";
    #endregion

    #region Fields
    private const string FIELD_TEMPERATURE = "Temperature";
    private const string FIELD_HUMIDITY = "Humidity";
    private const string FIELD_DEW_POINT = "DewPoint";
    private const string FIELD_TEMPERATURE_SET_POINT = "SetPointTemperature";
    private const string FIELD_TEMPERATURE_INLET = "InletTemperature";
    private const string FIELD_TEMPERATURE_OUTLET = "OutletTemperature";
    private const string FIELD_CHILLER_LOAD = "Load";
    private const string FIELD_TEMPERATURE_PROCESS_VALUE = "ProcessValueTemperature";
    private const string FIELD_DEHUMIDIFIER_RUNNING = "Running";
    #endregion
    
    #endregion
    
    private readonly IInfluxDbService _influxDb;
    private readonly IBmsRetrievalService _retrieval;

    public InsertionManagerService(IInfluxDbService influxDb, IBmsRetrievalService retrieval)
    {
        _influxDb = influxDb;
        _retrieval = retrieval;
    }

    
    /// <summary>
    /// Get the current measurements from the BMS System and insert into InfluxDB
    /// </summary>
    /// <returns></returns>
    public async Task InsertMeasurements()
    {
        Information? information = await _retrieval.GetInformation();

        if (information.HasValue == false)
        {
            Console.WriteLine("Couldn't get temps from retrieval service");
            return;
        }

        var points = GetPointDataForInformation(information.Value).ToArray();

        await _influxDb.WritePointData(points);
    }

    /// <summary>
    /// Gets the InfluxDb PointData for the given information
    /// </summary>
    /// <param name="information"></param>
    /// <returns></returns>
    private static IEnumerable<PointData> GetPointDataForInformation(Information information)
    {
        // Ice Temp Sensors
        yield return PointData.Measurement(MEASUREMENT_ICE_SENSOR)
                              .AddTimestamp(information)
                              .Tag(TAG_KEY_AREA, TAG_AREA_ICE_SHED)
                              .Tag(TAG_KEY_LOCATION, TAG_LOCATION_NEAR_HOG)
                              .Tag(TAG_KEY_SHEET, TAG_SHEET_A)
                              .Field(FIELD_TEMPERATURE, information.SheetANearHogLineIceSensor.Temperature.ValueFahrenheit);
        yield return PointData.Measurement(MEASUREMENT_ICE_SENSOR)
                              .AddTimestamp(information)
                              .Tag(TAG_KEY_AREA, TAG_AREA_ICE_SHED)
                              .Tag(TAG_KEY_LOCATION, TAG_LOCATION_FAR_HOG)
                              .Tag(TAG_KEY_SHEET, TAG_SHEET_A)
                              .Field(FIELD_TEMPERATURE, information.SheetAFarHogLineIceSensor.Temperature.ValueFahrenheit);
        yield return PointData.Measurement(MEASUREMENT_ICE_SENSOR)
                              .AddTimestamp(information)
                              .Tag(TAG_KEY_AREA, TAG_AREA_ICE_SHED)
                              .Tag(TAG_KEY_LOCATION, TAG_LOCATION_NEAR_HOG)
                              .Tag(TAG_KEY_SHEET, TAG_SHEET_D)
                              .Field(FIELD_TEMPERATURE, information.SheetDNearHogLineIceSensor.Temperature.ValueFahrenheit);
        yield return PointData.Measurement(MEASUREMENT_ICE_SENSOR)
                              .AddTimestamp(information)
                              .Tag(TAG_KEY_AREA, TAG_AREA_ICE_SHED)
                              .Tag(TAG_KEY_LOCATION, TAG_LOCATION_FAR_HOG)
                              .Tag(TAG_KEY_SHEET, TAG_SHEET_D)
                              .Field(FIELD_TEMPERATURE, information.SheetDFarHogLineIceSensor.Temperature.ValueFahrenheit);
        
        // Ice Shed Air Sensor
        yield return PointData.Measurement(MEASUREMENT_AIR_SENSOR)
                              .AddTimestamp(information)
                              .Tag(TAG_KEY_AREA, TAG_AREA_ICE_SHED)
                              .Tag(TAG_KEY_LOCATION, TAG_LOCATION_FAR_HOG)
                              .Tag(TAG_KEY_SHEET, TAG_SHEET_A)
                              .Field(FIELD_TEMPERATURE, information.IceShedAirSensor.AirTemperature.ValueFahrenheit)
                              .Field(FIELD_HUMIDITY, information.IceShedAirSensor.RelativeHumidity)
                              .Field(FIELD_DEW_POINT, information.IceShedAirSensor.DewPoint.ValueFahrenheit);
        
        // Outdoor Temp
        yield return PointData.Measurement(MEASUREMENT_TEMPERATURE_SENSOR)
                              .AddTimestamp(information)
                              .Tag(TAG_KEY_AREA, TAG_AREA_OUTDOOR)
                              .Tag(TAG_KEY_LOCATION, TAG_LOCATION_NEAR_CHILLER)
                              .Field(FIELD_TEMPERATURE, information.OutdoorTemperature.ValueFahrenheit);
        
        // Chiller Information
        yield return PointData.Measurement(MEASUREMENT_CHILLER_INFORMATION)
                              .AddTimestamp(information)
                              .Field(FIELD_CHILLER_LOAD, 
                                  information.ChillerInformation.Load)
                              .Field(FIELD_TEMPERATURE_SET_POINT,
                                  information.ChillerInformation.SetPoint.ValueFahrenheit)
                              .Field(FIELD_TEMPERATURE_INLET,
                                  information.ChillerInformation.GlycolEnterTemp.ValueFahrenheit)
                              .Field(FIELD_TEMPERATURE_OUTLET,
                                  information.ChillerInformation.GlycolExitTemp.ValueFahrenheit)
                              .Field(FIELD_TEMPERATURE_PROCESS_VALUE,
                                  information.ChillerInformation.ProcessValue.ValueFahrenheit);

        // Dehumidifier Information
        yield return PointData.Measurement(MEASUREMENT_DEHUMIDIFIER_INFORMATION)
                              .AddTimestamp(information)
                              .Field(FIELD_DEHUMIDIFIER_RUNNING,
                                  information.DehumidifierRunning);
    }
    
}