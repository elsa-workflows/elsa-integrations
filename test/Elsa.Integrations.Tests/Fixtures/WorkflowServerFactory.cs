using System.Net.Http.Headers;
using Elsa.Extensions;
using Elsa.Identity.Providers;
using Elsa.Testing.Shared.Handlers;
using Elsa.Testing.Shared.Services;
using Elsa.TestServer.Web;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Integrations.Tests.Fixtures;

/// <summary>
/// Creates a test workflow server.
/// </summary>
/// <typeparam name="TMarker">Registers all activities, workflows, and handlers from the assembly of the specified <c>TMarker</c>.</typeparam>
[UsedImplicitly]
public abstract class WorkflowServerFactory<TMarker> : WebApplicationFactory<Program>
{
    private readonly Lazy<IServiceScope> _testServiceScope;
    public override IServiceProvider Services => _testServiceScope.Value.ServiceProvider;

    protected WorkflowServerFactory()
    {
        _testServiceScope = new Lazy<IServiceScope>(() => base.Services.CreateScope());
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseUrls("http://localhost:5004");

        Program.ConfigureForTest ??= elsa =>
        {
            elsa.AddWorkflowsFrom<TMarker>();
            elsa.AddActivitiesFrom<TMarker>();
            elsa.UseDefaultAuthentication(defaultAuthentication => defaultAuthentication.UseAdminApiKey());
        };

        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton<SignalManager>();
            services.AddSingleton<WorkflowEvents>();
            services.AddNotificationHandlersFrom<TMarker>();
            services.AddNotificationHandlersFrom<WorkflowEventHandlers>();
        });
    }

    protected override void ConfigureClient(HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ApiKey", AdminApiKeyProvider.DefaultApiKey);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && _testServiceScope.IsValueCreated)
        {
            _testServiceScope.Value.Dispose();
        }

        base.Dispose(disposing);
    }
}