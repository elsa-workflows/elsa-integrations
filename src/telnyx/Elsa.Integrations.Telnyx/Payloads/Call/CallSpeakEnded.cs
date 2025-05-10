using Elsa.Integrations.Telnyx.Attributes;
using Elsa.Integrations.Telnyx.Payloads.Abstractions;

namespace Elsa.Integrations.Telnyx.Payloads.Call;

[Webhook(WebhookEventTypes.CallSpeakEnded)]
public sealed record CallSpeakEnded : CallPayload;