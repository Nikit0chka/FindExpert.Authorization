using API.Contracts;
using Application.CQRS.Authorization.Logout;

namespace API.Endpoints.Logout;

public sealed class LogoutErrorMapper:IErrorMapper
{
    public int GetStatusCode(string? errorCode) => errorCode switch
    {
        LogoutErrorCodes.RefreshTokenInvalid => StatusCodes.Status401Unauthorized,
        LogoutErrorCodes.SessionNotFound => StatusCodes.Status404NotFound,
        _ => StatusCodes.Status500InternalServerError
    };

    public string GetTitle(string? errorCode) => errorCode switch
    {
        LogoutErrorCodes.RefreshTokenInvalid => "Invalid refresh token",
        LogoutErrorCodes.SessionNotFound => "Session not found",
        _ => "Internal Server Error"
    };

    public string GetDetail(string? errorCode) => errorCode switch
    {
        LogoutErrorCodes.RefreshTokenInvalid => "Provided refresh token is expired or malformed",
        LogoutErrorCodes.SessionNotFound => "Authorized session by provided refresh token not found",
        _ => "An error occurred"
    };
}