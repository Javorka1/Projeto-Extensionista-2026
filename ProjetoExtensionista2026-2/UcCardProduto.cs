using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public partial class UcCardProduto : UserControl
    {
        private static readonly ConcurrentDictionary<string, Bitmap> _imageCache = new ConcurrentDictionary<string, Bitmap>();
        private static Bitmap _fallbackAjustado;

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
            DoubleBuffered = true;

            UpDownProduto.Minimum = 1;
            UpDownProduto.Maximum = 999;
            UpDownProduto.Value = 1;

            picFoto.SizeMode = PictureBoxSizeMode.Normal;

            VincularCliquesDeEdicao(this);
            VincularEfeitoHover(this);

            Disposed += (s, e) =>
            {
                if (picFoto != null && picFoto.Image != null && !ReferenceEquals(picFoto.Image, _fallbackAjustado))
                {
                    picFoto.Image.Dispose();
                    picFoto.Image = null;
                }
            };
        }

        private void VincularEfeitoHover(Control container)
        {
            EventHandler aoEntrar = (s, e) =>
            {
                if (this.Tag?.ToString() != "card_hover")
                {
                    this.Tag = "card_hover";
                    this.BackColor = GerenciadorTema.ModoEscuro ? GerenciadorTema.FundoCardHoverEscuro : GerenciadorTema.FundoCardHoverClaro;
                    this.Invalidate();
                }
            };

            EventHandler aoSair = (s, e) =>
            {
                if (!this.ClientRectangle.Contains(this.PointToClient(Cursor.Position)))
                {
                    this.Tag = "card";
                    this.BackColor = GerenciadorTema.ModoEscuro ? GerenciadorTema.FundoCardEscuro : GerenciadorTema.FundoCardClaro;
                    this.Invalidate();
                }
            };

            container.MouseEnter += aoEntrar;
            container.MouseLeave += aoSair;

            foreach (Control ctrl in container.Controls)
            {
                ctrl.MouseEnter += aoEntrar;
                ctrl.MouseLeave += aoSair;
                if (ctrl.HasChildren)
                {
                    VincularEfeitoHover(ctrl);
                }
            }
        }




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
            lblOculto.Tag = "subtitulo";
            lblOculto.ForeColor = GerenciadorTema.ModoEscuro ? GerenciadorTema.TextoSecundarioEscuro : GerenciadorTema.TextoSecundarioClaro;

            lblOculto.Visible = PodeVerCusto;

            lblEstoque.Text = $"Estoque: {p.Estoque}";

            if (p.Estoque <= 0)
            {
                lblEstoque.Tag = "badge_err";
            }
            else if (p.Estoque <= p.EstoqueMinimo)
            {
                lblEstoque.Tag = "badge_warn";
            }
            else
            {
                lblEstoque.Tag = "badge_ok";
            }

            GerenciadorTema.EstilizarBadgeEstoque(lblEstoque);

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
                lblOculto.ForeColor = GerenciadorTema.CorPerigo;
            }
            else
            {
                lblOculto.Text = "Mostrar Ocultados...";
                lblOculto.ForeColor = GerenciadorTema.ModoEscuro ? GerenciadorTema.TextoSecundarioEscuro : GerenciadorTema.TextoSecundarioClaro;
            }
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            int quantidade = (int)UpDownProduto.Value;
            AoAdicionarProduto?.Invoke(ProdutoId, PrecoVenda, quantidade);
            UpDownProduto.Value = 1;
        }

        private static readonly Image _imagemPadrao = Properties.Resources.produto_ausente;

        public static async Task<Image> ObterMiniaturaCompartilhadaAsync(string fotoOrigem)
        {
            if (string.IsNullOrWhiteSpace(fotoOrigem))
                return _imagemPadrao;

            if (_imageCache.TryGetValue(fotoOrigem, out Bitmap miniaturaCached))
                return miniaturaCached;

            try
            {
                Bitmap miniaturaGerada = null;

                if (fotoOrigem.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                    fotoOrigem.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    byte[] dados;
                    using (var clienteWeb = new WebClient())
                    {
                        clienteWeb.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                        dados = await clienteWeb.DownloadDataTaskAsync(new Uri(fotoOrigem));
                    }

                    miniaturaGerada = await Task.Run(() => CriarMiniaturaDeBytes(dados));
                }
                else if (File.Exists(fotoOrigem))
                {
                    miniaturaGerada = await Task.Run(() =>
                    {
                        using (var stream = new FileStream(fotoOrigem, FileMode.Open, FileAccess.Read, FileShare.Read))
                        using (var imgBruta = Image.FromStream(stream))
                        {
                            return RedimensionarMiniatura(imgBruta, 180);
                        }
                    });
                }
                else
                {
                    miniaturaGerada = await Task.Run(() =>
                    {
                        byte[] imageBytes = Convert.FromBase64String(fotoOrigem);
                        return CriarMiniaturaDeBytes(imageBytes);
                    });
                }

                if (miniaturaGerada != null)
                {
                    Bitmap miniaturaFinal = _imageCache.GetOrAdd(fotoOrigem, miniaturaGerada);
                    if (!ReferenceEquals(miniaturaFinal, miniaturaGerada))
                        miniaturaGerada.Dispose();

                    return miniaturaFinal;
                }
            }
            catch
            {

            }

            return _imagemPadrao;
        }

        private async Task CarregarImagemAsync(string fotoOrigem)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fotoOrigem))
                {
                    AplicarImagemFallback();
                    return;
                }

                if (_imageCache.TryGetValue(fotoOrigem, out Bitmap miniaturaCached))
                {
                    AplicarImagemAjustada(miniaturaCached);
                    return;
                }

                Bitmap miniaturaGerada = null;

                if (fotoOrigem.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                    fotoOrigem.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    byte[] dados;
                    using (var clienteWeb = new WebClient())
                    {
                        clienteWeb.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                        dados = await clienteWeb.DownloadDataTaskAsync(new Uri(fotoOrigem));
                    }

                    miniaturaGerada = await Task.Run(() => CriarMiniaturaDeBytes(dados));
                }
                else if (File.Exists(fotoOrigem))
                {
                    miniaturaGerada = await Task.Run(() =>
                    {
                        using (var stream = new FileStream(fotoOrigem, FileMode.Open, FileAccess.Read, FileShare.Read))
                        using (var imgBruta = Image.FromStream(stream))
                        {
                            return RedimensionarMiniatura(imgBruta, 180);
                        }
                    });
                }
                else
                {
                    miniaturaGerada = await Task.Run(() =>
                    {
                        byte[] imageBytes = Convert.FromBase64String(fotoOrigem);
                        return CriarMiniaturaDeBytes(imageBytes);
                    });
                }

                if (miniaturaGerada != null)
                {
                    Bitmap miniaturaFinal = _imageCache.GetOrAdd(fotoOrigem, miniaturaGerada);
                    if (!ReferenceEquals(miniaturaFinal, miniaturaGerada))
                    {
                        miniaturaGerada.Dispose();
                    }

                    if (!IsDisposed && string.Equals(FotoOrigem, fotoOrigem, StringComparison.Ordinal))
                    {
                        AplicarImagemAjustada(miniaturaFinal);
                    }
                }
                else if (!IsDisposed && string.Equals(FotoOrigem, fotoOrigem, StringComparison.Ordinal))
                {
                    AplicarImagemFallback();
                }
            }
            catch
            {
                if (!IsDisposed && string.Equals(FotoOrigem, fotoOrigem, StringComparison.Ordinal))
                {
                    AplicarImagemFallback();
                }
            }
        }

        private static Bitmap CriarMiniaturaDeBytes(byte[] dados)
        {
            if (dados == null || dados.Length == 0) return null;

            using (var ms = new MemoryStream(dados))
            using (var imgBruta = Image.FromStream(ms))
            {
                return RedimensionarMiniatura(imgBruta, 180);
            }
        }

        private static Bitmap RedimensionarMiniatura(Image origem, int maxLado)
        {
            if (origem == null) return null;

            float escala = Math.Min((float)maxLado / origem.Width, (float)maxLado / origem.Height);
            int largura = escala >= 1f ? origem.Width : Math.Max(1, (int)(origem.Width * escala));
            int altura = escala >= 1f ? origem.Height : Math.Max(1, (int)(origem.Height * escala));

            var miniatura = new Bitmap(largura, altura);
            using (var g = Graphics.FromImage(miniatura))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawImage(origem, 0, 0, largura, altura);
            }

            return miniatura;
        }

        private void AplicarImagemFallback()
        {
            if (picFoto == null || picFoto.Width <= 0 || picFoto.Height <= 0)
                return;

            if (_fallbackAjustado == null || _fallbackAjustado.Width != picFoto.Width || _fallbackAjustado.Height != picFoto.Height)
            {
                _fallbackAjustado?.Dispose();
                _fallbackAjustado = GerarBitmapRecortado(Properties.Resources.produto_ausente, picFoto.Width, picFoto.Height);
            }

            if (picFoto.Image != null && !ReferenceEquals(picFoto.Image, _fallbackAjustado))
            {
                picFoto.Image.Dispose();
            }

            picFoto.Image = _fallbackAjustado;
        }

        private void AplicarImagemAjustada(Image imgMiniatura)
        {
            if (imgMiniatura == null || picFoto == null || picFoto.Width <= 0 || picFoto.Height <= 0)
                return;

            Bitmap bmpCortado = GerarBitmapRecortado(imgMiniatura, picFoto.Width, picFoto.Height);

            if (picFoto.Image != null && !ReferenceEquals(picFoto.Image, _fallbackAjustado))
            {
                picFoto.Image.Dispose();
            }

            picFoto.Image = bmpCortado;
        }

        private static Bitmap GerarBitmapRecortado(Image origem, int alvoW, int alvoH)
        {
            Bitmap bmpCortado = new Bitmap(alvoW, alvoH);

            using (Graphics g = Graphics.FromImage(bmpCortado))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;

                float ratio = Math.Max((float)alvoW / origem.Width, (float)alvoH / origem.Height);

                int larguraFinal = (int)(origem.Width * ratio);
                int alturaFinal = (int)(origem.Height * ratio);

                int posX = (alvoW - larguraFinal) / 2;
                int posY = (alvoH - alturaFinal) / 2;

                g.DrawImage(origem, posX, posY, larguraFinal, alturaFinal);
            }

            return bmpCortado;
        }
    }
}