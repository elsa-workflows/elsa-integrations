using Microsoft.Extensions.AI;

namespace Elsa.Agents;

public interface IAgentServiceProvider
{
    string Name { get; }
    void ConfigureKernel(KernelBuilderContext context);
    IChatClient CreateChatClient(ChatClientContext context);
}