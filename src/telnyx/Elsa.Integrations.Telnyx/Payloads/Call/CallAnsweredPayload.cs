using Elsa.Integrations.Telnyx.Attributes;
using Elsa.Integrations.Telnyx.Payloads.Abstractions;

namespace Elsa.Integrations.Telnyx.Payloads.Call;

/// <summary>
/// Represents the payload of a webhook event that is triggered when an incoming call is answered.
/// </summary>
[Webhook(WebhookEventTypes.CallAnswered)]
public sealed record CallAnsweredPayload : CallPayload
{
    /// <summary>
    /// The from number.
    /// </summary>
    public string From { get; init; } = null!;
    
    /// <summary>
    /// The to number.
    /// </summary>
    public string To { get; init; } = null!;
    
    /// <summary>
    /// Call state.
    /// </summary>
    public string State { get; init; } = null!;
}