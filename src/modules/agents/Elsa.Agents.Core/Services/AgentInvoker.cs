using Microsoft.Extensions.AI;


namespace Elsa.Agents;

public class AgentInvoker(ChatClientFactory chatClientFactory, IKernelConfigProvider kernelConfigProvider)
{
    public async Task<InvokeAgentResult> InvokeAgentAsync(string agentName, IDictionary<string, object?> input, CancellationToken cancellationToken = default)
    {
        var kernelConfig = await kernelConfigProvider.GetKernelConfigAsync(cancellationToken);
        var chatClient = chatClientFactory.CreateChatClient(kernelConfig, agentName);
        var agentConfig = kernelConfig.Agents[agentName];
        var prompt = agentConfig.PromptTemplate;
        foreach (var variable in agentConfig.InputVariables)
        {
            if (input.TryGetValue(variable.Name, out var value))
                prompt = prompt.Replace($"{{{{{variable.Name}}}}}", value?.ToString());
        }

        var completion = await chatClient.CompleteAsync(prompt, cancellationToken);
        return new(agentConfig, completion.Message);
    }
}