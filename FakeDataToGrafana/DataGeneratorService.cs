using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FakeDataToGrafana;

public class DataGeneratorService : BackgroundService
{
    private readonly ILogger<DataGeneratorService> _logger;
    private readonly IoTDataGenerator _iotGenerator;
    private readonly LogDataGenerator _logGenerator;
    private readonly MetricsDataGenerator _metricsGenerator;
    private readonly PrometheusMetricsExporter _prometheusExporter;

    public DataGeneratorService(
        ILogger<DataGeneratorService> logger,
        IoTDataGenerator iotGenerator,
        LogDataGenerator logGenerator,
        MetricsDataGenerator metricsGenerator,
        PrometheusMetricsExporter prometheusExporter)
    {
        _logger = logger;
        _iotGenerator = iotGenerator;
        _logGenerator = logGenerator;
        _metricsGenerator = metricsGenerator;
        _prometheusExporter = prometheusExporter;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Gera dados IoT (a cada 5 segundos)
                await GenerateIoTData();

                // Gera logs (a cada 2 segundos)  
                await GenerateLogData();

                // Gera métricas de sistema (a cada 3 segundos)
                await GenerateSystemMetrics();

                await Task.Delay(1000, stoppingToken); // Aguarda 1 segundo
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante a geração de dados");
            }
        }
    }

    private async Task GenerateIoTData()
    {
        // Gera diferentes tipos de dados IoT
        var tasks = new[]
        {
            Task.Run(() =>
            {
                var tempData = _iotGenerator.GenerateTemperatureData();
                _prometheusExporter.ExportIoTData(tempData);
                _logger.LogInformation("🌡️  Temp: {Value}°C - Device: {Device} - Local: {Location}",
                    tempData.Value, tempData.DeviceId, tempData.Location);
            }),

            Task.Run(() =>
            {
                var humidityData = _iotGenerator.GenerateHumidityData();
                _prometheusExporter.ExportIoTData(humidityData);
                _logger.LogInformation("💧 Umidade: {Value}% - Device: {Device} - Local: {Location}",
                    humidityData.Value, humidityData.DeviceId, humidityData.Location);
            }),

            Task.Run(() =>
            {
                var pressureData = _iotGenerator.GeneratePressureData();
                _prometheusExporter.ExportIoTData(pressureData);
                _logger.LogInformation("📊 Pressão: {Value}hPa - Device: {Device} - Local: {Location}",
                    pressureData.Value, pressureData.DeviceId, pressureData.Location);
            }),

            Task.Run(() =>
            {
                var powerData = _iotGenerator.GeneratePowerConsumptionData();
                _prometheusExporter.ExportIoTData(powerData);
                _logger.LogInformation("⚡ Energia: {Value}W - Device: {Device} - Local: {Location}",
                    powerData.Value, powerData.DeviceId, powerData.Location);
            })
        };

        await Task.WhenAll(tasks);
    }

    private async Task GenerateLogData()
    {
        await Task.Run(() =>
        {
            var logEntry = _logGenerator.GenerateLogEntry();
            _prometheusExporter.ExportLogMetrics(logEntry);

            var logColor = logEntry.Level switch
            {
                "ERROR" => "❌",
                "WARN" => "⚠️",
                "INFO" => "ℹ️",
                _ => "🔍"
            };

            _logger.LogInformation("{Icon} [{Level}] {Source}: {Message}",
                logColor, logEntry.Level, logEntry.Source, logEntry.Message);
        });
    }

    private async Task GenerateSystemMetrics()
    {
        await Task.Run(() =>
        {
            var cpu = _metricsGenerator.GenerateCpuUsage();
            var memory = _metricsGenerator.GenerateMemoryUsage();
            var disk = _metricsGenerator.GenerateDiskUsage();
            var networkIn = _metricsGenerator.GenerateNetworkIn();
            var networkOut = _metricsGenerator.GenerateNetworkOut();
            var connections = _metricsGenerator.GenerateActiveConnections();
            var responseTime = _metricsGenerator.GenerateResponseTime();

            _prometheusExporter.ExportSystemMetrics(cpu, memory, disk, networkIn, networkOut, connections, responseTime);

            _logger.LogInformation("💻 Sistema - CPU: {Cpu}% | Mem: {Memory}% | Disco: {Disk}% | Conn: {Conn}",
                cpu, memory, disk, connections);
        });
    }
}
