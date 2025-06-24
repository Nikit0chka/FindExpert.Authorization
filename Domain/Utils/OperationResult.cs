namespace Domain.Utils;

/// <summary>
///     Base operation result logic
/// </summary>
public class OperationResult
{
    /// <summary>
    ///     Is operation result was success
    /// </summary>
    public bool IsSuccess { get; protected init; }

    /// <summary>
    ///     Operation result error code
    /// </summary>
    public string? ErrorCode { get; protected init; }

    /// <summary>
    ///     Fabric success operation result
    /// </summary>
    /// <returns> Success operation result </returns>
    public static OperationResult Success() => new() { IsSuccess = true };

    /// <summary>
    ///     Fabric failed operation result
    /// </summary>
    /// <param name="errorCode"> Error code </param>
    /// <returns> Failed operation result </returns>
    public static OperationResult Error(string? errorCode = null) => new() { IsSuccess = false, ErrorCode = errorCode };
}

/// <inheritdoc />
/// <typeparam name="TData"> Result data type </typeparam>
public sealed class OperationResult<TData>:OperationResult
{
    private OperationResult(TData data)
    {
        IsSuccess = true;
        Data = data;
    }

    private OperationResult(string? error)
    {
        IsSuccess = false;
        ErrorCode = error;
    }

    /// <summary>
    ///     Result data
    /// </summary>
    public TData? Data { get; }

    /// <summary>
    ///     Fabric success operation result
    /// </summary>
    /// <param name="data"> Result data </param>
    /// <returns> Success operation result </returns>
    public static OperationResult<TData> Success(TData data) => new(data);

    /// <summary>
    ///     Fabric failed operation result
    /// </summary>
    /// <param name="errorCode"> Error code </param>
    /// <returns> </returns>
    public new static OperationResult<TData> Error(string? errorCode = null) => new(errorCode);
}