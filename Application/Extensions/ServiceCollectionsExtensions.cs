using Application.CQRS.Authorization.Login;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.Extensions;

/// <summary>
///     Service collection extensions
/// </summary>
public static class ServiceCollectionsExtensions
{
    /// <summary>
    ///     Add application services to service collections
    /// </summary>
    /// <param name="serviceCollection"> Service collection </param>
    /// <param name="logger"> Logger </param>
    public static void AddApplicationServices(this IServiceCollection serviceCollection, ILogger logger)
    {
        logger.LogInformation("Adding application services...");

        logger.LogInformation("Adding mediator...");
        serviceCollection.AddMediatR(static cfg => cfg.RegisterServicesFromAssembly(typeof(LoginCommandResult).Assembly));

        logger.LogInformation("Application services added");
    }
}