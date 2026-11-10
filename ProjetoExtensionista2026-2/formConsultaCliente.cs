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
        }

        private void formConsultaCliente_Load(object sender, EventArgs e)
        {
            var consultaCliente = new UcConsultaCliente
            {
                Dock = DockStyle.Fill
            };

            consultaCliente.AoSelecionarCliente += FinalizarComCliente;
            panelConteudo.Controls.Add(consultaCliente);
            consultaCliente.Focus();
        }

        private void FinalizarComCliente(ClienteDTO cliente)
        {
            if (cliente == null) return;

            ClienteSelecionado = cliente;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void formConsultaCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}