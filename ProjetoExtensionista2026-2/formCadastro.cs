using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public partial class formCadastro : Form
    {
        private Button botaoAtivo;

        public formCadastro()
        {
            InitializeComponent();
            GerenciadorTema.AplicarTema(this);

            // Inicia exibindo o cadastro de Produto como padrão
            AtivarAba(btn_cadastarProduto, new UcCadastroProduto());
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
            telaConteudo.Dock = DockStyle.Fill;
            GerenciadorTema.AplicarTema(telaConteudo);
            panelConteudo.Controls.Add(telaConteudo);
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
