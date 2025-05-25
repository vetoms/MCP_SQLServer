using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Text;
using ModelContextProtocol.Server;

namespace MCP_SQL
{
    [McpServerToolType]
    public sealed class SQLTools
    {
        [McpServerTool, Description("Get all data from a SQL Server DB.")]
        public static async Task<string> ExecuteSql(
            [Description("SQL query")] string sql,
            [Description("connectionString of the SQL server")] string _connectionString
            )
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            var resultBuilder = new StringBuilder();

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    resultBuilder.Append($"{reader.GetName(i)}: {reader[i]}");
                    if (i < reader.FieldCount - 1)
                        resultBuilder.Append(" | ");
                }
                resultBuilder.AppendLine();
            }

            return resultBuilder.ToString();

        }
    }
}
