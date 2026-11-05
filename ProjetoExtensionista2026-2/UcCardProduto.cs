using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public partial class UcCardProduto : UserControl
    {
        private static readonly ConcurrentDictionary<string, Image> _imageCache = new ConcurrentDictionary<string, Image>();

        public ProdutoDTO DadosProduto { get; private set; }

        public int ProdutoId { get; private set; }
        public string NomeProduto { get; private set; }
        public string DescricaoProduto { get; private set; }
        public decimal PrecoVenda { get; private set; }
        public decimal PrecoCompra { get; private set; }
        public int Estoque { get; private set; }
        public string FotoOrigem { get; private set; }

        private bool _custoVisivel = false;
        public bool PodeVerCusto { get; private set; } = true;
        public bool PodeEditar { get; private set; } = true;

        public event Action<int, decimal, int> AoAdicionarProduto;
        public event Action<ProdutoDTO> AoClicarParaEditar;

        public UcCardProduto()
        {
            InitializeComponent();

            UpDownProduto.Minimum = 1;
            UpDownProduto.Maximum = 999;
            UpDownProduto.Value = 1;

            picFoto.SizeMode = PictureBoxSizeMode.Zoom;

            VincularCliquesDeEdicao(this);
        }

        /// <summary>
        /// Configura se o card pode exibir o preço de custo e se permite abrir para edição.
        /// </summary>
        public void ConfigurarPermissoes(bool podeVerCusto, bool podeEditar)
        {
            PodeVerCusto = podeVerCusto;
            PodeEditar = podeEditar;
            if (lblOculto != null) lblOculto.Visible = podeVerCusto;
            this.Cursor = podeEditar ? Cursors.Hand : Cursors.Default;
        }

        private void VincularCliquesDeEdicao(Control container)
        {
            EventHandler dispararEdicao = (s, e) =>
            {
                if (!PodeEditar) return;

                if (DadosProduto != null)
                {
                    AoClicarParaEditar?.Invoke(DadosProduto);
                }
            };

            foreach (Control ctrl in container.Controls)
            {
                if (ctrl == UpDownProduto || ctrl == btnAdicionar || ctrl == lblOculto || ctrl is NumericUpDown || ctrl is Button)
                {
                    ctrl.Cursor = Cursors.Default;
                    continue;
                }

                ctrl.Click += dispararEdicao;
                ctrl.Cursor = PodeEditar ? Cursors.Hand : Cursors.Default;

                if (ctrl.HasChildren)
                {
                    VincularCliquesDeEdicao(ctrl);
                }
            }

            container.Click += dispararEdicao;
            container.Cursor = PodeEditar ? Cursors.Hand : Cursors.Default;
        }

        public void CarregarDados(ProdutoDTO p)
        {
            if (p == null) return;

            DadosProduto = p;

            ProdutoId = p.Id;
            NomeProduto = p.Nome;
            DescricaoProduto = p.Descricao;
            PrecoVenda = p.PrecoVenda;
            PrecoCompra = p.PrecoCompra;
            Estoque = p.Estoque;
            FotoOrigem = p.FotoUrl;

            lblCodigo.Text = $"#{p.Id}";
            lblTitulo.Text = LimitarTexto(p.Nome, 60);
            lblDescricao.Text = LimitarTexto(string.IsNullOrWhiteSpace(p.Descricao) ? "Sem descrição disponível." : p.Descricao, 140);
            lblPrecoVenda.Text = p.PrecoVenda.ToString("C2");

            _custoVisivel = false;
            lblOculto.Text = "Mostrar Ocultados...";
            lblOculto.ForeColor = SystemColors.ControlDarkDark;

            lblOculto.Visible = PodeVerCusto;

            lblEstoque.Text = $"Estoque: {p.Estoque}";

            if (p.Estoque <= 0)
            {
                lblEstoque.BackColor = Color.LightCoral;
            }
            else if (p.Estoque <= p.EstoqueMinimo)
            {
                lblEstoque.BackColor = Color.LightSalmon;
            }
            else
            {
                lblEstoque.BackColor = Color.PaleGreen;
            }

            UpDownProduto.Value = 1;
            _ = CarregarImagemAsync(p.FotoUrl);
        }

        public void CarregarDados(int id, string nome, string descricao, decimal precoVenda, decimal precoCompra, int estoque, string fotoOrigem)
        {
            var dto = new ProdutoDTO
            {
                Id = id,
                Nome = nome,
                Descricao = descricao,
                PrecoVenda = precoVenda,
                PrecoCompra = precoCompra,
                Estoque = estoque,
                FotoUrl = fotoOrigem,
                Ativo = true
            };

            CarregarDados(dto);
        }

        private string LimitarTexto(string texto, int max)
        {
            if (string.IsNullOrWhiteSpace(texto)) return "";
            return texto.Length <= max ? texto : texto.Substring(0, max).TrimEnd() + "...";
        }

        private void btnOculto_Click(object sender, EventArgs e)
        {
            if (!PodeVerCusto) return;

            _custoVisivel = !_custoVisivel;

            if (_custoVisivel)
            {
                lblOculto.Text = $"Custo: {PrecoCompra:C2}";
                lblOculto.ForeColor = Color.Red;
            }
            else
            {
                lblOculto.Text = "Mostrar Ocultados...";
                lblOculto.ForeColor = SystemColors.ControlDarkDark;
            }
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            int quantidade = (int)UpDownProduto.Value;
            AoAdicionarProduto?.Invoke(ProdutoId, PrecoVenda, quantidade);
            UpDownProduto.Value = 1;
        }

        private async Task CarregarImagemAsync(string fotoOrigem)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fotoOrigem))
                {
                    AplicarImagemAjustada(Properties.Resources.produto_ausente);
                    return;
                }

                if (_imageCache.TryGetValue(fotoOrigem, out Image imagemCached))
                {
                    AplicarImagemAjustada(imagemCached);
                    return;
                }

                Image imgOriginal = null;

                if (fotoOrigem.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                    fotoOrigem.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    using (var clienteWeb = new WebClient())
                    {
                        clienteWeb.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                        byte[] dados = await clienteWeb.DownloadDataTaskAsync(new Uri(fotoOrigem));

                        using (var ms = new MemoryStream(dados))
                        {
                            imgOriginal = Image.FromStream(ms);
                        }
                    }
                }
                else if (File.Exists(fotoOrigem))
                {
                    using (var stream = new FileStream(fotoOrigem, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (var tempImg = Image.FromStream(stream))
                        {
                            imgOriginal = new Bitmap(tempImg);
                        }
                    }
                }
                else
                {
                    byte[] imageBytes = Convert.FromBase64String(fotoOrigem);
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        imgOriginal = Image.FromStream(ms);
                    }
                }

                if (imgOriginal != null)
                {
                    _imageCache.TryAdd(fotoOrigem, imgOriginal);
                    AplicarImagemAjustada(imgOriginal);
                }
                else
                {
                    AplicarImagemAjustada(Properties.Resources.produto_ausente);
                }
            }
            catch
            {
                AplicarImagemAjustada(Properties.Resources.produto_ausente);
            }
        }

        private void AplicarImagemAjustada(Image imgOriginal)
        {
            if (imgOriginal == null || picFoto.Width <= 0 || picFoto.Height <= 0)
                return;

            int alvoW = picFoto.Width;
            int alvoH = picFoto.Height;

            Bitmap bmpCortado = new Bitmap(alvoW, alvoH);

            using (Graphics g = Graphics.FromImage(bmpCortado))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                float ratio = Math.Max((float)alvoW / imgOriginal.Width, (float)alvoH / imgOriginal.Height);

                int larguraFinal = (int)(imgOriginal.Width * ratio);
                int alturaFinal = (int)(imgOriginal.Height * ratio);

                int posX = (alvoW - larguraFinal) / 2;
                int posY = (alvoH - alturaFinal) / 2;

                g.DrawImage(imgOriginal, posX, posY, larguraFinal, alturaFinal);
            }

            picFoto.Image?.Dispose();
            picFoto.SizeMode = PictureBoxSizeMode.Normal;
            picFoto.Image = bmpCortado;
        }
    }
}