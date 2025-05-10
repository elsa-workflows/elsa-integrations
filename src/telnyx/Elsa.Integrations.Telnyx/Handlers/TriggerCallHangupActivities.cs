using Elsa.Extensions;
using Elsa.Integrations.Telnyx.Activities;
using Elsa.Integrations.Telnyx.Bookmarks;
using Elsa.Integrations.Telnyx.Events;
using Elsa.Integrations.Telnyx.Extensions;
using Elsa.Integrations.Telnyx.Payloads.Call;
using Elsa.Mediator.Contracts;
using Elsa.Workflows.Runtime;
using JetBrains.Annotations;

namespace Elsa.Integrations.Telnyx.Handlers;

/// <summary>
/// Triggers all workflows starting with or blocked on a <see cref="CallHangup"/> activity.
/// </summary>
[PublicAPI]
internal class TriggerCallHangupActivities(IStimulusSender stimulusSender)
    : INotificationHandler<TelnyxWebhookReceived>
{
    public async Task HandleAsync(TelnyxWebhookReceived notification, CancellationToken cancellationToken)
    {
        var webhook = notification.Webhook;
        var payload = webhook.Data.Payload;

        if (payload is not CallHangupPayload callHangupPayload)
            return;

        var clientStatePayload = callHangupPayload.GetClientStatePayload();
        var workflowInstanceId = clientStatePayload?.WorkflowInstanceId;
        var input = new Dictionary<string, object>().AddInput(callHangupPayload);
        var callControlId = callHangupPayload.CallControlId;
        var stimulus = new CallHangupStimulus(callControlId);
        var metadata = new StimulusMetadata
        {
            WorkflowInstanceId = workflowInstanceId,
            Input = input
        };
        await stimulusSender.SendAsync<CallHangup>(stimulus, metadata, cancellationToken);
    }
}