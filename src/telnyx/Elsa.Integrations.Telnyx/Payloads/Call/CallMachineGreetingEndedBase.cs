using Elsa.Integrations.Telnyx.Payloads.Abstractions;

namespace Elsa.Integrations.Telnyx.Payloads.Call;

public record CallMachineGreetingEndedBase : CallPayload
{
    public string Result { get; set; } = null!;
}