using Elsa.Integrations.CommandLine.Activities;
using Elsa.Testing.Shared.Activities;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Models;

namespace Elsa.Integrations.CommandLine.Tests.Workflows;

public class InvokeOneCommandWorkflow : WorkflowBase
{
    protected override void Build(IWorkflowBuilder builder)
    {
        builder.Root = new Sequence
        {
            Activities =
            {
                new InvokeCommand
                {
                    Command = new Input<object>("echo"),
                    Arguments = new Input<object>("Hello world!"),
                    WorkingDirectory = new Input<string?>(AppDomain.CurrentDomain.BaseDirectory),
                    EnvironmentVariables = new Input<IDictionary<string, string?>?>(
                        new Dictionary<string, string>() { { "ELSA_TEST", "true" } }!)
                }
            }
        };
    }
}