using System;
using System.Drawing;
using System.Windows.Forms;
using ProjetoExtensionista2026_2.SQL.SQL_functions;

namespace ProjetoExtensionista2026_2
{
    public partial class formLogin : Form
    {
        public formLogin()
        {
            InitializeComponent();
            ConfigurarPlaceholder();
            try
            {
                lockIcon.Image = Properties.Resources.cadeado;
            }
            catch { }
        }

        private void formLogin_Load(object sender, EventArgs e)
        {
            CentralizarCard();
            GerenciadorTema.AplicarTema(this);
        }

        private void formLogin_Resize(object sender, EventArgs e)
        {
            CentralizarCard();
        }

        private void CentralizarCard()
        {
            if (pnlCardLogin != null && panel1 != null)
            {
                int x = Math.Max(10, (panel1.ClientSize.Width - pnlCardLogin.Width) / 2);
                int y = Math.Max(10, (panel1.ClientSize.Height - pnlCardLogin.Height) / 2);
                pnlCardLogin.Location = new Point(x, y);
            }
        }

        private void AlternarCadeado(object sender, EventArgs e)
        {
            string username = userTxtBox.Text.Trim();
            string password = pwdTxtBox.Text;

            if (username == "Insira seu nome de usuário" || string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Por favor, insira o nome de usuário.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                userTxtBox.Focus();
                return;
            }

            if (password == "Insira sua senha" || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Por favor, insira a senha.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                pwdTxtBox.Focus();
                return;
            }

            using (LoginSQL loginSQL = new LoginSQL())
            {
                string hash = loginSQL.HashificarSenha(password, out _);
                int count = loginSQL.VerificarUsuario(username, hash);
                bool ver = count == 1;

                if (ver)
                {
                    try
                    {
                        lockIcon.Image = Properties.Resources.cadeado_aberto;
                    }
                    catch { }

                    LoginSQL.SessaoUsuario.Nome = username;
                    LoginSQL.SessaoUsuario.Hash = hash;

                    formPrincipal principal = new formPrincipal();
                    principal.FormClosed += (s, args) => this.Close();

                    principal.Show();
                    this.Hide();
                }
                else
                {
                    try
                    {
                        lockIcon.Image = Properties.Resources.cadeado;
                    }
                    catch { }
                    MessageBox.Show("Nome de usuário ou senha inválidos.", "Erro de Autenticação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void ConfigurarPlaceholder()
        {
            userTxtBox.Text = "Insira seu nome de usuário";
            pwdTxtBox.Text = "Insira sua senha";
            userTxtBox.ForeColor = Color.Gray;
            pwdTxtBox.ForeColor = Color.Gray;
            pwdTxtBox.UseSystemPasswordChar = false;
        }

        private void TextBox_Enter(object sender, EventArgs e)
        {
            if (sender == userTxtBox && userTxtBox.Text == "Insira seu nome de usuário")
            {
                userTxtBox.Text = "";
                userTxtBox.ForeColor = GerenciadorTema.ModoEscuro ? GerenciadorTema.TextoEscuro : GerenciadorTema.TextoClaro;
            }
            else if (sender == pwdTxtBox && pwdTxtBox.Text == "Insira sua senha")
            {
                pwdTxtBox.Text = "";
                pwdTxtBox.ForeColor = GerenciadorTema.ModoEscuro ? GerenciadorTema.TextoEscuro : GerenciadorTema.TextoClaro;
                pwdTxtBox.UseSystemPasswordChar = true;
            }
        }

        private void TxtSenha_Leave(object sender, EventArgs e)
        {
            if (sender == userTxtBox && string.IsNullOrWhiteSpace(userTxtBox.Text))
            {
                userTxtBox.Text = "Insira seu nome de usuário";
                userTxtBox.ForeColor = Color.Gray;
            }
            else if (sender == pwdTxtBox && string.IsNullOrWhiteSpace(pwdTxtBox.Text))
            {
                pwdTxtBox.Text = "Insira sua senha";
                pwdTxtBox.ForeColor = Color.Gray;
                pwdTxtBox.UseSystemPasswordChar = false;
            }
        }

        private void labelSignIn_Click(object sender, EventArgs e)
        {
            Form signInForm = new miniFormSignIn();
            signInForm.FormBorderStyle = FormBorderStyle.FixedSingle;
            signInForm.StartPosition = FormStartPosition.CenterParent;
            GerenciadorTema.AplicarTema(signInForm);
            signInForm.ShowDialog(this);
        }
    }
}
