using Application.Contracts;
using Ardalis.SharedKernel;
using Domain.AggregateModels.SessionAggregate;
using Domain.Utils;
using HttpClients.Contracts;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Authorization.Login;

internal sealed class LoginCommandHandler(
    IUserServiceClient userServiceClient,
    IRepository<Session> sessionRepository,
    IJwtService jwtService,
    ILogger<LoginCommandHandler> logger):ICommandHandler<LoginCommand, OperationResult<LoginCommandResult>>
{
    public async Task<OperationResult<LoginCommandResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {Command} for user with email: {Email}", nameof(LoginCommand), request.Email);

        try
        {
            var validateResponseResult = await userServiceClient.ValidateUserAsync(new(request.Email, request.Password, request.Role), cancellationToken);

            if (!validateResponseResult.IsSuccess)
            {
                logger.LogWarning("Validate response result is not success. For Email: {Email}, Role: {Role} ErrorCode: {ErrorCode}", request.Email, request.Role, validateResponseResult.ErrorCode);
                return OperationResult<LoginCommandResult>.Error(validateResponseResult.ErrorCode);
            }

            var accessToken = jwtService.GenerateJwtAccessToken(validateResponseResult.Data!.UserId, validateResponseResult.Data!.Roles);
            var refreshToken = jwtService.GenerateJwtRefreshToken(validateResponseResult.Data!.UserId);

            logger.LogInformation("Tokens generated for user with Email: {Email}. UserId: {UserId}", request.Email, validateResponseResult.Data!.UserId);

            var session = new Session(refreshToken, validateResponseResult.Data!.UserId);
            await sessionRepository.AddAsync(session, cancellationToken);

            logger.LogInformation("{Command} handled successfully. Authorized session created. Email: {Email}. SessionId: {SessionId}. UserId: {UserId}", nameof(LoginCommand), request.Email, session.Id, validateResponseResult.Data!.UserId);
            return OperationResult<LoginCommandResult>.Success(new(accessToken, refreshToken));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while handling {Command} for user with Email: {Email}", nameof(LoginCommand), request.Email);
            return OperationResult<LoginCommandResult>.Error();
        }
    }
}