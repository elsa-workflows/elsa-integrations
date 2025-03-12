using Elsa.Features.Services;
using Elsa.Integrations.CommandLine.Features;

// ReSharper disable once CheckNamespace
namespace Elsa.Extensions;

/// <summary>
/// Provides methods to install and configure command line related features.
/// </summary>
public static class ModuleExtensions
{
    /// <summary>
    /// Enable and configure the <see cref="CommandLineFeature"/> feature. 
    /// </summary>
    public static IModule UseCommandLineIntegration(this IModule module, Action<CommandLineFeature>? setup = null) => module.Use(setup);
}