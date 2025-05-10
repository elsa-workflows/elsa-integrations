using Elsa.EntityFrameworkCore.Abstractions;
using Elsa.EntityFrameworkCore.Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Elsa.Integrations.Agents.Persistence.EFCore.Sqlite;

[UsedImplicitly]
public class SqliteAgentsDbContextFactory : DesignTimeDbContextFactoryBase<AgentsDbContext>
{
    protected override void ConfigureBuilder(DbContextOptionsBuilder<AgentsDbContext> builder, string connectionString)
    {
        builder.UseElsaSqlite(GetType().Assembly, connectionString);
    }
}