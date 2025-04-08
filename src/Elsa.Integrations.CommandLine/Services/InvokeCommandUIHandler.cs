using System.Reflection;
using CliWrap;
using Elsa.Integrations.CommandLine.Activities;
using Elsa.Workflows;

namespace Elsa.Integrations.CommandLine.Services;

/// <summary>
/// Provides a UI handler for the <see cref="InvokeCommand"/> activity.
/// </summary>
/// <param name="commandFinder"></param>
public class InvokeCommandUIHandler(CommandFinder commandFinder) : IPropertyUIHandler
{
    /// <inheritdoc />
    public ValueTask<IDictionary<string, object>> GetUIPropertiesAsync(
        PropertyInfo propertyInfo, object? context, CancellationToken cancellationToken = default) =>
        propertyInfo.Name switch
        {
            nameof(Command) =>
                ValueTask.FromResult<IDictionary<string, object>>(commandFinder
                    .GetAvailableCommands()
                    .ToDictionary(k => k, v => (object)v)),

            _ => ValueTask.FromResult<IDictionary<string, object>>(null!)
        };
}