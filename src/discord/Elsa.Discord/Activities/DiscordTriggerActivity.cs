using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;

namespace Elsa.Discord.Activities;

/// <summary>
/// Base class for Discord triggers.
/// </summary>
public abstract class DiscordTriggerActivity : DiscordActivity, ITrigger
{
    /// <summary>
    /// Returns the Discord event type handled by this trigger.
    /// </summary>
    public abstract string GetTriggerType();

    /// <summary>
    /// Returns payloads for trigger indexing.
    /// </summary>
    public abstract ValueTask<IEnumerable<object>> GetTriggerPayloadsAsync(TriggerIndexingContext context);
}
