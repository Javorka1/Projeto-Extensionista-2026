using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Npgsql;
using NpgsqlTypes;
using ProjetoExtensionista2026_2.DTOs;
using ProjetoExtensionista2026_2.SQL;

namespace ProjetoExtensionista2026_2
{
    public partial class formPagamento : Form
    {
        private decimal _subtotal;
        private decimal _totalFinal;
        private decimal _totalPago = 0m;

        private long _descontoCentavos = 0;
        private int _descontoPorcentagem = 0;
        private bool _atualizandoDesconto = false;
        private decimal DescontoAtual => _descontoCentavos / 100m;

        // Dados contextuais da venda
        private long _idCliente = 1; // Padrão 1 = Consumidor Não Identificado
        private List<ItemVenda> _itensVenda = new List<ItemVenda>();
        private long? _idVendaAberta = null; // Caso venha de uma pré-venda já salva

        public long IdVendaFinalizada { get; private set; }

        private UcPagamento _ucPagamento;
        private UcObservacoes _ucObservacoes;

        public formPagamento()
        {
            InitializeComponent();
            GerenciadorTema.AplicarTema(this);
            KeyPreview = true;
            this.Load += formPagamento_Load;
        }

        // Construtor completo recebendo os itens e o cliente do formVenda
        public formPagamento(decimal subtotal, decimal desconto, int descontoPorcentagem, decimal totalFinal,
                              List<ItemVenda> itens, long idCliente = 1, long? idVendaAberta = null) : this()
        {
            _subtotal = subtotal;
            _descontoCentavos = (long)Math.Round(desconto * 100m);
            _descontoPorcentagem = descontoPorcentagem;
            _totalFinal = totalFinal;
            _itensVenda = itens ?? new List<ItemVenda>();
            _idCliente = idCliente > 0 ? idCliente : 1;
            _idVendaAberta = idVendaAberta;
        }

        // Sobrecarga de compatibilidade
        public formPagamento(decimal subtotal, decimal desconto, int descontoPorcentagem, decimal totalFinal)
            : this(subtotal, desconto, descontoPorcentagem, totalFinal, new List<ItemVenda>(), 1, null)
        {
        }

        private void formPagamento_Load(object sender, EventArgs e)
        {
            dgvPagamentos.CellClick -= dgvPagamentos_CellClick;
            dgvPagamentos.CellClick += dgvPagamentos_CellClick;

            ConfigurarEventosDesconto();
            InicializarAbas();
            RecalcularTotais(preservarTextoDesconto: false);
            GerenciadorTema.AplicarTema(this);
        }

        private void dgvPagamentos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            bool ehColunaBotao = dgvPagamentos.Columns[e.ColumnIndex] is DataGridViewButtonColumn ||
                                 dgvPagamentos.Columns[e.ColumnIndex].Name == "colRemover";

            if (ehColunaBotao)
            {
                dgvPagamentos.Rows.RemoveAt(e.RowIndex);
                RecalcularTotais(preservarTextoDesconto: false);
            }
        }

        private void InicializarAbas()
        {
            if (panelConteudo == null) return;
            panelConteudo.Controls.Clear();

            _ucPagamento = new UcPagamento { Dock = DockStyle.Fill };
            _ucObservacoes = new UcObservacoes { Dock = DockStyle.Fill };

            GerenciadorTema.AplicarTema(_ucPagamento);
            GerenciadorTema.AplicarTema(_ucObservacoes);

            _ucPagamento.AoAdicionarPagamento += (tipo, valor, parcelas) =>
            {
                int idx = dgvPagamentos.Rows.Add(tipo, valor.ToString("C2"), $"{parcelas}x");
                dgvPagamentos.Rows[idx].Tag = valor;
                RecalcularTotais(preservarTextoDesconto: false);
            };

            panelConteudo.Controls.AddRange(new Control[] { _ucPagamento, _ucObservacoes });
            AlternarAba(_ucPagamento, btnPagamento);
        }

        private void AlternarAba(UserControl uc, Button botaoAtivo)
        {
            uc.BringToFront();
            uc.Focus();

            if (btnPagamento != null)
                btnPagamento.Tag = (btnPagamento == botaoAtivo) ? "aba_ativa" : null;

            if (btnObservacoes != null)
                btnObservacoes.Tag = (btnObservacoes == botaoAtivo) ? "aba_ativa" : null;

            if (panel9 != null)
                GerenciadorTema.AplicarTema(panel9);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return true;
            }

            if (keyData == Keys.F12)
            {
                FinalizarVenda();
                return true;
            }

            if (keyData == Keys.Delete && dgvPagamentos.Focused && dgvPagamentos.CurrentRow != null)
            {
                dgvPagamentos.Rows.Remove(dgvPagamentos.CurrentRow);
                RecalcularTotais(preservarTextoDesconto: false);
                return true;
            }

            if (keyData == Keys.Enter && ActiveControl != btnCancelar && ActiveControl != btnFinalizar && ActiveControl != btnPagamento && ActiveControl != btnObservacoes)
            {
                _ucPagamento?.ConfirmarLancamentoPagamento();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

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
                    _descontoCentavos = Math.Min(proximo, (long)Math.Round(_subtotal * 100m));
                    SincronizarDesconto(origemValor: true);
                };

                txtDesconto.KeyDown += (s, e) =>
                {
                    if (e.KeyCode == Keys.Back) { e.SuppressKeyPress = true; _descontoCentavos /= 10; }
                    else if (e.KeyCode == Keys.Delete) { e.SuppressKeyPress = true; _descontoCentavos = 0; }
                    else return;
                    SincronizarDesconto(origemValor: true);
                };
            }

            if (txtDescontoPorcentagem != null)
            {
                txtDescontoPorcentagem.KeyPress += (s, e) =>
                {
                    e.Handled = true;
                    if (!char.IsDigit(e.KeyChar)) return;

                    _descontoPorcentagem = Math.Min(100, (_descontoPorcentagem * 10) + (e.KeyChar - '0'));
                    SincronizarDesconto(origemValor: false);
                };

                txtDescontoPorcentagem.KeyDown += (s, e) =>
                {
                    if (e.KeyCode == Keys.Back) { e.SuppressKeyPress = true; _descontoPorcentagem /= 10; }
                    else if (e.KeyCode == Keys.Delete) { e.SuppressKeyPress = true; _descontoPorcentagem = 0; }
                    else return;
                    SincronizarDesconto(origemValor: false);
                };
            }
        }

        private void SincronizarDesconto(bool origemValor)
        {
            if (_atualizandoDesconto) return;
            _atualizandoDesconto = true;

            try
            {
                if (origemValor)
                {
                    int pct = _subtotal > 0m ? (int)Math.Round((DescontoAtual / _subtotal) * 100m) : 0;
                    _descontoPorcentagem = Math.Min(100, Math.Max(0, pct));
                }
                else
                {
                    decimal valor = Math.Round(_subtotal * (_descontoPorcentagem / 100m), 2);
                    _descontoCentavos = (long)Math.Round(valor * 100m);
                }

                if (txtDesconto != null)
                {
                    txtDesconto.Text = $"- {DescontoAtual:C2}";
                    txtDesconto.SelectionStart = txtDesconto.Text.Length;
                }

                if (txtDescontoPorcentagem != null)
                {
                    txtDescontoPorcentagem.Text = $"{_descontoPorcentagem} %";
                    txtDescontoPorcentagem.SelectionStart = txtDescontoPorcentagem.Text.Length;
                }

                RecalcularTotais(preservarTextoDesconto: true);
            }
            finally
            {
                _atualizandoDesconto = false;
            }
        }

        #endregion

        private void RecalcularTotais(bool preservarTextoDesconto)
        {
            _totalFinal = Math.Max(0m, _subtotal - DescontoAtual);

            _totalPago = dgvPagamentos.Rows.Cast<DataGridViewRow>()
                .Where(r => !r.IsNewRow && r.Tag is decimal)
                .Sum(r => (decimal)r.Tag);

            if (lblSubtotal != null) lblSubtotal.Text = _subtotal.ToString("C2");
            if (lblTotal != null) lblTotal.Text = _totalFinal.ToString("C2");

            if (!preservarTextoDesconto)
            {
                if (txtDesconto != null) txtDesconto.Text = $"- {DescontoAtual:C2}";
                if (txtDescontoPorcentagem != null) txtDescontoPorcentagem.Text = $"{_descontoPorcentagem} %";
            }

            bool temTroco = _totalPago > _totalFinal;
            bool quitado = _totalPago >= _totalFinal && _totalFinal > 0m;
            decimal diferenca = temTroco ? (_totalPago - _totalFinal) : (_totalFinal - _totalPago);
            Color corTextoPadrao = GerenciadorTema.ModoEscuro ? GerenciadorTema.TextoEscuro : GerenciadorTema.TextoClaro;

            if (lblExcedente != null)
            {
                lblExcedente.Text = temTroco ? "Troco:" : "Restante:";
                lblExcedente.ForeColor = quitado ? GerenciadorTema.CorSucesso : corTextoPadrao;
            }

            if (lblRestante != null)
            {
                lblRestante.Text = diferenca.ToString("C2");
                lblRestante.ForeColor = quitado ? GerenciadorTema.CorSucesso : corTextoPadrao;
            }

            _ucPagamento?.DefinirSaldoRestante(temTroco ? 0m : diferenca);

            // Vinculação com o botão btnFinalizar
            if (btnFinalizar != null)
            {
                btnFinalizar.Tag = quitado ? "sucesso" : null;
                btnFinalizar.BackColor = quitado
                    ? GerenciadorTema.CorSucesso
                    : (GerenciadorTema.ModoEscuro ? GerenciadorTema.FundoControleEscuro : GerenciadorTema.FundoControleClaro);
                btnFinalizar.ForeColor = quitado ? Color.White : corTextoPadrao;
            }
        }

        #region Gravação da Venda no Banco de Dados

        private void FinalizarVenda()
        {
            if (_totalPago < _totalFinal)
            {
                decimal falta = _totalFinal - _totalPago;
                MessageBox.Show($"Ainda resta um saldo de {falta:C2} a ser pago.", "Pagamento Incompleto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvPagamentos.Rows.Count == 0)
            {
                MessageBox.Show("Nenhuma forma de pagamento foi lançada.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                if (btnFinalizar != null) btnFinalizar.Enabled = false;

                GravarVendaNoBanco();

                MessageBox.Show("Venda finalizada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar a venda no banco de dados:\n\n{ex.Message}", "Falha na Gravação", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                if (btnFinalizar != null) btnFinalizar.Enabled = true;
            }
        }

        private void GravarVendaNoBanco()
        {
            // 1. Identifica se foi lançado Carnê, A Prazo ou Fiado
            bool temPagamentoAPrazo = dgvPagamentos.Rows.Cast<DataGridViewRow>()
                .Where(row => !row.IsNewRow)
                .Any(row =>
                {
                    string metodo = (row.Cells[0].Value?.ToString() ?? string.Empty).ToLower();
                    return metodo.Contains("carnê") || metodo.Contains("carne") ||
                           metodo.Contains("prazo") || metodo.Contains("fiado");
                });

            // 2. Se for Carnê, exige cliente cadastrado (não permite cliente padrão ID 1)[cite: 3]
            if (temPagamentoAPrazo && _idCliente <= 1)
            {
                throw new InvalidOperationException("Para compras no Carnê/A Prazo é obrigatório selecionar um cliente cadastrado no sistema.");
            }

            // 3. Define o status correto para o financeiro puxar[cite: 3]
            string statusPagamento = temPagamentoAPrazo ? "PENDENTE" : "PAGO";

            string observacoes = _ucObservacoes?.ObterObservacoes() ?? string.Empty;
            decimal trocoTotal = Math.Max(0m, _totalPago - _totalFinal);
            decimal trocoRestanteParaAbater = trocoTotal;

            using (var db = new Connection())
            {
                db.IniciarConexao();
                var conexao = db.Conexao;

                using (var transacao = conexao.BeginTransaction())
                {
                    try
                    {
                        long idVendaAtual;

                        // 1. Grava ou Atualiza a Venda principal (sale)[cite: 3]
                        if (_idVendaAberta.HasValue && _idVendaAberta.Value > 0)
                        {
                            idVendaAtual = _idVendaAberta.Value;

                            string sqlUpdateSale = @"
                                UPDATE public.sale 
                                SET subtotal = @subtotal,
                                    discount = @discount,
                                    total = @total,
                                    payment_status = @payment_status,
                                    notes = @notes,
                                    sale_type = 'VENDA',
                                    sale_date = NOW()
                                WHERE public.sale.id_sale = @id_sale;";

                            using (var cmdSale = new NpgsqlCommand(sqlUpdateSale, conexao, transacao))
                            {
                                cmdSale.Parameters.Add(new NpgsqlParameter("subtotal", NpgsqlDbType.Numeric) { Value = _subtotal });
                                cmdSale.Parameters.Add(new NpgsqlParameter("discount", NpgsqlDbType.Numeric) { Value = DescontoAtual });
                                cmdSale.Parameters.Add(new NpgsqlParameter("total", NpgsqlDbType.Numeric) { Value = _totalFinal });
                                cmdSale.Parameters.Add(new NpgsqlParameter("payment_status", NpgsqlDbType.Text) { Value = statusPagamento });
                                cmdSale.Parameters.Add(new NpgsqlParameter("notes", NpgsqlDbType.Text) { Value = string.IsNullOrWhiteSpace(observacoes) ? (object)DBNull.Value : observacoes });
                                cmdSale.Parameters.Add(new NpgsqlParameter("id_sale", NpgsqlDbType.Bigint) { Value = idVendaAtual });
                                cmdSale.ExecuteNonQuery();
                            }
                        }
                        else
                        {
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
                                    @payment_status, 
                                    @notes, 
                                    @id_client,
                                    'VENDA'
                                ) 
                                RETURNING public.sale.id_sale;";

                            using (var cmdSale = new NpgsqlCommand(sqlInsertSale, conexao, transacao))
                            {
                                cmdSale.Parameters.Add(new NpgsqlParameter("subtotal", NpgsqlDbType.Numeric) { Value = _subtotal });
                                cmdSale.Parameters.Add(new NpgsqlParameter("discount", NpgsqlDbType.Numeric) { Value = DescontoAtual });
                                cmdSale.Parameters.Add(new NpgsqlParameter("total", NpgsqlDbType.Numeric) { Value = _totalFinal });
                                cmdSale.Parameters.Add(new NpgsqlParameter("payment_status", NpgsqlDbType.Text) { Value = statusPagamento });
                                cmdSale.Parameters.Add(new NpgsqlParameter("notes", NpgsqlDbType.Text) { Value = string.IsNullOrWhiteSpace(observacoes) ? (object)DBNull.Value : observacoes });
                                cmdSale.Parameters.Add(new NpgsqlParameter("id_client", NpgsqlDbType.Bigint) { Value = _idCliente });

                                idVendaAtual = Convert.ToInt64(cmdSale.ExecuteScalar());
                            }
                        }

                        IdVendaFinalizada = idVendaAtual;

                        // 2. Grava os Itens e Abate o Estoque[cite: 3]
                        if (_itensVenda != null && _itensVenda.Count > 0 && !_idVendaAberta.HasValue)
                        {
                            foreach (var item in _itensVenda)
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
                                        COALESCE((SELECT public.product.purchase_price FROM public.product WHERE public.product.id_product = @id_product), 0),
                                        @subtotal, 
                                        @discount_product
                                    );

                                    UPDATE public.product 
                                    SET stock_quantity = public.product.stock_quantity - @quantity 
                                    WHERE public.product.id_product = @id_product;";

                                using (var cmdItem = new NpgsqlCommand(sqlItem, conexao, transacao))
                                {
                                    cmdItem.Parameters.Add(new NpgsqlParameter("id_sale", NpgsqlDbType.Bigint) { Value = idVendaAtual });
                                    cmdItem.Parameters.Add(new NpgsqlParameter("id_product", NpgsqlDbType.Integer) { Value = (int)item.IdProduto });
                                    cmdItem.Parameters.Add(new NpgsqlParameter("quantity", NpgsqlDbType.Numeric) { Value = item.Quantidade });
                                    cmdItem.Parameters.Add(new NpgsqlParameter("unit_price", NpgsqlDbType.Numeric) { Value = item.PrecoUnitario });
                                    cmdItem.Parameters.Add(new NpgsqlParameter("subtotal", NpgsqlDbType.Numeric) { Value = item.TotalItem });
                                    cmdItem.Parameters.Add(new NpgsqlParameter("discount_product", NpgsqlDbType.Numeric) { Value = 0m });

                                    cmdItem.ExecuteNonQuery();
                                }
                            }
                        }

                        // 3. Grava os pagamentos em sale_payment com abatimento de troco[cite: 3]
                        foreach (DataGridViewRow row in dgvPagamentos.Rows)
                        {
                            if (row.IsNewRow) continue;

                            string metodo = row.Cells[0].Value?.ToString() ?? "Dinheiro";
                            decimal valorBruto = row.Tag is decimal d ? d : 0m;

                            string textoParcelas = row.Cells[2].Value?.ToString() ?? "1";
                            string digitos = Regex.Replace(textoParcelas, @"\D", "");
                            int parcelas = int.TryParse(digitos, out int p) && p > 0 ? p : 1;

                            decimal valorLiquido = valorBruto;
                            if (metodo.ToLower().Contains("dinheiro") && trocoRestanteParaAbater > 0m)
                            {
                                decimal abatimento = Math.Min(valorLiquido, trocoRestanteParaAbater);
                                valorLiquido -= abatimento;
                                trocoRestanteParaAbater -= abatimento;
                            }

                            if (valorLiquido <= 0m) continue;

                            string sqlPayment = @"
                                INSERT INTO public.sale_payment (
                                    id_sale, 
                                    payment_method, 
                                    amount, 
                                    installments, 
                                    created_at
                                ) 
                                VALUES (
                                    @id_sale, 
                                    @payment_method, 
                                    @amount, 
                                    @installments, 
                                    NOW()
                                );";

                            using (var cmdPayment = new NpgsqlCommand(sqlPayment, conexao, transacao))
                            {
                                cmdPayment.Parameters.Add(new NpgsqlParameter("id_sale", NpgsqlDbType.Bigint) { Value = idVendaAtual });
                                cmdPayment.Parameters.Add(new NpgsqlParameter("payment_method", NpgsqlDbType.Text) { Value = metodo });
                                cmdPayment.Parameters.Add(new NpgsqlParameter("amount", NpgsqlDbType.Numeric) { Value = valorLiquido });
                                cmdPayment.Parameters.Add(new NpgsqlParameter("installments", NpgsqlDbType.Integer) { Value = parcelas });
                                cmdPayment.ExecuteNonQuery();
                            }
                        }

                        transacao.Commit();
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

        private void btnPagamento_Click(object sender, EventArgs e) => AlternarAba(_ucPagamento, btnPagamento);
        private void btnObservacoes_Click(object sender, EventArgs e) => AlternarAba(_ucObservacoes, btnObservacoes);
        private void btnFinalizar_Click(object sender, EventArgs e) => FinalizarVenda();
        private void btnCancelar_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }
        private void dgvPagamentos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dgvPagamentos.Rows.RemoveAt(e.RowIndex);
                RecalcularTotais(preservarTextoDesconto: false);
            }
        }
    }
}