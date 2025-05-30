namespace Elsa.ServiceBus.AzureServiceBus.Models;

/// <summary>
/// Represents a topic subscription that is available to the system.
/// </summary>
public class SubscriptionDefinition
{
    /// <summary>
    /// The subscription name.
    /// </summary>
    public string Name { get; set; } = null!;
    
    /// <summary>
    /// The topic.
    /// </summary>
    [Obsolete("Use TopicDefinition.Subscriptions instead.")]
    public string? Topic { get; set; }
}