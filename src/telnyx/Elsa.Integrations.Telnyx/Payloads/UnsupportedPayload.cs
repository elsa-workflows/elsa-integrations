using System.ComponentModel;
using Elsa.Integrations.Telnyx.Payloads.Abstractions;

namespace Elsa.Integrations.Telnyx.Payloads;

[Browsable(false)]
public sealed record UnsupportedPayload : Payload
{
}