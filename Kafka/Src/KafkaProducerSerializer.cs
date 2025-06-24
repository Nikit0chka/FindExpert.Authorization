using System.Text.Json;
using Confluent.Kafka;

namespace Kafka;

internal sealed class KafkaProducerSerializer<TMessage>:ISerializer<TMessage>
{
    public byte[] Serialize(TMessage data, SerializationContext context) => JsonSerializer.SerializeToUtf8Bytes(data);
}