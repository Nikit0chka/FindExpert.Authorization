namespace Domain.AggregateModels.SessionAggregate;

internal static class SessionValidationMessages
{
    public const string RefreshTokenIsRequired = "Refresh token is required";
    public const string UserIdCannotBeLessThanZero = "User id cannot be less than 0";
    public readonly static string RefreshTokenIsOutOfRange = $"RefreshToken must be between {SessionConstants.TokenMinLength} and {SessionConstants.TokenMaxLength} characters.";
}