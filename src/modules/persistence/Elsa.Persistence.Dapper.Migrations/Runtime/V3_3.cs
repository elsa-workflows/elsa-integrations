using System.Diagnostics.CodeAnalysis;
using FluentMigrator;
using JetBrains.Annotations;
using static System.Int32;

namespace Elsa.Persistence.Dapper.Migrations.Runtime;

/// <inheritdoc />
[Migration(20004, "Elsa:Runtime:V3.3")]
[PublicAPI]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class V3_3 : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Alter.Table("Triggers").AddColumn("TenantId").AsString().Nullable();
        Alter.Table("Bookmarks").AddColumn("TenantId").AsString().Nullable();
        Alter.Table("WorkflowExecutionLogRecords").AddColumn("TenantId").AsString().Nullable();
        Alter.Table("ActivityExecutionRecords").AddColumn("TenantId").AsString().Nullable();
        Alter.Table("ActivityExecutionRecords").AddColumn("SerializedProperties").AsString(MaxValue).Nullable();
        Alter.Table("KeyValuePairs").AddColumn("TenantId").AsString().Nullable();

        IfDatabase("SqlServer", "Oracle", "MySql", "Postgres")
            .Create
            .Table("BookmarkQueueItems")
            .WithColumn("Id").AsString().PrimaryKey()
            .WithColumn("TenantId").AsString().Nullable()
            .WithColumn("WorkflowInstanceId").AsString().Nullable()
            .WithColumn("CorrelationId").AsString().Nullable()
            .WithColumn("BookmarkId").AsString().Nullable()
            .WithColumn("StimulusHash").AsString().Nullable()
            .WithColumn("ActivityInstanceId").AsString().Nullable()
            .WithColumn("ActivityTypeName").AsString().Nullable()
            .WithColumn("CreatedAt").AsDateTimeOffset().NotNullable()
            ;

        IfDatabase("Sqlite")
            .Create
            .Table("BookmarkQueueItems")
            .WithColumn("Id").AsString().PrimaryKey()
            .WithColumn("TenantId").AsString().Nullable()
            .WithColumn("WorkflowInstanceId").AsString().Nullable()
            .WithColumn("CorrelationId").AsString().Nullable()
            .WithColumn("BookmarkId").AsString().Nullable()
            .WithColumn("StimulusHash").AsString().Nullable()
            .WithColumn("ActivityInstanceId").AsString().Nullable()
            .WithColumn("ActivityTypeName").AsString().Nullable()
            .WithColumn("CreatedAt").AsDateTime2().NotNullable()
            ;
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete.Column("TenantId").FromTable("Triggers");
        Delete.Column("TenantId").FromTable("Bookmarks");
        Delete.Column("TenantId").FromTable("WorkflowExecutionLogRecords");
        Delete.Column("TenantId").FromTable("ActivityExecutionRecords");
        Delete.Column("SerializedProperties").FromTable("ActivityExecutionRecords");
        Delete.Column("TenantId").FromTable("KeyValuePairs");
        Rename.Column("Id").OnTable("KeyValuePairs").To("Key");
        Delete.Table("BookmarkQueueItems");
    }
}