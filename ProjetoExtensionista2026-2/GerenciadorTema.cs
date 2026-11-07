using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public class GerenciadorTema
    {
        private static readonly string CaminhoArquivoConfig = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config_tema.txt");
        private static readonly HashSet<int> ControlesMonitorados = new HashSet<int>();
        private static readonly HashSet<int> ControlesComPaintCustom = new HashSet<int>();
        private static readonly Dictionary<string, Bitmap> CacheIcones = new Dictionary<string, Bitmap>();

        public static bool ModoEscuro { get; set; } = false;

        // =========================================================
        // PALETA SLATE & INDIGO (MODO ESCURO)
        // =========================================================
        public static readonly Color FundoEscuro = Color.FromArgb(24, 26, 32);              // #181A20 - Fundo principal
        public static readonly Color FundoCardEscuro = Color.FromArgb(34, 37, 46);          // #22252E - Cards, Painéis, GroupBoxes
        public static readonly Color FundoCardHoverEscuro = Color.FromArgb(40, 44, 55);     // #282C37 - Card em Hover
        public static readonly Color FundoSidebarEscuro = Color.FromArgb(31, 34, 43);       // #1F222B - Sidebar dedicada
        public static readonly Color FundoControleEscuro = Color.FromArgb(42, 46, 57);      // #2A2E39 - Botões secundários
        public static readonly Color InputEscuro = Color.FromArgb(30, 33, 41);              // #1E2129 - TextBox, ComboBox, Linhas tabela
        public static readonly Color InputFocoEscuro = Color.FromArgb(36, 41, 54);          // #242936 - Input focado
        public static readonly Color LinhaAlternadaEscura = Color.FromArgb(36, 40, 50);     // #242832 - Zebra DataGridView
        public static readonly Color BordaEscura = Color.FromArgb(56, 61, 76);              // #383D4C - Bordas sutis
        public static readonly Color TextoEscuro = Color.FromArgb(236, 239, 244);           // #ECEFF4 - Texto principal branco gelo
        public static readonly Color TextoSecundarioEscuro = Color.FromArgb(160, 174, 192); // #A0AEC0 - Subtítulos
        public static readonly Color BarraSuperiorEscura = Color.FromArgb(18, 20, 24);      // #121418 - Barra de título/status

        // =========================================================
        // PALETA SLATE & INDIGO (MODO CLARO REFINADO)
        // =========================================================
        public static readonly Color FundoClaro = Color.FromArgb(248, 250, 252);            // #F8FAFC - Fundo gelo moderno
        public static readonly Color FundoCardClaro = Color.FromArgb(255, 255, 255);        // #FFFFFF - Cards brancos puros
        public static readonly Color FundoCardHoverClaro = Color.FromArgb(241, 245, 249);   // #F1F5F9 - Card em Hover
        public static readonly Color FundoSidebarClaro = Color.FromArgb(255, 255, 255);     // #FFFFFF - Sidebar branca limpa
        public static readonly Color FundoControleClaro = Color.FromArgb(241, 245, 249);    // #F1F5F9 - Botões secundários claros
        public static readonly Color InputClaro = Color.FromArgb(255, 255, 255);            // #FFFFFF - Inputs brancos
        public static readonly Color InputFocoClaro = Color.FromArgb(239, 246, 255);        // #EFF6FF - Input focado
        public static readonly Color LinhaAlternadaClara = Color.FromArgb(248, 250, 252);   // #F8FAFC - Zebra DataGridView
        public static readonly Color BordaClara = Color.FromArgb(226, 232, 240);            // #E2E8F0 - Bordas limpas
        public static readonly Color TextoClaro = Color.FromArgb(15, 23, 42);               // #0F172A - Texto chumbo/preto legível
        public static readonly Color TextoSecundarioClaro = Color.FromArgb(100, 116, 139);  // #64748B - Subtítulos neutros
        public static readonly Color BarraSuperiorClara = Color.FromArgb(226, 232, 240);    // #E2E8F0 - Barra de título/status

        // Cores Semânticas e de Destaque
        public static Color CorDestaque => ModoEscuro ? Color.FromArgb(59, 130, 246) : Color.FromArgb(37, 99, 235);     // Indigo/Blue
        public static Color CorSucesso => ModoEscuro ? Color.FromArgb(16, 185, 129) : Color.FromArgb(5, 150, 105);      // Emerald
        public static Color CorAviso => ModoEscuro ? Color.FromArgb(245, 158, 11) : Color.FromArgb(217, 119, 6);        // Amber
        public static Color CorPerigo => ModoEscuro ? Color.FromArgb(239, 68, 68) : Color.FromArgb(220, 38, 38);        // Rose
        public static Color CorRoxo => ModoEscuro ? Color.FromArgb(168, 85, 247) : Color.FromArgb(126, 34, 206);        // Purple

        public static void CarregarTemaSalvo()
        {
            try
            {
                if (System.IO.File.Exists(CaminhoArquivoConfig))
                {
                    string conteudo = System.IO.File.ReadAllText(CaminhoArquivoConfig).Trim();
                    ModoEscuro = bool.TryParse(conteudo, out bool escuro) && escuro;
                }
            }
            catch { }
        }

        public static void SalvarTemaAtual()
        {
            try
            {
                System.IO.File.WriteAllText(CaminhoArquivoConfig, ModoEscuro.ToString());
            }
            catch { }
        }

        public static void AlternarTema(Form formAtivo = null)
        {
            ModoEscuro = !ModoEscuro;
            SalvarTemaAtual();

            if (formAtivo != null)
            {
                AplicarTema(formAtivo);
            }
        }

        /// <summary>
        /// Aplica o tema na tela e em todos os seus controles filhos recursivamente,
        /// aplicando Segoe UI, ícones vetoriais, cantos arredondados e bordas suaves.
        /// </summary>
        public static void AplicarTema(Control controle)
        {
            if (controle == null) return;

            Color corFundo = ModoEscuro ? FundoEscuro : FundoClaro;
            Color corCard = ModoEscuro ? FundoCardEscuro : FundoCardClaro;
            Color corTexto = ModoEscuro ? TextoEscuro : TextoClaro;
            Color corInput = ModoEscuro ? InputEscuro : InputClaro;
            Color corBotao = ModoEscuro ? FundoControleEscuro : FundoControleClaro;
            Color corBorda = ModoEscuro ? BordaEscura : BordaClara;

            PadronizarFonte(controle);
            MonitorarControlesDinamicos(controle);

            // 1. SplitContainer
            if (controle is SplitContainer split)
            {
                split.BackColor = corBorda;
                split.ForeColor = corTexto;
                AplicarTema(split.Panel1);
                AplicarTema(split.Panel2);
                return;
            }

            // 2. Containers / Formulários
            if (controle is Form || controle is FlowLayoutPanel || controle is TableLayoutPanel || controle is SplitterPanel)
            {
                controle.BackColor = corFundo;
                controle.ForeColor = corTexto;
            }
            else if (controle is UserControl uc)
            {
                bool ehCard = uc.GetType().Name.Contains("Card") || uc.Tag?.ToString() == "card";
                uc.BackColor = ehCard ? corCard : corFundo;
                uc.ForeColor = corTexto;

                if (ehCard)
                {
                    uc.BorderStyle = BorderStyle.None;
                    ConfigurarCardModerno(uc, 10);
                }
            }
            else if (controle is Panel pnl)
            {
                string nome = pnl.Name ?? "";
                if (nome.IndexOf("Sidebar", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    pnl.BackColor = ModoEscuro ? FundoSidebarEscuro : FundoSidebarClaro;
                    pnl.ForeColor = corTexto;
                }
                else if (nome.IndexOf("Topo", StringComparison.OrdinalIgnoreCase) >= 0 ||
                         nome.IndexOf("Status", StringComparison.OrdinalIgnoreCase) >= 0 ||
                         nome.IndexOf("Title", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    pnl.BackColor = ModoEscuro ? BarraSuperiorEscura : BarraSuperiorClara;
                    pnl.ForeColor = corTexto;
                }
                else if (nome.IndexOf("Card", StringComparison.OrdinalIgnoreCase) >= 0 ||
                         nome.IndexOf("Wrapper", StringComparison.OrdinalIgnoreCase) >= 0 ||
                         pnl.Tag?.ToString() == "card")
                {
                    pnl.BackColor = corCard;
                    pnl.ForeColor = corTexto;
                    pnl.BorderStyle = BorderStyle.None;
                    ConfigurarCardModerno(pnl, 10);
                }
                else if (pnl.Height <= 2)
                {
                    // Linha divisória horizontal fina
                    pnl.BackColor = corBorda;
                }
                else
                {
                    pnl.BackColor = corFundo;
                    pnl.ForeColor = corTexto;
                }
            }
            else if (controle is GroupBox grp)
            {
                grp.BackColor = corCard;
                grp.ForeColor = corTexto;
                ConfigurarGroupBoxModerno(grp);
            }
            else if (controle is TabPage page)
            {
                page.BackColor = corFundo;
                page.ForeColor = corTexto;
            }
            else if (controle is TabControl tab)
            {
                EstilizarTabControl(tab);
            }

            // 3. Estiliza controles individuais
            EstilizarControleIndividual(controle, corFundo, corCard, corTexto, corInput, corBotao, corBorda);

            // 4. Aplica recursivamente nos filhos
            foreach (Control filho in controle.Controls)
            {
                AplicarTema(filho);
            }

            controle.Invalidate();
        }

        private static void PadronizarFonte(Control ctrl)
        {
            try
            {
                if (ctrl.Font != null && ctrl.Font.FontFamily.Name != "Segoe UI")
                {
                    ctrl.Font = new Font("Segoe UI", ctrl.Font.Size, ctrl.Font.Style);
                }
            }
            catch { }
        }

        private static void EstilizarControleIndividual(Control ctrl, Color corFundo, Color corCard, Color corTexto, Color corInput, Color corBotao, Color corBorda)
        {
            if (ctrl is LinkLabel link)
            {
                link.LinkColor = ModoEscuro ? Color.FromArgb(96, 165, 250) : Color.FromArgb(37, 99, 235);
                link.ActiveLinkColor = ModoEscuro ? Color.FromArgb(147, 197, 253) : Color.FromArgb(29, 78, 216);
                link.VisitedLinkColor = ModoEscuro ? Color.FromArgb(167, 139, 250) : Color.FromArgb(109, 40, 217);
            }
            else if (ctrl is Label lbl)
            {
                if (lbl.Name == "lblAvatar")
                {
                    ConfigurarAvatarCircular(lbl);
                    return;
                }

                if (lbl.Tag?.ToString() == "subtitulo" || lbl.Name.StartsWith("lblDesc"))
                {
                    lbl.ForeColor = ModoEscuro ? TextoSecundarioEscuro : TextoSecundarioClaro;
                    lbl.BackColor = Color.Transparent;
                    return;
                }

                // Soft Badges modernos de estoque
                string badgeTag = lbl.Tag?.ToString() ?? "";
                if (lbl.Name == "lblEstoque" || badgeTag.StartsWith("badge_") ||
                    lbl.BackColor == Color.PaleGreen || lbl.BackColor == Color.LightCoral || lbl.BackColor == Color.LightSalmon)
                {
                    EstilizarBadgeEstoque(lbl);
                    return;
                }

                if (lbl.Name == "lblCodigo")
                {
                    lbl.BackColor = ModoEscuro ? Color.FromArgb(45, 50, 62) : Color.FromArgb(226, 232, 240);
                    lbl.ForeColor = ModoEscuro ? Color.FromArgb(147, 197, 253) : Color.FromArgb(30, 64, 175);
                    lbl.Font = new Font("Segoe UI", lbl.Font.Size, FontStyle.Bold);
                    return;
                }

                if (lbl.Name == "lblPrecoVenda" || lbl.Name == "lblPrecoTotal")
                {
                    lbl.ForeColor = CorSucesso;
                    lbl.Font = new Font("Segoe UI", lbl.Font.Size, FontStyle.Bold);
                    return;
                }

                if (!EhCorDeDestaqueSemantica(lbl.ForeColor))
                {
                    lbl.ForeColor = corTexto;
                }

                if (lbl.BackColor == SystemColors.Window || lbl.BackColor == SystemColors.Control || lbl.BackColor == Color.White || lbl.BackColor == SystemColors.GradientInactiveCaption)
                {
                    lbl.BackColor = Color.Transparent;
                }
            }
            else if (ctrl is TextBox txt)
            {
                if (txt.ReadOnly && txt.Multiline)
                {
                    txt.BackColor = corCard;
                    txt.ForeColor = CorAviso;
                    txt.BorderStyle = BorderStyle.None;
                    return;
                }

                if (txt.Name == "searchTxtBox")
                {
                    txt.BackColor = corCard;
                    txt.ForeColor = corTexto;
                    txt.BorderStyle = BorderStyle.None;
                    return;
                }

                bool ehPlaceholder = txt.Text == "Insira seu nome de usuário" || txt.Text == "Insira sua senha";
                txt.BackColor = corInput;
                txt.ForeColor = ehPlaceholder ? (ModoEscuro ? Color.FromArgb(120, 125, 140) : Color.FromArgb(160, 174, 192)) : corTexto;
                txt.BorderStyle = BorderStyle.FixedSingle;
                ConfigurarFocoTextBox(txt);
            }
            else if (ctrl is ComboBox cmb)
            {
                cmb.BackColor = corInput;
                cmb.ForeColor = corTexto;
                cmb.FlatStyle = FlatStyle.Flat;
            }
            else if (ctrl is MaskedTextBox mtxt)
            {
                mtxt.BackColor = corInput;
                mtxt.ForeColor = corTexto;
                mtxt.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (ctrl is NumericUpDown nud)
            {
                nud.BackColor = corInput;
                nud.ForeColor = corTexto;
                nud.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (ctrl is ListBox lb)
            {
                lb.BackColor = corCard;
                lb.ForeColor = corTexto;
                lb.BorderStyle = BorderStyle.None;
            }
            else if (ctrl is CheckBox || ctrl is RadioButton)
            {
                ctrl.ForeColor = corTexto;
            }
            else if (ctrl is Button btn)
            {
                EstilizarBotao(btn, corTexto, corBotao, corBorda);
            }
            else if (ctrl is DataGridView dgv)
            {
                EstilizarDataGridView(dgv, corFundo, corCard, corTexto, corInput, corBotao, corBorda);
            }
            else if (ctrl is TabControl tab)
            {
                EstilizarTabControl(tab);
            }
        }

        public static void EstilizarBadgeEstoque(Label lbl)
        {
            if (lbl == null) return;

            string tag = lbl.Tag?.ToString() ?? "";
            if (string.IsNullOrEmpty(tag))
            {
                if (lbl.BackColor == Color.LightCoral) tag = "badge_err";
                else if (lbl.BackColor == Color.LightSalmon) tag = "badge_warn";
                else tag = "badge_ok";
                lbl.Tag = tag;
            }

            if (tag == "badge_err")
            {
                lbl.BackColor = ModoEscuro ? Color.FromArgb(65, 24, 34) : Color.FromArgb(254, 226, 226);
                lbl.ForeColor = ModoEscuro ? Color.FromArgb(252, 165, 165) : Color.FromArgb(185, 28, 28);
            }
            else if (tag == "badge_warn")
            {
                lbl.BackColor = ModoEscuro ? Color.FromArgb(66, 42, 17) : Color.FromArgb(254, 243, 199);
                lbl.ForeColor = ModoEscuro ? Color.FromArgb(252, 211, 77) : Color.FromArgb(180, 83, 9);
            }
            else
            {
                lbl.BackColor = ModoEscuro ? Color.FromArgb(16, 56, 44) : Color.FromArgb(209, 250, 229);
                lbl.ForeColor = ModoEscuro ? Color.FromArgb(110, 231, 183) : Color.FromArgb(4, 120, 87);
            }

            lbl.Font = new Font("Segoe UI", 8.75F, FontStyle.Bold);
        }

        private static void ConfigurarFocoTextBox(TextBox txt)
        {
            int hash = txt.GetHashCode();
            if (ControlesComPaintCustom.Contains(hash)) return;
            ControlesComPaintCustom.Add(hash);

            txt.Enter += (s, e) =>
            {
                txt.BackColor = ModoEscuro ? InputFocoEscuro : InputFocoClaro;
            };
            txt.Leave += (s, e) =>
            {
                txt.BackColor = ModoEscuro ? InputEscuro : InputClaro;
            };
        }

        private static void ConfigurarAvatarCircular(Label lbl)
        {
            lbl.BackColor = Color.Transparent;
            lbl.ForeColor = Color.White;

            int hash = lbl.GetHashCode();
            if (ControlesComPaintCustom.Contains(hash)) return;
            ControlesComPaintCustom.Add(hash);

            lbl.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.Clear(lbl.Parent?.BackColor ?? (ModoEscuro ? FundoCardEscuro : FundoCardClaro));

                Rectangle rect = new Rectangle(1, 1, lbl.Width - 3, lbl.Height - 3);
                using (SolidBrush brush = new SolidBrush(CorDestaque))
                {
                    e.Graphics.FillEllipse(brush, rect);
                }

                TextRenderer.DrawText(
                    e.Graphics,
                    lbl.Text,
                    new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    lbl.ClientRectangle,
                    Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );
            };
        }

        private static void ConfigurarCardModerno(Control card, int raio)
        {
            int hash = card.GetHashCode();
            if (ControlesComPaintCustom.Contains(hash)) return;
            ControlesComPaintCustom.Add(hash);

            Action atualizarRegiao = () =>
            {
                if (card.Width <= 0 || card.Height <= 0) return;
                using (GraphicsPath path = CriarCaminhoArredondado(new Rectangle(0, 0, card.Width, card.Height), raio))
                {
                    card.Region = new Region(path);
                }
            };

            atualizarRegiao();
            card.Resize += (s, e) =>
            {
                atualizarRegiao();
                card.Invalidate();
            };

            card.Paint += (s, e) =>
            {
                if (card.Width <= 2 || card.Height <= 2) return;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                bool emHover = card.Tag?.ToString() == "card_hover";
                Color corBorda = emHover ? CorDestaque : (ModoEscuro ? BordaEscura : BordaClara);
                float espessura = emHover ? 2.0f : 1.2f;

                Rectangle rectBorda = new Rectangle(1, 1, card.Width - 3, card.Height - 3);
                using (GraphicsPath path = CriarCaminhoArredondado(rectBorda, raio))
                using (Pen pen = new Pen(corBorda, espessura))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            };
        }

        private static void ConfigurarGroupBoxModerno(GroupBox grp)
        {
            int hash = grp.GetHashCode();
            if (ControlesComPaintCustom.Contains(hash)) return;
            ControlesComPaintCustom.Add(hash);

            grp.Paint += (s, e) =>
            {
                if (grp.Width <= 2 || grp.Height <= 2) return;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Color corFundoPai = grp.Parent?.BackColor ?? (ModoEscuro ? FundoEscuro : FundoClaro);
                Color corCard = ModoEscuro ? FundoCardEscuro : FundoCardClaro;
                Color corBorda = ModoEscuro ? BordaEscura : BordaClara;
                Color corTitulo = ModoEscuro ? TextoSecundarioEscuro : TextoSecundarioClaro;

                e.Graphics.Clear(corFundoPai);

                Rectangle rectCard = new Rectangle(1, 1, grp.Width - 3, grp.Height - 3);
                using (GraphicsPath path = CriarCaminhoArredondado(rectCard, 10))
                using (SolidBrush brush = new SolidBrush(corCard))
                using (Pen pen = new Pen(corBorda, 1.2f))
                {
                    e.Graphics.FillPath(brush, path);
                    e.Graphics.DrawPath(pen, path);
                }

                // Accent Bar superior nos cards de KPI do Financeiro
                Color? corAccent = ObterCorAccentGroupBox(grp.Name);
                if (corAccent.HasValue)
                {
                    using (SolidBrush accentBrush = new SolidBrush(corAccent.Value))
                    {
                        e.Graphics.FillRectangle(accentBrush, new Rectangle(2, 2, grp.Width - 5, 4));
                    }
                }

                if (!string.IsNullOrWhiteSpace(grp.Text))
                {
                    TextRenderer.DrawText(
                        e.Graphics,
                        grp.Text.ToUpperInvariant(),
                        new Font("Segoe UI", 8.5F, FontStyle.Bold),
                        new Point(12, corAccent.HasValue ? 10 : 8),
                        corTitulo
                    );
                }
            };
        }

        private static Color? ObterCorAccentGroupBox(string nome)
        {
            if (string.IsNullOrEmpty(nome)) return null;
            if (nome.Contains("Custo")) return CorDestaque;
            if (nome.Contains("Receita")) return Color.FromArgb(99, 102, 241); // Indigo
            if (nome.Contains("Lucro")) return CorSucesso;
            if (nome.Contains("Margem")) return CorRoxo;
            return null;
        }

        private static void EstilizarBotao(Button btn, Color corTexto, Color corBotao, Color corBorda)
        {
            string tag = btn.Tag?.ToString() ?? "";
            string nome = btn.Name ?? "";
            string texto = btn.Text?.Trim().ToLowerInvariant() ?? "";

            btn.Cursor = Cursors.Hand;

            // Aplica ícone vetorial se aplicável
            AtribuirIconeBotao(btn, tag, nome, texto, corTexto);

            // 1. Botão de navegação ativa da sidebar ou aba ativa
            if (tag == "nav_ativa" || tag == "aba_ativa")
            {
                btn.BackColor = CorDestaque;
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = ModoEscuro ? Color.FromArgb(96, 165, 250) : Color.FromArgb(29, 78, 216);
                AtribuirIconeBotao(btn, tag, nome, texto, Color.White);
                ConfigurarIndicadorSidebar(btn);
                return;
            }

            // 2. Botão de navegação inativa da sidebar
            if (tag == "nav_item")
            {
                Color fg = ModoEscuro ? Color.FromArgb(203, 213, 225) : Color.FromArgb(51, 65, 85);
                btn.BackColor = ModoEscuro ? FundoSidebarEscuro : FundoSidebarClaro;
                btn.ForeColor = fg;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = ModoEscuro ? Color.FromArgb(42, 46, 57) : Color.FromArgb(241, 245, 249);
                AtribuirIconeBotao(btn, tag, nome, texto, fg);
                ConfigurarIndicadorSidebar(btn);
                return;
            }

            // 3. Tiles de forma de pagamento (selecionado vs normal)
            if (tag == "pagamento_ativo")
            {
                btn.BackColor = ModoEscuro ? Color.FromArgb(30, 58, 138) : Color.FromArgb(219, 234, 254);
                btn.ForeColor = ModoEscuro ? Color.White : Color.FromArgb(30, 64, 175);
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 2;
                btn.FlatAppearance.BorderColor = CorDestaque;
                btn.FlatAppearance.MouseOverBackColor = btn.BackColor;
                AtribuirIconeBotao(btn, tag, nome, texto, CorDestaque);
                return;
            }

            if (tag == "pagamento_item")
            {
                btn.BackColor = ModoEscuro ? FundoCardEscuro : FundoCardClaro;
                btn.ForeColor = corTexto;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = corBorda;
                btn.FlatAppearance.MouseOverBackColor = ModoEscuro ? FundoCardHoverEscuro : FundoCardHoverClaro;
                AtribuirIconeBotao(btn, tag, nome, texto, CorDestaque);
                return;
            }

            // 4. Botão de Sucesso / Pagamento / Salvar / Adicionar
            if (tag == "sucesso" || texto.Contains("avançar") || texto.Contains("finalizar") ||
                texto.Contains("salvar") || texto.Contains("confirmar") || nome == "btnAdicionar" || nome == "btnAdicionarPagamento")
            {
                btn.BackColor = CorSucesso;
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = ModoEscuro ? Color.FromArgb(52, 211, 153) : Color.FromArgb(4, 120, 87);
                btn.Font = new Font("Segoe UI", btn.Font.Size, FontStyle.Bold);
                AtribuirIconeBotao(btn, tag, nome, texto, Color.White);
                return;
            }

            // 5. Botão Primário (Login / Pesquisar / Novo / Atualizar / Cadastrar)
            if (tag == "primary" || texto == "login" || texto == "entrar" || texto.Contains("pesquisar") ||
                texto.Contains("buscar") || texto.Contains("atualizar") || texto.Contains("filtrar") ||
                nome == "btnCadastrar" || nome == "btnAdicionarCategoria" || nome == "btnAdicionarFornecedor")
            {
                btn.BackColor = CorDestaque;
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = ModoEscuro ? Color.FromArgb(96, 165, 250) : Color.FromArgb(29, 78, 216);
                AtribuirIconeBotao(btn, tag, nome, texto, Color.White);
                return;
            }

            // 6. Botão de Perigo / Excluir / Limpar / Sair
            if (tag == "danger" || texto.Contains("excluir") || texto.Contains("limpar") ||
                texto.Contains("sair") || texto.Contains("encerrar") || nome == "btnExcluir")
            {
                Color fgDanger = ModoEscuro ? Color.FromArgb(248, 113, 113) : Color.FromArgb(220, 38, 38);
                btn.BackColor = ModoEscuro ? Color.FromArgb(50, 26, 32) : Color.FromArgb(254, 242, 242);
                btn.ForeColor = fgDanger;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = ModoEscuro ? Color.FromArgb(127, 29, 29) : Color.FromArgb(254, 202, 202);
                btn.FlatAppearance.MouseOverBackColor = ModoEscuro ? Color.FromArgb(80, 30, 40) : Color.FromArgb(254, 226, 226);
                AtribuirIconeBotao(btn, tag, nome, texto, fgDanger);
                return;
            }

            // 7. Botão Neutro / Padrão Flat
            btn.BackColor = corBotao;
            btn.ForeColor = corTexto;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = corBorda;
            btn.FlatAppearance.MouseOverBackColor = ModoEscuro ? Color.FromArgb(56, 61, 76) : Color.FromArgb(226, 232, 240);
        }

        private static void ConfigurarIndicadorSidebar(Button btn)
        {
            int hash = btn.GetHashCode();
            if (ControlesComPaintCustom.Contains(hash)) return;
            ControlesComPaintCustom.Add(hash);

            btn.Paint += (s, e) =>
            {
                if (btn.Tag?.ToString() == "nav_ativa")
                {
                    using (SolidBrush b = new SolidBrush(Color.White))
                    {
                        e.Graphics.FillRectangle(b, new Rectangle(0, 8, 4, btn.Height - 16));
                    }
                }
            };
        }

        // =========================================================
        // GERADOR DE ÍCONES VETORIAIS GDI+ (SEM EMOJIS)
        // =========================================================
        private static void AtribuirIconeBotao(Button btn, string tag, string nome, string texto, Color corIcone)
        {
            string tipoIcone = IdentificarTipoIcone(nome, texto, tag);
            if (string.IsNullOrEmpty(tipoIcone)) return;

            int tamanho = (nome.StartsWith("btnDinheiro") || nome.StartsWith("btnDebito") ||
                           nome.StartsWith("btnCredito") || nome.StartsWith("btnPix") || nome.StartsWith("btnCarne")) ? 20 : 16;

            btn.Image = ObterIconeVetorial(tipoIcone, corIcone, tamanho);
            btn.ImageAlign = ContentAlignment.MiddleLeft;
            btn.TextImageRelation = TextImageRelation.ImageBeforeText;
            if (btn.Padding.Left < 8)
            {
                btn.Padding = new Padding(8, btn.Padding.Top, btn.Padding.Right, btn.Padding.Bottom);
            }
        }

        private static string IdentificarTipoIcone(string nome, string texto, string tag)
        {
            if (nome == "btnNavPrincipal") return "dashboard";
            if (nome == "btnNavVendas") return "cart";
            if (nome == "btnNavEstoque") return "box";
            if (nome == "btnNavFinanceiro") return "chart";
            if (nome == "btnNavCadastro") return "users";
            if (nome == "btnNavConfig") return "sliders";
            if (nome == "btnToggleTema" || nome == "btnAlternarTema") return "theme";
            if (nome == "btnSair") return "logout";

            if (nome == "btnDinheiro") return "cash";
            if (nome == "btnDebito" || nome == "btnCredito") return "card";
            if (nome == "btnPix") return "pix";
            if (nome == "btnCarne") return "invoice";
            if (nome == "btnAdicionar" || nome == "btnAdicionarPagamento" || nome == "btnCadastrar") return "plus";
            if (nome == "btnSalvar") return "check";
            if (nome == "searchBtn") return "search";
            if (nome == "btnAtualizar") return "refresh";
            if (nome == "btnExportarCsv") return "download";

            return null;
        }

        public static Bitmap ObterIconeVetorial(string tipo, Color cor, int size = 16)
        {
            string chave = $"{tipo}_{cor.ToArgb()}_{size}";
            if (CacheIcones.TryGetValue(chave, out Bitmap cached))
                return cached;

            Bitmap bmp = new Bitmap(size + 4, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (Pen pen = new Pen(cor, 1.7f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
                using (SolidBrush brush = new SolidBrush(cor))
                {
                    switch (tipo)
                    {
                        case "dashboard":
                            g.DrawRectangle(pen, 2, 2, 5, 5);
                            g.DrawRectangle(pen, 9, 2, 5, 5);
                            g.DrawRectangle(pen, 2, 9, 5, 5);
                            g.DrawRectangle(pen, 9, 9, 5, 5);
                            break;

                        case "cart":
                            g.DrawLines(pen, new[] { new Point(1, 2), new Point(3, 2), new Point(5, 10), new Point(13, 10), new Point(15, 5), new Point(4, 5) });
                            g.FillEllipse(brush, 5, 12, 2, 2);
                            g.FillEllipse(brush, 11, 12, 2, 2);
                            break;

                        case "box":
                            g.DrawPolygon(pen, new[] { new Point(8, 1), new Point(14, 4), new Point(14, 11), new Point(8, 14), new Point(2, 11), new Point(2, 4) });
                            g.DrawLines(pen, new[] { new Point(2, 4), new Point(8, 7), new Point(14, 4) });
                            g.DrawLine(pen, 8, 7, 8, 14);
                            break;

                        case "chart":
                            g.DrawLine(pen, 2, 14, 14, 14);
                            g.DrawLine(pen, 4, 14, 4, 9);
                            g.DrawLine(pen, 8, 14, 8, 6);
                            g.DrawLine(pen, 12, 14, 12, 3);
                            break;

                        case "users":
                            g.DrawEllipse(pen, 5, 2, 6, 5);
                            g.DrawArc(pen, 2, 8, 12, 7, 180, 180);
                            break;

                        case "sliders":
                            g.DrawLine(pen, 2, 4, 14, 4);
                            g.DrawLine(pen, 2, 8, 14, 8);
                            g.DrawLine(pen, 2, 12, 14, 12);
                            g.FillEllipse(brush, 4, 2, 4, 4);
                            g.FillEllipse(brush, 9, 6, 4, 4);
                            g.FillEllipse(brush, 5, 10, 4, 4);
                            break;

                        case "theme":
                            g.DrawEllipse(pen, 4, 4, 8, 8);
                            g.DrawLine(pen, 8, 1, 8, 2);
                            g.DrawLine(pen, 8, 14, 8, 15);
                            g.DrawLine(pen, 1, 8, 2, 8);
                            g.DrawLine(pen, 14, 8, 15, 8);
                            break;

                        case "logout":
                            g.DrawLines(pen, new[] { new Point(8, 2), new Point(3, 2), new Point(3, 14), new Point(8, 14) });
                            g.DrawLine(pen, 6, 8, 14, 8);
                            g.DrawLines(pen, new[] { new Point(11, 5), new Point(14, 8), new Point(11, 11) });
                            break;

                        case "cash":
                            g.DrawRectangle(pen, 2, 4, size - 4, size - 8);
                            g.DrawEllipse(pen, (size / 2) - 2, (size / 2) - 2, 4, 4);
                            break;

                        case "card":
                            g.DrawRectangle(pen, 2, 4, size - 4, size - 8);
                            g.DrawLine(pen, 2, 8, size - 2, 8);
                            g.FillRectangle(brush, 4, 11, 4, 2);
                            break;

                        case "pix":
                            int m = size / 2;
                            g.DrawPolygon(pen, new[] { new Point(m, 2), new Point(size - 2, m), new Point(m, size - 2), new Point(2, m) });
                            g.DrawEllipse(pen, m - 2, m - 2, 4, 4);
                            break;

                        case "invoice":
                            g.DrawRectangle(pen, 3, 2, size - 6, size - 4);
                            g.DrawLine(pen, 6, 6, size - 6, 6);
                            g.DrawLine(pen, 6, 10, size - 6, 10);
                            g.DrawLine(pen, 6, 14, size - 9, 14);
                            break;

                        case "plus":
                            int c = size / 2;
                            g.DrawLine(pen, c, 3, c, size - 3);
                            g.DrawLine(pen, 3, c, size - 3, c);
                            break;

                        case "check":
                            g.DrawLines(pen, new[] { new Point(3, 8), new Point(7, 12), new Point(13, 4) });
                            break;

                        case "search":
                            g.DrawEllipse(pen, 2, 2, 9, 9);
                            g.DrawLine(pen, 10, 10, 14, 14);
                            break;

                        case "refresh":
                            g.DrawArc(pen, 2, 2, 12, 12, -30, 300);
                            g.DrawLines(pen, new[] { new Point(11, 2), new Point(14, 5), new Point(10, 6) });
                            break;

                        case "download":
                            g.DrawLine(pen, 8, 2, 8, 11);
                            g.DrawLines(pen, new[] { new Point(5, 8), new Point(8, 11), new Point(11, 8) });
                            g.DrawLine(pen, 3, 14, 13, 14);
                            break;
                    }
                }
            }

            CacheIcones[chave] = bmp;
            return bmp;
        }

        private static GraphicsPath CriarCaminhoArredondado(Rectangle rect, int raio)
        {
            GraphicsPath path = new GraphicsPath();
            int diametro = raio * 2;
            if (diametro <= 0 || rect.Width < diametro || rect.Height < diametro)
            {
                path.AddRectangle(rect);
                return path;
            }

            path.AddArc(rect.X, rect.Y, diametro, diametro, 180, 90);
            path.AddArc(rect.Right - diametro, rect.Y, diametro, diametro, 270, 90);
            path.AddArc(rect.Right - diametro, rect.Bottom - diametro, diametro, diametro, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diametro, diametro, diametro, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static bool EhCorDeDestaqueSemantica(Color cor)
        {
            return cor == Color.ForestGreen ||
                   cor == Color.LimeGreen ||
                   cor == Color.Green ||
                   cor == Color.Crimson ||
                   cor == Color.Red ||
                   cor == Color.DarkOrange ||
                   cor == CorSucesso ||
                   cor == CorPerigo ||
                   cor == CorDestaque;
        }

        private static void EstilizarDataGridView(DataGridView dgv, Color corFundo, Color corCard, Color corTexto, Color corInput, Color corBotao, Color corBorda)
        {
            Color corZebra = ModoEscuro ? LinhaAlternadaEscura : LinhaAlternadaClara;

            dgv.BackgroundColor = corCard;
            dgv.GridColor = corBorda;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowHeadersVisible = false;

            // Mais espaçamento vertical (respiro estilo Dashboard)
            dgv.RowTemplate.Height = 36;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = 40;

            // Cabeçalho
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = corBotao;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = corTexto;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = corBotao;
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = corTexto;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 4, 8, 4);

            // Linhas normais
            dgv.DefaultCellStyle.BackColor = corInput;
            dgv.DefaultCellStyle.ForeColor = corTexto;
            dgv.DefaultCellStyle.SelectionBackColor = ModoEscuro ? Color.FromArgb(30, 58, 138) : Color.FromArgb(219, 234, 254);
            dgv.DefaultCellStyle.SelectionForeColor = ModoEscuro ? Color.White : Color.FromArgb(30, 64, 175);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.25F, FontStyle.Regular);
            dgv.DefaultCellStyle.Padding = new Padding(8, 2, 8, 2);

            // Linhas alternadas (Zebra)
            dgv.AlternatingRowsDefaultCellStyle.BackColor = corZebra;
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = corTexto;
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = ModoEscuro ? Color.FromArgb(30, 58, 138) : Color.FromArgb(219, 234, 254);
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = ModoEscuro ? Color.White : Color.FromArgb(30, 64, 175);
            dgv.AlternatingRowsDefaultCellStyle.Font = new Font("Segoe UI", 9.25F, FontStyle.Regular);
            dgv.AlternatingRowsDefaultCellStyle.Padding = new Padding(8, 2, 8, 2);
        }

        private static void EstilizarTabControl(TabControl tab)
        {
            tab.DrawMode = TabDrawMode.OwnerDrawFixed;
            tab.DrawItem -= TabControl_DrawItem;
            tab.DrawItem += TabControl_DrawItem;
            tab.Invalidate();
        }

        private static void TabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (!(sender is TabControl tab)) return;

            bool selecionada = (e.Index == tab.SelectedIndex);
            Color corFundoAba = selecionada
                ? (ModoEscuro ? CorDestaque : Color.White)
                : (ModoEscuro ? FundoCardEscuro : FundoControleClaro);

            Color corTextoAba = selecionada
                ? (ModoEscuro ? Color.White : TextoClaro)
                : (ModoEscuro ? TextoSecundarioEscuro : TextoSecundarioClaro);

            using (SolidBrush brushFundo = new SolidBrush(corFundoAba))
            {
                e.Graphics.FillRectangle(brushFundo, e.Bounds);
            }

            string titulo = tab.TabPages[e.Index].Text;
            TextRenderer.DrawText(
                e.Graphics,
                titulo,
                new Font("Segoe UI", tab.Font.Size, selecionada ? FontStyle.Bold : FontStyle.Regular),
                e.Bounds,
                corTextoAba,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
            );
        }

        private static void MonitorarControlesDinamicos(Control container)
        {
            int hash = container.GetHashCode();
            if (ControlesMonitorados.Contains(hash)) return;

            ControlesMonitorados.Add(hash);
            container.ControlAdded += (s, e) =>
            {
                if (e.Control != null)
                {
                    AplicarTema(e.Control);
                }
            };
        }
    }
}
