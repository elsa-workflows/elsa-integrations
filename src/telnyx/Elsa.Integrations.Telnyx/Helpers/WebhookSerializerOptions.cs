using System.Text.Json;
using Elsa.Integrations.Telnyx.Serialization;

namespace Elsa.Integrations.Telnyx.Helpers;

internal static class WebhookSerializerOptions
{
    public static JsonSerializerOptions Create()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = new SnakeCaseNamingPolicy()
        };
            
        options.Converters.Add(new WebhookDataJsonConverter());
        return options;
    }
}