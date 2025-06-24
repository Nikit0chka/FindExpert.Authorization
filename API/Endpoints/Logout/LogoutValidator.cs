using API.Validation;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.Logout;

internal class LogoutValidator:Validator<LogoutRequest>
{
    public LogoutValidator()
    {
        RuleFor(static request => request.RefreshToken)
            .NotEmpty().WithMessage(AuthorizationValidationMessages.RefreshTokenIsRequired)
            .MinimumLength(AuthorizationValidationConstants.RefreshTokenMinLength).WithMessage(AuthorizationValidationMessages.RefreshTokenMinLength)
            .MaximumLength(AuthorizationValidationConstants.RefreshTokenMaxLength).WithMessage(AuthorizationValidationMessages.RefreshTokenMaxLength);
    }
}