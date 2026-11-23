using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using ProjetoExtensionista2026_2.SQL;
using ProjetoExtensionista2026_2.SQL.SQL_functions;

namespace ProjetoExtensionista2026_2
{
    public partial class FormInicio : Form
    {
        private readonly Action<string> _abrirModulo;
        private readonly List<(Button Botao, bool Destaque, bool Permitido)> _botoesAtalho =
            new List<(Button, bool, bool)>();
        private Image _iconeCadeado;
        private bool _mostrarVendas;
        private bool _mostrarEstoque;

        public FormInicio(Action<string> abrirModulo = null)
        {
            _abrirModulo = abrirModulo;
            InitializeComponent();
            CarregarIconeCadeado();
            ConfigurarGrids();
        }

        private void CarregarIconeCadeado()
        {
            try
            {
                if (Properties.Resources.cadeado != null)
                {
                    _iconeCadeado = new Bitmap(Properties.Resources.cadeado, new Size(18, 18));
                }
            }
            catch
            {
                _iconeCadeado = null;
            }
        }

        private void FormInicio_Load(object sender, EventArgs e)
        {
            ConfigurarCabecalho();
            ConfigurarPorPerfil();
            GerenciadorTema.AplicarTema(this);
            AplicarEstiloVisual();
            CarregarDados();
        }

        private void ConfigurarCabecalho()
        {
            string nome = string.IsNullOrWhiteSpace(LoginSQL.SessaoUsuario.Nome)
                ? "Usuário"
                : LoginSQL.SessaoUsuario.Nome;

            string cargo = TraduzirCargo(LoginSQL.SessaoUsuario.Role);
            var cultura = new CultureInfo("pt-BR");
            string dataExtenso = DateTime.Now.ToString("dddd, dd 'de' MMMM 'de' yyyy", cultura);
            if (!string.IsNullOrEmpty(dataExtenso))
                dataExtenso = char.ToUpper(dataExtenso[0], cultura) + dataExtenso.Substring(1);

            lblSaudacao.Text = $"Olá, {nome}!";
            lblSubtitulo.Text = $"{cargo} • {dataExtenso}";
        }

        private static string TraduzirCargo(string role)
        {
            switch ((role ?? string.Empty).Trim().ToLowerInvariant())
            {
                case "admin": return "Administrador";
                case "seller": return "Vendedor";
                case "stocker": return "Estoquista";
                case "monetary": return "Financeiro";
                case "common": return "Comum";
                default: return "Colaborador";
            }
        }

        private static bool ModuloPermitido(string role, string chave)
        {
            switch (role)
            {
                case "admin":
                    return true;
                case "seller":
                    return chave == "vendas" || chave == "registro_vendas";
                case "stocker":
                    return chave == "estoque";
                case "monetary":
                    return chave == "registro_vendas" || chave == "clientes" || chave == "financeiro";
                case "common":
                default:
                    return false;
            }
        }

        private void ConfigurarPorPerfil()
        {
            string role = (LoginSQL.SessaoUsuario.Role ?? string.Empty).Trim().ToLowerInvariant();
            tblAtalhos.Controls.Clear();
            tblAtalhos.ColumnStyles.Clear();
            tblAtalhos.RowStyles.Clear();
            _botoesAtalho.Clear();

            var todosAtalhos = new List<(string Titulo, string Descricao, string Chave)>
            {
                ("Frente de Caixa [F1]", "Iniciar nova venda no PDV", "vendas"),
                ("Registro de Vendas", "Consultar histórico e comprovantes", "registro_vendas"),
                ("Gestão de Estoque", "Produtos, categorias e reposição", "estoque"),
                ("Gestão de Clientes", "Cadastro e consulta de clientes", "clientes"),
                ("Painel Financeiro", "Custos, margens e projeções", "financeiro"),
                ("Administração", "Usuários, permissões e auditoria", "admin")
            };

            _mostrarVendas = role == "admin" || role == "seller" || role == "monetary";
            _mostrarEstoque = role == "admin" || role == "stocker";

            int colunas = 3;
            int linhas = (int)Math.Ceiling(todosAtalhos.Count / (double)colunas);

            tblAtalhos.ColumnCount = colunas;
            tblAtalhos.RowCount = linhas;

            for (int c = 0; c < colunas; c++)
                tblAtalhos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / colunas));

            for (int r = 0; r < linhas; r++)
                tblAtalhos.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / linhas));

            pnlAtalhos.Height = (linhas * 76) + 46;

            bool primeiroPermitidoMarcado = false;

            for (int i = 0; i < todosAtalhos.Count; i++)
            {
                var item = todosAtalhos[i];
                int col = i % colunas;
                int row = i / colunas;

                bool permitido = ModuloPermitido(role, item.Chave);
                bool destaque = permitido && !primeiroPermitidoMarcado;
                if (destaque)
                    primeiroPermitidoMarcado = true;

                var btn = new Button
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(col == 0 ? 0 : 6, row == 0 ? 4 : 6, col == colunas - 1 ? 0 : 6, 4),
                    Padding = new Padding(14, 0, 12, 0),
                    Text = permitido
                        ? $"{item.Titulo}\n{item.Descricao}"
                        : $"{item.Titulo}\nAcesso restrito ao seu perfil",
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 9.5F, destaque ? FontStyle.Bold : FontStyle.Regular),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = permitido ? Cursors.Hand : Cursors.Default,
                    Enabled = permitido,
                    UseVisualStyleBackColor = false
                };

                if (!permitido && _iconeCadeado != null)
                {
                    btn.Image = _iconeCadeado;
                    btn.ImageAlign = ContentAlignment.MiddleLeft;
                    btn.TextImageRelation = TextImageRelation.ImageBeforeText;
                }

                if (permitido)
                {
                    string chave = item.Chave;
                    btn.Click += (s, args) => _abrirModulo?.Invoke(chave);
                }

                _botoesAtalho.Add((btn, destaque, permitido));
                tblAtalhos.Controls.Add(btn, col, row);
            }

            grpUltimasVendas.Visible = _mostrarVendas;
            grpEstoqueBaixo.Visible = _mostrarEstoque;

            if (_mostrarVendas && _mostrarEstoque)
            {
                tblPaineis.Visible = true;
                tblPaineis.ColumnStyles[0].Width = 50F;
                tblPaineis.ColumnStyles[1].Width = 50F;
                grpUltimasVendas.Margin = new Padding(0, 4, 10, 0);
                grpEstoqueBaixo.Margin = new Padding(10, 4, 0, 0);
            }
            else if (_mostrarVendas)
            {
                tblPaineis.Visible = true;
                tblPaineis.ColumnStyles[0].Width = 100F;
                tblPaineis.ColumnStyles[1].Width = 0F;
                grpUltimasVendas.Margin = new Padding(0, 4, 0, 0);
            }
            else if (_mostrarEstoque)
            {
                tblPaineis.Visible = true;
                tblPaineis.ColumnStyles[0].Width = 0F;
                tblPaineis.ColumnStyles[1].Width = 100F;
                grpEstoqueBaixo.Margin = new Padding(0, 4, 0, 0);
            }
            else
            {
                tblPaineis.Visible = false;
            }
        }

        private void AplicarEstiloVisual()
        {
            bool escuro = GerenciadorTema.ModoEscuro;

            lblSubtitulo.ForeColor = escuro ? Color.FromArgb(148, 163, 184) : Color.FromArgb(100, 116, 139);
            lblTituloAtalhos.ForeColor = escuro ? Color.FromArgb(148, 163, 184) : Color.FromArgb(100, 116, 139);

            foreach (var item in _botoesAtalho)
            {
                Button btn = item.Botao;
                btn.FlatStyle = FlatStyle.Flat;

                if (!item.Permitido)
                {
                    btn.BackColor = escuro ? Color.FromArgb(22, 27, 36) : Color.FromArgb(241, 245, 249);
                    btn.ForeColor = escuro ? Color.FromArgb(100, 116, 139) : Color.FromArgb(148, 163, 184);
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = escuro ? Color.FromArgb(38, 46, 61) : Color.FromArgb(226, 232, 240);
                }
                else if (item.Destaque)
                {
                    btn.BackColor = Color.FromArgb(37, 99, 235);
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(59, 130, 246);
                    btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(29, 78, 216);
                }
                else if (escuro)
                {
                    btn.BackColor = Color.FromArgb(30, 37, 50);
                    btn.ForeColor = Color.FromArgb(226, 232, 240);
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = Color.FromArgb(51, 65, 85);
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(44, 53, 72);
                    btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(51, 65, 85);
                }
                else
                {
                    btn.BackColor = Color.White;
                    btn.ForeColor = Color.FromArgb(15, 23, 42);
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(241, 245, 249);
                    btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(226, 232, 240);
                }
            }

            EstilizarGrid(dgvUltimasVendas, escuro);
            EstilizarGrid(dgvEstoqueBaixo, escuro);
        }

        private static void EstilizarGrid(DataGridView dgv, bool escuro)
        {
            dgv.GridColor = escuro ? Color.FromArgb(45, 55, 72) : Color.FromArgb(226, 232, 240);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            dgv.DefaultCellStyle.Padding = new Padding(8, 4, 8, 4);
            dgv.DefaultCellStyle.SelectionBackColor = escuro ? Color.FromArgb(45, 58, 82) : Color.FromArgb(224, 237, 255);
            dgv.DefaultCellStyle.SelectionForeColor = escuro ? Color.White : Color.FromArgb(15, 23, 42);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 4, 8, 4);
        }

        private void ConfigurarGrids()
        {
            dgvUltimasVendas.Columns.Clear();
            dgvUltimasVendas.Columns.Add("Id", "#");
            dgvUltimasVendas.Columns.Add("Cliente", "Cliente");
            dgvUltimasVendas.Columns.Add("Pagamento", "Pagamento");
            dgvUltimasVendas.Columns.Add("Total", "Total");
            dgvUltimasVendas.Columns.Add("DataHora", "Data/Hora");

            dgvUltimasVendas.Columns["Id"].FillWeight = 11;
            dgvUltimasVendas.Columns["Cliente"].FillWeight = 30;
            dgvUltimasVendas.Columns["Pagamento"].FillWeight = 23;
            dgvUltimasVendas.Columns["Total"].FillWeight = 18;
            dgvUltimasVendas.Columns["DataHora"].FillWeight = 18;
            dgvUltimasVendas.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvUltimasVendas.Columns["DataHora"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvEstoqueBaixo.Columns.Clear();
            dgvEstoqueBaixo.Columns.Add("Produto", "Produto");
            dgvEstoqueBaixo.Columns.Add("Categoria", "Categoria");
            dgvEstoqueBaixo.Columns.Add("Atual", "Atual");
            dgvEstoqueBaixo.Columns.Add("Minimo", "Mínimo");

            dgvEstoqueBaixo.Columns["Produto"].FillWeight = 42;
            dgvEstoqueBaixo.Columns["Categoria"].FillWeight = 26;
            dgvEstoqueBaixo.Columns["Atual"].FillWeight = 16;
            dgvEstoqueBaixo.Columns["Minimo"].FillWeight = 16;
            dgvEstoqueBaixo.Columns["Atual"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvEstoqueBaixo.Columns["Minimo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void CarregarDados()
        {
            if (_mostrarVendas)
                CarregarUltimasVendas();

            if (_mostrarEstoque)
                CarregarEstoqueBaixo();
        }

        private void CarregarUltimasVendas()
        {
            dgvUltimasVendas.Rows.Clear();

            try
            {
                using (var conn = new Connection())
                {
                    conn.IniciarConexao();
                    using (var cmd = conn.Conexao.CreateCommand())
                    {
                        cmd.CommandText = @"
                            SELECT
                                s.id_sale,
                                c.name AS cliente,
                                COALESCE(
                                    (SELECT GROUP_CONCAT(DISTINCT sp.payment_method SEPARATOR ', ')
                                     FROM sale_payment sp
                                     WHERE sp.id_sale = s.id_sale),
                                    s.payment_status
                                ) AS pagamento,
                                s.total,
                                s.sale_date
                            FROM sale s
                            INNER JOIN customer c ON c.id_customer = s.id_client
                            ORDER BY s.sale_date DESC, s.id_sale DESC
                            LIMIT 15;";

                        using (var reader = cmd.ExecuteReader())
                        {
                            var cultura = new CultureInfo("pt-BR");
                            while (reader.Read())
                            {
                                long id = Convert.ToInt64(reader["id_sale"]);
                                string cliente = reader["cliente"]?.ToString() ?? "CONSUMIDOR FINAL";
                                string pagamento = reader["pagamento"]?.ToString() ?? "-";
                                decimal total = Convert.ToDecimal(reader["total"]);
                                DateTime data = Convert.ToDateTime(reader["sale_date"]);

                                int rowIdx = dgvUltimasVendas.Rows.Add(
                                    $"#{id}",
                                    cliente,
                                    pagamento,
                                    total.ToString("C2", cultura),
                                    data.ToString("dd/MM HH:mm")
                                );

                                dgvUltimasVendas.Rows[rowIdx].Cells["Total"].Style.ForeColor = Color.FromArgb(52, 211, 153);
                                dgvUltimasVendas.Rows[rowIdx].Cells["Total"].Style.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                            }
                        }
                    }
                }
            }
            catch
            {
                // Mantém a grid vazia caso haja falha momentânea de conexão.
            }

            dgvUltimasVendas.ClearSelection();
        }

        private void CarregarEstoqueBaixo()
        {
            dgvEstoqueBaixo.Rows.Clear();

            try
            {
                using (var conn = new Connection())
                {
                    conn.IniciarConexao();
                    using (var cmd = conn.Conexao.CreateCommand())
                    {
                        cmd.CommandText = @"
                            SELECT
                                p.name AS produto,
                                COALESCE(cat.name_category, 'Sem categoria') AS categoria,
                                p.stock_quantity,
                                p.min_stock,
                                COALESCE(p.unit, 'UN') AS unit
                            FROM product p
                            LEFT JOIN category cat ON cat.id_category = p.id_category
                            WHERE p.is_active = 1
                              AND p.stock_quantity <= p.min_stock
                            ORDER BY p.stock_quantity ASC, p.name ASC
                            LIMIT 20;";

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string produto = reader["produto"]?.ToString() ?? "-";
                                string categoria = reader["categoria"]?.ToString() ?? "Sem categoria";
                                decimal atual = Convert.ToDecimal(reader["stock_quantity"]);
                                int minimo = Convert.ToInt32(reader["min_stock"]);
                                string unit = reader["unit"]?.ToString() ?? "UN";

                                string atualFmt = atual == Math.Truncate(atual)
                                    ? $"{atual:0} {unit}"
                                    : $"{atual:0.##} {unit}";

                                int rowIdx = dgvEstoqueBaixo.Rows.Add(
                                    produto,
                                    categoria,
                                    atualFmt,
                                    $"{minimo} {unit}"
                                );

                                var cellAtual = dgvEstoqueBaixo.Rows[rowIdx].Cells["Atual"];
                                cellAtual.Style.ForeColor = atual <= 0 ? Color.FromArgb(248, 113, 113) : Color.FromArgb(251, 191, 36);
                                cellAtual.Style.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                            }
                        }
                    }
                }
            }
            catch
            {
                // Mantém a grid vazia caso haja falha momentânea de conexão.
            }

            dgvEstoqueBaixo.ClearSelection();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            CarregarDados();
        }
    }
}
