namespace HttpClients.Base;

public sealed class BaseProblemDetails
{
    public string Type { get; init; }
    public string Title { get; init; }
    public int Status { get; init; }
    public string Detail { get; init; }
    public string Instance { get; init; }
    public string? ErrorCode { get; init; }
    public Dictionary<string, object> Extensions { get; init; }
}