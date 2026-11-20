using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public partial class UcPagamento : UserControl
    {
        public event Action<string, decimal, int> AoAdicionarPagamento;

        private const long MaxValorCentavos = 999_999_999;
        private long _valorCentavos = 0;
        private decimal _saldoRestante = 0m;
        private Button _botaoFormaAtivo = null;
        private string _formaSelecionada = null;

        private decimal ValorAtual => _valorCentavos / 100m;

        public UcPagamento()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ConfigurarEventos();
            ResetarForma();
        }

        public void DefinirSaldoRestante(decimal saldo)
        {
            _saldoRestante = Math.Max(0m, saldo);
            if (_botaoFormaAtivo != null) DefinirValorExato();
        }

        private void ConfigurarEventos()
        {
            if (btnDinheiro != null) btnDinheiro.Click += (s, e) => SelecionarForma(btnDinheiro, "Dinheiro", false);
            if (btnDebito != null) btnDebito.Click += (s, e) => SelecionarForma(btnDebito, "Cartão de Débito", false);
            if (btnCredito != null) btnCredito.Click += (s, e) => SelecionarForma(btnCredito, "Cartão de Crédito", true);
            if (btnPix != null) btnPix.Click += (s, e) => SelecionarForma(btnPix, "Pix", false);
            if (btnCarne != null) btnCarne.Click += (s, e) => SelecionarForma(btnCarne, "Carnê", true);
            if (btnExato != null) btnExato.Click += (s, e) => DefinirValorExato();
            if (btn10 != null) btn10.Click += (s, e) => Incrementar(10m);
            if (btn100 != null) btn100.Click += (s, e) => Incrementar(100m);
            if (btn1000 != null) btn1000.Click += (s, e) => Incrementar(1000m);
            if (txtPagamento != null)
            {
                txtPagamento.KeyPress += (s, e) =>
                {
                    e.Handled = true;
                    if (!char.IsDigit(e.KeyChar)) return;

                    long proximo = (_valorCentavos * 10) + (e.KeyChar - '0');
                    if (proximo <= MaxValorCentavos)
                    {
                        _valorCentavos = proximo;
                        AtualizarTexto();
                    }
                };

                txtPagamento.KeyDown += (s, e) =>
                {
                    if (e.KeyCode == Keys.Back) { e.SuppressKeyPress = true; _valorCentavos /= 10; AtualizarTexto(); }
                    else if (e.KeyCode == Keys.Delete) { e.SuppressKeyPress = true; _valorCentavos = 0; AtualizarTexto(); }
                };
            }

            if (btnAdicionarPagamento != null)
                btnAdicionarPagamento.Click += (s, e) => ConfirmarLancamentoPagamento();
        }

        private void SelecionarForma(Button botao, string nomeForma, bool permiteParcelamento)
        {
            _botaoFormaAtivo = botao;
            _formaSelecionada = nomeForma;

            AtualizarEstiloBotoesForma(botao);

            if (gpbLancamentoValores != null)
            {
                gpbLancamentoValores.Enabled = true;
                gpbLancamentoValores.Text = "Lançamento de Valores";
                gpbLancamentoValores.Invalidate();
            }

            if (cmbParcelas != null)
            {
                cmbParcelas.Enabled = permiteParcelamento;
                if (!permiteParcelamento && cmbParcelas.Items.Count > 0)
                    cmbParcelas.SelectedIndex = 0;
            }

            if (lblParcelas != null)
            {
                lblParcelas.Tag = permiteParcelamento ? null : "subtitulo";
                lblParcelas.Text = permiteParcelamento ? "Parcelas:" : "Parcelas (À vista):";
                GerenciadorTema.AplicarTema(lblParcelas);
            }

            DefinirValorExato();
            txtPagamento?.Focus();
        }

        private void AtualizarEstiloBotoesForma(Button botaoSelecionado)
        {
            Button[] todos = { btnDinheiro, btnDebito, btnCredito, btnPix, btnCarne };
            foreach (var b in todos.Where(x => x != null))
            {
                b.Tag = (b == botaoSelecionado) ? "pagamento_ativo" : "pagamento_item";
                GerenciadorTema.AplicarTema(b);
            }
        }

        private void Incrementar(decimal v)
        {
            _valorCentavos = Math.Min(MaxValorCentavos, _valorCentavos + (long)Math.Round(v * 100m));
            AtualizarTexto();
        }

        private void DefinirValorExato()
        {
            _valorCentavos = Math.Min(MaxValorCentavos, (long)Math.Round(_saldoRestante * 100m));
            AtualizarTexto();
        }

        private void AtualizarTexto()
        {
            if (txtPagamento != null)
            {
                txtPagamento.Text = ValorAtual.ToString("C2");
                txtPagamento.SelectionStart = txtPagamento.Text.Length;
            }
        }

        private void ResetarForma()
        {
            _botaoFormaAtivo = null;
            _formaSelecionada = null;

            AtualizarEstiloBotoesForma(null);

            if (gpbLancamentoValores != null)
            {
                gpbLancamentoValores.Enabled = false;
                gpbLancamentoValores.Text = "Valores (Selecione a forma)";
                gpbLancamentoValores.Invalidate();
            }

            if (cmbParcelas != null)
            {
                cmbParcelas.Enabled = false;
                if (cmbParcelas.Items.Count > 0) cmbParcelas.SelectedIndex = 0;
            }

            if (lblParcelas != null)
            {
                lblParcelas.Tag = "subtitulo";
                lblParcelas.Text = "Parcelas:";
                GerenciadorTema.AplicarTema(lblParcelas);
            }

            _valorCentavos = 0;
            AtualizarTexto();
        }

        public void ConfirmarLancamentoPagamento()
        {
            if (_botaoFormaAtivo == null)
            {
                MessageBox.Show("Selecione primeiro uma forma de pagamento.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ValorAtual <= 0m)
            {
                MessageBox.Show("Informe um valor maior que zero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_botaoFormaAtivo != btnDinheiro && ValorAtual > _saldoRestante)
            {
                MessageBox.Show($"Para {_formaSelecionada}, o valor não pode exceder o restante ({_saldoRestante:C2}).\nTroco é aceito apenas em Dinheiro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DefinirValorExato();
                return;
            }

            string num = Regex.Replace(cmbParcelas?.SelectedItem?.ToString() ?? "1", @"\D", "");
            int parcelas = int.TryParse(num, out int p) ? p : 1;

            AoAdicionarPagamento?.Invoke(_formaSelecionada, ValorAtual, parcelas);
            ResetarForma();
        }
    }
}