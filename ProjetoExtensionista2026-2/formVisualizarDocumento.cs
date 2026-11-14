using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public class formVisualizarDocumento : Form
    {
        private readonly string _titulo;
        private readonly List<Bitmap> _paginas;
        private int _indiceAtual;

        private readonly Label _lblTopo;
        private readonly Panel _pnlMolduraFolha;
        private readonly PictureBox _picFolha;

        public formVisualizarDocumento(string tituloDocumento, List<Bitmap> paginas, int paginaInicial = 0)
        {
            _titulo = tituloDocumento;
            _paginas = paginas ?? new List<Bitmap>();
            _indiceAtual = Math.Max(0, Math.Min(paginaInicial, _paginas.Count - 1));

            // Estilo popup em tela cheia sobre tudo
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(24, 26, 30);
            ShowInTaskbar = false;
            KeyPreview = true;
            Cursor = Cursors.Hand;

            // Clicar no fundo escuro (fora da folha) fecha o popup
            Click += (s, e) => Close();

            _lblTopo = new Label
            {
                Dock = DockStyle.Top,
                Height = 42,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };
            _lblTopo.Click += (s, e) => Close();

            _pnlMolduraFolha = new Panel
            {
                BackColor = Color.White,
                Padding = new Padding(4),
                Cursor = Cursors.Default
            };

            _picFolha = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                SizeMode = PictureBoxSizeMode.Zoom,
                Cursor = Cursors.Default
            };

            _pnlMolduraFolha.Controls.Add(_picFolha);
            Controls.Add(_pnlMolduraFolha);
            Controls.Add(_lblTopo);

            Resize += (s, e) => CentralizarFolha();
            Shown += (s, e) =>
            {
                ExibirPaginaAtual();
                Focus();
            };
        }

        private void ExibirPaginaAtual()
        {
            if (_paginas.Count == 0) return;

            _picFolha.Image = _paginas[_indiceAtual];
            _lblTopo.Text = $"{_titulo}  —  Página {_indiceAtual + 1} de {_paginas.Count}    |    Role o mouse para trocar de página  •  Clique fora ou pressione ESC para voltar";
            CentralizarFolha();
        }

        private void CentralizarFolha()
        {
            if (_paginas.Count == 0 || ClientSize.Width <= 0 || ClientSize.Height <= 0) return;

            Bitmap bmpAtual = _paginas[_indiceAtual];
            double proporcao = (double)bmpAtual.Width / bmpAtual.Height;

            int alturaDisponivel = ClientSize.Height - 72;
            int larguraCalculada = (int)(alturaDisponivel * proporcao);
            int larguraMaxima = ClientSize.Width - 80;

            if (larguraCalculada > larguraMaxima)
            {
                larguraCalculada = larguraMaxima;
                alturaDisponivel = (int)(larguraCalculada / proporcao);
            }

            _pnlMolduraFolha.Size = new Size(larguraCalculada, alturaDisponivel);
            _pnlMolduraFolha.Location = new Point(
                (ClientSize.Width - larguraCalculada) / 2,
                48 + (ClientSize.Height - 64 - alturaDisponivel) / 2
            );
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (_paginas.Count <= 1) return;

            if (e.Delta < 0 && _indiceAtual < _paginas.Count - 1)
            {
                _indiceAtual++;
                ExibirPaginaAtual();
            }
            else if (e.Delta > 0 && _indiceAtual > 0)
            {
                _indiceAtual--;
                ExibirPaginaAtual();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}