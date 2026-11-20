using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using ProjetoExtensionista2026_2.SQL;

namespace ProjetoExtensionista2026_2
{
    public partial class FormFinanceiro : Form
    {
        private DataTable _dtProdutosCache;
        private bool _carregandoDados;
        private bool _precisaAtualizar;

        public FormFinanceiro()
        {
            InitializeComponent();
            ConfigurarSincronizacaoEventos();
        }

        private void ConfigurarSincronizacaoEventos()
        {
            Action aoMudarDados = async () =>
            {
                if (IsDisposed || !IsHandleCreated)
                    return;

                if (InvokeRequired)
                {
                    BeginInvoke(new Action(async () =>
                    {
                        if (Visible)
                        {
                            _precisaAtualizar = false;
                            await AtualizarTudoAsync();
                        }
                        else
                        {
                            _precisaAtualizar = true;
                        }
                    }));
                    return;
                }

                if (Visible)
                {
                    _precisaAtualizar = false;
                    await AtualizarTudoAsync();
                }
                else
                {
                    _precisaAtualizar = true;
                }
            };

            EventosSistema.VendaConcluida += aoMudarDados;
            EventosSistema.EstoqueAtualizado += aoMudarDados;

            VisibleChanged += async (s, e) =>
            {
                if (Visible && _precisaAtualizar && !IsDisposed && IsHandleCreated)
                {
                    _precisaAtualizar = false;
                    await AtualizarTudoAsync();
                }
            };

            FormClosed += (s, e) =>
            {
                EventosSistema.VendaConcluida -= aoMudarDados;
                EventosSistema.EstoqueAtualizado -= aoMudarDados;
            };
        }

        private async void FormFinanceiro_Load(object sender, EventArgs e)
        {
            GerenciadorTema.AplicarTema(this);
            SelecionarAba(0);
            await AtualizarTudoAsync();
        }

        private void SelecionarAba(int indice)
        {
            dgvProdutosFinanceiro.Visible = (indice == 0);
            dgvResumoCategoria.Visible = (indice == 1);
            dgvResumoFornecedor.Visible = (indice == 2);

            btnAbaProdutos.Tag = (indice == 0) ? "aba_ativa" : null;
            btnAbaCategorias.Tag = (indice == 1) ? "aba_ativa" : null;
            btnAbaFornecedores.Tag = (indice == 2) ? "aba_ativa" : null;

            btnAbaProdutos.Font = new Font("Segoe UI", 9.5F, (indice == 0) ? FontStyle.Bold : FontStyle.Regular);
            btnAbaCategorias.Font = new Font("Segoe UI", 9.5F, (indice == 1) ? FontStyle.Bold : FontStyle.Regular);
            btnAbaFornecedores.Font = new Font("Segoe UI", 9.5F, (indice == 2) ? FontStyle.Bold : FontStyle.Regular);

            GerenciadorTema.AplicarTema(pnlAbas);
        }

        private void btnAbaProdutos_Click(object sender, EventArgs e) => SelecionarAba(0);
        private void btnAbaCategorias_Click(object sender, EventArgs e) => SelecionarAba(1);
        private void btnAbaFornecedores_Click(object sender, EventArgs e) => SelecionarAba(2);

        private async Task AtualizarTudoAsync()
        {
            if (_carregandoDados) return;

            _carregandoDados = true;
            try
            {
                btnAtualizar.Enabled = false;
                btnAtualizar.Text = "Carregando...";
                var resultado = await Task.Run(() => BuscarDadosUnificadosNoBanco());
                if (IsDisposed) return;

                string catAnterior = cmbCategoria.SelectedItem?.ToString() ?? "Todas";
                cmbCategoria.Items.Clear();
                cmbCategoria.Items.Add("Todas");
                foreach (string cat in resultado.Categorias)
                {
                    cmbCategoria.Items.Add(cat);
                }
                int idx = cmbCategoria.Items.IndexOf(catAnterior);
                cmbCategoria.SelectedIndex = idx >= 0 ? idx : 0;

                lblValorCustoEstoque.Text = resultado.CustoTotal.ToString("C2");
                lblValorReceitaPotencial.Text = resultado.ReceitaPotencial.ToString("C2");
                lblValorLucroEstimado.Text = resultado.LucroProjetado.ToString("C2");
                lblValorMargemMedia.Text = $"{resultado.MargemMedia:N1}%";
                lblValorLucroEstimado.ForeColor = resultado.LucroProjetado >= 0 ? Color.ForestGreen : Color.Crimson;

                _dtProdutosCache = resultado.TabelaProdutos;
                dgvProdutosFinanceiro.DataSource = _dtProdutosCache;
                dgvResumoCategoria.DataSource = resultado.TabelaPorCategoria;
                dgvResumoFornecedor.DataSource = resultado.TabelaPorFornecedor;

                dgvProdutosFinanceiro.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvResumoCategoria.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvResumoFornecedor.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                AplicarFiltroLocal();
            }
            catch (Exception ex)
            {
                if (!IsDisposed)
                    MessageBox.Show("Erro ao carregar dados financeiros: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _carregandoDados = false;
                if (!IsDisposed)
                {
                    btnAtualizar.Text = "Atualizar Dados";
                    btnAtualizar.Enabled = true;
                }
            }
        }

        private class DadosFinanceirosResult
        {
            public List<string> Categorias { get; set; } = new List<string>();
            public decimal CustoTotal { get; set; }
            public decimal ReceitaPotencial { get; set; }
            public decimal LucroProjetado { get; set; }
            public decimal MargemMedia { get; set; }
            public DataTable TabelaProdutos { get; set; }
            public DataTable TabelaPorCategoria { get; set; }
            public DataTable TabelaPorFornecedor { get; set; }
        }

        private DadosFinanceirosResult BuscarDadosUnificadosNoBanco()
        {
            var res = new DadosFinanceirosResult();

            string sqlCategorias = "SELECT name_category FROM public.category ORDER BY name_category ASC;";

            string sqlProdutos = @"
                SELECT 
                    p.id_product AS ""ID"",
                    p.name AS ""Produto"",
                    COALESCE(c.name_category, 'Sem Categoria') AS ""Categoria"",
                    p.stock_quantity AS ""Estoque"",
                    p.purchase_price AS ""Custo Unit. (R$)"",
                    p.sale_price AS ""Venda Unit. (R$)"",
                    (p.sale_price - p.purchase_price) AS ""Lucro Unit. (R$)"",
                    CASE 
                        WHEN p.purchase_price > 0 THEN ROUND(((p.sale_price - p.purchase_price) / p.purchase_price) * 100, 1)
                        ELSE 0 
                    END AS ""Margem (%)"",
                    ROUND(p.purchase_price * p.stock_quantity, 2) AS ""Total Custo (R$)"",
                    ROUND(p.sale_price * p.stock_quantity, 2) AS ""Total Venda (R$)""
                FROM public.product p
                LEFT JOIN public.category c ON c.id_category = p.id_category
                WHERE p.is_active = TRUE
                ORDER BY p.id_product ASC;";

            string sqlPorCategoria = @"
                SELECT 
                    COALESCE(c.name_category, 'Sem Categoria') AS ""Categoria"",
                    COUNT(p.id_product) AS ""Qtd Produtos"",
                    COALESCE(SUM(p.stock_quantity), 0) AS ""Unidades em Estoque"",
                    ROUND(COALESCE(SUM(p.purchase_price * p.stock_quantity), 0), 2) AS ""Custo Acumulado (R$)"",
                    ROUND(COALESCE(SUM(p.sale_price * p.stock_quantity), 0), 2) AS ""Venda Acumulada (R$)"",
                    ROUND(COALESCE(SUM((p.sale_price - p.purchase_price) * p.stock_quantity), 0), 2) AS ""Lucro Projetado (R$)""
                FROM public.product p
                LEFT JOIN public.category c ON c.id_category = p.id_category
                WHERE p.is_active = TRUE
                GROUP BY c.name_category
                ORDER BY ""Lucro Projetado (R$)"" DESC;";

            string sqlPorFornecedor = @"
                SELECT 
                    COALESCE(s.name, 'Sem Fornecedor') AS ""Fornecedor"",
                    COUNT(p.id_product) AS ""Qtd Produtos"",
                    COALESCE(SUM(p.stock_quantity), 0) AS ""Unidades em Estoque"",
                    ROUND(COALESCE(SUM(p.purchase_price * p.stock_quantity), 0), 2) AS ""Total Custo (R$)"",
                    ROUND(COALESCE(SUM(p.sale_price * p.stock_quantity), 0), 2) AS ""Total Venda (R$)""
                FROM public.product p
                LEFT JOIN public.supplier s ON s.id_supplier = p.id_supplier
                WHERE p.is_active = TRUE
                GROUP BY s.name
                ORDER BY ""Total Custo (R$)"" DESC;";

            using (var db = new Connection())
            {
                db.IniciarConexao();

                using (var cmdCat = new NpgsqlCommand(sqlCategorias, db.Conexao))
                using (var reader = cmdCat.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                            res.Categorias.Add(reader.GetString(0));
                    }
                }

                using (var adapterProd = new NpgsqlDataAdapter(sqlProdutos, db.Conexao))
                {
                    var dtProd = new DataTable();
                    adapterProd.Fill(dtProd);
                    res.TabelaProdutos = dtProd;

                    decimal custo = 0m;
                    decimal venda = 0m;
                    foreach (DataRow row in dtProd.Rows)
                    {
                        custo += row["Total Custo (R$)"] != DBNull.Value ? Convert.ToDecimal(row["Total Custo (R$)"]) : 0m;
                        venda += row["Total Venda (R$)"] != DBNull.Value ? Convert.ToDecimal(row["Total Venda (R$)"]) : 0m;
                    }
                    res.CustoTotal = custo;
                    res.ReceitaPotencial = venda;
                    res.LucroProjetado = venda - custo;
                    res.MargemMedia = custo > 0 ? (res.LucroProjetado / custo) * 100m : 0m;
                }

                using (var adapterCat = new NpgsqlDataAdapter(sqlPorCategoria, db.Conexao))
                {
                    var dtCat = new DataTable();
                    adapterCat.Fill(dtCat);
                    res.TabelaPorCategoria = dtCat;
                }

                using (var adapterForn = new NpgsqlDataAdapter(sqlPorFornecedor, db.Conexao))
                {
                    var dtForn = new DataTable();
                    adapterForn.Fill(dtForn);
                    res.TabelaPorFornecedor = dtForn;
                }
            }

            return res;
        }

        private void AplicarFiltroLocal()
        {
            if (_dtProdutosCache == null) return;

            string cat = cmbCategoria.SelectedItem?.ToString() ?? "Todas";
            string busca = txtBuscaProduto.Text.Trim().Replace("'", "''");

            var filtros = new List<string>();
            if (cat != "Todas")
                filtros.Add($"[Categoria] = '{cat}'");

            if (!string.IsNullOrEmpty(busca))
                filtros.Add($"[Produto] LIKE '%{busca}%'");

            _dtProdutosCache.DefaultView.RowFilter = string.Join(" AND ", filtros);
        }

        private async void btnAtualizar_Click(object sender, EventArgs e)
        {
            await AtualizarTudoAsync();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            AplicarFiltroLocal();
            SelecionarAba(0);
        }

        private void btnLimparFiltros_Click(object sender, EventArgs e)
        {
            txtBuscaProduto.Clear();
            if (cmbCategoria.Items.Count > 0) cmbCategoria.SelectedIndex = 0;
            AplicarFiltroLocal();
        }

        private void btnExportarCsv_Click(object sender, EventArgs e)
        {
            DataGridView dgvAtual = dgvProdutosFinanceiro;
            string nomeSugestao = "rentabilidade_produtos.csv";

            if (dgvResumoCategoria.Visible)
            {
                dgvAtual = dgvResumoCategoria;
                nomeSugestao = "resumo_categorias.csv";
            }
            else if (dgvResumoFornecedor.Visible)
            {
                dgvAtual = dgvResumoFornecedor;
                nomeSugestao = "resumo_fornecedores.csv";
            }

            ExportadorCsv.ExportarDataGridView(dgvAtual, nomeSugestao, "Exportar Relatório Financeiro");
        }
    }
}