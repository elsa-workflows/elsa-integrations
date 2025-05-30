using Confluent.Kafka;

namespace Elsa.ServiceBus.Kafka;

public interface IProducer : IDisposable
{
    Task ProduceAsync(string topic, object? key, object value, Headers? headers = null, CancellationToken cancellationToken = default);
}