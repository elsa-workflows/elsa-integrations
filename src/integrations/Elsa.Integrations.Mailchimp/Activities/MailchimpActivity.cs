using Elsa.Integrations.Mailchimp.Services;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using MailChimp.Net.Interfaces;

namespace Elsa.Integrations.Mailchimp.Activities;

/// <summary>
/// Generic base class inherited by all Mailchimp activities.
/// </summary>
public abstract class MailchimpActivity : Activity
{
    /// <summary>
    /// The Mailchimp API key.
    /// </summary>
    [Input(Description = "The Mailchimp API key.")]
    public Input<string> ApiKey { get; set; } = null!;

    /// <summary>
    /// Gets the Mailchimp API client.
    /// </summary>
    /// <param name="context">The current context to get the client.</param>
    /// <returns>The Mailchimp API client.</returns>
    protected IMailChimpManager GetClient(ActivityExecutionContext context)
    {
        MailchimpClientFactory mailchimpClientFactory = context.GetRequiredService<MailchimpClientFactory>();
        string apiKey = context.Get(ApiKey)!;
        return mailchimpClientFactory.GetClient(apiKey);
    }
}