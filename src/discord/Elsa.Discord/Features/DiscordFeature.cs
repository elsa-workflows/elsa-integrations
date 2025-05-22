using Elsa.Features.Abstractions;
using Elsa.Features.Services;
using Elsa.Discord.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Discord.Features;

/// <summary>
/// Configures Discord integration.
/// </summary>
public class DiscordFeature(IModule module) : FeatureBase(module)
{
    public override void Apply() =>
        Services.AddSingleton<DiscordClientFactory>();
}
