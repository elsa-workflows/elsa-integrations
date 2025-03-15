using Elsa.Extensions;
using Elsa.Integrations.Tests.Fixtures;
using Elsa.TestServer.Web;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;

namespace Elsa.Integrations.CommandLine.Tests.Fixtures;

[UsedImplicitly]
public class WorkflowServer : WorkflowServerFactory<WorkflowServer>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        Program.ConfigureForTest ??= elsa => elsa
            .AddWorkflowsFrom<WorkflowServer>()
            .AddActivitiesFrom<WorkflowServer>()
            .UseDefaultAuthentication(defaultAuthentication => defaultAuthentication.UseAdminApiKey())
            .UseScheduling()
            .UseCSharp()
            .UseJavaScript()
            .UseLiquid()
            .UseDsl()
            .UseWorkflowManagement()
            .UseWorkflows();
    }
}