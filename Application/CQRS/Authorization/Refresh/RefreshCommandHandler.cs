using Application.Contracts;
using Ardalis.SharedKernel;
using Domain.AggregateModels.SessionAggregate;
using Domain.AggregateModels.SessionAggregate.Specifications;
using Domain.Extensions;
using Domain.Utils;
using HttpClients.Contracts;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Authorization.Refresh;

internal sealed class RefreshCommandHandler(
    IReadRepository<Session> authorizedSessionRepository,
    IUserServiceClient userServiceClient,
    IJwtService jwtService,
    ILogger<RefreshCommandHandler> logger):
    ICommandHandler<RefreshCommand, OperationResult<RefreshCommandResult>>
{
    public async Task<OperationResult<RefreshCommandResult>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {Command} for refresh token (masked).", nameof(RefreshCommand));

        try
        {
            if (!jwtService.IsRefreshTokenValid(request.RefreshToken))
            {
                logger.LogWarning("Invalid refresh token provided.");
                return OperationResult<RefreshCommandResult>.Error(RefreshErrorCodes.InvalidRefreshToken);
            }

            var session = await authorizedSessionRepository.SingleOrDefaultAsync(new SessionByRefreshTokenSpecification(request.RefreshToken).AsNoTracking(), cancellationToken);

            if (session is null)
            {
                logger.LogWarning("Not found session for provided refresh token.");
                return OperationResult<RefreshCommandResult>.Error(RefreshErrorCodes.SessionNotFound);
            }

            var userId = jwtService.GetUserIdFromRefreshToken(request.RefreshToken);

            var validateResponseResult = await userServiceClient.GetUserAsync(new(userId), cancellationToken);

            if (!validateResponseResult.IsSuccess)
            {
                logger.LogWarning("Validate response result is not success. For UserId: {UserId}, ErrorCode: {ErrorCode}", userId, validateResponseResult.ErrorCode);
                return OperationResult<RefreshCommandResult>.Error(validateResponseResult.ErrorCode);
            }

            var accessToken = jwtService.GenerateJwtAccessToken(validateResponseResult.Data!.UserId, validateResponseResult.Data!.Roles);

            logger.LogInformation("{Command} handled successful for refresh token (masked). Access token generated for AuthorizedSessionId: {AuthorizedSessionId}", nameof(RefreshCommand), session.Id);
            return OperationResult<RefreshCommandResult>.Success(new(accessToken));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while handling {Command}", nameof(RefreshCommand));
            return OperationResult<RefreshCommandResult>.Error();
        }
    }
}