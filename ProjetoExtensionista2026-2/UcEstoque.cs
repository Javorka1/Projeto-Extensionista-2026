using Npgsql;
using ProjetoExtensionista2026_2.SQL;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public partial class UcEstoque : UserControl
    {
        public event Action<int, string, decimal, int, string, int> AoAdicionarAoCarrinho;

        private bool _podeVerOcultos = true;
        private string _filtroAtual = "";
        private CancellationTokenSource _ctsBusca;
        private bool _precisaAtualizar;

        public UcEstoque()
        {
            InitializeComponent();
            ConfigurarPainelFlutuante();
            ConfigurarEventosBusca();
            ConfigurarSincronizacaoEventos();
        }

        private void ConfigurarSincronizacaoEventos()
        {
            Action aoMudarDados = () =>
            {
                if (IsDisposed) return;

                if (Visible)
                {
                    _precisaAtualizar = false;
                    BuscarProdutosNoSupabase(_filtroAtual);
                }
                else
                {
                    _precisaAtualizar = true;
                }
            };

            EventosSistema.VendaConcluida += aoMudarDados;
            EventosSistema.EstoqueAtualizado += aoMudarDados;

            VisibleChanged += (s, e) =>
            {
                if (Visible && _precisaAtualizar && !IsDisposed)
                {
                    _precisaAtualizar = false;
                    BuscarProdutosNoSupabase(_filtroAtual);
                }
            };

            HandleDestroyed += (s, e) =>
            {
                EventosSistema.VendaConcluida -= aoMudarDados;
                EventosSistema.EstoqueAtualizado -= aoMudarDados;
                _ctsBusca?.Cancel();
                _ctsBusca?.Dispose();
            };
        }

        private async void UcEstoque_Load(object sender, EventArgs e)
        {
            await BuscarProdutosNoSupabaseAsync("");
        }

        public void FocarCampoBusca()
        {
            if (searchTxtBox != null)
            {
                searchTxtBox.Focus();
                searchTxtBox.SelectAll();
            }
        }

        private void ConfigurarEventosBusca()
        {
            if (searchTxtBox != null)
            {
                searchTxtBox.TextChanged += searchTxtBox_TextChanged;
                searchTxtBox.KeyDown += searchTxtBox_KeyDown;
            }

            if (searchBtn != null)
            {
                searchBtn.Click += (s, e) =>
                {
                    _ctsBusca?.Cancel();
                    _ = BuscarProdutosNoSupabaseAsync(searchTxtBox?.Text ?? "");
                };
            }
        }

        private async void searchTxtBox_TextChanged(object sender, EventArgs e)
        {
            _ctsBusca?.Cancel();
            _ctsBusca?.Dispose();
            _ctsBusca = new CancellationTokenSource();
            var token = _ctsBusca.Token;

            string termo = searchTxtBox.Text.Trim();

            try
            {
                await Task.Delay(250, token);

                if (!token.IsCancellationRequested)
                {
                    await BuscarProdutosNoSupabaseAsync(termo, token);
                }
            }
            catch (TaskCanceledException)
            {
                // Cancelamento esperado enquanto o operador digita
            }
        }

        private async void searchTxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                _ctsBusca?.Cancel();
                await BuscarProdutosNoSupabaseAsync(searchTxtBox.Text.Trim());
            }
        }

        public void ConfigurarPerfilVendedor(bool ehVendedor)
        {
            _podeVerOcultos = !ehVendedor;

            if (panel2 != null)
                panel2.Visible = !ehVendedor;

            if (tableLayoutPanel2 != null && tableLayoutPanel2.RowStyles.Count > 2)
                tableLayoutPanel2.RowStyles[2].Height = ehVendedor ? 10F : 84F;

            if (this.Controls.ContainsKey("btnOculto"))
                this.Controls["btnOculto"].Visible = _podeVerOcultos;

            foreach (Control controle in flowLayoutPanelProdutos.Controls)
            {
                if (controle is UcCardProduto card)
                {
                    AplicarPermissoesAoCard(card);
                }
            }
        }

        private void AplicarPermissoesAoCard(UcCardProduto card)
        {
            card.ConfigurarPermissoes(podeVerCusto: _podeVerOcultos, podeEditar: _podeVerOcultos);
        }

        private void ConfigurarPainelFlutuante()
        {
            if (flowLayoutPanelProdutos != null)
            {
                typeof(FlowLayoutPanel).InvokeMember("DoubleBuffered",
                    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                    null, flowLayoutPanelProdutos, new object[] { true });

                flowLayoutPanelProdutos.AutoScroll = true;
                flowLayoutPanelProdutos.Resize += (s, ev) => CentralizarCards();
            }
        }

        /// <summary>
        /// Sobrecarga síncrona mantida para compatibilidade com chamadas externas (ex: formVenda).
        /// </summary>
        public void BuscarProdutosNoSupabase(string filtroTexto)
        {
            _ctsBusca?.Cancel();
            _ = BuscarProdutosNoSupabaseAsync(filtroTexto);
        }

        /// <summary>
        /// Busca assíncrona otimizada para alto volume de dados, com ordenação por relevância e limite estrito.
        /// </summary>
        public async Task BuscarProdutosNoSupabaseAsync(string filtroTexto, CancellationToken token = default)
        {
            _filtroAtual = filtroTexto?.Trim() ?? "";

            try
            {
                var listaProdutos = await Task.Run(async () =>
                {
                    var produtos = new List<ProdutoDTO>();

                    using (var db = new Connection())
                    {
                        db.IniciarConexao();

                        const string sql = @"
                            SELECT 
                                p.id_product,
                                p.name AS name_product,
                                p.description AS description_product,
                                p.purchase_price AS price_buy,
                                p.sale_price AS price_sell,
                                p.location AS location_product,
                                p.stock_quantity,
                                p.min_stock AS min_stock_quantity,
                                p.unit AS unit_measure,
                                c.name_category AS category_product,
                                s.name AS supplier_product,
                                p.is_active AS active,
                                p.image_url AS picture_url
                            FROM public.product p
                            LEFT JOIN public.category c ON c.id_category = p.id_category
                            LEFT JOIN public.supplier s ON s.id_supplier = p.id_supplier
                            WHERE p.is_active = TRUE
                              AND (
                                  @filtro = '' 
                                  OR p.id_product::TEXT = @filtro
                                  OR p.name ILIKE @filtro_like 
                                  OR p.description ILIKE @filtro_like
                                  OR p.location ILIKE @filtro_like
                                  OR c.name_category ILIKE @filtro_like
                              )
                            ORDER BY 
                                CASE 
                                    WHEN p.id_product::TEXT = @filtro THEN 1
                                    WHEN p.name ILIKE @filtro_prefix THEN 2
                                    WHEN c.name_category ILIKE @filtro_prefix THEN 3
                                    ELSE 4
                                END,
                                p.name ASC
                            LIMIT 40;";

                        using (var cmd = new NpgsqlCommand(sql, db.Conexao))
                        {
                            cmd.Parameters.AddWithValue("@filtro", _filtroAtual);
                            cmd.Parameters.AddWithValue("@filtro_prefix", _filtroAtual + "%");
                            cmd.Parameters.AddWithValue("@filtro_like", "%" + _filtroAtual + "%");

                            using (var reader = await cmd.ExecuteReaderAsync(token))
                            {
                                while (await reader.ReadAsync(token))
                                {
                                    produtos.Add(new ProdutoDTO
                                    {
                                        Id = Convert.ToInt32(reader["id_product"]),
                                        Nome = reader.IsDBNull(reader.GetOrdinal("name_product")) ? "" : reader.GetString(reader.GetOrdinal("name_product")),
                                        Descricao = reader.IsDBNull(reader.GetOrdinal("description_product")) ? "" : reader.GetString(reader.GetOrdinal("description_product")),
                                        PrecoCompra = reader.IsDBNull(reader.GetOrdinal("price_buy")) ? 0m : reader.GetDecimal(reader.GetOrdinal("price_buy")),
                                        PrecoVenda = reader.IsDBNull(reader.GetOrdinal("price_sell")) ? 0m : reader.GetDecimal(reader.GetOrdinal("price_sell")),
                                        Localizacao = reader.IsDBNull(reader.GetOrdinal("location_product")) ? "" : reader.GetString(reader.GetOrdinal("location_product")),
                                        Estoque = reader.IsDBNull(reader.GetOrdinal("stock_quantity")) ? 0 : Convert.ToInt32(reader.GetDecimal(reader.GetOrdinal("stock_quantity"))),
                                        EstoqueMinimo = reader.IsDBNull(reader.GetOrdinal("min_stock_quantity")) ? 0 : reader.GetInt32(reader.GetOrdinal("min_stock_quantity")),
                                        Unidade = reader.IsDBNull(reader.GetOrdinal("unit_measure")) ? "" : reader.GetString(reader.GetOrdinal("unit_measure")),
                                        Categoria = reader.IsDBNull(reader.GetOrdinal("category_product")) ? "" : reader.GetString(reader.GetOrdinal("category_product")),
                                        Fornecedor = reader.IsDBNull(reader.GetOrdinal("supplier_product")) ? "" : reader.GetString(reader.GetOrdinal("supplier_product")),
                                        Ativo = reader.GetBoolean(reader.GetOrdinal("active")),
                                        FotoUrl = reader.IsDBNull(reader.GetOrdinal("picture_url")) ? null : reader.GetString(reader.GetOrdinal("picture_url"))
                                    });
                                }
                            }
                        }
                    }

                    return produtos;
                }, token);

                if (token.IsCancellationRequested || flowLayoutPanelProdutos == null || IsDisposed) return;

                RenderizarCards(listaProdutos);
            }
            catch (OperationCanceledException)
            {
                // Operação cancelada por nova digitação
            }
            catch (Exception ex)
            {
                if (!IsDisposed)
                    MessageBox.Show($"Erro ao consultar produtos: {ex.Message}", "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Reaproveita os cards já instanciados no painel e apenas oculta os excedentes para evitar recriação de controles.
        /// </summary>
        private void RenderizarCards(List<ProdutoDTO> produtos)
        {
            flowLayoutPanelProdutos.SuspendLayout();
            try
            {
                int existentes = flowLayoutPanelProdutos.Controls.Count;

                for (int i = 0; i < produtos.Count; i++)
                {
                    var prod = produtos[i];
                    UcCardProduto card;

                    if (i < existentes)
                    {
                        card = (UcCardProduto)flowLayoutPanelProdutos.Controls[i];
                        if (!card.Visible)
                            card.Visible = true;
                    }
                    else
                    {
                        card = new UcCardProduto
                        {
                            Margin = new Padding(8)
                        };
                        card.AoClicarParaEditar += (produtoClicado) =>
                        {
                            if (_podeVerOcultos && produtoClicado != null)
                                AbrirModalProduto(produtoClicado);
                        };

                        card.AoAdicionarProduto += (prodId, preco, qtd) =>
                        {
                            var p = card.DadosProduto;
                            if (p != null)
                                AoAdicionarAoCarrinho?.Invoke(p.Id, p.Nome, p.PrecoVenda, qtd, p.FotoUrl, p.Estoque);
                        };

                        GerenciadorTema.AplicarTema(card);
                        flowLayoutPanelProdutos.Controls.Add(card);
                    }

                    card.CarregarDados(prod);
                    AplicarPermissoesAoCard(card);
                }

                for (int i = produtos.Count; i < existentes; i++)
                {
                    if (flowLayoutPanelProdutos.Controls[i].Visible)
                        flowLayoutPanelProdutos.Controls[i].Visible = false;
                }
            }
            finally
            {
                flowLayoutPanelProdutos.ResumeLayout();
                CentralizarCards();
            }
        }

        public void AbrirModalProduto(ProdutoDTO produto = null)
        {
            if (!_podeVerOcultos) return;

            using (var formModal = new Form())
            {
                var ucCadastro = new UcCadastroProduto();

                if (produto != null)
                {
                    ucCadastro.CarregarDadosParaEdicao(produto);
                    formModal.Text = $"Editar Produto - {produto.Nome}";
                }
                else
                {
                    ucCadastro.LimparCampos();
                    formModal.Text = "Cadastrar Novo Produto";
                }

                ucCadastro.Dock = DockStyle.Fill;
                formModal.ClientSize = ucCadastro.Size;
                formModal.StartPosition = FormStartPosition.CenterParent;
                formModal.FormBorderStyle = FormBorderStyle.FixedDialog;
                formModal.MaximizeBox = false;
                formModal.MinimizeBox = false;
                formModal.Controls.Add(ucCadastro);
                GerenciadorTema.AplicarTema(formModal);

                ucCadastro.AoConcluirOperacao += () =>
                {
                    formModal.DialogResult = DialogResult.OK;
                    formModal.Close();
                };

                if (formModal.ShowDialog(this) == DialogResult.OK)
                {
                    EventosSistema.NotificarEstoqueAtualizado();
                }
            }
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            AbrirModalProduto();
        }

        private void CentralizarCards()
        {
            if (flowLayoutPanelProdutos == null || flowLayoutPanelProdutos.Controls.Count == 0)
                return;

            var cardsVisiveis = new List<Control>();
            foreach (Control c in flowLayoutPanelProdutos.Controls)
            {
                if (c.Visible)
                    cardsVisiveis.Add(c);
            }

            if (cardsVisiveis.Count == 0)
                return;

            flowLayoutPanelProdutos.SuspendLayout();
            try
            {
                Control primeiroCard = cardsVisiveis[0];
                int larguraCard = primeiroCard.Width;
                int margemPadrao = 15;
                int larguraDisponivel = flowLayoutPanelProdutos.ClientSize.Width - flowLayoutPanelProdutos.Padding.Horizontal - 2;
                int colunas = (larguraDisponivel + margemPadrao) / (larguraCard + margemPadrao);
                if (colunas < 1) colunas = 1;

                int totalCards = cardsVisiveis.Count;

                if (totalCards == 1)
                {
                    primeiroCard.Margin = new Padding(0, primeiroCard.Margin.Top, 0, primeiroCard.Margin.Bottom);
                    return;
                }

                if (totalCards < colunas)
                {
                    for (int i = 0; i < totalCards; i++)
                    {
                        Control card = cardsVisiveis[i];
                        int marginDir = (i == totalCards - 1) ? 0 : margemPadrao;
                        card.Margin = new Padding(0, card.Margin.Top, marginDir, card.Margin.Bottom);
                    }
                    return;
                }

                int sobra = larguraDisponivel - (colunas * larguraCard);
                int gap = (colunas > 1) ? (sobra / (colunas - 1)) : 0;

                for (int i = 0; i < totalCards; i++)
                {
                    Control card = cardsVisiveis[i];
                    int colunaAtual = i % colunas;
                    bool ehUltimoDaLinha = (colunaAtual == colunas - 1);
                    int marginDir = ehUltimoDaLinha ? 0 : gap;

                    card.Margin = new Padding(0, card.Margin.Top, marginDir, card.Margin.Bottom);
                }
            }
            finally
            {
                flowLayoutPanelProdutos.ResumeLayout();
            }
        }
    }
}