using Elsa.Persistence.Dapper.Contracts;
using Elsa.Persistence.Dapper.HostedServices;
using Elsa.Persistence.Dapper.Migrations.Management;
using Elsa.Extensions;
using Elsa.Features.Abstractions;
using Elsa.Features.Services;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Persistence.Dapper.Features;

/// <summary>
/// Configures migrations.
/// </summary>
public class DapperMigrationsFeature(IModule module) : FeatureBase(module)
{
    /// <summary>
    /// Configures migrations to use SQLite.
    /// </summary>
    public DapperMigrationsFeature UseSqlite()
    {
        ConfigureRunner += runner => runner.AddSQLite();
        return this;
    }

    /// <summary>
    /// Configures migrations to use SQLite.
    /// </summary>
    public DapperMigrationsFeature UseSqlServer()
    {
        ConfigureRunner += runner => runner.AddSqlServer();
        return this;
    }

    /// <summary>
    /// Gets or sets a delegate to configure migrations.
    /// </summary>
    public Action<IMigrationRunnerBuilder> ConfigureRunner { get; set; } = runner => runner
        .WithGlobalConnectionString(sp => sp.GetRequiredService<IDbConnectionProvider>().GetConnectionString())
        .WithMigrationsIn(typeof(Initial).Assembly);

    /// <inheritdoc />
    public override void Configure()
    {
        Services.AddFluentMigratorCore();
        Services.ConfigureRunner(ConfigureRunner);
        Services.AddStartupTask<RunMigrationsStartupTask>();
    }
}