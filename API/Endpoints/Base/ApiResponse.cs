namespace API.Endpoints.Base;

public record ApiResponse
{
    public BaseProblemDetails? Error { get; set; }
    internal static ApiResponse Success() => new();
    internal static ApiResponse Fail(BaseProblemDetails error) => new() { Error = error };
}

public sealed record ApiResponse<T>:ApiResponse
{
    public T? Data { get; private init; }

    internal static ApiResponse<T> Success(T data) => new() { Data = data };
    internal new static ApiResponse<T> Fail(BaseProblemDetails error) => new() { Error = error };
}