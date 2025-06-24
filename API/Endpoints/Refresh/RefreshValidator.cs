using API.Validation;
using Domain.AggregateModels.SessionAggregate;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.Refresh;

internal class RefreshValidator:Validator<RefreshRequest>
{
    public RefreshValidator()
    {
        RuleFor(static refreshRequest => refreshRequest.RefreshToken).NotEmpty().WithMessage(AuthorizationValidationMessages.RefreshTokenIsRequired)
            .MinimumLength(SessionConstants.TokenMinLength).WithMessage(AuthorizationValidationMessages.RefreshTokenMinLength)
            .MaximumLength(SessionConstants.TokenMaxLength).WithMessage(AuthorizationValidationMessages.RefreshTokenMaxLength);
    }
}