using Elsa.Features.Services;
using Elsa.Integrations.Mailchimp.Features;

// ReSharper disable once CheckNamespace
namespace Elsa.Extensions;

/// <summary>
/// Extends <see cref="IModule"/> with methods to use Mailchimp integration.
/// </summary>
public static class ModuleExtensions
{
    /// <summary>
    /// Installs the Mailchimp API feature.
    /// </summary>
    public static IModule UseMailchimp(this IModule module, Action<MailchimpFeature>? configure = null)
    {
        return module.Use(configure);
    }
}