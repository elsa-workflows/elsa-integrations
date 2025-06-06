using Elsa.Workflows.Runtime.ProtoActor.Features;
using Elsa.Workflows.Runtime.Features;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace Elsa.Extensions;

/// <summary>
/// Provides extension methods on <see cref="WorkflowRuntimeFeature"/>.
/// </summary>
[PublicAPI]
public static class WorkflowRuntimeFeatureExtensions
{
    /// <summary>
    /// Enable &amp; configure the <see cref="WorkflowRuntimeFeature"/>.
    /// </summary>
    public static WorkflowRuntimeFeature UseProtoActor(this WorkflowRuntimeFeature feature, Action<ProtoActorWorkflowRuntimeFeature>? configure = null)
    {
        feature.Module.Configure(configure);
        return feature;
    }
}