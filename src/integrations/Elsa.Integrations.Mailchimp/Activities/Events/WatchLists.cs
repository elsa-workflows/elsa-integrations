using Elsa.Integrations.Mailchimp.Activities;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using JetBrains.Annotations;
using MailChimp.Net.Models;

namespace Elsa.Integrations.Mailchimp.Activities.Events;

/// <summary>
/// Triggers when a new list is created in Mailchimp.
/// </summary>
[Activity(
    "Elsa.Mailchimp.Events",
    "Mailchimp Events",
    "Triggers when a new list is created in Mailchimp.",
    DisplayName = "Watch Lists")]
[UsedImplicitly]
public class WatchLists : MailchimpTriggerActivity
{
    /// <summary>
    /// The received list event.
    /// </summary>
    [Output(Description = "The received list event.")]
    public Output<List> ReceivedList { get; set; } = null!;

    /// <summary>
    /// Returns the trigger type identifier.
    /// </summary>
    public override string GetTriggerType() => "mailchimp.list.created";

    /// <summary>
    /// Returns the payloads to index.
    /// </summary>
    /// <param name="context">The trigger indexing context.</param>
    public override ValueTask<IEnumerable<object>> GetTriggerPayloadsAsync(TriggerIndexingContext context)
    {
        var apiKey = context.Get(ApiKey);
        var eventType = context.Get(EventType) ?? "list.created";
        
        return new ValueTask<IEnumerable<object>>(new object[] { new { ApiKey = apiKey, EventType = eventType } });
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