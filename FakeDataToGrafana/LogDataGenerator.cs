namespace FakeDataToGrafana;

public class LogDataGenerator
{
    private readonly Random _random = new();
    private readonly string[] _sources = { "WebAPI", "Database", "AuthService", "PaymentGateway", "NotificationService" };
    private readonly string[] _logLevels = { "INFO", "WARN", "ERROR", "DEBUG" };
    private readonly string[] _infoMessages = {
        "Usuário {userId} fez login com sucesso",
        "Processando pedido #{orderId}",
        "Cache atualizado para região {region}",
        "Backup automatico concluído",
        "Configuração recarregada"
    };
    private readonly string[] _warnMessages = {
        "Taxa de CPU alta detectada: {cpuUsage}%",
        "Conexão lenta com banco de dados: {latency}ms",
        "Fila de processamento com {queueSize} itens pendentes",
        "Tentativa de login inválida para usuário {userId}"
    };
    private readonly string[] _errorMessages = {
        "Falha na conexão com banco de dados",
        "Erro ao processar pagamento para pedido #{orderId}",
        "Timeout na chamada para serviço externo",
        "Memória insuficiente no servidor"
    };

    public LogEntry GenerateLogEntry()
    {
        var level = _logLevels[_random.Next(_logLevels.Length)];
        var source = _sources[_random.Next(_sources.Length)];

        var (message, properties) = level switch
        {
            "INFO" => GenerateInfoLog(),
            "WARN" => GenerateWarnLog(),
            "ERROR" => GenerateErrorLog(),
            _ => GenerateDebugLog()
        };

        return new LogEntry(
            Timestamp: DateTime.UtcNow,
            Level: level,
            Source: source,
            Message: message,
            Properties: properties
        );
    }

    private (string message, Dictionary<string, object> properties) GenerateInfoLog()
    {
        var templates = _infoMessages;
        var template = templates[_random.Next(templates.Length)];

        return template switch
        {
            var t when t.Contains("{userId}") => (
                template.Replace("{userId}", $"user_{_random.Next(1000, 9999)}"),
                new Dictionary<string, object> { ["userId"] = $"user_{_random.Next(1000, 9999)}" }
            ),
            var t when t.Contains("{orderId}") => (
                template.Replace("{orderId}", _random.Next(10000, 99999).ToString()),
                new Dictionary<string, object> { ["orderId"] = _random.Next(10000, 99999) }
            ),
            var t when t.Contains("{region}") => (
                template.Replace("{region}", new[] { "BR-SOUTH", "BR-SOUTHEAST", "US-EAST" }[_random.Next(3)]),
                new Dictionary<string, object> { ["region"] = "BR-SOUTH" }
            ),
            _ => (template, new Dictionary<string, object>())
        };
    }

    private (string message, Dictionary<string, object> properties) GenerateWarnLog()
    {
        var templates = _warnMessages;
        var template = templates[_random.Next(templates.Length)];

        return template switch
        {
            var t when t.Contains("{cpuUsage}") => (
                template.Replace("{cpuUsage}", _random.Next(70, 95).ToString()),
                new Dictionary<string, object> { ["cpuUsage"] = _random.Next(70, 95) }
            ),
            var t when t.Contains("{latency}") => (
                template.Replace("{latency}", _random.Next(1000, 5000).ToString()),
                new Dictionary<string, object> { ["latency"] = _random.Next(1000, 5000) }
            ),
            var t when t.Contains("{queueSize}") => (
                template.Replace("{queueSize}", _random.Next(100, 1000).ToString()),
                new Dictionary<string, object> { ["queueSize"] = _random.Next(100, 1000) }
            ),
            _ => (template, new Dictionary<string, object>())
        };
    }

    private (string message, Dictionary<string, object> properties) GenerateErrorLog()
    {
        var template = _errorMessages[_random.Next(_errorMessages.Length)];

        var exception = _random.NextDouble() < 0.3 ?
            "System.Exception: Conexão recusada pelo servidor remoto" : null;

        return template switch
        {
            var t when t.Contains("{orderId}") => (
                template.Replace("{orderId}", _random.Next(10000, 99999).ToString()),
                new Dictionary<string, object>
                {
                    ["orderId"] = _random.Next(10000, 99999),
                    ["exception"] = exception ?? ""
                }
            ),
            _ => (template, new Dictionary<string, object> { ["exception"] = exception ?? "" })
        };
    }

    private (string message, Dictionary<string, object> properties) GenerateDebugLog()
    {
        var messages = new[] {
            "Executando query: SELECT * FROM users WHERE active = 1",
            "Cache hit para chave: user_session_{sessionId}",
            "Iniciando processo de limpeza de arquivos temporários",
            "Conectando com serviço externo: {serviceUrl}"
        };

        var template = messages[_random.Next(messages.Length)];
        return (template, new Dictionary<string, object>());
    }
}
