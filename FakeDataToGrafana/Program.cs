using FakeDataToGrafana;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<DataGeneratorService>();
        services.AddSingleton<IoTDataGenerator>();
        services.AddSingleton<LogDataGenerator>();
        services.AddSingleton<MetricsDataGenerator>();
        services.AddSingleton<PrometheusMetricsExporter>();
    })
    .Build();

// Inicia o servidor de métricas do Prometheus na porta 9090
var metricServer = new MetricServer(hostname: "localhost", port: 9090);
metricServer.Start();

Console.WriteLine("🚀 Gerador de dados fake iniciado!");
Console.WriteLine("📊 Métricas Prometheus disponíveis em: http://localhost:9090/metrics");
Console.WriteLine("📝 Logs sendo gerados no console");
Console.WriteLine("🌡️  Dados IoT sendo simulados");
Console.WriteLine("\nPressione CTRL+C para parar...\n");

await host.RunAsync();