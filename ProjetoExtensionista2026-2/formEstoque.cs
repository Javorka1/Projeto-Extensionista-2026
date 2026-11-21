using System;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public partial class formEstoque : Form
    {
        private UcEstoque _ucEstoque;

        public formEstoque()
        {
            InitializeComponent();

            _ucEstoque = new UcEstoque
            {
                Dock = DockStyle.Fill
            };


            _ucEstoque.ConfigurarPerfilVendedor(false);

            this.Controls.Add(_ucEstoque);
            GerenciadorTema.AplicarTema(this);

            this.VisibleChanged += async (s, e) =>
            {
                if (this.Visible && _ucEstoque != null)
                {
                    await _ucEstoque.BuscarProdutosNoBancoAsync("");
                }
            };
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (_ucEstoque != null)
            {
                await _ucEstoque.BuscarProdutosNoBancoAsync("");
            }
        }

        private async void formEstoque_Load(object sender, EventArgs e)
        {
            if (_ucEstoque != null)
            {
                await _ucEstoque.BuscarProdutosNoBancoAsync("");
            }
        }
    }
}
