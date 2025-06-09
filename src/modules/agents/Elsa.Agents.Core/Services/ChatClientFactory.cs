using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Elsa.Agents;

public class ChatClientFactory(IServiceDiscoverer serviceDiscoverer, ILogger<ChatClientFactory> logger)
{
    public IChatClient CreateChatClient(KernelConfig kernelConfig, string agentName)
    {
        var agent = kernelConfig.Agents[agentName];
        var services = serviceDiscoverer.Discover().ToDictionary(x => x.Name);

        foreach (var serviceName in agent.Services)
        {
            if (!kernelConfig.Services.TryGetValue(serviceName, out var serviceConfig))
            {
                logger.LogWarning($"Service {serviceName} not found");
                continue;
            }

            if (!services.TryGetValue(serviceConfig.Type, out var provider))
            {
                logger.LogWarning($"Service provider {serviceConfig.Type} not found");
                continue;
            }

            var context = new ChatClientContext(kernelConfig, serviceConfig);
            return provider.CreateChatClient(context);
        }

        throw new InvalidOperationException("No service provider configured for agent");
    }
}
