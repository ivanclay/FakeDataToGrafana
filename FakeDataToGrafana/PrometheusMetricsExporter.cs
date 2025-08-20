using Prometheus;
using System;
using System.Diagnostics.Metrics;

namespace FakeDataToGrafana;

public class PrometheusMetricsExporter
{
    // IoT Metrics
    private readonly Gauge _temperatureGauge = Metrics.CreateGauge("iot_temperature_celsius", "Temperatura dos sensores IoT", new[] { "device_id", "location" });
    private readonly Gauge _humidityGauge = Metrics.CreateGauge("iot_humidity_percent", "Umidade dos sensores IoT", new[] { "device_id", "location" });
    private readonly Gauge _pressureGauge = Metrics.CreateGauge("iot_pressure_hpa", "Pressão dos sensores IoT", new[] { "device_id", "location" });
    private readonly Gauge _powerGauge = Metrics.CreateGauge("iot_power_watts", "Consumo de energia dos dispositivos IoT", new[] { "device_id", "location" });

    // System Metrics
    private readonly Gauge _cpuUsageGauge = Metrics.CreateGauge("system_cpu_usage_percent", "Uso de CPU do sistema");
    private readonly Gauge _memoryUsageGauge = Metrics.CreateGauge("system_memory_usage_percent", "Uso de memória do sistema");
    private readonly Gauge _diskUsageGauge = Metrics.CreateGauge("system_disk_usage_percent", "Uso de disco do sistema");
    private readonly Counter _networkInCounter = Metrics.CreateCounter("system_network_in_bytes_total", "Total de bytes recebidos pela rede");
    private readonly Counter _networkOutCounter = Metrics.CreateCounter("system_network_out_bytes_total", "Total de bytes enviados pela rede");
    private readonly Gauge _activeConnectionsGauge = Metrics.CreateGauge("system_active_connections", "Número de conexões ativas");
    private readonly Histogram _responseTimeHistogram = Metrics.CreateHistogram("http_request_duration_seconds", "Tempo de resposta das requisições HTTP");

    // Log Metrics
    private readonly Counter _logEntriesCounter = Metrics.CreateCounter("log_entries_total", "Total de entradas de log", new[] { "level", "source" });

    public void ExportIoTData(IoTSensorData data)
    {
        switch (data.SensorType)
        {
            case "temperature":
                _temperatureGauge.WithLabels(data.DeviceId, data.Location).Set(data.Value);
                break;
            case "humidity":
                _humidityGauge.WithLabels(data.DeviceId, data.Location).Set(data.Value);
                break;
            case "pressure":
                _pressureGauge.WithLabels(data.DeviceId, data.Location).Set(data.Value);
                break;
            case "power":
                _powerGauge.WithLabels(data.DeviceId, data.Location).Set(data.Value);
                break;
        }
    }

    public void ExportSystemMetrics(double cpu, double memory, double disk, long networkIn, long networkOut, int connections, double responseTime)
    {
        _cpuUsageGauge.Set(cpu);
        _memoryUsageGauge.Set(memory);
        _diskUsageGauge.Set(disk);
        _networkInCounter.Inc(networkIn);
        _networkOutCounter.Inc(networkOut);
        _activeConnectionsGauge.Set(connections);
        _responseTimeHistogram.Observe(responseTime / 1000.0); // Convert to seconds
    }

    public void ExportLogMetrics(LogEntry logEntry)
    {
        _logEntriesCounter.WithLabels(logEntry.Level, logEntry.Source).Inc();
    }
}
