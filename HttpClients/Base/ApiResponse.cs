namespace HttpClients.Base;

public record ApiResponse
{
    public BaseProblemDetails? Error { get; init; }
    public bool IsSuccess => Error == null;
}

public sealed record ApiResponse<T>:ApiResponse
{
    public T? Data { get; init; }
}