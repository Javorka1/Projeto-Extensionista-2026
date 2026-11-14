using System;
using System.Data;
using Npgsql;

namespace ProjetoExtensionista2026_2.SQL.SQL_functions
{
    internal static class AuditLogSQL
    {
        public static void EnsureSchema()
        {
            using (var connection = new Connection())
            {
                connection.IniciarConexao();
                using (var command = connection.Conexao.CreateCommand())
                {
                    command.CommandText = @"
                        ALTER TABLE ""Users"" ADD COLUMN IF NOT EXISTS active BOOLEAN NOT NULL DEFAULT TRUE;
                        ALTER TABLE ""Users"" ADD COLUMN IF NOT EXISTS created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW();
                        UPDATE ""Users"" SET created_at = NOW() WHERE created_at IS NULL;
                        CREATE TABLE IF NOT EXISTS logs (
                            id BIGSERIAL PRIMARY KEY,
                            occurred_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
                            action VARCHAR(120) NOT NULL,
                            username VARCHAR(80) NOT NULL
                        );";
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void Register(string action, string username)
        {
            if (string.IsNullOrWhiteSpace(action))
                return;

            try
            {
                using (var connection = new Connection())
                {
                    connection.IniciarConexao();
                    using (var command = connection.Conexao.CreateCommand())
                    {
                        command.CommandText = @"
                            INSERT INTO logs (created_at, action, username)
                            VALUES (NOW(), @action, @username);";
                        command.Parameters.AddWithValue("action", action);
                        command.Parameters.AddWithValue("username", string.IsNullOrWhiteSpace(username) ? "Sistema" : username);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                // O histórico não deve impedir a operação principal do sistema.
            }
        }

        public static DataTable GetLogs()
        {
            var table = new DataTable();
            using (var connection = new Connection())
            {
                connection.IniciarConexao();
                using (var command = connection.Conexao.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT created_at AS occurred_at, action, username
                        FROM logs
                        ORDER BY created_at DESC
                        LIMIT 500;";
                    using (var adapter = new NpgsqlDataAdapter(command))
                    {
                        adapter.Fill(table);
                    }
                }
            }
            return table;
        }
    }
}
