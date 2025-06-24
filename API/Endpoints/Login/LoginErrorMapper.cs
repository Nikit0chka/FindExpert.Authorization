using API.Contracts;
using Application.CQRS.Authorization.Login;

namespace API.Endpoints.Login;

public sealed class LoginErrorMapper:IErrorMapper
{
    public int GetStatusCode(string? errorCode) => errorCode switch
    {
        LoginErrorCodes.UserNotFound => StatusCodes.Status401Unauthorized,
        LoginErrorCodes.UserEmailNotConfirmed => StatusCodes.Status403Forbidden,
        _ => StatusCodes.Status500InternalServerError
    };

    public string GetTitle(string? errorCode) => errorCode switch
    {
        LoginErrorCodes.UserNotFound => "Unauthorized",
        LoginErrorCodes.UserEmailNotConfirmed => "Forbidden",
        _ => "Internal Server Error"
    };

    public string GetDetail(string? errorCode) => errorCode switch
    {
        LoginErrorCodes.UserNotFound => "User not found or password is incorrect",
        LoginErrorCodes.UserEmailNotConfirmed => "Email is not confirmed. Please check your inbox",
        _ => "An error occurred"
    };
}