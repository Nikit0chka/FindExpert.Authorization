using Application.Contracts;
using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Extensions;

/// <summary>
///     Service collection extensions
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Add infrastructure services to service collections
    /// </summary>
    /// <param name="serviceCollection"> Service collection </param>
    /// <param name="configurationManager"> Configuration manager </param>
    /// <param name="logger"> Logger </param>
    public static void AddInfrastructureServices(this IServiceCollection serviceCollection, IConfigurationManager configurationManager, ILogger logger)
    {
        logger.LogInformation("Adding infrastructure services...");

        logger.LogInformation("Adding database...");
        var dbConnectionString = configurationManager.GetConnectionString("DefaultConnection");

        serviceCollection.AddDbContext<DbContext, Context>
            (options => options.UseSqlServer(Guard.Against.NullOrEmpty(dbConnectionString,
                                                                       nameof(dbConnectionString),
                                                                       "Database connection string was null or empty.")));

        logger.LogInformation("Adding repositories...");
        serviceCollection.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        serviceCollection.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));

        serviceCollection.AddScoped<IJwtService, JwtService>();

        logger.LogInformation("Infrastructure services added");
    }
}