using System.Net.Http.Headers;
using Elsa.Alterations.Extensions;
using Elsa.Persistence.EFCore.Extensions;
using Elsa.Persistence.EFCore.Modules.Alterations;
using Elsa.Persistence.EFCore.Modules.Identity;
using Elsa.Persistence.EFCore.Modules.Management;
using Elsa.Persistence.EFCore.Modules.Runtime;
using Elsa.Extensions;
using Elsa.Identity.Providers;
using Elsa.ServiceBus.AzureServiceBus.ComponentTests.Extensions;
using Elsa.ServiceBus.MassTransit.Extensions;
using Elsa.Testing.Shared.Handlers;
using Elsa.Testing.Shared.Services;
using Elsa.TestServer.Web;
using FluentStorage;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.ServiceBus.AzureServiceBus.ComponentTests;

[UsedImplicitly]
public class WorkflowServer(Infrastructure infrastructure, string url) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var dbConnectionString = infrastructure.DbContainer.GetConnectionString();
        var rabbitMqConnectionString = infrastructure.RabbitMqContainer.GetConnectionString();

        builder.UseUrls(url);

        if (Program.ConfigureForTest == null)
        {
            Program.ConfigureForTest = elsa =>
            {
                elsa.AddWorkflowsFrom<WorkflowServer>();
                elsa.AddActivitiesFrom<WorkflowServer>();
                elsa.UseDefaultAuthentication(defaultAuthentication => defaultAuthentication.UseAdminApiKey());
                elsa.UseFluentStorageProvider(sp =>
                {
                    var assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    var assemblyDirectory = Path.GetDirectoryName(assemblyLocation)!;
                    var workflowsDirectorySegments = new[]
                    {
                        assemblyDirectory, "Scenarios"
                    };
                    var workflowsDirectory = Path.Join(workflowsDirectorySegments);
                    return StorageFactory.Blobs.DirectoryFiles(workflowsDirectory);
                });
                elsa.UseMassTransit(massTransit =>
                {
                    massTransit.UseRabbitMq(rabbitMqConnectionString);
                });
                elsa.UseIdentity(identity => identity.UseEntityFrameworkCore(ef => ef.UsePostgreSql(dbConnectionString)));
                elsa.UseWorkflowManagement(management =>
                {
                    management.UseEntityFrameworkCore(ef => ef.UsePostgreSql(dbConnectionString));
                    management.UseMassTransitDispatcher();
                    management.UseCache();
                });
                elsa.UseWorkflowRuntime(runtime =>
                {
                    runtime.UseEntityFrameworkCore(ef => ef.UsePostgreSql(dbConnectionString));
                    runtime.UseCache();
                    runtime.UseMassTransitDispatcher();
                    runtime.UseProtoActor();
                });
                elsa.UseAlterations(alterations =>
                {
                    alterations.UseEntityFrameworkCore(e => e.UsePostgreSql(dbConnectionString));
                });
                elsa.UseHttp(http =>
                {
                    http.UseCache();
                });
                elsa.UseAzureServiceBus();
            };
        }

        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton<SignalManager>();
            services.AddSingleton<WorkflowEvents>();
            services.AddNotificationHandlersFrom<WorkflowServer>();
            services.AddNotificationHandlersFrom<WorkflowEventHandlers>();
            services.AddAzureServiceBusTestServices();
        });
    }

    protected override void ConfigureClient(HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ApiKey", AdminApiKeyProvider.DefaultApiKey);
    }
}