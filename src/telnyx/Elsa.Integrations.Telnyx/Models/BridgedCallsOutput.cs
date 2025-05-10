using Elsa.Integrations.Telnyx.Activities;
using Elsa.Integrations.Telnyx.Payloads.Call;

namespace Elsa.Integrations.Telnyx.Models;

/// <summary>
/// Contains output of the <see cref="BridgeCalls"/> activity.
/// </summary>
/// <param name="PayloadA">The payload from leg A.</param>
/// <param name="PayloadB">The payload from leg B.</param>
public record BridgedCallsOutput(CallBridgedPayload PayloadA, CallBridgedPayload PayloadB);