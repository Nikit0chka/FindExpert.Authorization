using Application.Abstractions;
using Application.CQRS.Authorization.Commands;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Domain.AggregatesModel.AuthorizedSessionAggregate;
using Domain.AggregatesModel.UserAggregate;
using Domain.AggregatesModel.UserAggregate.Specifications;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Authorization.Handlers;

internal class LoginCommandHandler(
    IReadRepository<User> userRepository,
    IRepository<AuthorizedSession> authorizedSessionRepository,
    IJwtService jwtService,
    ILogger<LoginCommandHandler> logger,
    IPasswordHasherService passwordHasherService):ICommandHandler<LoginCommand, Result<(string, string)>>
{
    public async Task<Result<(string, string)>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {Command} for user with login: {Login}", nameof(LoginCommand), request.Login);

        try
        {
            var user = await userRepository.SingleOrDefaultAsync(new UserByLoginSpecification(request.Login), cancellationToken);

            if (user is null)
            {
                logger.LogWarning("User with login {Login} not found", request.Login);
                return Result.NotFound("User not found");
            }

            if (!passwordHasherService.VerifyPassword(request.Password, user.PasswordHash))
            {
                logger.LogWarning("Invalid password for user with login {Login}. UserId: {UserId}", request.Login, user.Id);
                return Result.NotFound("User not found");
            }

            var accessToken = jwtService.GenerateJwtAccessToken(user);
            var refreshToken = jwtService.GenerateJwtRefreshToken(user);

            logger.LogInformation("Tokens generated for user with login {Login}. UserId: {UserId}", request.Login, user.Id);

            var authorizedSession = new AuthorizedSession(refreshToken, user.Id);
            await authorizedSessionRepository.AddAsync(authorizedSession, cancellationToken);

            logger.LogInformation("Authorized session created for user with login {Login}. SessionId: {SessionId}. UserId: {UserId}", request.Login, authorizedSession.Id, user.Id);

            logger.LogInformation("{Command} handled successfully for user with login {Login}. SessionId: {SessionId}. UserId: {UserId}", nameof(LoginCommand), request.Login, authorizedSession.Id, user.Id);
            return (accessToken, refreshToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while handling {Command} for user with login {Login}", nameof(LoginCommand), request.Login);
            return Result.Error("An error occurred while processing your request.");
        }
    }
}