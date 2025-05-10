using Elsa.Agents.Features;
using Elsa.Features.Abstractions;
using Elsa.Features.Attributes;
using Elsa.Features.Services;
using Elsa.Integrations.Agents.Activities.ActivityProviders;
using Elsa.Integrations.Agents.Activities.Handlers;
using Elsa.Workflows.Management.Features;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Integrations.Agents.Activities.Features;

/// <summary>
/// A feature that installs Semantic Kernel functionality.
/// </summary>
[DependsOn(typeof(WorkflowManagementFeature))]
[DependsOn(typeof(AgentsFeature))]
[UsedImplicitly]
public class AgentActivitiesFeature(IModule module) : FeatureBase(module)
{
    /// <inheritdoc />
    public override void Apply()
    {
        Services
            .AddActivityProvider<AgentActivityProvider>()
            .AddNotificationHandler<RefreshActivityRegistry>()
            ;
    }
}