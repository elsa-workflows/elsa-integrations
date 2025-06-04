using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;

namespace Elsa.Integrations.Mailchimp.Activities;

/// <summary>
/// Base class for Mailchimp event trigger activities.
/// </summary>
public abstract class MailchimpTriggerActivity : MailchimpActivity, ITrigger
{
    /// <summary>
    /// The webhook event type to listen for.
    /// </summary>
    [Input(Description = "The webhook event type to listen for.")]
    public Input<string> EventType { get; set; } = null!;

    /// <summary>
    /// Returns the trigger type identifier.
    /// </summary>
    public abstract string GetTriggerType();

    /// <summary>
    /// Returns the payloads to index.
    /// </summary>
    /// <param name="context">The trigger indexing context.</param>
    public abstract ValueTask<IEnumerable<object>> GetTriggerPayloadsAsync(TriggerIndexingContext context);
}