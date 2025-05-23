using Elsa.Features.Services;
using Elsa.OrchardCore.Features;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace Elsa.Extensions;

/// <summary>
/// Adds extensions to <see cref="IModule"/> that enables the <see cref="OrchardCoreFeature"/> feature.
/// </summary>
[UsedImplicitly]
public static class ModuleExtensions
{
    /// <summary>
    /// Enables and configures the <see cref="OrchardCoreFeature"/> feature.
    /// </summary>
    public static IModule UseOrchardCore(this IModule module, Action<OrchardCoreFeature>? configure = null)
    {
        module.Configure(configure);
        return module;
    }
}