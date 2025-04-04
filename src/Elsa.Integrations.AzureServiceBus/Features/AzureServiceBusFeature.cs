using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Elsa.Extensions;
using Elsa.Features.Abstractions;
using Elsa.Features.Services;
using Elsa.Integrations.AzureServiceBus.Contracts;
using Elsa.Integrations.AzureServiceBus.Handlers;
using Elsa.Integrations.AzureServiceBus.HostedServices;
using Elsa.Integrations.AzureServiceBus.Options;
using Elsa.Integrations.AzureServiceBus.Providers;
using Elsa.Integrations.AzureServiceBus.Services;
using Elsa.Integrations.AzureServiceBus.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Elsa.Integrations.AzureServiceBus.Features;

/// <summary>
/// Enables and configures the Azure Service Bus feature.
/// </summary>
public class AzureServiceBusFeature : FeatureBase
{
    /// <inheritdoc />
    public AzureServiceBusFeature(IModule module) : base(module)
    {
    }

    /// <summary>
    /// A value controlling whether queues, topics and subscriptions should be created automatically. 
    /// </summary>
    public bool CreateQueuesTopicsAndSubscriptions { get; set; } = true;

    /// <summary>
    /// A delegate to configure <see cref="AzureServiceBusOptions"/>.
    /// </summary>
    public Action<AzureServiceBusOptions> AzureServiceBusOptions { get; set; } = _ => { };

    /// <summary>
    /// A delegate to create a <see cref="ServiceBusAdministrationClient"/> instance.
    /// </summary>
    public Func<IServiceProvider, ServiceBusClient> ServiceBusClientFactory { get; set; } = sp => new(GetConnectionString(sp));
    
    /// <summary>
    /// A delegate to create a <see cref="ServiceBusAdministrationClient"/> instance.
    /// </summary>
    public Func<IServiceProvider, ServiceBusAdministrationClient> ServiceBusAdministrationClientFactory { get; set; } = sp => new(GetConnectionString(sp));

    /// <inheritdoc />
    public override void ConfigureHostedServices()
    {
        if (CreateQueuesTopicsAndSubscriptions)
            Module.ConfigureHostedService<CreateQueuesTopicsAndSubscriptions>();
    }

    /// <inheritdoc />
    public override void Configure()
    {
        // Activities.
        Module.AddActivitiesFrom<AzureServiceBusFeature>();
    }

    /// <inheritdoc />
    public override void Apply()
    {
        Services.Configure(AzureServiceBusOptions);

        Services
            .AddSingleton(ServiceBusAdministrationClientFactory)
            .AddSingleton(ServiceBusClientFactory)
            .AddSingleton<ConfigurationQueueTopicAndSubscriptionProvider>()
            .AddSingleton<IWorkerManager, WorkerManager>()
            .AddScoped<IServiceBusInitializer, ServiceBusInitializer>();
        
        // Tasks.
        Services.AddBackgroundTask<StartWorkers>();

        // Definition providers.
        Services
            .AddSingleton<IQueueProvider>(sp => sp.GetRequiredService<ConfigurationQueueTopicAndSubscriptionProvider>())
            .AddSingleton<ITopicProvider>(sp => sp.GetRequiredService<ConfigurationQueueTopicAndSubscriptionProvider>())
            .AddSingleton<ISubscriptionProvider>(sp => sp.GetRequiredService<ConfigurationQueueTopicAndSubscriptionProvider>());

        // Handlers.
        Services.AddHandlersFrom<UpdateWorkers>();
    }

    private static string GetConnectionString(IServiceProvider serviceProvider)
    {
        var options = serviceProvider.GetRequiredService<IOptions<AzureServiceBusOptions>>().Value;
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        return configuration.GetConnectionString(options.ConnectionStringOrName) ?? options.ConnectionStringOrName;
    }
}