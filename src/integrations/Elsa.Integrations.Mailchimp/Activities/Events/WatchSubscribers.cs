using Elsa.Integrations.Mailchimp.Activities;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using JetBrains.Annotations;
using MailChimp.Net.Models;

namespace Elsa.Integrations.Mailchimp.Activities.Events;

/// <summary>
/// Triggers when a new subscriber joins a list or is updated in Mailchimp.
/// </summary>
[Activity(
    "Elsa.Mailchimp.Events",
    "Mailchimp Events",
    "Triggers when a new subscriber joins a list or is updated in Mailchimp.",
    DisplayName = "Watch Subscribers")]
[UsedImplicitly]
public class WatchSubscribers : MailchimpTriggerActivity
{
    /// <summary>
    /// The list ID to watch for subscriber events.
    /// </summary>
    [Input(Description = "The list ID to watch for subscriber events.")]
    public Input<string?> ListId { get; set; } = default!;

    /// <summary>
    /// The received subscriber event.
    /// </summary>
    [Output(Description = "The received subscriber event.")]
    public Output<Member> ReceivedSubscriber { get; set; } = null!;

    /// <summary>
    /// Returns the trigger type identifier.
    /// </summary>
    public override string GetTriggerType() => "mailchimp.subscriber.updated";

    /// <summary>
    /// Returns the payloads to index.
    /// </summary>
    /// <param name="context">The trigger indexing context.</param>
    public override ValueTask<IEnumerable<object>> GetTriggerPayloadsAsync(TriggerIndexingContext context)
    {
        var apiKey = context.Get(ApiKey);
        var listId = context.Get(ListId);
        var eventType = context.Get(EventType) ?? "subscriber.updated";
        
        return new ValueTask<IEnumerable<object>>(new object[] { new { ApiKey = apiKey, ListId = listId, EventType = eventType } });
    }

    /// <summary>
    /// Executes the activity.
    /// </summary>
    protected override ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        // This would be triggered by a webhook event
        // The actual implementation would depend on webhook infrastructure
        // For now, this is a placeholder that shows the expected structure
        throw new NotImplementedException("Webhook trigger implementation requires webhook infrastructure setup.");
    }
}