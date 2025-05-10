using Elsa.Agents;
using Elsa.Common.Entities;

namespace Elsa.Integrations.Agents.Persistence.Entities;

public class AgentDefinition : Entity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public AgentConfig AgentConfig { get; set; } = null!;
    public AgentConfig ToAgentConfig() => AgentConfig;
}