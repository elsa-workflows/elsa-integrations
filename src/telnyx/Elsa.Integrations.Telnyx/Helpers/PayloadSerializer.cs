using System.Text.Json;
using Elsa.Integrations.Telnyx.Payloads;
using Elsa.Integrations.Telnyx.Payloads.Abstractions;

namespace Elsa.Integrations.Telnyx.Helpers;

internal static class PayloadSerializer
{
    public static Payload Deserialize(string eventType, JsonElement dataModel, JsonSerializerOptions? options = null)
    {
        var payloadType = WebhookPayloadTypes.PayloadTypeDictionary.TryGetValue(eventType, out var value) ? value : typeof(UnsupportedPayload);
        return (Payload)dataModel.Deserialize(payloadType, options)!;
    }
}