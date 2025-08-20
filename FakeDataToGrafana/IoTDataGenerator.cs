namespace FakeDataToGrafana;

public class IoTDataGenerator
{
    private readonly Random _random = new();
    private readonly string[] _devices = { "sensor-001", "sensor-002", "sensor-003", "gateway-01", "gateway-02" };
    private readonly string[] _locations = { "Sala A", "Sala B", "Almoxarifado", "Estacionamento", "Telhado" };

    public IoTSensorData GenerateTemperatureData()
    {
        return new IoTSensorData(
            DeviceId: _devices[_random.Next(_devices.Length)],
            SensorType: "temperature",
            Value: Math.Round(_random.NextDouble() * 15 + 20, 2), // 20-35°C
            Unit: "celsius",
            Timestamp: DateTime.UtcNow,
            Location: _locations[_random.Next(_locations.Length)],
            Latitude: -12.9714 + (_random.NextDouble() - 0.5) * 0.01, // Salvador, BA
            Longitude: -38.5014 + (_random.NextDouble() - 0.5) * 0.01
        );
    }

    public IoTSensorData GenerateHumidityData()
    {
        return new IoTSensorData(
            DeviceId: _devices[_random.Next(_devices.Length)],
            SensorType: "humidity",
            Value: Math.Round(_random.NextDouble() * 40 + 40, 2), // 40-80%
            Unit: "percent",
            Timestamp: DateTime.UtcNow,
            Location: _locations[_random.Next(_locations.Length)]
        );
    }

    public IoTSensorData GeneratePressureData()
    {
        return new IoTSensorData(
            DeviceId: _devices[_random.Next(_devices.Length)],
            SensorType: "pressure",
            Value: Math.Round(_random.NextDouble() * 50 + 1000, 2), // 1000-1050 hPa
            Unit: "hPa",
            Timestamp: DateTime.UtcNow,
            Location: _locations[_random.Next(_locations.Length)]
        );
    }

    public IoTSensorData GeneratePowerConsumptionData()
    {
        return new IoTSensorData(
            DeviceId: _devices[_random.Next(_devices.Length)],
            SensorType: "power",
            Value: Math.Round(_random.NextDouble() * 200 + 50, 2), // 50-250W
            Unit: "watts",
            Timestamp: DateTime.UtcNow,
            Location: _locations[_random.Next(_locations.Length)]
        );
    }
}
