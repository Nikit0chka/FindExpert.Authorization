using API.Contracts;
using Application.CQRS.Authorization.Refresh;

namespace API.Endpoints.Refresh;

public sealed class RefreshErrorMapper:IErrorMapper
{
    public int GetStatusCode(string? errorCode) => errorCode switch
    {
        RefreshErrorCodes.SessionNotFound => StatusCodes.Status401Unauthorized,
        RefreshErrorCodes.InvalidRefreshToken => StatusCodes.Status401Unauthorized,
        _ => StatusCodes.Status500InternalServerError
    };

    public string GetTitle(string? errorCode) => errorCode switch
    {
        RefreshErrorCodes.SessionNotFound => "Session not found",
        RefreshErrorCodes.InvalidRefreshToken => "Invalid refresh token",
        _ => "Internal Server Error"
    };

    public string GetDetail(string? errorCode) => errorCode switch
    {
        RefreshErrorCodes.SessionNotFound => "Not found active session by provided refresh token",
        RefreshErrorCodes.InvalidRefreshToken => "Provided refresh token is expired or malformed",
        _ => "An error occurred"
    };
}