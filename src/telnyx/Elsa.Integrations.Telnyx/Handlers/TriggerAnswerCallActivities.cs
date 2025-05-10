using Elsa.Extensions;
using Elsa.Integrations.Telnyx.Activities;
using Elsa.Integrations.Telnyx.Bookmarks;
using Elsa.Integrations.Telnyx.Events;
using Elsa.Integrations.Telnyx.Extensions;
using Elsa.Integrations.Telnyx.Payloads.Call;
using Elsa.Mediator.Contracts;
using Elsa.Workflows.Helpers;
using Elsa.Workflows.Runtime;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Elsa.Integrations.Telnyx.Handlers;

/// <summary>
/// Triggers all workflows blocked on a <see cref="AnswerCall"/> or <see cref="FlowAnswerCall"/> activity.
/// </summary>
[PublicAPI]
internal class TriggerAnswerCallActivities(IStimulusSender stimulusSender, ILogger<TriggerAnswerCallActivities> logger)
    : INotificationHandler<TelnyxWebhookReceived>
{
    private readonly ILogger _logger = logger;

    public async Task HandleAsync(TelnyxWebhookReceived notification, CancellationToken cancellationToken)
    {
        var webhook = notification.Webhook;
        var payload = webhook.Data.Payload;

        if (payload is not CallAnsweredPayload callAnsweredPayload)
            return;

        var clientStatePayload = callAnsweredPayload.GetClientStatePayload();
        var workflowInstanceId = clientStatePayload?.WorkflowInstanceId;
        var activityInstanceId = clientStatePayload?.ActivityInstanceId!;
        var input = new Dictionary<string, object>().AddInput(callAnsweredPayload);
        var callControlId = callAnsweredPayload.CallControlId;

        var activityTypeNames = new[]
        {
            ActivityTypeNameHelper.GenerateTypeName<AnswerCall>(), 
            ActivityTypeNameHelper.GenerateTypeName<FlowAnswerCall>(),
        };

        foreach (var activityTypeName in activityTypeNames)
        {
            var stimulus = new AnswerCallStimulus(callControlId);
            var metadata = new StimulusMetadata
            {
                WorkflowInstanceId = workflowInstanceId,
                ActivityInstanceId = activityInstanceId,
                Input = input
            };
            await stimulusSender.SendAsync(activityTypeName, stimulus, metadata, cancellationToken);
        }
    }
}