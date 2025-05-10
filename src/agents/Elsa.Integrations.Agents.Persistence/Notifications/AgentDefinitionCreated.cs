using Elsa.Integrations.Agents.Persistence.Entities;
using Elsa.Mediator.Contracts;

namespace Elsa.Integrations.Agents.Persistence.Notifications;

public record AgentDefinitionCreated(AgentDefinition AgentDefinition) : INotification;