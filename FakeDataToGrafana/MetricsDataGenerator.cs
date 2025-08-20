namespace FakeDataToGrafana;

public class MetricsDataGenerator
{
    private readonly Random _random = new();

    public double GenerateCpuUsage() => Math.Round(_random.NextDouble() * 80 + 10, 2);
    public double GenerateMemoryUsage() => Math.Round(_random.NextDouble() * 60 + 30, 2);
    public double GenerateDiskUsage() => Math.Round(_random.NextDouble() * 50 + 40, 2);
    public long GenerateNetworkIn() => _random.Next(1000, 50000);
    public long GenerateNetworkOut() => _random.Next(500, 25000);
    public int GenerateActiveConnections() => _random.Next(10, 200);
    public double GenerateResponseTime() => Math.Round(_random.NextDouble() * 500 + 50, 2);
}
