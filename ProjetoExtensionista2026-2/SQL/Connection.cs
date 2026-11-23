using DotNetEnv;
using System;
using System.IO;
using MySql.Data.MySqlClient;

namespace ProjetoExtensionista2026_2.SQL
{
    public class Connection : IDisposable
    {
        private static string _cachedConnectionString;
        private static readonly object _lock = new object();
        public MySqlConnection Conexao { get; private set; }

        public static void ResetCache()
        {
            lock (_lock)
            {
                _cachedConnectionString = null;
            }
        }

        public static bool TestConnection()
        {
            try
            {
                CarregarVariaveisAmbiente();
                string connString = ObterConnectionStringMySql();
                return TestarConnectionString(connString);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool TestConnection(string server, uint port, string user, string password, string database = "sistema")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(server))
                    return false;

                var builder = new MySqlConnectionStringBuilder
                {
                    Server = server.Trim(),
                    Port = port == 0 ? 3306 : port,
                    UserID = user ?? string.Empty,
                    Password = password ?? string.Empty,
                    Database = string.IsNullOrWhiteSpace(database) ? "sistema" : database
                };

                return TestarConnectionString(builder.ConnectionString);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool TestarConnectionString(string connString)
        {
            if (string.IsNullOrWhiteSpace(connString))
                return false;

            using (var conn = new MySqlConnection(connString))
            {
                conn.Open();
                conn.Close();
            }

            return true;
        }

        public static DbConfig GetCurrentConfigOrFallback()
        {
            var jsonConfig = DbConfig.LoadFromJson();
            if (jsonConfig != null && !string.IsNullOrWhiteSpace(jsonConfig.Server))
                return jsonConfig;

            try
            {
                CarregarVariaveisAmbiente();
                var rawConn = Environment.GetEnvironmentVariable("MYSQL_CONNECTION");
                if (!string.IsNullOrWhiteSpace(rawConn))
                {
                    var builder = new MySqlConnectionStringBuilder(rawConn);
                    return new DbConfig
                    {
                        Server = string.IsNullOrWhiteSpace(builder.Server) ? "127.0.0.1" : builder.Server,
                        Port = builder.Port == 0 ? 3306 : builder.Port,
                        User = string.IsNullOrWhiteSpace(builder.UserID) ? "root" : builder.UserID,
                        Password = builder.Password ?? string.Empty,
                        Database = string.IsNullOrWhiteSpace(builder.Database) ? "sistema" : builder.Database
                    };
                }
            }
            catch
            {
                // Ignora erro de parse do .env e retorna valores padrão.
            }

            return new DbConfig();
        }

        private static void CarregarVariaveisAmbiente()
        {
            string envFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".env");
            if (File.Exists(envFile))
            {
                DotNetEnv.Env.Load(envFile);
            }
            else
            {
                DotNetEnv.Env.TraversePath().Load();
            }
        }

        private static string ObterConnectionStringMySql()
        {
            var jsonConfig = DbConfig.LoadFromJson();
            if (jsonConfig != null && !string.IsNullOrWhiteSpace(jsonConfig.Server))
            {
                var jsonBuilder = new MySqlConnectionStringBuilder
                {
                    Server = jsonConfig.Server.Trim(),
                    Port = jsonConfig.Port == 0 ? 3306 : jsonConfig.Port,
                    UserID = jsonConfig.User ?? string.Empty,
                    Password = jsonConfig.Password ?? string.Empty,
                    Database = string.IsNullOrWhiteSpace(jsonConfig.Database) ? "sistema" : jsonConfig.Database
                };
                return jsonBuilder.ConnectionString;
            }

            var rawConn = Environment.GetEnvironmentVariable("MYSQL_CONNECTION");
            if (string.IsNullOrWhiteSpace(rawConn))
                throw new InvalidOperationException("A variável de ambiente MYSQL_CONNECTION não foi definida.");

            var builder = new MySqlConnectionStringBuilder(rawConn);
            if (string.IsNullOrWhiteSpace(builder.Server))
                throw new InvalidOperationException("A connection string MYSQL_CONNECTION não contém o servidor MySQL.");
            return builder.ConnectionString;
        }

        public void IniciarConexao2()
        {
            IniciarConexao();
        }

        public void IniciarConexao()
        {
            if (string.IsNullOrEmpty(_cachedConnectionString))
            {
                lock (_lock)
                {
                    if (string.IsNullOrEmpty(_cachedConnectionString))
                    {
                        CarregarVariaveisAmbiente();
                        _cachedConnectionString = ObterConnectionStringMySql();
                    }
                }
            }

            Conexao = new MySqlConnection(_cachedConnectionString);
            Conexao.Open();
        }

        public void FecharConexao()
        {
            if (Conexao != null && Conexao.State == System.Data.ConnectionState.Open)
            {
                Conexao.Close();
                Conexao.Dispose();
            }
        }

        public void Dispose()
        {
            FecharConexao();
        }
    }
}
