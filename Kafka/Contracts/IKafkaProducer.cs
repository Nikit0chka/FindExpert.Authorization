namespace Kafka.Contracts;

public interface IKafkaProducer<in TMessage>:IDisposable
{
    Task ProduceAsync(TMessage message, CancellationToken cancellationTokens);
}