using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public partial class formCadastro : Form
    {
        private Button botaoAtivo;
        private UserControl _telaAtiva;

        public formCadastro()
        {
            InitializeComponent();
            panelConteudo.Resize += (s, e) => CentralizarTelaAtiva();
            GerenciadorTema.AplicarTema(this);

            // Inicia exibindo o cadastro de Produto como padrão
            AtivarAba(btn_cadastarProduto, new UcCadastroProduto());
        }

        private void CentralizarTelaAtiva()
        {
            if (_telaAtiva != null && panelConteudo != null)
            {
                int x = Math.Max(20, (panelConteudo.ClientSize.Width - _telaAtiva.Width) / 2);
                int y = Math.Max(15, (panelConteudo.ClientSize.Height - _telaAtiva.Height) / 4);
                _telaAtiva.Location = new Point(x, y);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CentralizarTelaAtiva();
        }

        private void AtivarAba(Button botaoClicado, UserControl telaConteudo)
        {
            // 1. Apaga o botão que estava aceso
            if (botaoAtivo != null)
            {
                botaoAtivo.Tag = null;
            }

            // 2. Acende o botão clicado com a tag de aba ativa
            botaoAtivo = botaoClicado;
            if (botaoAtivo != null)
            {
                botaoAtivo.Tag = "aba_ativa";
            }

            GerenciadorTema.AplicarTema(pnlAbasTopo);

            // 3. Substitui o conteúdo do panel inferior
            panelConteudo.Controls.Clear();
            _telaAtiva = telaConteudo;
            telaConteudo.Dock = DockStyle.None;
            telaConteudo.Anchor = AnchorStyles.None;
            GerenciadorTema.AplicarTema(telaConteudo);
            panelConteudo.Controls.Add(telaConteudo);
            CentralizarTelaAtiva();
        }

        private void btn_cadastarProduto_Click(object sender, EventArgs e)
        {
            AtivarAba(btn_cadastarProduto, new UcCadastroProduto());
        }

        private void btn_cadastrarCliente_Click(object sender, EventArgs e)
        {
            AtivarAba(btn_cadastrarCliente, new UcCadastroCliente());
        }
    }
}
