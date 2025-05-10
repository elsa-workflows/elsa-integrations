using System.ComponentModel.DataAnnotations;

namespace Elsa.Integrations.Agents.Api.Endpoints.ApiKeys.Delete;

public class Request
{
    [Required] public string Id { get; set; } = null!;
}