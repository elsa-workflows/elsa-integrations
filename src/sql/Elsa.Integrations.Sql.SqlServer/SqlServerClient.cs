using System.Data.Common;
using Elsa.Integrations.Sql.Client;
using Microsoft.Data.SqlClient;

namespace Elsa.Integrations.Sql.SqlServer;

/// <summary>
/// Microsoft SQL server client implementation.
/// </summary>
/// <param name="connectionString"></param>
public class SqlServerClient(string connectionString) : BaseSqlClient(connectionString)
{
    protected override DbConnection CreateConnection() => new SqlConnection(_connectionString);

    protected override DbCommand CreateCommand(string query, DbConnection connection) => new SqlCommand(query, (SqlConnection)connection);
}