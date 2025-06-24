namespace API.Validation;

internal static class AuthorizationValidationMessages
{
    public const string EmailIsRequired = "Email is required";
    public const string InvalidEmailFormat = "Invalid email format";
    public const string ConfirmationCodeIsRequired = "Confirmation code is required";
    public const string PasswordIsRequired = "Password is required";
    public const string RefreshTokenIsRequired = "Refresh token is required";
    public const string RoleIsRequired = "Role is required";
    public readonly static string EmailMinLength = $"Email must be at least {AuthorizationValidationConstants.EmailMinLength} characters long";
    public readonly static string EmailMaxLength = $"Email must be at most {AuthorizationValidationConstants.EmailMaxLength} characters long";
    public readonly static string RefreshTokenMinLength = $"Refresh token must be at least {AuthorizationValidationConstants.RefreshTokenMinLength} characters long";
    public readonly static string RefreshTokenMaxLength = $"Refresh token must be at most {AuthorizationValidationConstants.RefreshTokenMaxLength} characters long";
}