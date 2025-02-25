using Application.Abstractions;
using Application.CQRS.Authorization.Commands;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Domain.AggregatesModel.UserAggregate;
using Domain.AggregatesModel.UserAggregate.Specifications;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Authorization.Handlers;

internal class RegisterCommandHandler(IRepository<User> userRepository, IPasswordHasherService passwordHasherService, ILogger<RegisterCommandHandler> logger):ICommandHandler<RegisterCommand, Result>
{
    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {Command} for user with login: {Login}", nameof(RegisterCommand), request.Login);

        try
        {
            var existingUser = await userRepository.SingleOrDefaultAsync(new UserByLoginSpecification(request.Login), cancellationToken);

            if (existingUser is not null)
            {
                logger.LogWarning("User with login {Login} already exists", request.Login);
                return Result.Invalid(new ValidationError("Login", "User already exists"));
            }

            var hashedPassword = passwordHasherService.HashPassword(request.Password);

            var user = new User(request.Login, hashedPassword);
            await userRepository.AddAsync(user, cancellationToken);

            logger.LogInformation("User created successfully. UserId: {UserId}, Login: {Login}", user.Id, user.Login);

            logger.LogInformation("{Command} handled successful. UserId: {UserId}, Login: {Login}", nameof(RegisterCommand), user.Id, user.Login);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while handling {Command} for user with login {Login}", nameof(RegisterCommand), request.Login);
            return Result.Error("An error occurred while processing your request.");
        }
    }
}