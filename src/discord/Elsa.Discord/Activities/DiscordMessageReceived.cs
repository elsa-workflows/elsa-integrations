using Discord;
using Discord.WebSocket;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using JetBrains.Annotations;

namespace Elsa.Discord.Activities;

/// <summary>
/// Triggers when a Discord message is received.
/// </summary>
[Activity(
    "Elsa.Discord.Events",
    "Discord",
    "Triggers when a message is received.",
    DisplayName = "Discord Message Received")]
[UsedImplicitly]
public class DiscordMessageReceived : DiscordTriggerActivity
{
    [Input(Description = "Optional channel ID to filter messages.")]
    public Input<ulong?> ChannelId { get; set; } = null!;

    [Output(Description = "The received message content.")]
    public Output<string> Message { get; set; } = null!;

    [Output(Description = "The author ID.")]
    public Output<ulong> AuthorId { get; set; } = null!;

    public override string GetTriggerType() => nameof(DiscordMessageReceived);

    public override ValueTask<IEnumerable<object>> GetTriggerPayloadsAsync(TriggerIndexingContext context)
    {
        return ValueTask.FromResult<IEnumerable<object>>(Array.Empty<object>());
    }

    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        ulong? channelId = context.Get(ChannelId);
        DiscordSocketClient client = await GetClientAsync(context);

        TaskCompletionSource<SocketMessage> tcs = new();

        Task Handler(SocketMessage message)
        {
            if (channelId.HasValue && message.Channel.Id != channelId.Value)
                return Task.CompletedTask;

            tcs.TrySetResult(message);
            return Task.CompletedTask;
        }

        client.MessageReceived += Handler;

        try
        {
            SocketMessage msg = await tcs.Task;
            context.Set(Message, msg.Content);
            context.Set(AuthorId, msg.Author.Id);
        }
        finally
        {
            client.MessageReceived -= Handler;
        }
    }
}
