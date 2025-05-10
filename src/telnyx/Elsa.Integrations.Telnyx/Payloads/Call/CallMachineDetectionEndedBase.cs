using Elsa.Integrations.Telnyx.Payloads.Abstractions;

namespace Elsa.Integrations.Telnyx.Payloads.Call;

public record CallMachineDetectionEndedBase : CallPayload
{
    public string Result { get; set; } = null!;
}