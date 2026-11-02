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

        public formPagamento()
        {
            InitializeComponent();
        }

        public formPagamento(decimal subtotal, decimal desconto, int descontoPorcentagem, decimal totalFinal) : this()
        {
            _subtotal = subtotal;
            _desconto = desconto;
            _descontoPorcentagem = descontoPorcentagem;
            _totalFinal = totalFinal;

            PreencherTotais();
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
