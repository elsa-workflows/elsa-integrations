using Elsa.Integrations.Telnyx.Attributes;
using Elsa.Integrations.Telnyx.Payloads.Abstractions;

namespace Elsa.Integrations.Telnyx.Payloads.Call;

[WebhookActivity(WebhookEventTypes.CallPlaybackStarted, WebhookActivityTypeNames.CallPlaybackStarted, "Call Playback Started", "Triggered when an audio playback has started.")]
public sealed record CallPlaybackStartedPayload : CallPlayback;