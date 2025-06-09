using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;

namespace Elsa.Agents;

public class OpenAIChatCompletionProvider : IAgentServiceProvider
{
    public string Name => "OpenAIChatCompletion";
    public void ConfigureKernel(KernelBuilderContext context)
    {
        var modelId = (string)context.ServiceConfig.Settings["ModelId"];
        var apiKey = context.GetApiKey();
        context.KernelBuilder.AddOpenAIChatCompletion(modelId, apiKey);
    }

    public IChatClient CreateChatClient(ChatClientContext context)
    {
        var modelId = (string)context.ServiceConfig.Settings["ModelId"];
        var apiKey = context.GetApiKey();
        var client = new OpenAIClient(apiKey).AsChatClient(modelId);
        return new ChatClientBuilder(client).Build();
    }
}