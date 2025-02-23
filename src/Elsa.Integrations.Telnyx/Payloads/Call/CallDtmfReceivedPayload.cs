﻿using Elsa.Integrations.Telnyx.Attributes;
using Elsa.Integrations.Telnyx.Payloads.Abstractions;

namespace Elsa.Integrations.Telnyx.Payloads.Call;

[WebhookActivity(WebhookEventTypes.CallDtmfReceived, WebhookActivityTypeNames.CallDtmfReceived, "Call DTMF Received", "Triggered when DTMF input is received.")]
public sealed record CallDtmfReceivedPayload : CallPayload
{
    public string Digit { get; set; } = null!;
    public string From { get; set; } = null!;
    public string To { get; set; } = null!;
}