using Application.Abstractions;
using Application.CQRS.Authorization.Commands;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Domain.AggregatesModel.AuthorizedSessionAggregate;
using Domain.AggregatesModel.AuthorizedSessionAggregate.Specifications;
using Domain.AggregatesModel.UserAggregate;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Authorization.Handlers;

internal class RefreshCommandHandler(
    IRepository<AuthorizedSession> authorizedSessionRepository,
    IReadRepository<User> userRepository,
    IJwtService jwtService,
    ILogger<RefreshCommandHandler> logger):
    ICommandHandler<RefreshCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {Command} for refresh token (masked).", nameof(RefreshCommand));

        try
        {
            if (!jwtService.IsTokenValid(request.RefreshToken))
            {
                logger.LogWarning("Invalid refresh token provided.");
                return Result.Invalid(new ValidationError(request.RefreshToken, "Invalid refresh token"));
            }

            var authorizedSession = await authorizedSessionRepository.SingleOrDefaultAsync(new AuthorizedSessionByRefreshTokenSpecification(request.RefreshToken), cancellationToken);

            if (authorizedSession is null)
            {
                logger.LogWarning("Not found session for provided refresh token.");
                return Result.NotFound("Session for refresh token not found");
            }

            var userId = jwtService.GetUserIdFromToken(request.RefreshToken);

            var user = await userRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                logger.LogWarning("Not found user for provided refresh token.");
                return Result.NotFound("User for refresh token not found");
            }

            var accessToken = jwtService.GenerateJwtAccessToken(user);
            logger.LogInformation("Access token generated successful for UserId: {UserId}", user.Id);

            logger.LogInformation("{Command} handled successful for refresh token (masked).", nameof(RefreshCommand));
            return accessToken;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while handling {Command}", nameof(RefreshCommand));
            return Result.Error("An error occurred while processing your request.");
        }
    }
}