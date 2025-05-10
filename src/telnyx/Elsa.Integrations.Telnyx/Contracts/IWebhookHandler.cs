using Microsoft.AspNetCore.Http;

namespace Elsa.Integrations.Telnyx.Contracts;

internal interface IWebhookHandler
{
    Task HandleAsync(HttpContext httpContext);
}