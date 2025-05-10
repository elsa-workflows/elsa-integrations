using Elsa.Extensions;
using Elsa.Features.Abstractions;
using Elsa.Features.Services;
using Elsa.Integrations.Sql.Activities;
using Elsa.Integrations.Sql.Contracts;
using Elsa.Integrations.Sql.Factory;
using Elsa.Integrations.Sql.Providers;
using Elsa.Integrations.Sql.Services;
using Elsa.Integrations.Sql.UIHints;
using Elsa.Workflows;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Integrations.Sql.Features;

/// <summary>
/// Setup SQL client features
/// </summary>
public class SqlFeature : FeatureBase
{
    /// <summary>
    /// Set a callback to configure <see cref="ClientStore"/>.
    /// </summary>
    public Action<ClientStore> Clients { get; set; } = _ => { };

    /// <summary>
    ///  <inheritdoc/>
    /// </summary>
    /// <param name="module"></param>
    public SqlFeature(IModule module) : base(module)
    {
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override void Configure()
    {
        Module.AddActivitiesFrom<SqlFeature>();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override void Apply()
    {
        Services
            // Services
            .AddSingleton(provider =>
            {
                ClientStore clientRegistry = new();
                Clients.Invoke(clientRegistry);
                return clientRegistry;
            })
            .AddSingleton<ISqlClientFactory, SqlClientFactory>()
            .AddScoped<ISqlEvaluator, SqlEvaluator>()

            // Providers
            .AddExpressionDescriptorProvider<SqlExpressionDescriptorProvider>()
            .AddScoped<IPropertyUIHandler, SqlCodeOptionsProvider>()
            .AddScoped<IPropertyUIHandler, SqlClientsDropDownProvider>()
            .AddScoped<ISqlClientNamesProvider, SqlClientNamesProvider>();
    }
}