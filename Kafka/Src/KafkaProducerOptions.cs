namespace Kafka;

internal sealed class KafkaProducerOptions
{
    public required string BootstrapServers { get; init; }
    public required string Topic { get; init; }
}