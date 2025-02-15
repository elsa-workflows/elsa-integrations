using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using JetBrains.Annotations;
using SlackNet;
using SlackNet.Events;
using SlackNet.WebApi;

namespace Elsa.Integrations.Slack.Activities.Channels;

/// <summary>
/// Retrieves a message from a private channel.
/// </summary>
[Activity(
    "Elsa.Integrations.Slack.Channels",
    "Slack Channels",
    "Gets a message from a private channel.",
    DisplayName = "Get Private Channel Message")]
[UsedImplicitly]
public class GetChannelMessage : SlackActivity
{
    /// <summary>
    /// The ID of the private channel containing the message.
    /// </summary>
    [Input(Name = "Channel Id", Description = "The ID of the private channel containing the message.")]
    public Input<string> ChannelId { get; set; } = default!;

    /// <summary>
    /// Timestamp of the message to retrieve.
    /// </summary>
    [Input(Name = "Message Timestamp", Description = "Timestamp of the message to retrieve.")]
    public Input<string> MessageTs { get; set; } = default!;

    /// <summary>
    /// The retrieved message.
    /// </summary>
    [Output(Description = "The retrieved message.")]
    public Output<Message> Message { get; set; } = default!;

    /// <summary>
    /// Executes the activity.
    /// </summary>
    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        string channelId = context.Get(ChannelId)!;
        string messageTs = context.Get(MessageTs)!;

        ISlackApiClient client = GetClient(context);
        ConversationHistoryResponse history = await client.Conversations.History(
            channelId,
            latestTs: messageTs,
            limit: 1,
            inclusive: true);

        MessageEvent? message = history.Messages.FirstOrDefault(m => m.Ts == messageTs);

        if (message == null)
        {
            throw new InvalidOperationException($"Message with timestamp {messageTs} not found in channel {channelId}");
        }

        context.Set(Message, message);
    }
}