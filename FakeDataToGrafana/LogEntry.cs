namespace FakeDataToGrafana;

public record LogEntry(
    DateTime Timestamp,
    string Level,
    string Source,
    string Message,
    string? Exception = null,
    Dictionary<string, object>? Properties = null
);
