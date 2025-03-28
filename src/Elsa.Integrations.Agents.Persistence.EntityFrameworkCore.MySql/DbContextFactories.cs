﻿using Elsa.EntityFrameworkCore.Abstractions;
using Elsa.EntityFrameworkCore.Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Elsa.Integrations.Agents.Persistence.EntityFrameworkCore.MySql;

[UsedImplicitly]
public class MySqlAgentsDbContextFactory : DesignTimeDbContextFactoryBase<AgentsDbContext>
{
    protected override void ConfigureBuilder(DbContextOptionsBuilder<AgentsDbContext> builder, string connectionString)
    {
        builder.UseElsaMySql(GetType().Assembly, connectionString, serverVersion: MySqlServerVersion.LatestSupportedServerVersion);
    }
}