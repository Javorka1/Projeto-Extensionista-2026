using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using ProjetoExtensionista2026_2.SQL;

namespace ProjetoExtensionista2026_2.SQL.SQL_functions
{
    internal class LoginSQL : IDisposable
    {
        public static class SessaoUsuario
        {
            public static string Role { get; set; } = string.Empty;
            public static string Nome { get; set; } = string.Empty;
            public static string Hash { get; set; } = string.Empty;

            public static void EncerrarSessao()
            {
                Role = string.Empty;
                Nome = string.Empty;
                Hash = string.Empty;
            }

            public static bool EstaAutenticado => !string.IsNullOrEmpty(Nome);
        }
        public string HashificarSenha(string password, out string passwordHash)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return passwordHash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        public string RolePicker(string username)
        {
            if (!string.IsNullOrWhiteSpace(username))
            {
                using (var conn = new Connection())
                {
                    conn.IniciarConexao();
                    try
                    {
                        using (var cmd = conn.Conexao.CreateCommand())
                        {
                            cmd.CommandText = "SELECT role FROM `Users` WHERE username = @u;";
                            cmd.Parameters.AddWithValue("u", username);
                            var role = cmd.ExecuteScalar()?.ToString();
                            return role;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Erro ao buscar papel do usuário", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                    finally
                    {
                        conn.FecharConexao();
                    }
                }
            }
            else
            {
                MessageBox.Show("Nome de usuário inválido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }


        public int VerificarUsuario(string username, string passwordHash)
        {
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(passwordHash))
            {
                using (var conn = new Connection())
                {
                    conn.IniciarConexao();
                    try
                    {
                        using (var cmd = conn.Conexao.CreateCommand())
                        {
                            cmd.CommandText = "SELECT count(*) FROM `Users` WHERE username = @u AND hash = @p;";
                            cmd.Parameters.AddWithValue("u", username);
                            cmd.Parameters.AddWithValue("p", passwordHash);
                            int count = Convert.ToInt32(cmd.ExecuteScalar());
                            return count;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Erro ao verificar usuário", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return -1;
                    }
                    finally
                    {
                        conn.FecharConexao();
                    }
                }
            }
            else
            {
                MessageBox.Show("Nome de usuário ou senha inválidos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
        }

        public int RegistrarUsuario(string username, string passwordHash)
        {
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(passwordHash))
            {
                using (var conn = new Connection())
                {
                    conn.IniciarConexao();
                    try
                    {
                        using (var cmd = conn.Conexao.CreateCommand())
                        {
                            cmd.CommandText = "INSERT INTO `Users` (username, hash) VALUES (@u, @p);";
                            cmd.Parameters.AddWithValue("u", username);
                            cmd.Parameters.AddWithValue("p", passwordHash);
                            cmd.ExecuteNonQuery();
                            return 1;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Erro ao registrar usuário", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return -1;
                    }
                    finally
                    {
                        conn.FecharConexao();
                    }
                }
            }
            else
            {
                MessageBox.Show("Nome de usuário ou senha inválidos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
