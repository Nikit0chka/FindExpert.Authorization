using Application.Contracts;
using Ardalis.SharedKernel;
using Domain.AggregateModels.SessionAggregate;
using Domain.AggregateModels.SessionAggregate.Specifications;
using Domain.Utils;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Authorization.Logout;

internal sealed class LogoutCommandHandler(IRepository<Session> authorizedSessionRepository, IJwtService jwtService, ILogger<LogoutCommandHandler> logger):ICommandHandler<LogoutCommand, OperationResult>
{
    public async Task<OperationResult> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {Command} for refresh token (masked).", nameof(LogoutCommand));

        try
        {
            if (!jwtService.IsRefreshTokenValid(request.RefreshToken))
            {
                logger.LogWarning("Invalid refresh token provided.");
                return OperationResult.Error(LogoutErrorCodes.RefreshTokenInvalid);
            }

            var authorizedSession = await authorizedSessionRepository.SingleOrDefaultAsync(new SessionByRefreshTokenSpecification(request.RefreshToken),
                                                                                           cancellationToken);

            if (authorizedSession is null)
            {
                logger.LogWarning("Authorized session not found for provided refresh token.");
                return OperationResult.Error(LogoutErrorCodes.SessionNotFound);
            }

            await authorizedSessionRepository.DeleteAsync(authorizedSession, cancellationToken);

            logger.LogInformation("{Command} handled successful for refresh token (masked). Authorized session deleted. SessionId: {SessionId}, UserId: {UserId}",
                                  nameof(LogoutCommand),
                                  authorizedSession.Id,
                                  authorizedSession.UserId);

            return OperationResult.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while handling {Command}", nameof(LogoutCommand));
            return OperationResult.Error();
        }
    }
}