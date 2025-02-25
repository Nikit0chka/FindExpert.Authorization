using FastEndpoints;

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

        logger.LogInformation("Adding swagger gen...");
        serviceCollection.AddSwaggerGen(static c => { c.SwaggerDoc("v1", new() { Title = "My API", Version = "v1" }); });

        logger.LogInformation("Adding fast endpoints...");
        serviceCollection.AddFastEndpoints();

        logger.LogInformation("Adding open api endpoints...");
        serviceCollection.AddOpenApi();

        logger.LogInformation("Api services added");
    }
}