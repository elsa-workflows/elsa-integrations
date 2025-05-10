using Elsa.Integrations.Telnyx.Models;
using Elsa.Integrations.Telnyx.Payloads.Abstractions;

namespace Elsa.Integrations.Telnyx.Extensions;

/// <summary>
/// Provides extensions on <see cref="Payload"/>.
/// </summary>
public static class PayloadExtensions
{
    /// <summary>
    /// Extracts a <see cref="ClientStatePayload"/> from the specified <see cref="Payload"/>. 
    /// </summary>
    public static ClientStatePayload? GetClientStatePayload(this Payload payload)
    {
        return !string.IsNullOrWhiteSpace(payload.ClientState) 
            ? ClientStatePayload.FromBase64(payload.ClientState) 
            : null;
    }
}