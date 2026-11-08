using System;
using System.Drawing;
using System.Windows.Forms;
using ProjetoExtensionista2026_2.SQL.SQL_functions;

namespace ProjetoExtensionista2026_2
{
    public partial class formPrincipal : Form
    {
        private Button _botaoNavAtivo;

        public formPrincipal()
        {
            InitializeComponent();
            GerenciadorTema.AplicarTema(this);
        }

        private void formPrincipal_Load(object sender, EventArgs e)
        {
            using (var sql = new LoginSQL()) 
            {
                string nomeUsuario = LoginSQL.SessaoUsuario.Nome;
                string role = sql.RolePicker(nomeUsuario);

                if (!string.IsNullOrEmpty(role))
                {
                    LoginSQL.SessaoUsuario.Role = role;
                    
                    // Configura o Card de Usuário
                    ConfigurarCardUsuario(nomeUsuario, role);

                    // Configura os botões da sidebar de acordo com a permissão
                    var rolesManager = new rolesClass();
                    rolesManager.RoleSidebar(this, role, nomeUsuario, LoginSQL.SessaoUsuario.Hash);

                    // Abre a tela padrão inicial (Vendas se tiver permissão, senão Principal/Financeiro)
                    if (btnNavVendas.Visible)
                    {
                        AtivarNavegacao(btnNavVendas, new formVenda());
                    }
                    else if (btnNavFinanceiro.Visible)
                    {
                        AtivarNavegacao(btnNavFinanceiro, new FormFinanceiro());
                    }
                    else if (btnNavEstoque.Visible)
                    {
                        AtivarNavegacao(btnNavEstoque, new formEstoque());
                    }
                }
                else
                {
                    MessageBox.Show("Não foi possível determinar o papel do usuário. O aplicativo será encerrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
        }

        private void ConfigurarCardUsuario(string nome, string role)
        {
            if (string.IsNullOrWhiteSpace(nome)) nome = "Usuario";

            lblNomeUsuario.Text = nome;

            // Iniciais no avatar
            string iniciais = nome.Length >= 2 ? nome.Substring(0, 2).ToUpper() : nome.ToUpper();
            lblAvatar.Text = iniciais;

            // Formatação do cargo
            switch (role.ToLower())
            {
                case "admin":
                    lblCargoUsuario.Text = "Administrador";
                    lblCargoUsuario.ForeColor = Color.ForestGreen;
                    break;
                case "seller":
                    lblCargoUsuario.Text = "Vendedor";
                    lblCargoUsuario.ForeColor = Color.RoyalBlue;
                    break;
                case "stocker":
                    lblCargoUsuario.Text = "Estoquista";
                    lblCargoUsuario.ForeColor = Color.DarkOrange;
                    break;
                case "monetary":
                    lblCargoUsuario.Text = "Financeiro";
                    lblCargoUsuario.ForeColor = Color.MediumPurple;
                    break;
                default:
                    lblCargoUsuario.Text = role;
                    lblCargoUsuario.ForeColor = Color.Gray;
                    break;
            }
        }

        public void AtivarNavegacao(Button botaoClicado, Form formFilho)
        {
            // Reseta botão anterior
            if (_botaoNavAtivo != null)
            {
                _botaoNavAtivo.Tag = "nav_item";
            }

            // Ativa o novo botão
            _botaoNavAtivo = botaoClicado;
            if (_botaoNavAtivo != null)
            {
                _botaoNavAtivo.Tag = "nav_ativa";
            }

            GerenciadorTema.AplicarTema(pnlNav);

            // Carrega formulário no workspace
            AbrirFormNoWorkspace(formFilho);
        }

        public void AbrirFormNoWorkspace(object formFilho)
        {
            if (this.panelWorkspace.Controls.Count > 0)
            {
                Control antigo = this.panelWorkspace.Controls[0];
                this.panelWorkspace.Controls.Remove(antigo);
                antigo.Dispose();
            }

            if (formFilho is Form fh)
            {
                fh.TopLevel = false;
                fh.FormBorderStyle = FormBorderStyle.None;
                fh.Dock = DockStyle.Fill;
                GerenciadorTema.AplicarTema(fh);
                this.panelWorkspace.Controls.Add(fh);
                this.panelWorkspace.Tag = fh;
                fh.Show();
            }
        }

        private void btnNavPrincipal_Click(object sender, EventArgs e)
        {
            AtivarNavegacao(btnNavPrincipal, new FormFinanceiro());
        }

        private void btnNavVendas_Click(object sender, EventArgs e)
        {
            AtivarNavegacao(btnNavVendas, new formVenda());
        }

        private void btnNavEstoque_Click(object sender, EventArgs e)
        {
            AtivarNavegacao(btnNavEstoque, new formEstoque());
        }

        private void btnNavFinanceiro_Click(object sender, EventArgs e)
        {
            AtivarNavegacao(btnNavFinanceiro, new FormFinanceiro());
        }

        private void btnNavCadastro_Click(object sender, EventArgs e)
        {
            AtivarNavegacao(btnNavCadastro, new formCadastro());
        }

        private void btnNavConfig_Click(object sender, EventArgs e)
        {
            AtivarNavegacao(btnNavConfig, new FormConfig());
        }

        private void btnToggleTema_Click(object sender, EventArgs e)
        {
            GerenciadorTema.AlternarTema(this);

            if (this.panelWorkspace.Controls.Count > 0 && this.panelWorkspace.Controls[0] is Form formAtual)
            {
                GerenciadorTema.AplicarTema(formAtual);
            }
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            var confirmacao = MessageBox.Show(
                "Deseja realmente encerrar a sessão e sair do sistema?",
                "Encerrar Sessão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmacao == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void IdentificarSidebarListBox(object sender, EventArgs e)
        {
            if (listBoxSidebar.SelectedItem == null) return;
            string opcao = listBoxSidebar.SelectedItem.ToString();
            switch (opcao)
            {
                case "Principal":
                    btnNavPrincipal.PerformClick();
                    break;
                case "Cadastro":
                    btnNavCadastro.PerformClick();
                    break;
                case "Vendas":
                    btnNavVendas.PerformClick();
                    break;
                case "Estoque":
                    btnNavEstoque.PerformClick();
                    break;
                case "Financeiro":
                    btnNavFinanceiro.PerformClick();
                    break;
                case "Configurações":
                    btnNavConfig.PerformClick();
                    break;
                case "Sair":
                    btnSair.PerformClick();
                    break;
            }
        }
    }
}
