using System.Reflection;
using Elsa.Extensions;
using Elsa.Integrations.Telnyx.Activities;
using Elsa.Integrations.Telnyx.Attributes;
using Elsa.Integrations.Telnyx.Bookmarks;
using Elsa.Integrations.Telnyx.Events;
using Elsa.Integrations.Telnyx.Extensions;
using Elsa.Integrations.Telnyx.Payloads.Abstractions;
using Elsa.Mediator.Contracts;
using Elsa.Workflows.Runtime;
using JetBrains.Annotations;

namespace Elsa.Integrations.Telnyx.Handlers;

/// <summary>
/// Triggers all workflows starting with or blocked on a <see cref="WebhookEvent"/> activity.
/// </summary>
[PublicAPI]
internal class TriggerWebhookActivities(IStimulusSender stimulusSender) : INotificationHandler<TelnyxWebhookReceived>
{
    public async Task HandleAsync(TelnyxWebhookReceived notification, CancellationToken cancellationToken)
    {
        var webhook = notification.Webhook;
        var eventType = webhook.Data.EventType;
        var payload = webhook.Data.Payload;
        var activityType = payload.GetType().GetCustomAttribute<WebhookActivityAttribute>()?.ActivityType;

        if (activityType == null)
            return;

        var workflowInstanceId = ((Payload)webhook.Data.Payload).GetClientStatePayload()?.WorkflowInstanceId;
        var callControlId = (webhook.Data.Payload as CallPayload)?.CallControlId;
        var stimulus = new WebhookEventStimulus(eventType, callControlId);
        var input = new Dictionary<string, object>().AddInput(webhook);

        var metadata = new StimulusMetadata
        {
            Input = input,
            WorkflowInstanceId = workflowInstanceId,

        };
        await stimulusSender.SendAsync(activityType, stimulus, metadata, cancellationToken);
    }
}