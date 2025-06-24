using API.Validation;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.Login;

internal class LoginValidator:Validator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(static loginRequest => loginRequest.Email)
            .NotEmpty().WithMessage(AuthorizationValidationMessages.EmailIsRequired)
            .MinimumLength(AuthorizationValidationConstants.EmailMinLength).WithMessage(AuthorizationValidationMessages.RefreshTokenMinLength)
            .MaximumLength(AuthorizationValidationConstants.EmailMaxLength).WithMessage(AuthorizationValidationMessages.RefreshTokenMaxLength)
            .EmailAddress().WithMessage(AuthorizationValidationMessages.InvalidEmailFormat);

        RuleFor(static loginRequest => loginRequest.Password)
            .NotEmpty().WithMessage(AuthorizationValidationMessages.PasswordIsRequired);

        RuleFor(static loginRequest => loginRequest.Role).NotEmpty().WithMessage(AuthorizationValidationMessages.RoleIsRequired);
    }
}