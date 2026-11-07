using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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

        private UcPagamento _ucPagamento;
        private UcObservacoes _ucObservacoes;

        public formPagamento()
        {
            InitializeComponent();
            GerenciadorTema.AplicarTema(this);
            KeyPreview = true;
            this.Load += formPagamento_Load;
        }

        public formPagamento(decimal subtotal, decimal desconto, int descontoPorcentagem, decimal totalFinal) : this()
        {
            _subtotal = subtotal;
            _descontoCentavos = (long)Math.Round(desconto * 100m);
            _descontoPorcentagem = descontoPorcentagem;
            _totalFinal = totalFinal;
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
            {
                btnPagamento.Tag = (btnPagamento == botaoAtivo) ? "aba_ativa" : null;
            }

            if (btnObservacoes != null)
            {
                btnObservacoes.Tag = (btnObservacoes == botaoAtivo) ? "aba_ativa" : null;
            }

            if (panel9 != null)
            {
                GerenciadorTema.AplicarTema(panel9);
            }
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

            if (btnFinalizar != null)
            {
                btnFinalizar.Tag = quitado ? "sucesso" : null;
                btnFinalizar.BackColor = quitado
                    ? GerenciadorTema.CorSucesso
                    : (GerenciadorTema.ModoEscuro ? GerenciadorTema.FundoControleEscuro : GerenciadorTema.FundoControleClaro);
                btnFinalizar.ForeColor = quitado ? Color.White : corTextoPadrao;
            }
        }

        private void FinalizarVenda()
        {
            if (_totalPago < _totalFinal)
            {
                MessageBox.Show($"Ainda resta um saldo de {(_totalFinal - _totalPago):C2} a ser pago.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

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