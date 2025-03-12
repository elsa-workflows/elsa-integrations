using Elsa.Extensions;
using Elsa.Features.Abstractions;
using Elsa.Features.Services;
using Elsa.Integrations.CommandLine.Activities;
using Elsa.Integrations.CommandLine.Contracts;
using Elsa.Integrations.CommandLine.Options;
using Elsa.Integrations.CommandLine.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Integrations.CommandLine.Features;

/// <summary>
/// Represents a feature for setting up Command Line integration within the Elsa framework.
/// </summary>
public class CommandLineFeature(IModule module) : FeatureBase(module)
{
    /// <summary>
    /// Set a callback to configure <see cref="CommandLineOptions"/>.
    /// </summary>
    public Action<CommandLineOptions> ConfigureOptions { get; set; } = _ => { };

    /// <summary>
    /// Gets or sets a callback for configuring the <see cref="ICommandValidator"/> implementation.
    /// </summary>
    public Func<IServiceProvider, ICommandValidator> CommandValidator { get; set; } = sp => sp.GetRequiredService<DefaultCommandValidator>();

    /// <inheritdoc />
    public override void Configure()
    {
        Module.AddActivity<InvokeCommand>();
    }

    /// <inheritdoc />
    public override void Apply()
    {
        Services
            .Configure(ConfigureOptions)
            .AddSingleton((sp) => CommandValidator.Invoke(sp))
            .AddTransient<CommandFinder>()
            .AddTransient<ICommandRunner, CliWrapCommandRunner>();
    }
}