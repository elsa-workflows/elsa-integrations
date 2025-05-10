using Elsa.Integrations.Telnyx.Attributes;
using Elsa.Integrations.Telnyx.Payloads.Abstractions;

namespace Elsa.Integrations.Telnyx.Payloads.Call;

[WebhookActivity(WebhookEventTypes.CallPlaybackEnded, WebhookActivityTypeNames.CallPlaybackEnded, "Call Playback Ended", "Triggered when an audio playback has ended.")]
public sealed record CallPlaybackEndedPayload : CallPlayback
{
    public string Status { get; set; } = null!;
}