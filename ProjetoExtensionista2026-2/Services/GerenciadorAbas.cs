using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public class GerenciadorAbas
    {
        public class AbaItem
        {
            public string ChaveUnica { get; set; }
            public string Titulo { get; set; }
            public Control Conteudo { get; set; }
            public Panel Cabecalho { get; set; }
            public Button BotaoTitulo { get; set; }
            public Button BotaoFechar { get; set; }
            public Func<bool> PodeFechar { get; set; }
            public Func<Control> FabricaDuplicacao { get; set; }
        }

        private readonly FlowLayoutPanel _barraAbas;
        private readonly Panel _painelConteudo;
        private readonly Button _btnNovaAba;
        private readonly List<AbaItem> _abas = new List<AbaItem>();
        private AbaItem _abaAtiva;

        public int LimiteMaximoAbas { get; set; } = 6;
        public event Action AoSolicitarNovaAba;
        public AbaItem AbaAtiva => _abaAtiva;
        public int TotalAbas => _abas.Count;

        public GerenciadorAbas(Panel containerBarraSuperior, Panel painelConteudo, bool mostrarBotaoAdicionar = true)
        {
            _painelConteudo = painelConteudo;

            _barraAbas = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(6, 4, 6, 0),
                Margin = new Padding(0)
            };

            containerBarraSuperior.Controls.Clear();
            containerBarraSuperior.Controls.Add(_barraAbas);

            if (mostrarBotaoAdicionar)
            {
                _btnNovaAba = new Button
                {
                    Text = "+",
                    Width = 32,
                    Height = 30,
                    Margin = new Padding(4, 2, 0, 0),
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                _btnNovaAba.Click += (s, e) => AoSolicitarNovaAba?.Invoke();
                _barraAbas.Controls.Add(_btnNovaAba);
            }
        }




        public AbaItem AbrirOuFocarAba(
            string chaveUnica,
            string titulo,
            Func<Control> fabricaTela,
            bool podeFechar = true,
            Func<bool> validacaoFechamento = null)
        {
            if (!string.IsNullOrWhiteSpace(chaveUnica))
            {
                var existente = _abas.FirstOrDefault(a =>
                    string.Equals(a.ChaveUnica, chaveUnica, StringComparison.OrdinalIgnoreCase));

                if (existente != null)
                {
                    SelecionarAba(existente);
                    return existente;
                }
            }

            if (_abas.Count >= LimiteMaximoAbas)
            {
                MessageBox.Show(
                    $"Limite de {LimiteMaximoAbas} abas simultâneas atingido.\nFeche uma aba inativa antes de abrir outra.",
                    "Limite de Abas",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return null;
            }

            return AdicionarAba(titulo, fabricaTela(), podeFechar, validacaoFechamento, chaveUnica, fabricaTela);
        }




        public AbaItem AdicionarAba(
            string titulo,
            Control tela,
            bool podeFechar = true,
            Func<bool> validacaoFechamento = null,
            string chaveUnica = null,
            Func<Control> fabricaDuplicacao = null)
        {
            if (_abas.Count >= LimiteMaximoAbas)
            {
                tela?.Dispose();
                MessageBox.Show(
                    $"Limite de {LimiteMaximoAbas} abas simultâneas atingido.\nFeche uma aba inativa antes de abrir outra.",
                    "Limite de Abas",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return null;
            }

            if (tela is Form frm)
            {
                frm.TopLevel = false;
                frm.FormBorderStyle = FormBorderStyle.None;
            }

            tela.Dock = DockStyle.Fill;
            tela.Visible = false;

            var painelTab = new Panel
            {
                Height = 32,
                Width = 195,
                Margin = new Padding(0, 2, 4, 0),
                Padding = new Padding(0)
            };

            var btnTitulo = new Button
            {
                Text = titulo,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 4, 0),
                AutoEllipsis = true,
                Tag = "aba_inativa"
            };

            Button btnFechar = null;
            if (podeFechar)
            {
                btnFechar = new Button
                {
                    Text = "×",
                    Dock = DockStyle.Right,
                    Width = 26,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Tag = "aba_inativa"
                };
            }

            var aba = new AbaItem
            {
                ChaveUnica = chaveUnica,
                Titulo = titulo,
                Conteudo = tela,
                Cabecalho = painelTab,
                BotaoTitulo = btnTitulo,
                BotaoFechar = btnFechar,
                PodeFechar = validacaoFechamento,
                FabricaDuplicacao = fabricaDuplicacao
            };

            btnTitulo.Click += (s, e) => SelecionarAba(aba);
            btnTitulo.MouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Middle && podeFechar)
                    FecharAba(aba);
            };
            btnTitulo.ContextMenuStrip = CriarMenuContextoAba(aba);

            if (btnFechar != null)
                btnFechar.Click += (s, e) => FecharAba(aba);

            painelTab.Controls.Add(btnTitulo);
            if (btnFechar != null)
                painelTab.Controls.Add(btnFechar);

            _abas.Add(aba);
            _painelConteudo.Controls.Add(tela);
            _barraAbas.Controls.Add(painelTab);

            if (_btnNovaAba != null)
                _barraAbas.Controls.SetChildIndex(_btnNovaAba, _barraAbas.Controls.Count - 1);

            AtualizarEstadoBotaoAdicionar();
            SelecionarAba(aba);
            return aba;
        }

        private ContextMenuStrip CriarMenuContextoAba(AbaItem aba)
        {
            var menu = new ContextMenuStrip();

            if (aba.FabricaDuplicacao != null && string.IsNullOrWhiteSpace(aba.ChaveUnica))
            {
                var itemDuplicar = new ToolStripMenuItem("Nova Aba de Caixa");
                itemDuplicar.Click += (s, e) => AoSolicitarNovaAba?.Invoke();
                menu.Items.Add(itemDuplicar);
            }

            if (aba.BotaoFechar != null)
            {
                var itemFechar = new ToolStripMenuItem("Fechar Aba");
                itemFechar.Click += (s, e) => FecharAba(aba);
                menu.Items.Add(itemFechar);
            }

            var itemFecharOutras = new ToolStripMenuItem("Fechar Outras Abas");
            itemFecharOutras.Click += (s, e) =>
            {
                var outras = _abas.Where(a => a != aba && a.BotaoFechar != null).ToList();
                foreach (var outra in outras)
                    FecharAba(outra);
            };
            menu.Items.Add(itemFecharOutras);

            return menu;
        }

        public void SelecionarAba(AbaItem aba)
        {
            if (aba == null || !_abas.Contains(aba)) return;

            _painelConteudo.SuspendLayout();
            try
            {
                foreach (var item in _abas)
                {
                    bool ativa = (item == aba);
                    item.Conteudo.Visible = ativa;
                    AplicarEstiloAba(item, ativa);
                }

                aba.Conteudo.BringToFront();
                _abaAtiva = aba;
                GerenciadorTema.AplicarTema(aba.Conteudo);
            }
            finally
            {
                _painelConteudo.ResumeLayout();
            }
        }

        public void AtualizarTemaTodasAbas()
        {
            foreach (var item in _abas)
                AplicarEstiloAba(item, item == _abaAtiva);

            if (_abaAtiva != null)
                GerenciadorTema.AplicarTema(_abaAtiva.Conteudo);
        }

        private static void AplicarEstiloAba(AbaItem item, bool ativa)
        {
            string tag = ativa ? "aba_ativa" : "aba_inativa";
            item.BotaoTitulo.Tag = tag;
            GerenciadorTema.AplicarTema(item.BotaoTitulo);

            if (item.BotaoFechar != null)
            {
                item.BotaoFechar.Tag = tag;
                GerenciadorTema.AplicarTema(item.BotaoFechar);
            }
        }

        public void RenomearAba(AbaItem aba, string novoTitulo)
        {
            if (aba == null) return;
            aba.Titulo = novoTitulo;
            aba.BotaoTitulo.Text = novoTitulo;
        }

        public void FecharAba(AbaItem aba)
        {
            if (aba == null || !_abas.Contains(aba)) return;
            if (aba.PodeFechar != null && !aba.PodeFechar()) return;

            int indice = _abas.IndexOf(aba);
            _abas.Remove(aba);

            _barraAbas.Controls.Remove(aba.Cabecalho);
            _painelConteudo.Controls.Remove(aba.Conteudo);

            aba.Cabecalho.Dispose();
            aba.Conteudo.Dispose();

            AtualizarEstadoBotaoAdicionar();

            if (_abaAtiva == aba)
            {
                _abaAtiva = null;
                if (_abas.Count > 0)
                    SelecionarAba(_abas[Math.Max(0, Math.Min(indice, _abas.Count - 1))]);
            }
        }

        private void AtualizarEstadoBotaoAdicionar()
        {
            if (_btnNovaAba != null)
                _btnNovaAba.Enabled = _abas.Count < LimiteMaximoAbas;
        }

        public void AvancarProximaAba()
        {
            if (_abas.Count <= 1 || _abaAtiva == null) return;
            int proximo = (_abas.IndexOf(_abaAtiva) + 1) % _abas.Count;
            SelecionarAba(_abas[proximo]);
        }

        public bool ProcessarAtalhos(Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.T))
            {
                AoSolicitarNovaAba?.Invoke();
                return true;
            }
            if (keyData == (Keys.Control | Keys.W) && _abaAtiva?.BotaoFechar != null)
            {
                FecharAba(_abaAtiva);
                return true;
            }
            if (keyData == (Keys.Control | Keys.Tab))
            {
                AvancarProximaAba();
                return true;
            }
            return false;
        }
    }
}