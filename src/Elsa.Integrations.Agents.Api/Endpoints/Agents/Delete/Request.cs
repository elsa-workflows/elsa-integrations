using System.ComponentModel.DataAnnotations;

namespace Elsa.Integrations.Agents.Api.Endpoints.Agents.Delete;

public class Request
{
    [Required] public string Id { get; set; } = null!;
}