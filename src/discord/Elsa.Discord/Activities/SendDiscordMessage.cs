using Discord;
using Discord.WebSocket;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;

namespace Elsa.Discord.Activities;

/// <summary>
/// Sends a message to a Discord channel or user.
/// </summary>
[Activity(
    "Elsa.Discord.Messages",
    "Discord",
    "Sends a message to a Discord channel or user.",
    DisplayName = "Send Discord Message")]
public class SendDiscordMessage : DiscordActivity
{
    [Input(Description = "The target channel or user ID.")]
    public Input<ulong> TargetId { get; set; } = null!;

    [Input(Description = "The message content to send.")]
    public Input<string> Content { get; set; } = null!;

    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        ulong id = context.Get(TargetId);
        string message = context.Get(Content)!;
        DiscordSocketClient client = await GetClientAsync(context);

        IMessageChannel? channel = client.GetChannel(id) as IMessageChannel;
        if (channel != null)
            await channel.SendMessageAsync(message);
        else if (client.GetUser(id) is IUser user)
            await user.SendMessageAsync(message);
        else
            throw new Exception($"Target {id} not found.");
    }
}
