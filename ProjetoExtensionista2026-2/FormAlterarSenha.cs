using System;
using System.Drawing;
using System.Windows.Forms;
using ProjetoExtensionista2026_2.SQL.SQL_functions;

namespace ProjetoExtensionista2026_2
{
    public partial class FormAlterarSenha : Form
    {
        public FormAlterarSenha()
        {
            InitializeComponent();
        }

        private void FormAlterarSenha_Load(object sender, EventArgs e)
        {
            GerenciadorTema.AplicarTema(this);
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            string senhaAtual = txtSenhaAtual.Text;
            string novaSenha = txtNovaSenha.Text;
            string confirmaSenha = txtConfirmaSenha.Text;


            if (string.IsNullOrEmpty(senhaAtual))
            {
                MessageBox.Show("Por favor, digite a sua senha atual.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSenhaAtual.Focus();
                return;
            }

            if (string.IsNullOrEmpty(novaSenha))
            {
                MessageBox.Show("Por favor, digite a nova senha.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNovaSenha.Focus();
                return;
            }

            if (novaSenha.Length < 4)
            {
                MessageBox.Show("A nova senha deve ter no mínimo 4 caracteres.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNovaSenha.Focus();
                return;
            }

            if (novaSenha != confirmaSenha)
            {
                MessageBox.Show("A confirmação da nova senha não confere com a nova senha digitada.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtConfirmaSenha.Focus();
                return;
            }


            bool sucesso = Program.AlterarSenha(senhaAtual, novaSenha);

            if (sucesso)
            {
                AuditLogSQL.Register("Senha alterada", LoginSQL.SessaoUsuario.Nome);
                MessageBox.Show("Senha alterada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("A senha atual informada está incorreta.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSenhaAtual.SelectAll();
                txtSenhaAtual.Focus();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

