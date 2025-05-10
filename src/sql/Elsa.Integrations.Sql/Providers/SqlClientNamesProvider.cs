using Elsa.Integrations.Sql.Contracts;
using Elsa.Integrations.Sql.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Integrations.Sql.Providers;

/// <summary>
/// Returns registered client names
/// </summary>
public class SqlClientNamesProvider : ISqlClientNamesProvider
{
    private readonly IServiceProvider _serviceProvider;

    public SqlClientNamesProvider(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public Task<IReadOnlyDictionary<string, Type>> GetRegisteredSqlClientNamesAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(_serviceProvider.GetRequiredService<ClientStore>().Clients);
    }
}