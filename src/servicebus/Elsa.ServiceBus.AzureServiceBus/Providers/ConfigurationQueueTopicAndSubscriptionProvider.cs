using Elsa.ServiceBus.AzureServiceBus.Contracts;
using Elsa.ServiceBus.AzureServiceBus.Models;
using Elsa.ServiceBus.AzureServiceBus.Options;
using Microsoft.Extensions.Options;

namespace Elsa.ServiceBus.AzureServiceBus.Providers;

/// <summary>
/// Represents a queue provider that reads queue definitions from configuration.
/// </summary>
public class ConfigurationQueueTopicAndSubscriptionProvider : IQueueProvider, ITopicProvider, ISubscriptionProvider
{
    private readonly AzureServiceBusOptions _options;
    
    /// <summary>
    /// Constructor.
    /// </summary>
    public ConfigurationQueueTopicAndSubscriptionProvider(IOptions<AzureServiceBusOptions> options) => _options = options.Value;

    /// <inheritdoc />
    public ValueTask<ICollection<QueueDefinition>> GetQueuesAsync(CancellationToken cancellationToken) => new(_options.Queues);

    /// <inheritdoc />
    public ValueTask<ICollection<TopicDefinition>> GetTopicsAsync(CancellationToken cancellationToken) => new(_options.Topics);

    /// <inheritdoc />
    public ValueTask<ICollection<SubscriptionDefinition>> GetSubscriptionsAsync(CancellationToken cancellationToken) => new(_options.Subscriptions);
}