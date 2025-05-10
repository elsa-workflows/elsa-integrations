using Elsa.Integrations.Telnyx.Attributes;
using Elsa.Integrations.Telnyx.Payloads.Abstractions;

namespace Elsa.Integrations.Telnyx.Payloads.Call;

[Webhook(WebhookEventTypes.CallSpeakStarted)]
public sealed record CallSpeakStarted : CallPayload;