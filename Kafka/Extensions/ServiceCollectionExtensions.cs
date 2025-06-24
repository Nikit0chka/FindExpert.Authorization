using Kafka.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Kafka.Extensions;

/// <summary>
///     Service collection extensions
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Add kafka producer
    /// </summary>
    /// <param name="serviceCollection"> Service collection </param>
    /// <param name="configurationSection"> Config </param>
    /// <param name="logger"> Logger </param>
    /// <typeparam name="TMessage"> Type of message </typeparam>
    public static void AddKafkaProducer<TMessage>(this IServiceCollection serviceCollection, IConfigurationSection configurationSection, ILogger logger)
    {
        serviceCollection.Configure<KafkaProducerOptions>(configurationSection);
        serviceCollection.AddSingleton<IKafkaProducer<TMessage>, KafkaProducer<TMessage>>();

        logger.LogInformation("Producer added for message type {MessageType}", typeof(TMessage).Name);
    }
}