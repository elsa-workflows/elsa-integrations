using Azure.Messaging.ServiceBus.Administration;
using Elsa.Common;
using Elsa.Extensions;
using Elsa.Features.Abstractions;
using Elsa.Features.Attributes;
using Elsa.Features.Services;
using Elsa.Hosting.Management.Contracts;
using Elsa.Hosting.Management.Features;
using Elsa.ServiceBus.MassTransit.AzureServiceBus.Handlers;
using Elsa.ServiceBus.MassTransit.AzureServiceBus.HostedServices;
using Elsa.ServiceBus.MassTransit.AzureServiceBus.Models;
using Elsa.ServiceBus.MassTransit.AzureServiceBus.Options;
using Elsa.ServiceBus.MassTransit.AzureServiceBus.Services;
using Elsa.ServiceBus.MassTransit.Extensions;
using Elsa.ServiceBus.MassTransit.Features;
using Elsa.ServiceBus.MassTransit.Models;
using Elsa.ServiceBus.MassTransit.Options;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Elsa.ServiceBus.MassTransit.AzureServiceBus.Features;

/// <summary>
/// Configures MassTransit to use the Azure Service Bus transport.
/// See https://masstransit.io/documentation/configuration/transports/azure-service-bus
/// </summary>
[DependsOn(typeof(MassTransitFeature))]
[DependsOn(typeof(ClusteringFeature))]
public class AzureServiceBusFeature : FeatureBase
{
    /// <inheritdoc />
    public AzureServiceBusFeature(IModule module) : base(module)
    {
    }

    /// <summary>
    /// An Azure Service Bus connection string.
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// Delete subscriptions where their connected queues could not be found.
    /// Topics without any subscriptions will be deleted as well.
    /// </summary>
    /// <remarks>
    /// - All subscriptions will be cleaned up, including those that were not created by Elsa.
    /// - Queues in other namespaces will not be found and the subscription will therefore be removed.
    /// </remarks>
    public bool EnableAutomatedSubscriptionCleanup { get; set; }

    /// <summary>
    /// A delegate that configures the Azure Service Bus transport options.
    /// </summary>
    /// <remarks>This method is being marked as obsolete in favor of the ConfigureTransportBus which will provide additional access to the <see cref="IBusRegistrationContext"/></remarks>
    [Obsolete("Use ConfigureTransportBus instead which provides a reference to IBusRegistrationContext.")]
    public Action<IServiceBusBusFactoryConfigurator>? ConfigureServiceBus { get; set; }

    /// <summary>
    /// Configures the Azure Service Bus within MassTransit for additional transport level components or features.
    /// This action provides access to the <see cref="IBusRegistrationContext"/> and <see cref="IServiceBusBusFactoryConfigurator"/>.
    /// </summary>
    /// <remarks>
    /// Use this action to configure advanced settings and features for the Azure Service Bus, such as middleware 
    /// or additional endpoints. This action will run in addition to the Elsa required configuration.
    /// </remarks>
    public Action<IBusRegistrationContext, IServiceBusBusFactoryConfigurator> ConfigureTransportBus { get; set; }

    /// <summary>
    /// A delegate to configure <see cref="AzureServiceBusOptions"/>.
    /// </summary>
    public Action<AzureServiceBusOptions> AzureServiceBusOptions { get; set; } = _ => { };

    /// <summary>
    /// A delegate to configure <see cref="SubscriptionCleanupOptions"/>.
    /// </summary>
    public Action<SubscriptionCleanupOptions> SubscriptionCleanupOptions { get; set; } = _ => { };

    /// <summary>
    /// A delegate to create a <see cref="ServiceBusAdministrationClient"/> instance.
    /// </summary>
    public Func<IServiceProvider, ServiceBusAdministrationClient> ServiceBusAdministrationClientFactory { get; set; } = sp => new(GetConnectionString(sp));

    /// <inheritdoc />
    public override void Configure()
    {
        Module.Configure<MassTransitFeature>(massTransitFeature =>
        {
            massTransitFeature.BusConfigurator = configure =>
            {
                var consumers = massTransitFeature.GetConsumers().ToList();
                var temporaryConsumers = consumers
                    .Where(c => c.IsTemporary)
                    .ToList();

                RegisterConsumers(consumers);
                configure.AddServiceBusMessageScheduler();

                // Consumers need to be added before the UsingAzureServiceBus statement to prevent exceptions.
                foreach (var consumer in temporaryConsumers)
                    configure.AddConsumer(consumer.ConsumerType).ExcludeFromConfigureEndpoints();

                configure.UsingAzureServiceBus((context, configurator) =>
                {
                    if (ConnectionString != null)
                        configurator.Host(ConnectionString);

                    var options = context.GetRequiredService<IOptions<MassTransitOptions>>().Value;

                    if (options.PrefetchCount is not null)
                        configurator.PrefetchCount = options.PrefetchCount.Value;
                    if (options.MaxAutoRenewDuration is not null)
                        configurator.MaxAutoRenewDuration = options.MaxAutoRenewDuration.Value;
                    configurator.ConcurrentMessageLimit = options.ConcurrentMessageLimit;

                    configurator.UseServiceBusMessageScheduler();
                    ConfigureServiceBus?.Invoke(configurator);
                    ConfigureTransportBus?.Invoke(context, configurator);
                    var instanceNameProvider = context.GetRequiredService<IApplicationInstanceNameProvider>();

                    foreach (var consumer in temporaryConsumers)
                    {
                        // Start with the instance name since we will be using that to delete queues / subscriptions that are no longer needed.
                        // This is the only way to guarantee we can match subscriptions to an application instance, since the Azure Service Bus transport for MassTransit trims names that are too large.
                        var queueName = $"{instanceNameProvider.GetName()}-{consumer.Name}";
                        configurator.ReceiveEndpoint(queueName, endpointConfigurator =>
                        {
                            endpointConfigurator.AutoDeleteOnIdle = options.TemporaryQueueTtl ?? TimeSpan.FromHours(1);
                            endpointConfigurator.ConcurrentMessageLimit = options.ConcurrentMessageLimit;
                            endpointConfigurator.ConfigureConsumer(context, consumer.ConsumerType);
                        });
                    }

                    if (!massTransitFeature.DisableConsumers)
                    {
                        if (Module.HasFeature<MassTransitWorkflowDispatcherFeature>())
                            configurator.SetupWorkflowDispatcherEndpoints(context);
                    }

                    configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter("Elsa", false));
                    
                    configurator.ConfigureJsonSerializerOptions(serializerOptions =>
                    {
                        var serializer = context.GetRequiredService<IJsonSerializer>();
                        serializer.ApplyOptions(serializerOptions);
                        return serializerOptions;
                    });
                    
                    configurator.ConfigureTenantMiddleware(context);
                });
            };
        });
    }

    /// <inheritdoc />
    public override void Apply()
    {
        Services.Configure(AzureServiceBusOptions);
        Services.Configure(SubscriptionCleanupOptions);
        Services.AddSingleton(ServiceBusAdministrationClientFactory);
    }

    /// <inheritdoc />
    public override void ConfigureHostedServices()
    {
        if (EnableAutomatedSubscriptionCleanup)
        {
            Module.ConfigureHostedService<CleanSubscriptionsWithoutQueues>();
        }
    }

    private static string GetConnectionString(IServiceProvider serviceProvider)
    {
        var options = serviceProvider.GetRequiredService<IOptions<AzureServiceBusOptions>>().Value;
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        return configuration.GetConnectionString(options.ConnectionStringOrName) ?? options.ConnectionStringOrName;
    }

    private void RegisterConsumers(List<ConsumerTypeDefinition> consumers)
    {
        var subscriptionTopology = (
            from consumer in consumers
            from consumerInterface in consumer.ConsumerType.GetInterfaces()
            where consumerInterface.IsGenericType && consumerInterface.GetGenericTypeDefinition() == typeof(IConsumer<>)
            let genericType = consumerInterface.GetGenericArguments()[0]
            let topicName = $"{genericType.Namespace.ToLower()}/{genericType.Name.ToLower()}"
            select new MessageSubscriptionTopology(topicName, consumer.Name ?? genericType.Name.ToLower(), consumer.IsTemporary)
        ).ToList();

        Services.AddSingleton(new MessageTopologyProvider(subscriptionTopology));
        Services.AddNotificationHandler<RemoveOrphanedSubscriptions>();
        Services.AddCommandHandler<CleanupOrphanedTopology>();
    }
}