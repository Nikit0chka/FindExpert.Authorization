namespace API.Validation;

internal static class AuthorizationValidationConstants
{
    public const int EmailMinLength = 3;
    public const int EmailMaxLength = 64;
    public const int RefreshTokenMinLength = 1;
    public const int RefreshTokenMaxLength = 256;
}