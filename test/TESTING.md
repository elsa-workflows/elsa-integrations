
# Writing Tests for Elsa Integrations

This document will assist you in writing tests for your `Elsa.Integrations` project. 

Each integration (or module as it is sometimes referred to) has a one-to-one correlation to a test project with all of the tests contained inside of it. There are three categories (four, if we include performance tests, but these are not implemented yet) and we decorate tests with the corresponding trait attribute:

| Test Type | Trait | Description |
|--|--|--|
| Unit Tests | `[UnitTest]` | Focus on single components/methods. |
| Integration Tests | `[IntegrationTest]` | Focus on multiple components interacting. |
| Component Tests | `[Component]` | End-to-end tests - uses a `WorkflowServer` instance which is a `WebApplicationFactory` to instantiate a workflow runtime for integration testing. |

## 📝 Getting Started

### 1️⃣ Create a New Test Project 
Create a new XUnit test project under `test\Elsa.Integrations.<IntegrationName>.Tests` making sure your .csproj only contains `<PackageReference>` nodes for nuget packages.

### 2️⃣ Create Your WorkflowServer
Create an instance of `WorkflowServer` extending `WorkflowServerFactory` and passing in your definition of the `WorkflowServer` as the generic parameter. This will register all of the activities and workflows defined in your test project. This class extends from .NET Core's `WebApplicationFactory` and is useful for overriding methods to configure the host for the `Elsa.TestServer.Web` project, allowing you to add dependencies specific to your tests for this specific integration.

```csharp
using Elsa.Extensions;
using Elsa.TestServer.Web;
using Microsoft.AspNetCore.Hosting;

namespace Elsa.Integrations.<IntegrationName>.Tests.Fixtures;

/// <summary>
/// A test workflow server.
/// </summary>
public class WorkflowServer : WorkflowServerFactory<WorkflowServer>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

		// Configure specific Elsa modules needed for your tests
        Program.ConfigureForTest ??= elsa => elsa
            .UseABC()
            .UseXYX();
    }
}
```
If you don't need to override the default configuration provided by `WebApplicationFactory<T>` then you can just use the built-in `WorkflowServer` by default that is already injectable as an XUnit Collection by decorating your test class with `[Collection(nameof(WorkflowServerCollection))]` rather than making your own.

```csharp
[Collection(nameof(WorkflowServerCollection))]
public class InvokeCommandTests(WorkflowServer workflowServer)
{
    [Fact]
    public async Task MyTest()
    {
        InvokeCommand invokeCommand = CreateEchoCommand();

        RunWorkflowResult result = await workflowServer.Services.RunActivityAsync(invokeCommand);

        result.Result.Should().BeNull();
    }
}
```

### 3️⃣ Begin Writing Tests  
For activity unit testing, although it would have been great to be able to test the logic in isolation for a single activity, the fact that activities depend on `ActivityExecutionContext` and `WorkflowExecutionContext`, it is better to do it as an integration test. Practically, this should feel close to a unit test, with the key different being that it depends on some factory code to establish the execution contexts.
