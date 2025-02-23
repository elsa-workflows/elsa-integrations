﻿using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using JetBrains.Annotations;
using SlackNet;

namespace Elsa.Integrations.Slack.Activities.Channels;

/// <summary>
/// Sets the purpose of a channel.
/// </summary>
[Activity(
    "Elsa.Integrations.Slack.Channels",
    "Slack Channels",
    "Sets the purpose for a channel.",
    DisplayName = "Set Channel Purpose")]
[UsedImplicitly]
public class SetChannelPurpose : SlackActivity
{
    /// <summary>
    /// The ID of the channel to set the purpose of.
    /// </summary>
    [Input(Name = "Channel Id", Description = "The ID of the channel to set the purpose of.")]
    public Input<string> ChannelId { get; set; } = default!;

    /// <summary>
    /// The new purpose.
    /// </summary>
    [Input(Description = "The new purpose.")]
    public Input<string> Purpose { get; set; } = default!;

    /// <summary>
    /// The updated purpose.
    /// </summary>
    [Output(Description = "The updated purpose.")]
    public Output<string> UpdatedPurpose { get; set; } = default!;

    /// <summary>
    /// Executes the activity.
    /// </summary>
    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        string channelId = context.Get(ChannelId)!;
        string purpose = context.Get(Purpose)!;

        ISlackApiClient client = GetClient(context);
        string updatedPurpose = await client.Conversations.SetPurpose(channelId, purpose);

        context.Set(UpdatedPurpose, updatedPurpose);
    }
}