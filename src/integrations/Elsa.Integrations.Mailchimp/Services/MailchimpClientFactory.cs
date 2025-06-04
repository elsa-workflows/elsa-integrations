using MailChimp.Net;
using MailChimp.Net.Interfaces;

namespace Elsa.Integrations.Mailchimp.Services;

/// <summary>
/// Factory for creating Mailchimp API clients.
/// </summary>
public class MailchimpClientFactory
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly Dictionary<string, IMailChimpManager> _mailchimpClients = new();

    /// <summary>
    /// Gets a Mailchimp API client for the specified API key.
    /// </summary>
    public IMailChimpManager GetClient(string apiKey)
    {
        if (_mailchimpClients.TryGetValue(apiKey, out IMailChimpManager? client))
            return client;

        try
        {
            _semaphore.Wait();

            if (_mailchimpClients.TryGetValue(apiKey, out client))
                return client;

            var newClient = new MailChimpManager(apiKey);
            
            _mailchimpClients[apiKey] = newClient;
            return newClient;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}