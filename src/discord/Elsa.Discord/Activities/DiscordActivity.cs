using Elsa.Discord.Services;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using Discord.WebSocket;

namespace Elsa.Discord.Activities;

/// <summary>
/// Base class for Discord activities.
/// </summary>
public abstract class DiscordActivity : Activity
{
    /// <summary>
    /// The Discord bot token.
    /// </summary>
    [Input(Description = "The Discord bot token.")]
    public Input<string> Token { get; set; } = null!;

    /// <summary>
    /// Gets the Discord client.
    /// </summary>
    protected async Task<DiscordSocketClient> GetClientAsync(ActivityExecutionContext context)
    {
        DiscordClientFactory factory = context.GetRequiredService<DiscordClientFactory>();
        string token = context.Get(Token)!;
        return await factory.GetClientAsync(token);
    }
}
