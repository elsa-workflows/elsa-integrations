using System.Diagnostics.CodeAnalysis;
using FluentMigrator;
using JetBrains.Annotations;

namespace Elsa.Persistence.Dapper.Migrations.Identity;

/// <inheritdoc />
[Migration(30003, "Elsa:Identity:V3.2")]
[PublicAPI]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class V3_2 : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
    }

    /// <inheritdoc />
    public override void Down()
    {
    }
}