using ProjetoExtensionista2026_2.DTOs;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public partial class formConsultaCliente : Form
    {
        public ClienteDTO ClienteSelecionado { get; private set; }

        // Instâncias fixas dos dois UserControls
        private UcConsultaCliente _ucConsulta;
        private UcCadastroCliente _ucCadastro;

        private readonly Color _corAtivo = SystemColors.Control;
        private readonly Color _corInativo = Color.FromArgb(224, 224, 224); // Cinza suave

        public formConsultaCliente()
        {
            InitializeComponent();
        }

        private void formConsultaCliente_Load(object sender, EventArgs e)
        {
            InicializarUserControls();

            AlternarAba(btnConsultarCliente, btnCadastrarCliente, _ucConsulta);
        }

        private void InicializarUserControls()
        {
            _ucConsulta = new UcConsultaCliente
            {
                Dock = DockStyle.Fill
            };

            _ucConsulta.AoSelecionarCliente += FinalizarComCliente;

            _ucCadastro = new UcCadastroCliente
            {
                Dock = DockStyle.Fill
            };
        }

        private void btnConsultarCliente_Click(object sender, EventArgs e)
        {
            AlternarAba(btnConsultarCliente, btnCadastrarCliente, _ucConsulta);
        }

        private void btnCadastrarCliente_Click(object sender, EventArgs e)
        {
            AlternarAba(btnCadastrarCliente, btnConsultarCliente, _ucCadastro);
        }

        private void AlternarAba(Button botaoAtivo, Button botaoInativo, UserControl ucParaExibir)
        {
            botaoAtivo.BackColor = _corAtivo;
            botaoInativo.BackColor = _corInativo;

            panelConteudo.SuspendLayout();
            try
            {
                if (!panelConteudo.Controls.Contains(ucParaExibir))
                {
                    panelConteudo.Controls.Add(ucParaExibir);
                }

                ucParaExibir.BringToFront();
                ucParaExibir.Focus();
            }
            finally
            {
                panelConteudo.ResumeLayout();
            }
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