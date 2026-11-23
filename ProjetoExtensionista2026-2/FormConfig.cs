using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public partial class FormConfig : Form
    {
        public FormConfig()
        {
            InitializeComponent();
            AtualizarTextoBotao();
        }

        private void formConfig_Load(object sender, EventArgs e)
        {
            GerenciadorTema.AplicarTema(this);
            bool isAdmin = EhAdmin();
            btnConfigurarBanco.Visible = isAdmin;
            tblBotoesBanco.ColumnStyles[0].Width = isAdmin ? 50F : 100F;
            tblBotoesBanco.ColumnStyles[1].Width = isAdmin ? 50F : 0F;
        }

        private static bool EhAdmin()
        {
            string role = (SQL.SQL_functions.LoginSQL.SessaoUsuario.Role ?? string.Empty).Trim();
            return string.Equals(role, "admin", StringComparison.OrdinalIgnoreCase);
        }

        private void btnAlternarTema_Click(object sender, EventArgs e)
        {

            GerenciadorTema.ModoEscuro = !GerenciadorTema.ModoEscuro;


            GerenciadorTema.SalvarTemaAtual();


            foreach (Form formAberto in Application.OpenForms)
            {
                GerenciadorTema.AplicarTema(formAberto);
            }


            AtualizarTextoBotao();
        }

        private void btnTestarBanco_Click(object sender, EventArgs e)
        {
            lblStatusBanco.Text = "Conectando ao MySQL...";
            lblStatusBanco.ForeColor = Color.DarkOrange;
            btnTestarBanco.Enabled = false;
            this.Refresh();

            bool conectado = SQL.Connection.TestConnection();

            if (conectado)
            {
                lblStatusBanco.Text = "Status: Conectado com sucesso!";
                lblStatusBanco.ForeColor = Color.ForestGreen;
            }
            else
            {
                lblStatusBanco.Text = "Status: Falha na conexão com o banco!";
                lblStatusBanco.ForeColor = Color.Red;
            }

            btnTestarBanco.Enabled = true;
        }

        private void btnConfigurarBanco_Click(object sender, EventArgs e)
        {
            if (!EhAdmin())
            {
                MessageBox.Show(
                    "Apenas administradores podem configurar a conexão com o banco.",
                    "Acesso Negado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            using (var formBanco = new FormConfigBanco())
            {
                if (formBanco.ShowDialog(this) == DialogResult.OK)
                {
                    lblStatusBanco.Text = "Status: Configuração salva. Clique em Testar Conexão.";
                    lblStatusBanco.ForeColor = Color.DarkOrange;
                }
            }
        }

        private void btnAlterarSenha_Click(object sender, EventArgs e)
        {
            using (FormAlterarSenha formSenha = new FormAlterarSenha())
            {
                formSenha.ShowDialog(this);
            }
        }

        private void AtualizarTextoBotao()
        {
            if (GerenciadorTema.ModoEscuro)
            {
                btnAlternarTema.Text = "Mudar para Modo Claro";
            }
            else
            {
                btnAlternarTema.Text = "Mudar para Modo Escuro";
            }
        }

        private void lnkGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "https://github.com/Javorka1/Projeto-Extensionista-2026",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível abrir o link: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
