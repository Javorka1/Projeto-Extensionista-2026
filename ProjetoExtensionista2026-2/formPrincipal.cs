using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ProjetoExtensionista2026_2.SQL.SQL_functions;

namespace ProjetoExtensionista2026_2
{
    public partial class formPrincipal : Form
    {
        private Button _botaoNavAtivo;
        private GerenciadorAbas _abas;
        private Panel _panelBarraAbas;
        private int _contadorCaixa = 0;
        private NotifyIcon _notificacaoEstoque;
        private Timer _timerEstoque;

        public formPrincipal()
        {
            InitializeComponent();
            _notificacaoEstoque = new NotifyIcon
            {
                Icon = SystemIcons.Warning,
                Visible = true,
                Text = "NexStore - Estoque"
            };
            _timerEstoque = new Timer
            {
                Interval = 60000
            };
            _timerEstoque.Tick += (s, e) => AuditLogSQL.NotifyLowStock(_notificacaoEstoque);
            FormClosed += (s, e) =>
            {
                _timerEstoque.Stop();
                _timerEstoque.Dispose();
                AuditLogSQL.StopLowStockNotifications();
                _notificacaoEstoque.Visible = false;
                _notificacaoEstoque.Dispose();
            };
            InicializarSistemaAbas();
            GerenciadorTema.AplicarTema(this);
        }

        private void InicializarSistemaAbas()
        {
            _abas = new GerenciadorAbas(panelBarraAbas, panelWorkspace, mostrarBotaoAdicionar: true)
            {
                LimiteMaximoAbas = 6
            };

            _abas.AoSolicitarNovaAba += () =>
            {
                if (btnNavVendas.Visible)
                    AbrirNovaAbaCaixa();
                else if (btnNavEstoque.Visible)
                    AbrirModuloUnico(btnNavEstoque, "estoque", "Gestão de Estoque", () => new formEstoque());
                else if (btnNavFinanceiro.Visible)
                    AbrirModuloUnico(btnNavFinanceiro, "financeiro", "Painel Financeiro", () => new FormFinanceiro());
            };
        }

        private void formPrincipal_Load(object sender, EventArgs e)
        {
            AuditLogSQL.EnsureSchema();
            AuditLogSQL.NotifyLowStock(_notificacaoEstoque);
            _timerEstoque.Start();

            using (var sql = new LoginSQL())
            {
                string nomeUsuario = LoginSQL.SessaoUsuario.Nome;
                string role = sql.RolePicker(nomeUsuario);

                if (!string.IsNullOrEmpty(role))
                {
                    LoginSQL.SessaoUsuario.Role = role;
                    ConfigurarCardUsuario(nomeUsuario, role);
                    var rolesManager = new rolesClass();
                    rolesManager.RoleSidebar(this, role, nomeUsuario, LoginSQL.SessaoUsuario.Hash);
                    AbrirPrincipalPorCargo();
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

            string iniciais = nome.Length >= 2 ? nome.Substring(0, 2).ToUpper() : nome.ToUpper();
            lblAvatar.Text = iniciais;

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

        private void DestacarBotaoSidebar(Button botaoClicado)
        {
            if (_botaoNavAtivo != null)
                _botaoNavAtivo.Tag = "nav_item";

            _botaoNavAtivo = botaoClicado;
            if (_botaoNavAtivo != null)
                _botaoNavAtivo.Tag = "nav_ativa";

            GerenciadorTema.AplicarTema(pnlNav);
        }

        private void VincularSincronizacaoSidebar(Form formFilho, Button botaoSidebar)
        {
            if (formFilho == null || botaoSidebar == null) return;

            formFilho.VisibleChanged += (s, e) =>
            {
                if (formFilho.Visible)
                    DestacarBotaoSidebar(botaoSidebar);
            };
        }

        private void AbrirNovaAbaCaixa()
        {
            _contadorCaixa++;
            int numero = _contadorCaixa;

            var vendaForm = new formVenda();
            VincularSincronizacaoSidebar(vendaForm, btnNavVendas);

            var aba = _abas.AdicionarAba(
                titulo: $"Frente de Caixa #{numero}",
                tela: vendaForm,
                podeFechar: true,
                validacaoFechamento: () =>
                {
                    return MessageBox.Show(
                        $"Deseja fechar a aba Frente de Caixa #{numero}?",
                        "Fechar Aba",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes;
                },
                chaveUnica: null,
                fabricaDuplicacao: () =>
                {
                    var novo = new formVenda();
                    VincularSincronizacaoSidebar(novo, btnNavVendas);
                    return novo;
                }
            );

            if (aba != null)
                DestacarBotaoSidebar(btnNavVendas);
        }

        private void AbrirModuloUnico(Button botaoSidebar, string chaveUnica, string titulo, Func<Form> fabricaForm)
        {
            var aba = _abas.AbrirOuFocarAba(
                chaveUnica: chaveUnica,
                titulo: titulo,
                fabricaTela: () =>
                {
                    Form frm = fabricaForm();
                    VincularSincronizacaoSidebar(frm, botaoSidebar);
                    return frm;
                }
            );

            if (aba != null)
                DestacarBotaoSidebar(botaoSidebar);
        }
        public void AtivarNavegacao(Button botaoClicado, Form formFilho)
        {
            if (formFilho == null) return;

            DestacarBotaoSidebar(botaoClicado);
            VincularSincronizacaoSidebar(formFilho, botaoClicado);

            string titulo = botaoClicado != null && !string.IsNullOrWhiteSpace(botaoClicado.Text)
                ? botaoClicado.Text.Trim()
                : formFilho.Text;

            _abas.AdicionarAba(titulo, formFilho, podeFechar: true);
        }

        public void AbrirFormNoWorkspace(object formFilho)
        {
            if (formFilho is Form fh)
            {
                VincularSincronizacaoSidebar(fh, _botaoNavAtivo);
                _abas.AdicionarAba(string.IsNullOrWhiteSpace(fh.Text) ? "Aba" : fh.Text, fh, podeFechar: true);
            }
        }

        public void AbrirHistoricoCliente(DTOs.ClienteDTO cliente)
        {
            if (cliente == null) return;

            AbrirModuloUnico(
                btnNavRegistroVendas,
                chaveUnica: $"historico_cliente_{cliente.Id}",
                titulo: $"Histórico: {cliente.Nome}",
                fabricaForm: () => new FormRegistroVendas(cliente)
            );
        }

        private void AbrirPrincipalPorCargo()
        {
            string role = (LoginSQL.SessaoUsuario.Role ?? string.Empty).Trim().ToLowerInvariant();

            switch (role)
            {
                case "admin":
                case "monetary":
                    AbrirModuloUnico(btnNavPrincipal, "financeiro", "Painel Financeiro", () => new FormFinanceiro());
                    break;
                case "seller":
                    AbrirModuloUnico(btnNavPrincipal, "principal_vendedor", "Frente de Caixa", () => new formVenda());
                    break;
                case "stocker":
                    AbrirModuloUnico(btnNavPrincipal, "estoque", "Gestão de Estoque", () => new formEstoque());
                    break;
            }
        }

        private void btnNavPrincipal_Click(object sender, EventArgs e)
        {
            AbrirPrincipalPorCargo();
        }

        private void btnNavVendas_Click(object sender, EventArgs e)
        {
            AbrirNovaAbaCaixa();
        }

        private void btnNavRegistroVendas_Click(object sender, EventArgs e)
        {
            AbrirModuloUnico(btnNavRegistroVendas, "registro_vendas", "Registro de Vendas", () => new FormRegistroVendas());
        }

        private void btnNavEstoque_Click(object sender, EventArgs e)
        {
            AbrirModuloUnico(btnNavEstoque, "estoque", "Gestão de Estoque", () => new formEstoque());
        }

        private void btnNavFinanceiro_Click(object sender, EventArgs e)
        {
            AbrirModuloUnico(btnNavFinanceiro, "financeiro", "Painel Financeiro", () => new FormFinanceiro());
        }

        private void btnNavClientes_Click(object sender, EventArgs e)
        {
            AbrirModuloUnico(btnNavClientes, "clientes", "Gestão de Clientes", () => new formConsultaCliente());
        }

        private void btnNavConfig_Click(object sender, EventArgs e)
        {
            AbrirModuloUnico(btnNavConfig, "config", "Configurações", () => new FormConfig());
        }

        private void btnNavAdmin_Click(object sender, EventArgs e)
        {
            AbrirModuloUnico(btnNavAdmin, "admin", "Admin", () => new formAdmin());
        }

        private void btnToggleTema_Click(object sender, EventArgs e)
        {
            GerenciadorTema.AlternarTema(this);
            _abas?.AtualizarTemaTodasAbas();
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
                    btnNavFinanceiro.PerformClick();
                    break;
                case "Vendas":
                    btnNavVendas.PerformClick();
                    break;
                case "Estoque":
                    btnNavEstoque.PerformClick();
                    break;
                case "Financeiro":
                    btnNavClientes.PerformClick();
                    break;
                case "Administração":
                    btnNavAdmin.PerformClick();
                    break;
                case "Configurações":
                    btnNavConfig.PerformClick();
                    break;
                case "Sair":
                    btnSair.PerformClick();
                    break;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F1 && btnNavVendas.Visible)
            {
                AbrirNovaAbaCaixa();
                return true;
            }

            if (_abas != null && _abas.ProcessarAtalhos(keyData))
                return true;

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}