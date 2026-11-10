using Npgsql;
using NpgsqlTypes;
using ProjetoExtensionista2026_2.DTOs;
using ProjetoExtensionista2026_2.SQL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public partial class formVenda : Form
    {
        private UcEstoque _ucEstoque;

        private long _descontoCentavos = 0;
        private int _descontoPorcentagem = 0;
        private bool _atualizandoDesconto = false;
        private decimal DescontoAtual => _descontoCentavos / 100m;

        private ClienteDTO _clienteSelecionado;
        public long IdClienteSelecionado => _clienteSelecionado?.Id ?? 1;

        public formVenda()
        {
            InitializeComponent();

            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }

        private void formVenda_Load(object sender, EventArgs e)
        {
            _ucEstoque = new UcEstoque { Dock = DockStyle.Fill };
            _ucEstoque.ConfigurarPerfilVendedor(true);
            _ucEstoque.AoAdicionarAoCarrinho += AdicionarOuIncrementarCarrinho;

            panelConteudo.Controls.Clear();
            panelConteudo.Controls.Add(_ucEstoque);
            _ucEstoque.BringToFront();
            _ucEstoque.BuscarProdutosNoSupabase(string.Empty);

            if (cmbTipoVenda != null && cmbTipoVenda.SelectedIndex < 0 && cmbTipoVenda.Items.Count > 0)
                cmbTipoVenda.SelectedIndex = 0;

            AjustarLarguraCarrinho();
            ConfigurarEventosDesconto();
            DefinirClientePadrao();
            AtualizarTotaisCarrinho();
            AtualizarRotuloBotaoFinalizar();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AjustarLarguraCarrinho();
        }

        private void AjustarLarguraCarrinho()
        {
            if (splitContainer_catalogo != null && splitContainer_catalogo.Width > 650)
            {
                const int larguraDesejada = 455;
                splitContainer_catalogo.SplitterDistance = Math.Max(250, splitContainer_catalogo.Width - larguraDesejada);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F2:
                    btnConsultarCliente.PerformClick();
                    return true;

                case Keys.F3:
                    _ucEstoque?.FocarCampoBusca();
                    return true;

                case Keys.F4:
                    btnLimparCarrinho.PerformClick();
                    return true;

                case Keys.F12:
                    btnAvancarPagamento.PerformClick();
                    return true;

                case Keys.Delete:
                    if (!(ActiveControl is TextBox))
                    {
                        var ultimoCard = panelCarrinho.Controls.OfType<UcCardVenda>().FirstOrDefault();
                        if (ultimoCard != null)
                        {
                            panelCarrinho.Controls.Remove(ultimoCard);
                            ultimoCard.Dispose();
                            AtualizarTotaisCarrinho();
                            return true;
                        }
                    }
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region Tipo de Venda (Venda vs Orçamento)

        private void cmbTipoVenda_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarRotuloBotaoFinalizar();
        }

        private bool EhOrcamentoSelecionado()
        {
            if (cmbTipoVenda?.SelectedItem == null) return false;
            string valor = cmbTipoVenda.SelectedItem.ToString().Trim().ToLower();
            return valor.Contains("orçamento") || valor.Contains("orcamento");
        }

        private void AtualizarRotuloBotaoFinalizar()
        {
            if (btnAvancarPagamento == null) return;

            btnAvancarPagamento.Text = EhOrcamentoSelecionado()
                ? "Salvar Orçamento [F12]"
                : "Finalizar Venda [F12]";
        }

        #endregion

        #region Gerenciamento de Clientes

        private void btnConsultarCliente_Click(object sender, EventArgs e)
        {
            using (var formConsulta = new formConsultaCliente())
            {
                if (formConsulta.ShowDialog(this) == DialogResult.OK && formConsulta.ClienteSelecionado != null)
                {
                    SelecionarCliente(formConsulta.ClienteSelecionado);
                }
            }
        }

        private void btnLimparCliente_Click(object sender, EventArgs e) => DefinirClientePadrao();

        private void DefinirClientePadrao()
        {
            SelecionarCliente(new ClienteDTO
            {
                Id = 1,
                Nome = "Cliente não informado",
                Apelido = "Cliente",
                CpfCnpj = string.Empty,
                Telefone = string.Empty,
                LimiteCredito = 0m,
                Ativo = true
            });
        }

        private void SelecionarCliente(ClienteDTO cliente)
        {
            if (cliente == null) return;
            _clienteSelecionado = cliente;
            AtualizarExibicaoCliente();
        }

        private void AtualizarExibicaoCliente()
        {
            if (_clienteSelecionado == null) return;

            if (lblNomeCliente != null) lblNomeCliente.Text = _clienteSelecionado.Nome;
            if (lblDocumentoCliente != null) lblDocumentoCliente.Text = FormatarCpfCnpj(_clienteSelecionado.CpfCnpj);
            if (lblTelefoneCliente != null) lblTelefoneCliente.Text = FormatarTelefone(_clienteSelecionado.Telefone);
            if (lblEnderecoCliente != null) lblEnderecoCliente.Text = FormatarEndereco(_clienteSelecionado);

            if (lblStatusCliente != null)
            {
                lblStatusCliente.Text = _clienteSelecionado.Ativo ? "Status: Ativo" : "Status: Inativo";
                lblStatusCliente.ForeColor = _clienteSelecionado.Ativo ? Color.FromArgb(40, 167, 69) : Color.FromArgb(220, 53, 69);
            }

            if (lblLimiteCliente != null)
                lblLimiteCliente.Text = $"Limite de Crédito: {_clienteSelecionado.LimiteCredito:C2}";
        }

        private static string FormatarEndereco(ClienteDTO c)
        {
            if (c == null) return "Endereço: Não informado";

            var partes = new List<string>();
            if (!string.IsNullOrWhiteSpace(c.Rua)) partes.Add(c.Rua.Trim());
            if (!string.IsNullOrWhiteSpace(c.Numero)) partes.Add(c.Numero.Trim());
            if (!string.IsNullOrWhiteSpace(c.Complemento)) partes.Add(c.Complemento.Trim());
            if (!string.IsNullOrWhiteSpace(c.Bairro)) partes.Add(c.Bairro.Trim());

            string cidade = c.Cidade?.Trim();
            string uf = c.Uf?.Trim();

            if (!string.IsNullOrWhiteSpace(cidade) && !string.IsNullOrWhiteSpace(uf))
                partes.Add($"{cidade} - {uf}");
            else if (!string.IsNullOrWhiteSpace(cidade))
                partes.Add(cidade);
            else if (!string.IsNullOrWhiteSpace(uf))
                partes.Add(uf);

            if (!string.IsNullOrWhiteSpace(c.Cep))
                partes.Add($"CEP: {FormatarCep(c.Cep)}");

            return partes.Count > 0 ? string.Join(", ", partes) : "Endereço: Não informado";
        }

        private static string FormatarCep(string cep)
        {
            string digitos = ExtrairDigitos(cep);
            return digitos.Length == 8 ? $"{digitos.Substring(0, 5)}-{digitos.Substring(5, 3)}" : cep.Trim();
        }

        private static string FormatarCpfCnpj(string doc)
        {
            if (string.IsNullOrWhiteSpace(doc)) return "CPF/CNPJ: Não informado";

            string digitos = ExtrairDigitos(doc);
            if (digitos.Length == 11) return Convert.ToUInt64(digitos).ToString(@"CPF: 000\.000\.000\-00");
            if (digitos.Length == 14) return Convert.ToUInt64(digitos).ToString(@"CNPJ: 00\.000\.000\/0000\-00");

            return doc.Trim();
        }

        private static string FormatarTelefone(string fone)
        {
            if (string.IsNullOrWhiteSpace(fone)) return "Telefone: Não informado";

            string digitos = ExtrairDigitos(fone);
            if (digitos.Length == 11) return Convert.ToUInt64(digitos).ToString(@"Telefone: \(00\) 00000\-0000");
            if (digitos.Length == 10) return Convert.ToUInt64(digitos).ToString(@"Telefone: \(00\) 0000\-0000");

            return fone.Trim();
        }

        private static string ExtrairDigitos(string texto) =>
            string.IsNullOrEmpty(texto) ? string.Empty : Regex.Replace(texto, @"\D", string.Empty);

        #endregion

        #region Carrinho de Compras

        private void AdicionarOuIncrementarCarrinho(int idProduto, string nomeProduto, decimal precoVenda, int quantidade, string fotoUrl, int estoqueDisponivel)
        {
            if (panelCarrinho == null) return;

            if (estoqueDisponivel <= 0)
            {
                MessageBox.Show($"O produto '{nomeProduto}' está com o estoque esgotado!", "Estoque Indisponível", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var cardExistente = panelCarrinho.Controls.OfType<UcCardVenda>().FirstOrDefault(c => c.ProdutoId == idProduto);

            if (cardExistente != null)
            {
                int limiteEstoque = cardExistente.EstoqueMaximo;
                int qtdAtual = cardExistente.Quantidade;

                if (qtdAtual >= limiteEstoque)
                {
                    MessageBox.Show($"Limite atingido! O carrinho já possui todas as {limiteEstoque} unidade(s) disponíveis no estoque para '{cardExistente.NomeProduto}'.", "Estoque Máximo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (qtdAtual + quantidade > limiteEstoque)
                {
                    int restantePermitido = limiteEstoque - qtdAtual;
                    MessageBox.Show($"Não é possível adicionar mais {quantidade} unidade(s). Você já tem {qtdAtual} no carrinho e restam apenas {restantePermitido} no estoque.", "Estoque Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cardExistente.CarregarItem(idProduto, cardExistente.NomeProduto, precoVenda, qtdAtual + quantidade, limiteEstoque, cardExistente.FotoOrigem ?? fotoUrl);
                AtualizarTotaisCarrinho();
                return;
            }

            if (quantidade > estoqueDisponivel)
            {
                MessageBox.Show($"Não é possível adicionar {quantidade} unidades de '{nomeProduto}'. O estoque atual possui apenas {estoqueDisponivel} unidade(s).", "Estoque Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            panelCarrinho.SuspendLayout();
            try
            {
                var novoItem = new UcCardVenda { Dock = DockStyle.Top, Margin = new Padding(0, 0, 0, 4) };
                novoItem.CarregarItem(idProduto, nomeProduto, precoVenda, quantidade, estoqueDisponivel, fotoUrl);
                novoItem.AoAlterarQuantidade += (id, novaQtd, novoTotalItem) => AtualizarTotaisCarrinho();
                novoItem.AoRemoverItem += (id, cardRemover) =>
                {
                    panelCarrinho.Controls.Remove(cardRemover);
                    cardRemover.Dispose();
                    AtualizarTotaisCarrinho();
                };

                panelCarrinho.Controls.Add(novoItem);
                novoItem.BringToFront();
            }
            finally
            {
                panelCarrinho.ResumeLayout();
            }

            AtualizarTotaisCarrinho();
        }

        private (decimal Subtotal, int TotalItens) CalcularMetricasCarrinho()
        {
            decimal subtotal = 0m;
            int totalItens = 0;

            if (panelCarrinho != null)
            {
                foreach (UcCardVenda item in panelCarrinho.Controls.OfType<UcCardVenda>())
                {
                    subtotal += item.PrecoTotal;
                    totalItens += item.Quantidade;
                }
            }

            return (subtotal, totalItens);
        }

        private decimal ObterSubtotalAtual() => CalcularMetricasCarrinho().Subtotal;

        private List<ItemVenda> ObterItensDoCarrinho()
        {
            var itens = new List<ItemVenda>();

            if (panelCarrinho != null)
            {
                foreach (UcCardVenda card in panelCarrinho.Controls.OfType<UcCardVenda>())
                {
                    decimal precoUnit = card.Quantidade > 0 ? (card.PrecoTotal / card.Quantidade) : 0m;
                    itens.Add(new ItemVenda
                    {
                        IdProduto = card.ProdutoId,
                        Descricao = card.NomeProduto,
                        Quantidade = card.Quantidade,
                        PrecoUnitario = precoUnit
                    });
                }
            }

            return itens;
        }

        private void LimparCarrinhoCompleto()
        {
            if (panelCarrinho != null)
            {
                panelCarrinho.SuspendLayout();
                try
                {
                    while (panelCarrinho.Controls.Count > 0)
                    {
                        Control c = panelCarrinho.Controls[0];
                        panelCarrinho.Controls.Remove(c);
                        c.Dispose();
                    }
                }
                finally
                {
                    panelCarrinho.ResumeLayout();
                }
            }

            _descontoCentavos = 0;
            _descontoPorcentagem = 0;
            SincronizarCamposDesconto(0m, originadoPorValor: true);
            DefinirClientePadrao();
            AtualizarTotaisCarrinho();
        }

        private void AtualizarTotaisCarrinho()
        {
            var (subtotal, totalItens) = CalcularMetricasCarrinho();

            if (DescontoAtual > subtotal)
            {
                _descontoCentavos = (long)Math.Round(subtotal * 100m);
                SincronizarCamposDesconto(subtotal, originadoPorValor: true);
            }

            decimal totalFinal = Math.Max(0m, subtotal - DescontoAtual);

            if (lblSubtotal != null) lblSubtotal.Text = subtotal.ToString("C2");
            if (lblTotal != null) lblTotal.Text = totalFinal.ToString("C2");
            if (lblQtdItens != null) lblQtdItens.Text = $"Itens do Pedido: {totalItens} item(ns)";
        }

        #endregion

        #region Descontos Sincronizados

        private void ConfigurarEventosDesconto()
        {
            if (txtDesconto != null)
            {
                txtDesconto.KeyPress += (s, e) =>
                {
                    e.Handled = true;
                    if (!char.IsDigit(e.KeyChar)) return;

                    long proximo = (_descontoCentavos * 10) + (e.KeyChar - '0');
                    decimal subtotal = ObterSubtotalAtual();
                    _descontoCentavos = Math.Min(proximo, (long)Math.Round(subtotal * 100m));
                    SincronizarCamposDesconto(subtotal, originadoPorValor: true);
                    AtualizarTotaisCarrinho();
                };

                txtDesconto.KeyDown += (s, e) =>
                {
                    if (e.KeyCode == Keys.Back) { e.SuppressKeyPress = true; _descontoCentavos /= 10; }
                    else if (e.KeyCode == Keys.Delete) { e.SuppressKeyPress = true; _descontoCentavos = 0; }
                    else return;

                    SincronizarCamposDesconto(ObterSubtotalAtual(), originadoPorValor: true);
                    AtualizarTotaisCarrinho();
                };
            }

            if (txtDescontoPorcentagem != null)
            {
                txtDescontoPorcentagem.KeyPress += (s, e) =>
                {
                    e.Handled = true;
                    if (!char.IsDigit(e.KeyChar)) return;

                    _descontoPorcentagem = Math.Min(100, (_descontoPorcentagem * 10) + (e.KeyChar - '0'));
                    SincronizarCamposDesconto(ObterSubtotalAtual(), originadoPorValor: false);
                    AtualizarTotaisCarrinho();
                };

                txtDescontoPorcentagem.KeyDown += (s, e) =>
                {
                    if (e.KeyCode == Keys.Back) { e.SuppressKeyPress = true; _descontoPorcentagem /= 10; }
                    else if (e.KeyCode == Keys.Delete) { e.SuppressKeyPress = true; _descontoPorcentagem = 0; }
                    else return;

                    SincronizarCamposDesconto(ObterSubtotalAtual(), originadoPorValor: false);
                    AtualizarTotaisCarrinho();
                };
            }
        }

        private void SincronizarCamposDesconto(decimal subtotal, bool originadoPorValor)
        {
            if (_atualizandoDesconto) return;
            _atualizandoDesconto = true;

            try
            {
                if (originadoPorValor)
                {
                    if (DescontoAtual > subtotal)
                        _descontoCentavos = (long)Math.Round(subtotal * 100m);

                    int pctCalculada = subtotal > 0m ? (int)Math.Round((DescontoAtual / subtotal) * 100m) : 0;
                    _descontoPorcentagem = Math.Min(100, Math.Max(0, pctCalculada));
                }
                else
                {
                    _descontoPorcentagem = Math.Min(100, Math.Max(0, _descontoPorcentagem));
                    decimal valorCalculado = Math.Min(subtotal, Math.Round(subtotal * (_descontoPorcentagem / 100m), 2));
                    _descontoCentavos = (long)Math.Round(valorCalculado * 100m);
                }

                if (txtDesconto != null)
                {
                    txtDesconto.Text = $"- R$ {DescontoAtual:N2}";
                    txtDesconto.SelectionStart = txtDesconto.Text.Length;
                }

                if (txtDescontoPorcentagem != null)
                {
                    txtDescontoPorcentagem.Text = $"{_descontoPorcentagem} %";
                    txtDescontoPorcentagem.SelectionStart = txtDescontoPorcentagem.Text.Length;
                }
            }
            finally
            {
                _atualizandoDesconto = false;
            }
        }

        #endregion

        #region Ações de Finalização e Gravação de Orçamento

        private void splitContainer_menu_SplitterMoved(object sender, SplitterEventArgs e)
        {
            LimitarSplitter(splitContainer_menu, 0.18, 0.25);
        }

        private void splitContainer_catalogo_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (splitContainer_catalogo == null || splitContainer_catalogo.Width <= 650) return;
            int dist = splitContainer_catalogo.SplitterDistance;
            int minDistance = splitContainer_catalogo.Width - 600;
            int maxDistance = splitContainer_catalogo.Width - 455;
            if (dist < minDistance) splitContainer_catalogo.SplitterDistance = minDistance;
            else if (dist > maxDistance) splitContainer_catalogo.SplitterDistance = maxDistance;
        }

        private static void LimitarSplitter(SplitContainer split, double percMin, double percMax)
        {
            if (split == null || split.Width <= 0) return;

            int min = (int)(split.Width * percMin);
            int max = (int)(split.Width * percMax);

            if (split.SplitterDistance < min) split.SplitterDistance = min;
            else if (split.SplitterDistance > max) split.SplitterDistance = max;
        }

        private void btnLimparCarrinho_Click(object sender, EventArgs e)
        {
            if (panelCarrinho == null || panelCarrinho.Controls.Count == 0) return;

            var confirmacao = MessageBox.Show(
                "Deseja realmente esvaziar o carrinho de compras?",
                "Limpar Carrinho",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmacao == DialogResult.Yes)
                LimparCarrinhoCompleto();
        }

        private void btnAvancarPagamento_Click(object sender, EventArgs e)
        {
            var (subtotal, totalItens) = CalcularMetricasCarrinho();
            if (totalItens <= 0 || subtotal <= 0m)
            {
                MessageBox.Show(
                    "Adicione ao menos um produto ao carrinho antes de continuar.",
                    "Carrinho Vazio",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            decimal desconto = DescontoAtual;
            int descontoPct = _descontoPorcentagem;
            decimal totalFinal = Math.Max(0m, subtotal - desconto);

            var itens = ObterItensDoCarrinho();
            long idCliente = IdClienteSelecionado;

            // FLUXO DE ORÇAMENTO (sale_type = 'ORCAMENTO', payment_status = 'PENDENTE')
            if (EhOrcamentoSelecionado())
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    if (btnAvancarPagamento != null) btnAvancarPagamento.Enabled = false;

                    long idOrcamento = SalvarOrcamentoNoBanco(subtotal, desconto, totalFinal, itens, idCliente);

                    MessageBox.Show(
                        $"Orçamento Nº {idOrcamento} registrado com sucesso!\n\nNenhum valor foi baixado do estoque.",
                        "Orçamento Salvo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    LimparCarrinhoCompleto();
                    _ucEstoque?.FocarCampoBusca();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Erro ao registrar o orçamento:\n\n{ex.Message}",
                        "Falha ao Salvar",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
                finally
                {
                    Cursor = Cursors.Default;
                    if (btnAvancarPagamento != null) btnAvancarPagamento.Enabled = true;
                }

                return;
            }

            // FLUXO NORMAL DE VENDA -> Abre formPagamento (onde Carnê, Dinheiro, etc. são lançados)
            using (var formPagto = new formPagamento(subtotal, desconto, descontoPct, totalFinal, itens, idCliente))
            {
                if (formPagto.ShowDialog(this) == DialogResult.OK)
                {
                    LimparCarrinhoCompleto();
                    _ucEstoque?.BuscarProdutosNoSupabase(string.Empty);
                    _ucEstoque?.FocarCampoBusca();
                }
            }
        }

        private long SalvarOrcamentoNoBanco(decimal subtotal, decimal desconto, decimal totalFinal, List<ItemVenda> itens, long idCliente)
        {
            using (var db = new Connection())
            {
                db.IniciarConexao();
                var conexao = db.Conexao;

                using (var transacao = conexao.BeginTransaction())
                {
                    try
                    {
                        long idOrcamento;

                        // Grava com sale_type = 'ORCAMENTO' e payment_status = 'PENDENTE'
                        string sqlInsertSale = @"
                            INSERT INTO public.sale (
                                sale_date, 
                                subtotal, 
                                discount, 
                                total, 
                                payment_status, 
                                notes, 
                                id_client,
                                sale_type
                            ) 
                            VALUES (
                                NOW(), 
                                @subtotal, 
                                @discount, 
                                @total, 
                                'PENDENTE', 
                                'Orçamento gerado pelo PDV', 
                                @id_client,
                                'ORCAMENTO'
                            ) 
                            RETURNING public.sale.id_sale;";

                        using (var cmdSale = new NpgsqlCommand(sqlInsertSale, conexao, transacao))
                        {
                            cmdSale.Parameters.Add(new NpgsqlParameter("subtotal", NpgsqlDbType.Numeric) { Value = subtotal });
                            cmdSale.Parameters.Add(new NpgsqlParameter("discount", NpgsqlDbType.Numeric) { Value = desconto });
                            cmdSale.Parameters.Add(new NpgsqlParameter("total", NpgsqlDbType.Numeric) { Value = totalFinal });
                            cmdSale.Parameters.Add(new NpgsqlParameter("id_client", NpgsqlDbType.Bigint) { Value = idCliente });

                            idOrcamento = Convert.ToInt64(cmdSale.ExecuteScalar());
                        }

                        foreach (var item in itens)
                        {
                            string sqlItem = @"
                                INSERT INTO public.sale_product (
                                    id_sale, 
                                    id_product, 
                                    quantity, 
                                    unit_price, 
                                    unit_cost,
                                    subtotal, 
                                    discount_product
                                ) 
                                VALUES (
                                    @id_sale, 
                                    @id_product, 
                                    @quantity, 
                                    @unit_price, 
                                    0,
                                    @subtotal, 
                                    0
                                );";

                            using (var cmdItem = new NpgsqlCommand(sqlItem, conexao, transacao))
                            {
                                cmdItem.Parameters.Add(new NpgsqlParameter("id_sale", NpgsqlDbType.Bigint) { Value = idOrcamento });
                                cmdItem.Parameters.Add(new NpgsqlParameter("id_product", NpgsqlDbType.Integer) { Value = (int)item.IdProduto });
                                cmdItem.Parameters.Add(new NpgsqlParameter("quantity", NpgsqlDbType.Numeric) { Value = item.Quantidade });
                                cmdItem.Parameters.Add(new NpgsqlParameter("unit_price", NpgsqlDbType.Numeric) { Value = item.PrecoUnitario });
                                cmdItem.Parameters.Add(new NpgsqlParameter("subtotal", NpgsqlDbType.Numeric) { Value = item.TotalItem });

                                cmdItem.ExecuteNonQuery();
                            }
                        }

                        transacao.Commit();
                        return idOrcamento;
                    }
                    catch
                    {
                        transacao.Rollback();
                        throw;
                    }
                }
            }
        }

        #endregion
    }
}