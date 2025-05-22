using Discord;
using Discord.WebSocket;

namespace Elsa.Discord.Services;

/// <summary>
/// Factory for creating Discord socket clients.
/// </summary>
public class DiscordClientFactory
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly Dictionary<string, DiscordSocketClient> _clients = new();

    /// <summary>
    /// Gets a configured <see cref="DiscordSocketClient"/> for the specified token.
    /// </summary>
    public async Task<DiscordSocketClient> GetClientAsync(string token)
    {
        if (_clients.TryGetValue(token, out DiscordSocketClient? client))
            return client;

        await _semaphore.WaitAsync();
        try
        {
            if (_clients.TryGetValue(token, out client))
                return client;

            DiscordSocketClient newClient = new();
            await newClient.LoginAsync(TokenType.Bot, token);
            await newClient.StartAsync();
            _clients[token] = newClient;
            return newClient;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
