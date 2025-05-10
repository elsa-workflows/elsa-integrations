using Elsa.Integrations.Telnyx.Attributes;

namespace Elsa.Integrations.Telnyx.Payloads.Call;

[WebhookActivity(
    WebhookEventTypes.CallMachinePremiumDetectionEnded, 
    WebhookActivityTypeNames.CallMachinePremiumDetectionEnded, 
    "Call Machine Premium Detection Ended", 
    "Triggered when machine detection has ended."
)]
public sealed record CallMachinePremiumDetectionEnded : CallMachineDetectionEndedBase
{
}