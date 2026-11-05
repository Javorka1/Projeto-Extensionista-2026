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
        }

        private void btnAlternarTema_Click(object sender, EventArgs e)
        {
            // 1. Inverte o tema atual
            GerenciadorTema.ModoEscuro = !GerenciadorTema.ModoEscuro;

            // 2. Salva a preferência
            GerenciadorTema.SalvarTemaAtual();

            // 3. Aplica o novo tema nesta tela e em todas as outras telas abertas
            foreach (Form formAberto in Application.OpenForms)
            {
                GerenciadorTema.AplicarTema(formAberto);
            }

            // 4. Atualiza o texto do botão
            AtualizarTextoBotao();
        }

        private void btnTestarBanco_Click(object sender, EventArgs e)
        {
            lblStatusBanco.Text = "Conectando ao Supabase...";
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
