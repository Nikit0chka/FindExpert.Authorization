namespace Application.CQRS.Authorization.Logout;

public static class LogoutErrorCodes
{
    public const string RefreshTokenInvalid = "LOGOUT_REFRESH_TOKEN_INVALID";
    public const string SessionNotFound = "LOGOUT_SESSION_NOT_FOUND";
}