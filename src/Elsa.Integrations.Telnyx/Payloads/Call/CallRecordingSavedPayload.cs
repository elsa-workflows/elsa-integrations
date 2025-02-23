﻿using Elsa.Integrations.Telnyx.Attributes;
using Elsa.Integrations.Telnyx.Models;
using Elsa.Integrations.Telnyx.Payloads.Abstractions;

namespace Elsa.Integrations.Telnyx.Payloads.Call;

[WebhookActivity(WebhookEventTypes.CallRecordingSaved, WebhookActivityTypeNames.CallRecordingSaved, "Call Recording Saved", "Triggered when a recording has been saved.")]
public sealed record CallRecordingSavedPayload : CallPayload
{
    public string Channels { get; set; } = null!;
    public CallRecordingUrls PublicRecordingUrls { get; set; } = null!;
    public CallRecordingUrls RecordingUrls { get; set; } = null!;
    public DateTimeOffset RecordingEndedAt { get; set; }
    public DateTimeOffset RecordingStartedAt { get; set; }
    public TimeSpan Duration => RecordingEndedAt - RecordingStartedAt;
}