﻿using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using JetBrains.Annotations;
using SlackNet.WebApi;

namespace Elsa.Integrations.Slack.Activities.Events;

/// <summary>
/// Triggers when a message is added to a multiparty direct message.
/// </summary>
[Activity(
    "Elsa.Integrations.Slack.Events",
    "Slack Events",
    "Triggers when a message is added to a multiparty direct message.",
    DisplayName = "Watch Multiparty Direct Messages")]
[UsedImplicitly]
public class WatchMultipartyDirectMessages : SlackEventActivity
{
    /// <summary>
    /// The received message.
    /// </summary>
    [Output(Description = "The received message.")]
    public Output<Message> ReceivedMessage { get; set; } = default!;

    /// <summary>
    /// Executes the activity.
    /// </summary>
    protected override ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        // Implementation depends on Slack's Events API and WebSocket support
        throw new NotImplementedException("Event subscription requires WebSocket implementation.");
    }
}