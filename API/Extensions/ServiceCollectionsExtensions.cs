using API.Contracts;
using API.Endpoints.Login;
using API.Endpoints.Logout;
using API.Endpoints.Refresh;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;

namespace API.Extensions;

/// <summary>
///     Service collection extensions
/// </summary>
internal static class ServiceCollectionsExtensions
{
    /// <summary>
    ///     Add api services to service collections
    /// </summary>
    /// <param name="serviceCollection"> Service collection </param>
    /// <param name="logger"> Logger </param>
    public static void AddApiServices(this IServiceCollection serviceCollection, ILogger logger)
    {
        logger.LogInformation("Adding api services...");

        serviceCollection.AddFastEndpoints().SwaggerDocument();
        serviceCollection.AddValidatorsFromAssemblyContaining<RefreshValidator>();
        serviceCollection.AddOpenApi();

        logger.LogInformation("Adding api mappers");
        serviceCollection.AddApiErrorMappers();

        logger.LogInformation("Api services added");
    }

    /// <summary>
    ///     Add api error mappers <see cref="IErrorMapper" />
    /// </summary>
    /// <param name="serviceCollection"> Service collection </param>
    private static void AddApiErrorMappers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<LoginErrorMapper>();
        serviceCollection.AddScoped<LogoutErrorMapper>();
        serviceCollection.AddScoped<RefreshErrorMapper>();
    }
}