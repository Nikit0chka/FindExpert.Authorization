using Confluent.Kafka;
using Kafka.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kafka;

internal sealed class KafkaProducer<TMessage>:IKafkaProducer<TMessage>
{
    private readonly ILogger<IKafkaProducer<TMessage>> _logger;
    private readonly IProducer<string, TMessage> _producer;
    private readonly string _topic;

    public KafkaProducer(IOptions<KafkaProducerOptions> settings, ILogger<IKafkaProducer<TMessage>> logger)
    {
        _logger = logger;

        var config = new ProducerConfig
                     {
                         BootstrapServers = settings.Value.BootstrapServers
                     };

        _producer = new ProducerBuilder<string, TMessage>(config).SetValueSerializer(new KafkaProducerSerializer<TMessage>()).Build();
        _topic = settings.Value.Topic;
    }

    public async Task ProduceAsync(TMessage message, CancellationToken cancellationToken)
    {
        try
        {
            await _producer.ProduceAsync(_topic, new() { Value = message }, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error producing message type {messageType}", message?.GetType());
        }
    }

    public void Dispose() { _producer.Dispose(); }
}