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

        public static bool TestConnection()
        {
            try
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

                var connString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION");
                if (string.IsNullOrWhiteSpace(connString))
                    return false;

                var builder = new MySqlConnectionStringBuilder(connString);
                if (string.IsNullOrWhiteSpace(builder.Server))
                    return false;

                using (var conn = new MySqlConnection(builder.ConnectionString))
                {
                    conn.Open();
                    conn.Close();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
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