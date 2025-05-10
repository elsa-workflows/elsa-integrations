using Elsa.Abstractions;
using Elsa.Integrations.Agents.Persistence.Contracts;
using JetBrains.Annotations;

namespace Elsa.Integrations.Agents.Api.Endpoints.Services.Delete;

/// <summary>
/// Delete an API key.
/// </summary>
[UsedImplicitly]
public class Endpoint(IServiceStore store) : ElsaEndpoint<Request>
{
    /// <inheritdoc />
    public override void Configure()
    {
        Delete("/ai/services/{id}");
        ConfigurePermissions("ai/services:delete");
    }

    /// <inheritdoc />
    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var entity = await store.GetAsync(req.Id, ct);
        
        if(entity == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        
        await store.DeleteAsync(entity, ct);
    }
}