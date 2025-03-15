using System.Net;
using Elsa.Integrations.CommandLine.Activities;
using Elsa.Integrations.Tests.Fixtures;
using Elsa.Testing.Shared;
using Elsa.Workflows.Models;
using Microsoft.Extensions.DependencyInjection;


namespace Elsa.Integrations.CommandLine.Tests.Activities;

[Collection(nameof(WorkflowServerCollection))]
public class InvokeCommandTests(WorkflowServer workflowServer)
{
    [Fact]
    public async Task InvokeSimpleCommand_CompletesSuccessfully()
    {
        InvokeCommand invokeCommand = CreateEchoCommand();

        RunWorkflowResult result = await workflowServer.Services.RunActivityAsync(invokeCommand);

        result.Result.Should().BeNull();
        // invokeCommand.ExecutedCommand.GetOutput<Command>()
    }

    private InvokeCommand CreateEchoCommand()
    {
        InvokeCommand invokeCommand = workflowServer.Services.GetRequiredService<InvokeCommand>();
        invokeCommand.Command = new Input<object>("echo");
        invokeCommand.Arguments = new Input<object>("Hello world!");
        invokeCommand.WorkingDirectory = new Input<string?>(AppDomain.CurrentDomain.BaseDirectory);
        invokeCommand.EnvironmentVariables = new Input<IDictionary<string, string?>?>(
            new Dictionary<string, string>() { { "ELSA_TEST", "true" } }!);
        invokeCommand.Credentials = new Input<NetworkCredential>(new NetworkCredential("user", "password", "domain"));
        invokeCommand.SuccessfulExitCode = new Input<int?>(0);
        invokeCommand.SuccessfulOutputText = new Input<string>("Hello world!");
        invokeCommand.Timeout = new Input<object>(TimeSpan.FromSeconds(5));
        invokeCommand.GracefulCancellation = new Input<bool>(true);
        return invokeCommand;
    }
}