using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public partial class formDocumentosFiscais : Form
    {
        public bool EmitirCupomFiscal => chkCupom.Checked;
        public bool EmitirNotaPromissoria => chkPromissoria.Checked;
        public string ImpressoraSelecionada => cmbImpressoras.SelectedItem?.ToString() ?? string.Empty;

        private readonly DadosDocumentoVenda _dados;
        private List<Bitmap> _paginasCupom = new List<Bitmap>();
        private List<Bitmap> _paginasPromissoria = new List<Bitmap>();
        private int _paginaAtualCupom = 0;
        private int _paginaAtualPromissoria = 0;
        private bool _possuiImpressora = false;

        public formDocumentosFiscais(DadosDocumentoVenda dados, bool vendaAPrazo = false)
        {
            InitializeComponent();

            _dados = dados ?? new DadosDocumentoVenda();

            GerarPaginasEmMemoria();
            CarregarImpressoras();
            VincularEventosDosControles(vendaAPrazo);
            GerenciadorTema.AplicarTema(this);
            AtualizarEstadoTela();
        }

        private void GerarPaginasEmMemoria()
        {
            _paginasCupom = ImpressorDocumentos.RenderizarPaginasCupom(_dados);
            _paginasPromissoria = ImpressorDocumentos.RenderizarPaginasPromissoria(_dados);

            ConfigurarPainelMiniatura(picCupom, ehCupom: true);
            ConfigurarPainelMiniatura(picPromissoria, ehCupom: false);
        }

        private void ConfigurarPainelMiniatura(PictureBox pic, bool ehCupom)
        {
            if (pic == null) return;

            pic.Image = null;
            pic.BackgroundImage = null;
            pic.Paint -= PainelMiniatura_Paint;
            pic.Paint += PainelMiniatura_Paint;
            pic.Resize -= PainelMiniatura_Resize;
            pic.Resize += PainelMiniatura_Resize;
            pic.Tag = ehCupom;
            pic.Invalidate();
        }

        private void PainelMiniatura_Resize(object sender, EventArgs e)
        {
            (sender as Control)?.Invalidate();
        }

        private void PainelMiniatura_Paint(object sender, PaintEventArgs e)
        {
            if (!(sender is PictureBox pic) || !(pic.Tag is bool ehCupom)) return;

            var lista = ehCupom ? _paginasCupom : _paginasPromissoria;
            int indice = ehCupom ? _paginaAtualCupom : _paginaAtualPromissoria;
            if (lista == null || lista.Count == 0 || indice >= lista.Count) return;

            Bitmap bmp = lista[indice];
            Graphics g = e.Graphics;

            g.Clear(Color.FromArgb(228, 232, 238));
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            int margem = 10;
            float areaW = Math.Max(10, pic.ClientSize.Width - margem * 2);
            float areaH = Math.Max(10, pic.ClientSize.Height - margem * 2);

            float escalaX = areaW / bmp.Width;
            float escalaY = areaH / bmp.Height;

            bool cupomComprido = ehCupom && (bmp.Height > bmp.Width * 1.75f);
            float escala = cupomComprido ? escalaX : Math.Min(escalaX, escalaY);

            int destW = (int)(bmp.Width * escala);
            int destH = (int)(bmp.Height * escala);
            int destX = (pic.ClientSize.Width - destW) / 2;
            int destY = cupomComprido ? margem : (pic.ClientSize.Height - destH) / 2;

            using (var brushSombra = new SolidBrush(Color.FromArgb(45, 0, 0, 0)))
            using (var penBorda = new Pen(Color.FromArgb(170, 175, 185), 1f))
            {
                g.FillRectangle(brushSombra, destX + 3, destY + 3, destW, destH);
                g.DrawImage(bmp, new Rectangle(destX, destY, destW, destH));
                g.DrawRectangle(penBorda, destX, destY, destW, destH);
            }
        }

        private void TrocarPaginaMiniatura(bool ehCupom, int delta)
        {
            var lista = ehCupom ? _paginasCupom : _paginasPromissoria;
            if (lista.Count <= 1) return;

            if (ehCupom)
            {
                if (delta < 0 && _paginaAtualCupom < lista.Count - 1) _paginaAtualCupom++;
                else if (delta > 0 && _paginaAtualCupom > 0) _paginaAtualCupom--;
                picCupom?.Invalidate();
            }
            else
            {
                if (delta < 0 && _paginaAtualPromissoria < lista.Count - 1) _paginaAtualPromissoria++;
                else if (delta > 0 && _paginaAtualPromissoria > 0) _paginaAtualPromissoria--;
                picPromissoria?.Invalidate();
            }
        }

        private void VincularEventosDosControles(bool vendaAPrazo)
        {
            chkCupom.Checked = true;
            chkPromissoria.Checked = vendaAPrazo;

            chkCupom.CheckedChanged += (s, e) => AtualizarEstadoTela();
            chkPromissoria.CheckedChanged += (s, e) => AtualizarEstadoTela();

            // Botões de expandir em tela cheia
            if (btnVerCupom != null)
                btnVerCupom.Click += (s, e) => AbrirPopupTelaCheia(ehCupom: true);

            if (btnVerPromissoria != null)
                btnVerPromissoria.Click += (s, e) => AbrirPopupTelaCheia(ehCupom: false);

            // Também permite clicar direto na miniatura ou rolar o mouse sobre ela
            if (picCupom != null)
            {
                picCupom.Cursor = Cursors.Hand;
                picCupom.Click += (s, e) => AbrirPopupTelaCheia(ehCupom: true);
                picCupom.MouseEnter += (s, e) => picCupom.Focus();
                picCupom.MouseWheel += (s, e) => TrocarPaginaMiniatura(ehCupom: true, delta: e.Delta);
            }

            if (picPromissoria != null)
            {
                picPromissoria.Cursor = Cursors.Hand;
                picPromissoria.Click += (s, e) => AbrirPopupTelaCheia(ehCupom: false);
                picPromissoria.MouseEnter += (s, e) => picPromissoria.Focus();
                picPromissoria.MouseWheel += (s, e) => TrocarPaginaMiniatura(ehCupom: false, delta: e.Delta);
            }

            if (btnSalvarPdf != null)
                btnSalvarPdf.Click += BtnSalvarPdf_Click;

            if (btnImprimir != null)
                btnImprimir.Click += BtnImprimir_Click;

            if (btnCancelar != null)
                btnCancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
        }

        private void AbrirPopupTelaCheia(bool ehCupom)
        {
            var paginas = ehCupom ? _paginasCupom : _paginasPromissoria;
            if (paginas == null || paginas.Count == 0) return;

            string titulo = ehCupom ? "Cupom Fiscal (80 mm)" : "Nota Promissória";
            int paginaInicial = ehCupom ? _paginaAtualCupom : _paginaAtualPromissoria;

            using (var popup = new formVisualizarDocumento(titulo, paginas, paginaInicial))
            {
                popup.ShowDialog(this);
            }
        }

        private void CarregarImpressoras()
        {
            cmbImpressoras.Items.Clear();
            _possuiImpressora = false;

            try
            {
                if (PrinterSettings.InstalledPrinters.Count > 0)
                {
                    string padrao = new PrinterSettings().PrinterName;

                    foreach (string impressora in PrinterSettings.InstalledPrinters)
                        cmbImpressoras.Items.Add(impressora);

                    int idxPadrao = cmbImpressoras.Items.IndexOf(padrao);
                    cmbImpressoras.SelectedIndex = idxPadrao >= 0 ? idxPadrao : 0;
                    cmbImpressoras.Enabled = true;
                    _possuiImpressora = true;
                    return;
                }
            }
            catch
            {
                // Spooler parado ou sem impressoras instaladas
            }

            cmbImpressoras.Items.Add("Nenhuma impressora instalada");
            cmbImpressoras.SelectedIndex = 0;
            cmbImpressoras.Enabled = false;
            _possuiImpressora = false;
        }

        private void AtualizarEstadoTela()
        {
            if (lblAviso != null) lblAviso.Text = string.Empty;
            if (picCupom != null) picCupom.Visible = chkCupom.Checked;
            if (btnVerCupom != null) btnVerCupom.Enabled = chkCupom.Checked;
            if (picPromissoria != null) picPromissoria.Visible = chkPromissoria.Checked;
            if (btnVerPromissoria != null) btnVerPromissoria.Enabled = chkPromissoria.Checked;
            if (chkPromissoria.Checked && !_dados.PossuiClienteIdentificado)
            {
                if (lblAviso != null)
                    lblAviso.Text = "Sem cliente informado: será emitido sem valor de Nota Promissória.";
            }

            bool algumMarcado = chkCupom.Checked || chkPromissoria.Checked;

            if (btnSalvarPdf != null)
                btnSalvarPdf.Enabled = algumMarcado;

            if (btnImprimir != null)
            {
                if (!algumMarcado)
                {
                    btnImprimir.Enabled = true;
                    btnImprimir.Text = "Finalizar sem Emitir";
                }
                else if (!_possuiImpressora)
                {
                    btnImprimir.Enabled = false;
                    btnImprimir.Text = "Imprimir";
                    if (lblAviso != null && string.IsNullOrEmpty(lblAviso.Text))
                        lblAviso.Text = "Nenhuma impressora no PC. Salve em PDF para emitir.";
                }
                else
                {
                    btnImprimir.Enabled = true;
                    btnImprimir.Text = "Imprimir";
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (picCupom != null && picCupom.Visible &&
                picCupom.ClientRectangle.Contains(picCupom.PointToClient(Cursor.Position)))
            {
                TrocarPaginaMiniatura(ehCupom: true, delta: e.Delta);
            }
            else if (picPromissoria != null && picPromissoria.Visible &&
                     picPromissoria.ClientRectangle.Contains(picPromissoria.PointToClient(Cursor.Position)))
            {
                TrocarPaginaMiniatura(ehCupom: false, delta: e.Delta);
            }
        }

        private void BtnSalvarPdf_Click(object sender, EventArgs e)
        {
            var paginasParaSalvar = new List<Bitmap>();
            if (chkCupom.Checked) paginasParaSalvar.AddRange(_paginasCupom);
            if (chkPromissoria.Checked) paginasParaSalvar.AddRange(_paginasPromissoria);

            if (paginasParaSalvar.Count == 0) return;

            using (var sfd = new SaveFileDialog
            {
                Title = "Salvar Documentos da Venda em PDF",
                Filter = "Arquivo PDF (*.pdf)|*.pdf",
                FileName = $"Documentos_Venda_{_dados.NumeroVenda:D6}.pdf"
            })
            {
                if (sfd.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    ImpressorDocumentos.SalvarComoPdf(sfd.FileName, paginasParaSalvar);
                    MessageBox.Show("Documento(s) salvo(s) em PDF com sucesso!", "Arquivo Salvo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao salvar o arquivo PDF:\n{ex.Message}", "Erro ao Salvar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnImprimir_Click(object sender, EventArgs e)
        {
            if (!chkCupom.Checked && !chkPromissoria.Checked)
            {
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            if (!_possuiImpressora || string.IsNullOrWhiteSpace(ImpressoraSelecionada))
            {
                MessageBox.Show("Nenhuma impressora disponível no computador. Utilize a opção de salvar em PDF.", "Impressão Bloqueada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (chkCupom.Checked)
                    ImpressorDocumentos.ImprimirPaginasNaImpressora(_paginasCupom, ImpressoraSelecionada, $"Cupom_{_dados.NumeroVenda:D6}", true);

                if (chkPromissoria.Checked)
                    ImpressorDocumentos.ImprimirPaginasNaImpressora(_paginasPromissoria, ImpressoraSelecionada, $"Promissoria_{_dados.NumeroVenda:D6}", false);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao enviar para a impressora:\n{ex.Message}", "Erro de Impressão", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            foreach (var bmp in _paginasCupom) bmp?.Dispose();
            foreach (var bmp in _paginasPromissoria) bmp?.Dispose();
            base.OnFormClosed(e);
        }
    }
}