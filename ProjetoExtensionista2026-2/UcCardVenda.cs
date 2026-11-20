using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public partial class UcCardVenda : UserControl
    {
        public int ProdutoId { get; private set; }
        public string NomeProduto { get; private set; }
        public decimal PrecoUnitario { get; private set; }
        public int Quantidade { get; private set; }
        public int EstoqueMaximo { get; private set; } = 999;
        public string FotoOrigem { get; private set; }
        public decimal PrecoTotal => PrecoUnitario * Quantidade;
        public event Action<int, int, decimal> AoAlterarQuantidade;
        public event Action<int, UcCardVenda> AoRemoverItem;

        public UcCardVenda()
        {
            InitializeComponent();
            DoubleBuffered = true;

            if (picFoto != null)
                picFoto.SizeMode = PictureBoxSizeMode.Zoom;

            if (btnAumentar != null)
            {
                btnAumentar.Click -= btnAumentar_Click;
                btnAumentar.Click += btnAumentar_Click;
            }

            if (btnDiminuir != null)
            {
                btnDiminuir.Click -= btnDiminuir_Click;
                btnDiminuir.Click += btnDiminuir_Click;
            }

            if (btnExcluir != null)
            {
                btnExcluir.Click -= btnExcluir_Click;
                btnExcluir.Click += btnExcluir_Click;
            }

            Disposed += (s, e) =>
            {
                if (picFoto != null)
                    picFoto.Image = null;
            };
        }




        public void CarregarItem(int id, string nome, decimal precoUnitario, int quantidadeInicial = 1, int estoqueDisponivel = 999, string fotoUrl = null)
        {
            bool mesmaImagemJaCarregada = picFoto != null &&
                                          picFoto.Image != null &&
                                          string.Equals(FotoOrigem, fotoUrl, StringComparison.Ordinal);

            ProdutoId = id;
            NomeProduto = string.IsNullOrWhiteSpace(nome) ? $"Item #{id}" : nome;
            PrecoUnitario = precoUnitario;
            EstoqueMaximo = estoqueDisponivel > 0 ? estoqueDisponivel : 1;
            Quantidade = Math.Max(1, Math.Min(quantidadeInicial, EstoqueMaximo));
            FotoOrigem = fotoUrl;

            AtualizarInterface();

            if (!mesmaImagemJaCarregada)
                _ = CarregarImagemAsync(FotoOrigem);
        }




        private void AtualizarInterface()
        {
            if (lblCodigo != null)
                lblCodigo.Text = $"#{ProdutoId}";

            if (lblNomeProduto != null)
                lblNomeProduto.Text = NomeProduto;

            if (lblPrecoUnitario != null)
                lblPrecoUnitario.Text = PrecoUnitario.ToString("C2");

            if (lblPrecoTotal != null)
                lblPrecoTotal.Text = PrecoTotal.ToString("C2");

            if (lblQuantidade != null)
                lblQuantidade.Text = Quantidade.ToString();

            if (btnDiminuir != null)
                btnDiminuir.Enabled = Quantidade > 1;

            if (btnAumentar != null)
                btnAumentar.Enabled = Quantidade < EstoqueMaximo;
        }

        private void btnAumentar_Click(object sender, EventArgs e)
        {
            if (Quantidade < EstoqueMaximo)
            {
                Quantidade++;
                AtualizarInterface();
                AoAlterarQuantidade?.Invoke(ProdutoId, Quantidade, PrecoTotal);
            }
        }

        private void btnDiminuir_Click(object sender, EventArgs e)
        {
            if (Quantidade > 1)
            {
                Quantidade--;
                AtualizarInterface();
                AoAlterarQuantidade?.Invoke(ProdutoId, Quantidade, PrecoTotal);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            AoRemoverItem?.Invoke(ProdutoId, this);
        }




        private async Task CarregarImagemAsync(string fotoOrigem)
        {
            if (picFoto == null) return;

            Image miniatura = await UcCardProduto.ObterMiniaturaCompartilhadaAsync(fotoOrigem);

            if (!IsDisposed && picFoto != null && string.Equals(FotoOrigem, fotoOrigem, StringComparison.Ordinal))
            {
                picFoto.Image = miniatura;
            }
        }
    }
}