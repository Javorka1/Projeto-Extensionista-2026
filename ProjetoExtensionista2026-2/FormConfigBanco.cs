using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ProjetoExtensionista2026_2.SQL;
using ProjetoExtensionista2026_2.SQL.SQL_functions;

namespace ProjetoExtensionista2026_2
{
    public partial class FormConfigBanco : Form
    {
        public FormConfigBanco()
        {
            InitializeComponent();
        }

        private void FormConfigBanco_Load(object sender, EventArgs e)
        {
            string role = (LoginSQL.SessaoUsuario.Role ?? string.Empty).Trim();
            if (!string.Equals(role, "admin", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "Apenas administradores podem alterar as configurações do banco de dados.",
                    "Acesso Negado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            GerenciadorTema.AplicarTema(this);
            CarregarDadosAtuais();
        }

        private void CarregarDadosAtuais()
        {
            DbConfig atual = Connection.GetCurrentConfigOrFallback();

            txtServidor.Text = atual.Server ?? "127.0.0.1";
            txtPorta.Text = (atual.Port == 0 ? 3306 : atual.Port).ToString();
            txtUsuario.Text = atual.User ?? "root";
            txtSenha.Text = atual.Password ?? string.Empty;
            txtBanco.Text = string.IsNullOrWhiteSpace(atual.Database) ? "sistema" : atual.Database;
            txtCaminhoJson.Text = DbConfig.GetActiveConfigPath();
        }

        private bool TentarLerCampos(out DbConfig config, out string mensagemErro)
        {
            config = null;
            mensagemErro = null;

            string servidor = txtServidor.Text.Trim();
            if (string.IsNullOrWhiteSpace(servidor))
            {
                mensagemErro = "Informe o IP ou endereço do servidor.";
                txtServidor.Focus();
                return false;
            }

            if (!uint.TryParse(txtPorta.Text.Trim(), out uint porta) || porta == 0 || porta > 65535)
            {
                mensagemErro = "Informe uma porta válida (entre 1 e 65535).";
                txtPorta.Focus();
                return false;
            }

            string usuario = txtUsuario.Text.Trim();
            if (string.IsNullOrWhiteSpace(usuario))
            {
                mensagemErro = "Informe o usuário do banco de dados.";
                txtUsuario.Focus();
                return false;
            }

            string banco = txtBanco.Text.Trim();
            if (string.IsNullOrWhiteSpace(banco))
            {
                mensagemErro = "Informe o nome do banco de dados.";
                txtBanco.Focus();
                return false;
            }

            config = new DbConfig
            {
                Server = servidor,
                Port = porta,
                User = usuario,
                Password = txtSenha.Text ?? string.Empty,
                Database = banco
            };
            return true;
        }

        private void chkMostrarSenha_CheckedChanged(object sender, EventArgs e)
        {
            txtSenha.UseSystemPasswordChar = !chkMostrarSenha.Checked;
        }

        private void btnAlterarCaminho_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Title = "Escolher local do arquivo JSON de configuração";
                dialog.Filter = "Arquivos JSON (*.json)|*.json";
                dialog.DefaultExt = "json";
                dialog.OverwritePrompt = false;

                string caminhoAtual = txtCaminhoJson.Text.Trim();
                if (!string.IsNullOrWhiteSpace(caminhoAtual))
                {
                    string dir = Path.GetDirectoryName(caminhoAtual);
                    if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                        dialog.InitialDirectory = dir;
                    dialog.FileName = Path.GetFileName(caminhoAtual);
                }
                else
                {
                    dialog.FileName = "dbconfig.json";
                }

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    txtCaminhoJson.Text = dialog.FileName;
                }
            }
        }

        private void btnTestar_Click(object sender, EventArgs e)
        {
            if (!TentarLerCampos(out DbConfig cfg, out string erro))
            {
                MessageBox.Show(erro, "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lblResultadoTeste.Text = "Testando conexão...";
            lblResultadoTeste.ForeColor = Color.DarkOrange;
            btnTestar.Enabled = false;
            this.Refresh();

            bool ok = Connection.TestConnection(cfg.Server, cfg.Port, cfg.User, cfg.Password, cfg.Database);

            if (ok)
            {
                lblResultadoTeste.Text = "Conexão bem-sucedida!";
                lblResultadoTeste.ForeColor = Color.ForestGreen;
            }
            else
            {
                lblResultadoTeste.Text = "Falha ao conectar com esses dados.";
                lblResultadoTeste.ForeColor = Color.Red;
            }

            btnTestar.Enabled = true;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (!TentarLerCampos(out DbConfig cfg, out string erro))
            {
                MessageBox.Show(erro, "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string caminho = txtCaminhoJson.Text.Trim();
            if (string.IsNullOrWhiteSpace(caminho))
                caminho = DbConfig.DefaultConfigPath;

            try
            {
                DbConfig.SaveToJson(cfg, caminho);
                MessageBox.Show(
                    "Configurações salvas em:\n" + caminho,
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível salvar o arquivo JSON:\n" + ex.Message,
                    "Erro ao Salvar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
