namespace Application.CQRS.Authorization.Login;

public static class LoginErrorCodes
{
    public const string UserNotFound = "LOGIN_USER_NOT_FOUND";
    public const string UserEmailNotConfirmed = "LOGIN_EMAIL_NOT_CONFIRMED";
}