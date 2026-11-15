using ProjetoExtensionista2026_2.DTOs;
using System;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public partial class formConsultaCliente : Form
    {
        public ClienteDTO ClienteSelecionado { get; private set; }

        public formConsultaCliente()
        {
            InitializeComponent();

            var consultaCliente = new UcConsultaCliente { Dock = DockStyle.Fill };
            consultaCliente.AoSelecionarCliente += FinalizarComCliente;
            panelConteudo.Controls.Add(consultaCliente);
        }

        private void FinalizarComCliente(ClienteDTO cliente)
        {
            if (cliente == null || !this.Modal) return;

            ClienteSelecionado = cliente;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void formConsultaCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.Modal && e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}