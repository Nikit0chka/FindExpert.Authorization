using HttpClients.Contracts;
using HttpClients.UserService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HttpClients.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddHttpClients(this IServiceCollection services, ILogger logger)
    {
        logger.LogInformation("Adding http clients...");

        services.AddHttpClient<IUserServiceClient, UserServiceClient>(static client =>
        {
            client.BaseAddress = new("http://localhost:5005");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            // Опционально: таймауты
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        logger.LogInformation("Http clients added");
    }
}