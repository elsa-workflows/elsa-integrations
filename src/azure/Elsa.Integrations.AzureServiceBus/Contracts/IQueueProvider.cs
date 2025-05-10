using Elsa.Integrations.AzureServiceBus.Models;

namespace Elsa.Integrations.AzureServiceBus.Contracts;

/// <summary>
/// Provides queue definitions to the system. 
/// </summary>
public interface IQueueProvider
{
    /// <summary>
    /// Returns a list of <see cref="QueueDefinition"/>s.
    /// </summary>
    ValueTask<ICollection<QueueDefinition>> GetQueuesAsync(CancellationToken cancellationToken);
}