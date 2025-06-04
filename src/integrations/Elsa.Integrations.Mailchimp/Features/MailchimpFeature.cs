using Elsa.Features.Abstractions;
using Elsa.Features.Services;
using Elsa.Integrations.Mailchimp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Integrations.Mailchimp.Features;

/// <summary>
/// Represents a feature for setting up Mailchimp integration within the Elsa framework.
/// </summary>
public class MailchimpFeature(IModule module) : FeatureBase(module)
{
    /// <summary>
    /// Applies the feature to the specified service collection.
    /// </summary>
    public override void Apply() =>
        Services
            .AddSingleton<MailchimpClientFactory>();
}