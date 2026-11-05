using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public partial class formPagamento : Form
    {
        private decimal _subtotal;
        private decimal _desconto;
        private int _descontoPorcentagem;
        private decimal _totalFinal;

        private UcPagamento _ucPagamento;
        private UcObservacoes _ucObservacoes;

        public formPagamento()
        {
            InitializeComponent();
            GerenciadorTema.AplicarTema(this);

            this.Load += formPagamento_Load;
        }

        public formPagamento(decimal subtotal, decimal desconto, int descontoPorcentagem, decimal totalFinal) : this()
        {
            _subtotal = subtotal;
            _desconto = desconto;
            _descontoPorcentagem = descontoPorcentagem;
            _totalFinal = totalFinal;

            PreencherTotais();
        }

        private void formPagamento_Load(object sender, EventArgs e)
        {
            InicializarUserControls();

            ExibirControle(_ucPagamento);
            AtualizarEstiloBotoes(btnPagamento);
        }

        private void InicializarUserControls()
        {
            if (panelConteudo == null) return;

            _ucPagamento = new UcPagamento
            {
                Dock = DockStyle.Fill
            };
            GerenciadorTema.AplicarTema(_ucPagamento);

            _ucObservacoes = new UcObservacoes
            {
                Dock = DockStyle.Fill
            };
            GerenciadorTema.AplicarTema(_ucObservacoes);

            panelConteudo.SuspendLayout();
            panelConteudo.Controls.Add(_ucPagamento);
            panelConteudo.Controls.Add(_ucObservacoes);
            panelConteudo.ResumeLayout();
        }

        private void ExibirControle(UserControl uc)
        {
            if (panelConteudo == null || uc == null) return;

            panelConteudo.SuspendLayout();
            try
            {
                if (!panelConteudo.Controls.Contains(uc))
                    panelConteudo.Controls.Add(uc);

                uc.BringToFront();
                uc.Focus();
            }
            finally
            {
                panelConteudo.ResumeLayout();
            }
        }

        private void AtualizarEstiloBotoes(Button botaoAtivo)
        {
            if (btnPagamento != null)
            {
                bool ativo = (btnPagamento == botaoAtivo);
                btnPagamento.BackColor = ativo ? SystemColors.Control : SystemColors.ControlDark;
                btnPagamento.ForeColor = ativo ? Color.Black : Color.White;
            }

            if (btnObservacoes != null)
            {
                bool ativo = (btnObservacoes == botaoAtivo);
                btnObservacoes.BackColor = ativo ? SystemColors.Control : SystemColors.ControlDark;
                btnObservacoes.ForeColor = ativo ? Color.Black : Color.White;
            }
        }

        private void btnPagamento_Click(object sender, EventArgs e)
        {
            ExibirControle(_ucPagamento);
            AtualizarEstiloBotoes(btnPagamento);
        }

        private void btnObservacoes_Click(object sender, EventArgs e)
        {
            ExibirControle(_ucObservacoes);
            AtualizarEstiloBotoes(btnObservacoes);
        }

        private void PreencherTotais()
        {
            if (lblSubtotal != null)
                lblSubtotal.Text = _subtotal.ToString("C2");

            if (txtDesconto != null)
                txtDesconto.Text = $"- {_desconto:C2}";

            if (txtDescontoPorcentagem != null)
                txtDescontoPorcentagem.Text = $"{_descontoPorcentagem} %";

            if (lblTotal != null)
                lblTotal.Text = _totalFinal.ToString("C2");

            if (lblRestante != null)
                lblRestante.Text = _totalFinal.ToString("C2");
        }
    }
}