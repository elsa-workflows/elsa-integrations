using System.Text.Json;


namespace Elsa.Agents;

public static class FunctionResultExtensions
{
    public static async Task<JsonElement> AsJsonElementAsync(this Task<InvokeAgentResult> resultTask)
    {
        var result = await resultTask;
        return JsonSerializer.Deserialize<JsonElement>(result.Response);
    }
}