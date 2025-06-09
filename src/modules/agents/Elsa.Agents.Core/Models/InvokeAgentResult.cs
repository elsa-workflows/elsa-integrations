using System.Text.Json;

namespace Elsa.Agents;

public record InvokeAgentResult(AgentConfig Function, string Response)
{
    public object? ParseResult()
    {
        var targetType = Type.GetType(Function.OutputVariable.Type) ?? typeof(JsonElement);
        return JsonSerializer.Deserialize(Response, targetType);
    }
}