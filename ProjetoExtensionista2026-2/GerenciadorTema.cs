using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public class GerenciadorTema
    {
        private static readonly string CaminhoArquivoConfig = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config_tema.txt");
        private static readonly HashSet<int> ControlesMonitorados = new HashSet<int>();

        public static bool ModoEscuro { get; set; } = false;

        // =========================================================
        // PALETA SLATE & INDIGO (MODO ESCURO)
        // =========================================================
        public static readonly Color FundoEscuro = Color.FromArgb(24, 26, 32);              // #181A20 - Fundo principal
        public static readonly Color FundoCardEscuro = Color.FromArgb(34, 37, 46);          // #22252E - Cards, Painéis, GroupBoxes
        public static readonly Color FundoSidebarEscuro = Color.FromArgb(31, 34, 43);       // #1F222B - Sidebar dedicada
        public static readonly Color FundoControleEscuro = Color.FromArgb(42, 46, 57);      // #2A2E39 - Botões secundários
        public static readonly Color InputEscuro = Color.FromArgb(30, 33, 41);              // #1E2129 - TextBox, ComboBox, Linhas tabela
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
        public static readonly Color FundoSidebarClaro = Color.FromArgb(255, 255, 255);     // #FFFFFF - Sidebar branca limpa
        public static readonly Color FundoControleClaro = Color.FromArgb(241, 245, 249);    // #F1F5F9 - Botões secundários claros
        public static readonly Color InputClaro = Color.FromArgb(255, 255, 255);            // #FFFFFF - Inputs brancos
        public static readonly Color LinhaAlternadaClara = Color.FromArgb(248, 250, 252);   // #F8FAFC - Zebra DataGridView
        public static readonly Color BordaClara = Color.FromArgb(226, 232, 240);            // #E2E8F0 - Bordas limpas
        public static readonly Color TextoClaro = Color.FromArgb(15, 23, 42);               // #0F172A - Texto chumbo/preto legível
        public static readonly Color TextoSecundarioClaro = Color.FromArgb(100, 116, 139);  // #64748B - Subtítulos neutros
        public static readonly Color BarraSuperiorClara = Color.FromArgb(226, 232, 240);    // #E2E8F0 - Barra de título/status

        // Cores de Destaque
        public static Color CorDestaque => ModoEscuro ? Color.FromArgb(59, 130, 246) : Color.FromArgb(37, 99, 235);     // Indigo
        public static Color CorSucesso => ModoEscuro ? Color.FromArgb(16, 185, 129) : Color.FromArgb(5, 150, 105);      // Emerald
        public static Color CorPerigo => ModoEscuro ? Color.FromArgb(239, 68, 68) : Color.FromArgb(220, 38, 38);        // Rose

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
        /// aplicando Segoe UI e ajustando componentes individualmente.
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

            // Aplica fonte Segoe UI preservando tamanho e estilo
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
                bool ehCard = uc.GetType().Name.Contains("Card");
                uc.BackColor = ehCard ? corCard : corFundo;
                uc.ForeColor = corTexto;
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
                if (lbl.Tag?.ToString() == "subtitulo")
                {
                    lbl.ForeColor = ModoEscuro ? TextoSecundarioEscuro : TextoSecundarioClaro;
                    return;
                }

                // Preserva badges coloridos de estoque ou status
                if (lbl.BackColor == Color.PaleGreen || lbl.BackColor == Color.LightCoral || lbl.BackColor == Color.LightSalmon)
                {
                    lbl.ForeColor = Color.FromArgb(20, 20, 20);
                    return;
                }

                if (lbl.Name == "lblCodigo")
                {
                    lbl.BackColor = ModoEscuro ? Color.FromArgb(45, 50, 62) : Color.FromArgb(226, 232, 240);
                    lbl.ForeColor = corTexto;
                    return;
                }

                if (EhCorDeDestaqueSemantica(lbl.ForeColor))
                {
                    return;
                }

                lbl.ForeColor = corTexto;
            }
            else if (ctrl is TextBox txt)
            {
                bool ehPlaceholder = txt.Text == "Insira seu nome de usuário" || txt.Text == "Insira sua senha";
                txt.BackColor = corInput;
                txt.ForeColor = ehPlaceholder ? (ModoEscuro ? Color.FromArgb(120, 125, 140) : Color.FromArgb(160, 174, 192)) : corTexto;
                txt.BorderStyle = BorderStyle.FixedSingle;
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

        private static void EstilizarBotao(Button btn, Color corTexto, Color corBotao, Color corBorda)
        {
            string tag = btn.Tag?.ToString() ?? "";
            string texto = btn.Text?.Trim().ToLowerInvariant() ?? "";

            // Botão de navegação ativa da sidebar
            if (tag == "nav_ativa" || tag == "aba_ativa")
            {
                btn.BackColor = CorDestaque;
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = ModoEscuro ? Color.FromArgb(96, 165, 250) : Color.FromArgb(29, 78, 216);
                return;
            }

            // Botão de navegação inativa da sidebar
            if (tag == "nav_item")
            {
                btn.BackColor = ModoEscuro ? FundoSidebarEscuro : FundoSidebarClaro;
                btn.ForeColor = ModoEscuro ? Color.FromArgb(203, 213, 225) : Color.FromArgb(51, 65, 85);
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = ModoEscuro ? Color.FromArgb(42, 46, 57) : Color.FromArgb(241, 245, 249);
                return;
            }

            // Botão de Sucesso / Pagamento / Salvar
            if (tag == "sucesso" || texto.Contains("pagamento") || texto.Contains("salvar") || texto.Contains("confirmar"))
            {
                btn.BackColor = CorSucesso;
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = ModoEscuro ? Color.FromArgb(52, 211, 153) : Color.FromArgb(4, 120, 87);
                return;
            }

            // Botão Primário (Login / Pesquisar / Novo)
            if (tag == "primary" || texto == "login" || texto == "entrar" || texto.Contains("pesquisar") || texto.Contains("buscar"))
            {
                btn.BackColor = CorDestaque;
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = ModoEscuro ? Color.FromArgb(96, 165, 250) : Color.FromArgb(29, 78, 216);
                return;
            }

            // Botão de Perigo / Excluir / Limpar / Sair
            if (tag == "danger" || texto.Contains("excluir") || texto.Contains("limpar") || texto.Contains("sair") || texto.Contains("encerrar"))
            {
                btn.BackColor = ModoEscuro ? Color.FromArgb(50, 26, 32) : Color.FromArgb(254, 242, 242);
                btn.ForeColor = ModoEscuro ? Color.FromArgb(248, 113, 113) : Color.FromArgb(220, 38, 38);
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = ModoEscuro ? Color.FromArgb(127, 29, 29) : Color.FromArgb(254, 202, 202);
                btn.FlatAppearance.MouseOverBackColor = CorPerigo;
                return;
            }

            // Botão Neutro / Padrão Flat
            btn.BackColor = corBotao;
            btn.ForeColor = corTexto;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = corBorda;
            btn.FlatAppearance.MouseOverBackColor = ModoEscuro ? Color.FromArgb(56, 61, 76) : Color.FromArgb(226, 232, 240);
        }

        private static bool EhCorDeDestaqueSemantica(Color cor)
        {
            return cor == Color.ForestGreen ||
                   cor == Color.LimeGreen ||
                   cor == Color.Green ||
                   cor == Color.Crimson ||
                   cor == Color.Red ||
                   cor == Color.DarkOrange;
        }

        private static void EstilizarDataGridView(DataGridView dgv, Color corFundo, Color corCard, Color corTexto, Color corInput, Color corBotao, Color corBorda)
        {
            Color corZebra = ModoEscuro ? LinhaAlternadaEscura : LinhaAlternadaClara;

            dgv.BackgroundColor = corCard;
            dgv.GridColor = corBorda;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.EnableHeadersVisualStyles = false;

            // Cabeçalho
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = corBotao;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = corTexto;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = corBotao;
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = corTexto;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);

            // Linhas normais
            dgv.DefaultCellStyle.BackColor = corInput;
            dgv.DefaultCellStyle.ForeColor = corTexto;
            dgv.DefaultCellStyle.SelectionBackColor = CorDestaque;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            // Linhas alternadas (Zebra)
            dgv.AlternatingRowsDefaultCellStyle.BackColor = corZebra;
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = corTexto;
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = CorDestaque;
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
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
