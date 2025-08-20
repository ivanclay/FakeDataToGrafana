namespace FakeDataToGrafana;

public record IoTSensorData(
    string DeviceId,
    string SensorType,
    double Value,
    string Unit,
    DateTime Timestamp,
    string Location,
    double? Latitude = null,
    double? Longitude = null
);
