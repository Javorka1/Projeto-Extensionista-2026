using ProjetoExtensionista2026_2.SQL.SQL_functions;
using ProjetoExtensionista2026_2.SQL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ProjetoExtensionista2026_2
{
    public partial class miniFormSignIn : Form
    {
        public miniFormSignIn()
        {
            InitializeComponent();
            ConfigurarPlaceholder();
        }
        private void ConfigurarPlaceholder()
        {
            nameTextBox.Text = "Insira seu nome de usuário";
            pwdTextBox.Text = "Insira sua senha";
            nameTextBox.ForeColor = Color.Gray;
            pwdTextBox.ForeColor = Color.Gray;
            pwdTextBox.UseSystemPasswordChar = false;
        }
        private void TextBox_Enter(object sender, EventArgs e)
        {
            if (sender == nameTextBox && nameTextBox.Text == "Insira seu nome de usuário")
            {
                nameTextBox.Text = "";
                nameTextBox.ForeColor = Color.Black;
            }
            else if (sender == pwdTextBox && pwdTextBox.Text == "Insira sua senha")
            {
                pwdTextBox.Text = "";
                pwdTextBox.ForeColor = Color.Black;
                pwdTextBox.UseSystemPasswordChar = true;
            }
        }

        private void TxtSenha_Leave(object sender, EventArgs e)
        {
            if (sender == nameTextBox && string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                nameTextBox.Text = "Insira seu nome de usuário";
                nameTextBox.ForeColor = Color.Gray;
            }
            else if (sender == pwdTextBox && string.IsNullOrWhiteSpace(pwdTextBox.Text))
            {
                pwdTextBox.Text = "Insira sua senha";
                pwdTextBox.ForeColor = Color.Gray;
                pwdTextBox.UseSystemPasswordChar = false;
            }
        }

        private async void RegisterBtn_Click(object sender, EventArgs e)
        {
            LoginSQL loginSQL = new LoginSQL();
            string username = nameTextBox.Text;
            string password = pwdTextBox.Text;
            string hash = loginSQL.HashificarSenha(password, out _);
            int result = loginSQL.RegistrarUsuario(username, hash);
            if (result == 1){
                AuditLogSQL.Register("Usuário cadastrado: " + username.Trim(), "Sistema");
                this.Close();
            }
        }
    }
}
