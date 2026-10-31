using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public partial class formCadastro : Form
    {
        private readonly Color CorPadrao = SystemColors.ControlDark;
        private readonly Color CorAcesa = SystemColors.Control;
        private readonly Color CorTexto = SystemColors.ActiveCaptionText;

        private Button botaoAtivo;

        public formCadastro()
        {
            InitializeComponent();
            ConfigurarEstiloBotoes();

            // Inicia exibindo o cadastro de Produto como padrão
            AtivarAba(btn_cadastarProduto, new UcCadastroProduto());
        }

        private void ConfigurarEstiloBotoes()
        {
            btn_cadastarProduto.BackColor = CorPadrao;
            btn_cadastrarCliente.BackColor = CorPadrao;

            btn_cadastarProduto.ForeColor = CorTexto;
            btn_cadastrarCliente.ForeColor = CorTexto;
        }

        private void AtivarAba(Button botaoClicado, UserControl telaConteudo)
        {
            // 1. Apaga o botão que estava aceso
            if (botaoAtivo != null)
            {
                botaoAtivo.Tag = null;
            }

            // 2. Acende o botão clicado com a cor de destaque Indigo
            botaoAtivo = botaoClicado;
            botaoAtivo.Tag = "aba_ativa";
            GerenciadorTema.AplicarTema(this);

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