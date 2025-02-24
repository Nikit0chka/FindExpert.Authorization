using Ardalis.Result;
using Ardalis.SharedKernel;
using Authorization.Application.Abstractions;
using Authorization.Application.CQRS.Authorization.Commands;
using Authorization.Domain.AggregatesModel.AuthorizedSessionAggregate;
using Authorization.Domain.AggregatesModel.AuthorizedSessionAggregate.Specifications;
using Microsoft.Extensions.Logging;

namespace Authorization.Application.CQRS.Authorization.Handlers;

internal class LogoutCommandHandler(IRepository<AuthorizedSession> authorizedSessionRepository, IJwtService jwtService, ILogger<LogoutCommandHandler> logger):ICommandHandler<LogoutCommand, Result>
{
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {Command} for refresh token (masked).", nameof(LogoutCommand));

        try
        {
            if (!jwtService.IsTokenValid(request.RefreshToken))
            {
                logger.LogWarning("Invalid refresh token provided.");
                return Result.Invalid(new ValidationError("RefreshToken is invalid or expired."));
            }

            var authorizedSession = await authorizedSessionRepository.SingleOrDefaultAsync(new AuthorizedSessionByRefreshTokenSpecification(request.RefreshToken),
                                                                                           cancellationToken);

            if (authorizedSession is null)
            {
                logger.LogWarning("Authorized session not found for provided refresh token.");
                return Result.NotFound("Session not found.");
            }

            await authorizedSessionRepository.DeleteAsync(authorizedSession, cancellationToken);

            logger.LogInformation("Authorized session deleted. SessionId: {SessionId}, UserId: {UserId}",
                                  authorizedSession.Id,
                                  authorizedSession.UserId);

            logger.LogInformation("{Command} handled successful for refresh token (masked).", nameof(LogoutCommand));
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while handling {Command}", nameof(LogoutCommand));
            return Result.Error("An error occurred while processing your request.");
        }
    }
}