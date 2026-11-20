namespace ProjetoExtensionista2026_2
{
    partial class FormRegistroVendas
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.btnAtualizar = new System.Windows.Forms.Button();
            this.btnExportarCsv = new System.Windows.Forms.Button();
            this.pnlKpis = new System.Windows.Forms.TableLayoutPanel();
            this.cardTotalVendido = new System.Windows.Forms.Panel();
            this.lblTituloTotalVendido = new System.Windows.Forms.Label();
            this.lblValorTotalVendido = new System.Windows.Forms.Label();
            this.lblSubTotalVendido = new System.Windows.Forms.Label();
            this.cardTicketMedio = new System.Windows.Forms.Panel();
            this.lblTituloTicketMedio = new System.Windows.Forms.Label();
            this.lblValorTicketMedio = new System.Windows.Forms.Label();
            this.lblSubTicketMedio = new System.Windows.Forms.Label();
            this.cardPendente = new System.Windows.Forms.Panel();
            this.lblTituloPendente = new System.Windows.Forms.Label();
            this.lblValorPendente = new System.Windows.Forms.Label();
            this.lblSubPendente = new System.Windows.Forms.Label();
            this.cardOrcamentos = new System.Windows.Forms.Panel();
            this.lblTituloOrcamentos = new System.Windows.Forms.Label();
            this.lblValorOrcamentos = new System.Windows.Forms.Label();
            this.lblSubOrcamentos = new System.Windows.Forms.Label();
            this.pnlFiltros = new System.Windows.Forms.Panel();
            this.lblPeriodo = new System.Windows.Forms.Label();
            this.pnlDtpInicio = new System.Windows.Forms.Panel();
            this.lblDtpInicio = new System.Windows.Forms.Label();
            this.dtpInicio = new System.Windows.Forms.DateTimePicker();
            this.lblAte = new System.Windows.Forms.Label();
            this.pnlDtpFim = new System.Windows.Forms.Panel();
            this.lblDtpFim = new System.Windows.Forms.Label();
            this.dtpFim = new System.Windows.Forms.DateTimePicker();
            this.btnHoje = new System.Windows.Forms.Button();
            this.btn7Dias = new System.Windows.Forms.Button();
            this.btn30Dias = new System.Windows.Forms.Button();
            this.lblTipo = new System.Windows.Forms.Label();
            this.cmbTipo = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblBusca = new System.Windows.Forms.Label();
            this.txtBusca = new System.Windows.Forms.TextBox();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.splitPrincipal = new System.Windows.Forms.SplitContainer();
            this.dgvVendas = new System.Windows.Forms.DataGridView();
            this.pnlDetalhesCard = new System.Windows.Forms.Panel();
            this.lblPagamentosTitulo = new System.Windows.Forms.Label();
            this.dgvPagamentos = new System.Windows.Forms.DataGridView();
            this.pnlAcoes = new System.Windows.Forms.TableLayoutPanel();
            this.btnReimprimir = new System.Windows.Forms.Button();
            this.btnEfetivar = new System.Windows.Forms.Button();
            this.btnQuitar = new System.Windows.Forms.Button();
            this.btnCancelarVenda = new System.Windows.Forms.Button();
            this.dgvItens = new System.Windows.Forms.DataGridView();
            this.lblItensTitulo = new System.Windows.Forms.Label();
            this.pnlDetalhesHeader = new System.Windows.Forms.Panel();
            this.lblDetalheNotas = new System.Windows.Forms.Label();
            this.lblDetalheMeta = new System.Windows.Forms.Label();
            this.lblDetalheTitulo = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.pnlKpis.SuspendLayout();
            this.cardTotalVendido.SuspendLayout();
            this.cardTicketMedio.SuspendLayout();
            this.cardPendente.SuspendLayout();
            this.cardOrcamentos.SuspendLayout();
            this.pnlFiltros.SuspendLayout();
            this.pnlDtpInicio.SuspendLayout();
            this.pnlDtpFim.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitPrincipal)).BeginInit();
            this.splitPrincipal.Panel1.SuspendLayout();
            this.splitPrincipal.Panel2.SuspendLayout();
            this.splitPrincipal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendas)).BeginInit();
            this.pnlDetalhesCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPagamentos)).BeginInit();
            this.pnlAcoes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).BeginInit();
            this.pnlDetalhesHeader.SuspendLayout();
            this.SuspendLayout();



            this.pnlHeader.Controls.Add(this.lblTitulo);
            this.pnlHeader.Controls.Add(this.btnAtualizar);
            this.pnlHeader.Controls.Add(this.btnExportarCsv);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1120, 60);
            this.pnlHeader.TabIndex = 0;



            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(20, 14);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(212, 30);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Registro de Vendas";



            this.btnAtualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAtualizar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAtualizar.Location = new System.Drawing.Point(810, 12);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(140, 36);
            this.btnAtualizar.TabIndex = 1;
            this.btnAtualizar.Tag = "primary";
            this.btnAtualizar.Text = "Atualizar";
            this.btnAtualizar.UseVisualStyleBackColor = true;
            this.btnAtualizar.Click += new System.EventHandler(this.btnAtualizar_Click);



            this.btnExportarCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportarCsv.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportarCsv.Location = new System.Drawing.Point(960, 12);
            this.btnExportarCsv.Name = "btnExportarCsv";
            this.btnExportarCsv.Size = new System.Drawing.Size(140, 36);
            this.btnExportarCsv.TabIndex = 2;
            this.btnExportarCsv.Text = "  Exportar CSV";
            this.btnExportarCsv.UseVisualStyleBackColor = true;
            this.btnExportarCsv.Click += new System.EventHandler(this.btnExportarCsv_Click);



            this.pnlKpis.ColumnCount = 4;
            this.pnlKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlKpis.Controls.Add(this.cardTotalVendido, 0, 0);
            this.pnlKpis.Controls.Add(this.cardTicketMedio, 1, 0);
            this.pnlKpis.Controls.Add(this.cardPendente, 2, 0);
            this.pnlKpis.Controls.Add(this.cardOrcamentos, 3, 0);
            this.pnlKpis.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlKpis.Location = new System.Drawing.Point(0, 60);
            this.pnlKpis.Name = "pnlKpis";
            this.pnlKpis.Padding = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.pnlKpis.RowCount = 1;
            this.pnlKpis.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlKpis.Size = new System.Drawing.Size(1120, 110);
            this.pnlKpis.TabIndex = 1;



            this.cardTotalVendido.Controls.Add(this.lblTituloTotalVendido);
            this.cardTotalVendido.Controls.Add(this.lblValorTotalVendido);
            this.cardTotalVendido.Controls.Add(this.lblSubTotalVendido);
            this.cardTotalVendido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardTotalVendido.Location = new System.Drawing.Point(20, 10);
            this.cardTotalVendido.Margin = new System.Windows.Forms.Padding(5);
            this.cardTotalVendido.Name = "cardTotalVendido";
            this.cardTotalVendido.Size = new System.Drawing.Size(262, 90);
            this.cardTotalVendido.TabIndex = 0;
            this.cardTotalVendido.Tag = "card|accent_success";



            this.lblTituloTotalVendido.AutoSize = true;
            this.lblTituloTotalVendido.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloTotalVendido.Location = new System.Drawing.Point(14, 12);
            this.lblTituloTotalVendido.Name = "lblTituloTotalVendido";
            this.lblTituloTotalVendido.Size = new System.Drawing.Size(100, 15);
            this.lblTituloTotalVendido.TabIndex = 0;
            this.lblTituloTotalVendido.Tag = "subtitulo";
            this.lblTituloTotalVendido.Text = "TOTAL VENDIDO";



            this.lblValorTotalVendido.AutoSize = true;
            this.lblValorTotalVendido.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorTotalVendido.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblValorTotalVendido.Location = new System.Drawing.Point(12, 34);
            this.lblValorTotalVendido.Name = "lblValorTotalVendido";
            this.lblValorTotalVendido.Size = new System.Drawing.Size(91, 30);
            this.lblValorTotalVendido.TabIndex = 1;
            this.lblValorTotalVendido.Text = "R$ 0,00";



            this.lblSubTotalVendido.AutoSize = true;
            this.lblSubTotalVendido.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubTotalVendido.Location = new System.Drawing.Point(14, 68);
            this.lblSubTotalVendido.Name = "lblSubTotalVendido";
            this.lblSubTotalVendido.Size = new System.Drawing.Size(160, 13);
            this.lblSubTotalVendido.TabIndex = 2;
            this.lblSubTotalVendido.Tag = "subtitulo";
            this.lblSubTotalVendido.Text = "Vendas efetivadas no período";



            this.cardTicketMedio.Controls.Add(this.lblTituloTicketMedio);
            this.cardTicketMedio.Controls.Add(this.lblValorTicketMedio);
            this.cardTicketMedio.Controls.Add(this.lblSubTicketMedio);
            this.cardTicketMedio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardTicketMedio.Location = new System.Drawing.Point(292, 10);
            this.cardTicketMedio.Margin = new System.Windows.Forms.Padding(5);
            this.cardTicketMedio.Name = "cardTicketMedio";
            this.cardTicketMedio.Size = new System.Drawing.Size(262, 90);
            this.cardTicketMedio.TabIndex = 1;
            this.cardTicketMedio.Tag = "card|accent_primary";



            this.lblTituloTicketMedio.AutoSize = true;
            this.lblTituloTicketMedio.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloTicketMedio.Location = new System.Drawing.Point(14, 12);
            this.lblTituloTicketMedio.Name = "lblTituloTicketMedio";
            this.lblTituloTicketMedio.Size = new System.Drawing.Size(88, 15);
            this.lblTituloTicketMedio.TabIndex = 0;
            this.lblTituloTicketMedio.Tag = "subtitulo";
            this.lblTituloTicketMedio.Text = "TICKET MÉDIO";



            this.lblValorTicketMedio.AutoSize = true;
            this.lblValorTicketMedio.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorTicketMedio.Location = new System.Drawing.Point(12, 34);
            this.lblValorTicketMedio.Name = "lblValorTicketMedio";
            this.lblValorTicketMedio.Size = new System.Drawing.Size(91, 30);
            this.lblValorTicketMedio.TabIndex = 1;
            this.lblValorTicketMedio.Text = "R$ 0,00";



            this.lblSubTicketMedio.AutoSize = true;
            this.lblSubTicketMedio.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubTicketMedio.Location = new System.Drawing.Point(14, 68);
            this.lblSubTicketMedio.Name = "lblSubTicketMedio";
            this.lblSubTicketMedio.Size = new System.Drawing.Size(112, 13);
            this.lblSubTicketMedio.TabIndex = 2;
            this.lblSubTicketMedio.Tag = "subtitulo";
            this.lblSubTicketMedio.Text = "0 vendas registradas";



            this.cardPendente.Controls.Add(this.lblTituloPendente);
            this.cardPendente.Controls.Add(this.lblValorPendente);
            this.cardPendente.Controls.Add(this.lblSubPendente);
            this.cardPendente.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardPendente.Location = new System.Drawing.Point(564, 10);
            this.cardPendente.Margin = new System.Windows.Forms.Padding(5);
            this.cardPendente.Name = "cardPendente";
            this.cardPendente.Size = new System.Drawing.Size(262, 90);
            this.cardPendente.TabIndex = 2;
            this.cardPendente.Tag = "card|accent_indigo";



            this.lblTituloPendente.AutoSize = true;
            this.lblTituloPendente.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloPendente.Location = new System.Drawing.Point(14, 12);
            this.lblTituloPendente.Name = "lblTituloPendente";
            this.lblTituloPendente.Size = new System.Drawing.Size(137, 15);
            this.lblTituloPendente.TabIndex = 0;
            this.lblTituloPendente.Tag = "subtitulo";
            this.lblTituloPendente.Text = "A RECEBER (PENDENTE)";



            this.lblValorPendente.AutoSize = true;
            this.lblValorPendente.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorPendente.Location = new System.Drawing.Point(12, 34);
            this.lblValorPendente.Name = "lblValorPendente";
            this.lblValorPendente.Size = new System.Drawing.Size(91, 30);
            this.lblValorPendente.TabIndex = 1;
            this.lblValorPendente.Text = "R$ 0,00";



            this.lblSubPendente.AutoSize = true;
            this.lblSubPendente.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubPendente.Location = new System.Drawing.Point(14, 68);
            this.lblSubPendente.Name = "lblSubPendente";
            this.lblSubPendente.Size = new System.Drawing.Size(142, 13);
            this.lblSubPendente.TabIndex = 2;
            this.lblSubPendente.Tag = "subtitulo";
            this.lblSubPendente.Text = "Vendas no Carnê / A Prazo";



            this.cardOrcamentos.Controls.Add(this.lblTituloOrcamentos);
            this.cardOrcamentos.Controls.Add(this.lblValorOrcamentos);
            this.cardOrcamentos.Controls.Add(this.lblSubOrcamentos);
            this.cardOrcamentos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardOrcamentos.Location = new System.Drawing.Point(836, 10);
            this.cardOrcamentos.Margin = new System.Windows.Forms.Padding(5);
            this.cardOrcamentos.Name = "cardOrcamentos";
            this.cardOrcamentos.Size = new System.Drawing.Size(264, 90);
            this.cardOrcamentos.TabIndex = 3;
            this.cardOrcamentos.Tag = "card|accent_purple";



            this.lblTituloOrcamentos.AutoSize = true;
            this.lblTituloOrcamentos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloOrcamentos.Location = new System.Drawing.Point(14, 12);
            this.lblTituloOrcamentos.Name = "lblTituloOrcamentos";
            this.lblTituloOrcamentos.Size = new System.Drawing.Size(157, 15);
            this.lblTituloOrcamentos.TabIndex = 0;
            this.lblTituloOrcamentos.Tag = "subtitulo";
            this.lblTituloOrcamentos.Text = "ORÇAMENTOS EM ABERTO";



            this.lblValorOrcamentos.AutoSize = true;
            this.lblValorOrcamentos.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorOrcamentos.Location = new System.Drawing.Point(12, 34);
            this.lblValorOrcamentos.Name = "lblValorOrcamentos";
            this.lblValorOrcamentos.Size = new System.Drawing.Size(26, 30);
            this.lblValorOrcamentos.TabIndex = 1;
            this.lblValorOrcamentos.Text = "0";



            this.lblSubOrcamentos.AutoSize = true;
            this.lblSubOrcamentos.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubOrcamentos.Location = new System.Drawing.Point(14, 68);
            this.lblSubOrcamentos.Name = "lblSubOrcamentos";
            this.lblSubOrcamentos.Size = new System.Drawing.Size(113, 13);
            this.lblSubOrcamentos.TabIndex = 2;
            this.lblSubOrcamentos.Tag = "subtitulo";
            this.lblSubOrcamentos.Text = "Total orçado: R$ 0,00";



            this.pnlFiltros.Controls.Add(this.lblPeriodo);
            this.pnlFiltros.Controls.Add(this.pnlDtpInicio);
            this.pnlFiltros.Controls.Add(this.lblAte);
            this.pnlFiltros.Controls.Add(this.pnlDtpFim);
            this.pnlFiltros.Controls.Add(this.btnHoje);
            this.pnlFiltros.Controls.Add(this.btn7Dias);
            this.pnlFiltros.Controls.Add(this.btn30Dias);
            this.pnlFiltros.Controls.Add(this.lblTipo);
            this.pnlFiltros.Controls.Add(this.cmbTipo);
            this.pnlFiltros.Controls.Add(this.lblStatus);
            this.pnlFiltros.Controls.Add(this.cmbStatus);
            this.pnlFiltros.Controls.Add(this.lblBusca);
            this.pnlFiltros.Controls.Add(this.txtBusca);
            this.pnlFiltros.Controls.Add(this.btnFiltrar);
            this.pnlFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFiltros.Location = new System.Drawing.Point(0, 170);
            this.pnlFiltros.Name = "pnlFiltros";
            this.pnlFiltros.Size = new System.Drawing.Size(1120, 52);
            this.pnlFiltros.TabIndex = 2;



            this.lblPeriodo.AutoSize = true;
            this.lblPeriodo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriodo.Location = new System.Drawing.Point(20, 17);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(51, 15);
            this.lblPeriodo.TabIndex = 0;
            this.lblPeriodo.Text = "Período:";



            this.pnlDtpInicio.Controls.Add(this.lblDtpInicio);
            this.pnlDtpInicio.Controls.Add(this.dtpInicio);
            this.pnlDtpInicio.Location = new System.Drawing.Point(75, 13);
            this.pnlDtpInicio.Name = "pnlDtpInicio";
            this.pnlDtpInicio.Padding = new System.Windows.Forms.Padding(2);
            this.pnlDtpInicio.Size = new System.Drawing.Size(112, 24);
            this.pnlDtpInicio.TabIndex = 1;
            this.pnlDtpInicio.Tag = "card";



            this.lblDtpInicio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblDtpInicio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDtpInicio.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDtpInicio.Location = new System.Drawing.Point(2, 2);
            this.lblDtpInicio.Name = "lblDtpInicio";
            this.lblDtpInicio.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lblDtpInicio.Size = new System.Drawing.Size(90, 20);
            this.lblDtpInicio.TabIndex = 0;
            this.lblDtpInicio.Text = "01/01/2026";
            this.lblDtpInicio.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;



            this.dtpInicio.Dock = System.Windows.Forms.DockStyle.Right;
            this.dtpInicio.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpInicio.Location = new System.Drawing.Point(92, 2);
            this.dtpInicio.Name = "dtpInicio";
            this.dtpInicio.Size = new System.Drawing.Size(18, 23);
            this.dtpInicio.TabIndex = 1;



            this.lblAte.AutoSize = true;
            this.lblAte.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAte.Location = new System.Drawing.Point(191, 17);
            this.lblAte.Name = "lblAte";
            this.lblAte.Size = new System.Drawing.Size(23, 15);
            this.lblAte.TabIndex = 2;
            this.lblAte.Text = "até";



            this.pnlDtpFim.Controls.Add(this.lblDtpFim);
            this.pnlDtpFim.Controls.Add(this.dtpFim);
            this.pnlDtpFim.Location = new System.Drawing.Point(216, 13);
            this.pnlDtpFim.Name = "pnlDtpFim";
            this.pnlDtpFim.Padding = new System.Windows.Forms.Padding(2);
            this.pnlDtpFim.Size = new System.Drawing.Size(112, 24);
            this.pnlDtpFim.TabIndex = 3;
            this.pnlDtpFim.Tag = "card";



            this.lblDtpFim.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblDtpFim.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDtpFim.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDtpFim.Location = new System.Drawing.Point(2, 2);
            this.lblDtpFim.Name = "lblDtpFim";
            this.lblDtpFim.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lblDtpFim.Size = new System.Drawing.Size(90, 20);
            this.lblDtpFim.TabIndex = 0;
            this.lblDtpFim.Text = "01/01/2026";
            this.lblDtpFim.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;



            this.dtpFim.Dock = System.Windows.Forms.DockStyle.Right;
            this.dtpFim.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpFim.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFim.Location = new System.Drawing.Point(92, 2);
            this.dtpFim.Name = "dtpFim";
            this.dtpFim.Size = new System.Drawing.Size(18, 23);
            this.dtpFim.TabIndex = 1;



            this.btnHoje.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.btnHoje.Location = new System.Drawing.Point(334, 12);
            this.btnHoje.Name = "btnHoje";
            this.btnHoje.Size = new System.Drawing.Size(52, 26);
            this.btnHoje.TabIndex = 4;
            this.btnHoje.Text = "Hoje";
            this.btnHoje.UseVisualStyleBackColor = true;
            this.btnHoje.Click += new System.EventHandler(this.btnHoje_Click);



            this.btn7Dias.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.btn7Dias.Location = new System.Drawing.Point(390, 12);
            this.btn7Dias.Name = "btn7Dias";
            this.btn7Dias.Size = new System.Drawing.Size(54, 26);
            this.btn7Dias.TabIndex = 5;
            this.btn7Dias.Text = "7 dias";
            this.btn7Dias.UseVisualStyleBackColor = true;
            this.btn7Dias.Click += new System.EventHandler(this.btn7Dias_Click);



            this.btn30Dias.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.btn30Dias.Location = new System.Drawing.Point(448, 12);
            this.btn30Dias.Name = "btn30Dias";
            this.btn30Dias.Size = new System.Drawing.Size(60, 26);
            this.btn30Dias.TabIndex = 6;
            this.btn30Dias.Text = "30 dias";
            this.btn30Dias.UseVisualStyleBackColor = true;
            this.btn30Dias.Click += new System.EventHandler(this.btn30Dias_Click);



            this.lblTipo.AutoSize = true;
            this.lblTipo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTipo.Location = new System.Drawing.Point(518, 17);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(34, 15);
            this.lblTipo.TabIndex = 7;
            this.lblTipo.Text = "Tipo:";



            this.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbTipo.Items.AddRange(new object[] {
            "Todos",
            "VENDA",
            "ORCAMENTO"});
            this.cmbTipo.Location = new System.Drawing.Point(552, 13);
            this.cmbTipo.Name = "cmbTipo";
            this.cmbTipo.Size = new System.Drawing.Size(98, 23);
            this.cmbTipo.TabIndex = 8;



            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatus.Location = new System.Drawing.Point(658, 17);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(42, 15);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.Text = "Status:";



            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbStatus.Items.AddRange(new object[] {
            "Todos",
            "PAGO",
            "PENDENTE",
            "CANCELADA"});
            this.cmbStatus.Location = new System.Drawing.Point(702, 13);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(98, 23);
            this.cmbStatus.TabIndex = 10;



            this.lblBusca.AutoSize = true;
            this.lblBusca.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBusca.Location = new System.Drawing.Point(808, 17);
            this.lblBusca.Name = "lblBusca";
            this.lblBusca.Size = new System.Drawing.Size(45, 15);
            this.lblBusca.TabIndex = 11;
            this.lblBusca.Text = "Buscar:";



            this.txtBusca.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtBusca.Location = new System.Drawing.Point(856, 13);
            this.txtBusca.Name = "txtBusca";
            this.txtBusca.Size = new System.Drawing.Size(148, 24);
            this.txtBusca.TabIndex = 12;
            this.txtBusca.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBusca_KeyDown);



            this.btnFiltrar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnFiltrar.Location = new System.Drawing.Point(1012, 11);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(88, 28);
            this.btnFiltrar.TabIndex = 13;
            this.btnFiltrar.Tag = "primary";
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = true;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);



            this.splitPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitPrincipal.Location = new System.Drawing.Point(0, 222);
            this.splitPrincipal.Name = "splitPrincipal";



            this.splitPrincipal.Panel1.Controls.Add(this.dgvVendas);



            this.splitPrincipal.Panel2.Controls.Add(this.pnlDetalhesCard);
            this.splitPrincipal.Size = new System.Drawing.Size(1120, 440);
            this.splitPrincipal.SplitterDistance = 720;
            this.splitPrincipal.SplitterWidth = 8;
            this.splitPrincipal.TabIndex = 3;



            this.dgvVendas.AllowUserToAddRows = false;
            this.dgvVendas.AllowUserToDeleteRows = false;
            this.dgvVendas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVendas.Location = new System.Drawing.Point(0, 0);
            this.dgvVendas.MultiSelect = false;
            this.dgvVendas.Name = "dgvVendas";
            this.dgvVendas.ReadOnly = true;
            this.dgvVendas.RowHeadersVisible = false;
            this.dgvVendas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVendas.Size = new System.Drawing.Size(720, 440);
            this.dgvVendas.TabIndex = 0;
            this.dgvVendas.SelectionChanged += new System.EventHandler(this.dgvVendas_SelectionChanged);



            this.pnlDetalhesCard.Controls.Add(this.lblPagamentosTitulo);
            this.pnlDetalhesCard.Controls.Add(this.dgvPagamentos);
            this.pnlDetalhesCard.Controls.Add(this.pnlAcoes);
            this.pnlDetalhesCard.Controls.Add(this.dgvItens);
            this.pnlDetalhesCard.Controls.Add(this.lblItensTitulo);
            this.pnlDetalhesCard.Controls.Add(this.pnlDetalhesHeader);
            this.pnlDetalhesCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetalhesCard.Location = new System.Drawing.Point(0, 0);
            this.pnlDetalhesCard.Name = "pnlDetalhesCard";
            this.pnlDetalhesCard.Padding = new System.Windows.Forms.Padding(12);
            this.pnlDetalhesCard.Size = new System.Drawing.Size(392, 440);
            this.pnlDetalhesCard.TabIndex = 0;
            this.pnlDetalhesCard.Tag = "card";



            this.lblPagamentosTitulo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblPagamentosTitulo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPagamentosTitulo.Location = new System.Drawing.Point(12, 206);
            this.lblPagamentosTitulo.Name = "lblPagamentosTitulo";
            this.lblPagamentosTitulo.Padding = new System.Windows.Forms.Padding(2, 6, 0, 4);
            this.lblPagamentosTitulo.Size = new System.Drawing.Size(368, 28);
            this.lblPagamentosTitulo.TabIndex = 0;
            this.lblPagamentosTitulo.Text = "Pagamentos Lançados";



            this.dgvPagamentos.AllowUserToAddRows = false;
            this.dgvPagamentos.AllowUserToDeleteRows = false;
            this.dgvPagamentos.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvPagamentos.Location = new System.Drawing.Point(12, 234);
            this.dgvPagamentos.MultiSelect = false;
            this.dgvPagamentos.Name = "dgvPagamentos";
            this.dgvPagamentos.ReadOnly = true;
            this.dgvPagamentos.RowHeadersVisible = false;
            this.dgvPagamentos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPagamentos.Size = new System.Drawing.Size(368, 110);
            this.dgvPagamentos.TabIndex = 1;



            this.pnlAcoes.ColumnCount = 2;
            this.pnlAcoes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlAcoes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlAcoes.Controls.Add(this.btnReimprimir, 0, 0);
            this.pnlAcoes.Controls.Add(this.btnEfetivar, 1, 0);
            this.pnlAcoes.Controls.Add(this.btnQuitar, 0, 1);
            this.pnlAcoes.Controls.Add(this.btnCancelarVenda, 1, 1);
            this.pnlAcoes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAcoes.Location = new System.Drawing.Point(12, 344);
            this.pnlAcoes.Name = "pnlAcoes";
            this.pnlAcoes.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.pnlAcoes.RowCount = 2;
            this.pnlAcoes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlAcoes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlAcoes.Size = new System.Drawing.Size(368, 84);
            this.pnlAcoes.TabIndex = 2;



            this.btnReimprimir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReimprimir.Enabled = false;
            this.btnReimprimir.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnReimprimir.Location = new System.Drawing.Point(2, 10);
            this.btnReimprimir.Margin = new System.Windows.Forms.Padding(2);
            this.btnReimprimir.Name = "btnReimprimir";
            this.btnReimprimir.Size = new System.Drawing.Size(180, 34);
            this.btnReimprimir.TabIndex = 0;
            this.btnReimprimir.Text = "Reimprimir Docs";
            this.btnReimprimir.UseVisualStyleBackColor = true;
            this.btnReimprimir.Click += new System.EventHandler(this.btnReimprimir_Click);



            this.btnEfetivar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEfetivar.Enabled = false;
            this.btnEfetivar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEfetivar.Location = new System.Drawing.Point(186, 10);
            this.btnEfetivar.Margin = new System.Windows.Forms.Padding(2);
            this.btnEfetivar.Name = "btnEfetivar";
            this.btnEfetivar.Size = new System.Drawing.Size(180, 34);
            this.btnEfetivar.TabIndex = 1;
            this.btnEfetivar.Tag = "primary";
            this.btnEfetivar.Text = "Efetivar Orçamento";
            this.btnEfetivar.UseVisualStyleBackColor = true;
            this.btnEfetivar.Click += new System.EventHandler(this.btnEfetivar_Click);



            this.btnQuitar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnQuitar.Enabled = false;
            this.btnQuitar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnQuitar.Location = new System.Drawing.Point(2, 48);
            this.btnQuitar.Margin = new System.Windows.Forms.Padding(2);
            this.btnQuitar.Name = "btnQuitar";
            this.btnQuitar.Size = new System.Drawing.Size(180, 34);
            this.btnQuitar.TabIndex = 2;
            this.btnQuitar.Tag = "success";
            this.btnQuitar.Text = "Quitar Pendência";
            this.btnQuitar.UseVisualStyleBackColor = true;
            this.btnQuitar.Click += new System.EventHandler(this.btnQuitar_Click);



            this.btnCancelarVenda.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancelarVenda.Enabled = false;
            this.btnCancelarVenda.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancelarVenda.Location = new System.Drawing.Point(186, 48);
            this.btnCancelarVenda.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancelarVenda.Name = "btnCancelarVenda";
            this.btnCancelarVenda.Size = new System.Drawing.Size(180, 34);
            this.btnCancelarVenda.TabIndex = 3;
            this.btnCancelarVenda.Tag = "danger";
            this.btnCancelarVenda.Text = "Cancelar";
            this.btnCancelarVenda.UseVisualStyleBackColor = true;
            this.btnCancelarVenda.Click += new System.EventHandler(this.btnCancelarVenda_Click);



            this.dgvItens.AllowUserToAddRows = false;
            this.dgvItens.AllowUserToDeleteRows = false;
            this.dgvItens.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItens.Location = new System.Drawing.Point(12, 126);
            this.dgvItens.MultiSelect = false;
            this.dgvItens.Name = "dgvItens";
            this.dgvItens.ReadOnly = true;
            this.dgvItens.RowHeadersVisible = false;
            this.dgvItens.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItens.Size = new System.Drawing.Size(368, 302);
            this.dgvItens.TabIndex = 3;



            this.lblItensTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblItensTitulo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblItensTitulo.Location = new System.Drawing.Point(12, 100);
            this.lblItensTitulo.Name = "lblItensTitulo";
            this.lblItensTitulo.Padding = new System.Windows.Forms.Padding(2, 4, 0, 4);
            this.lblItensTitulo.Size = new System.Drawing.Size(368, 26);
            this.lblItensTitulo.TabIndex = 4;
            this.lblItensTitulo.Text = "Itens do Pedido";



            this.pnlDetalhesHeader.Controls.Add(this.lblDetalheNotas);
            this.pnlDetalhesHeader.Controls.Add(this.lblDetalheMeta);
            this.pnlDetalhesHeader.Controls.Add(this.lblDetalheTitulo);
            this.pnlDetalhesHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDetalhesHeader.Location = new System.Drawing.Point(12, 12);
            this.pnlDetalhesHeader.Name = "pnlDetalhesHeader";
            this.pnlDetalhesHeader.Size = new System.Drawing.Size(368, 88);
            this.pnlDetalhesHeader.TabIndex = 5;



            this.lblDetalheNotas.AutoEllipsis = true;
            this.lblDetalheNotas.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblDetalheNotas.Location = new System.Drawing.Point(2, 48);
            this.lblDetalheNotas.Name = "lblDetalheNotas";
            this.lblDetalheNotas.Size = new System.Drawing.Size(340, 34);
            this.lblDetalheNotas.TabIndex = 0;
            this.lblDetalheNotas.Tag = "subtitulo";



            this.lblDetalheMeta.AutoSize = true;
            this.lblDetalheMeta.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblDetalheMeta.Location = new System.Drawing.Point(2, 26);
            this.lblDetalheMeta.Name = "lblDetalheMeta";
            this.lblDetalheMeta.Size = new System.Drawing.Size(193, 15);
            this.lblDetalheMeta.TabIndex = 1;
            this.lblDetalheMeta.Tag = "subtitulo";
            this.lblDetalheMeta.Text = "Cliente e vendedor aparecerão aqui";



            this.lblDetalheTitulo.AutoSize = true;
            this.lblDetalheTitulo.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblDetalheTitulo.Location = new System.Drawing.Point(2, 2);
            this.lblDetalheTitulo.Name = "lblDetalheTitulo";
            this.lblDetalheTitulo.Size = new System.Drawing.Size(173, 21);
            this.lblDetalheTitulo.TabIndex = 2;
            this.lblDetalheTitulo.Text = "Selecione uma venda";



            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1120, 662);
            this.Controls.Add(this.splitPrincipal);
            this.Controls.Add(this.pnlFiltros);
            this.Controls.Add(this.pnlKpis);
            this.Controls.Add(this.pnlHeader);
            this.Name = "FormRegistroVendas";
            this.Text = "Registro de Vendas";
            this.Load += new System.EventHandler(this.FormRegistroVendas_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlKpis.ResumeLayout(false);
            this.cardTotalVendido.ResumeLayout(false);
            this.cardTotalVendido.PerformLayout();
            this.cardTicketMedio.ResumeLayout(false);
            this.cardTicketMedio.PerformLayout();
            this.cardPendente.ResumeLayout(false);
            this.cardPendente.PerformLayout();
            this.cardOrcamentos.ResumeLayout(false);
            this.cardOrcamentos.PerformLayout();
            this.pnlFiltros.ResumeLayout(false);
            this.pnlFiltros.PerformLayout();
            this.pnlDtpInicio.ResumeLayout(false);
            this.pnlDtpFim.ResumeLayout(false);
            this.splitPrincipal.Panel1.ResumeLayout(false);
            this.splitPrincipal.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitPrincipal)).EndInit();
            this.splitPrincipal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendas)).EndInit();
            this.pnlDetalhesCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPagamentos)).EndInit();
            this.pnlAcoes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).EndInit();
            this.pnlDetalhesHeader.ResumeLayout(false);
            this.pnlDetalhesHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnAtualizar;
        private System.Windows.Forms.Button btnExportarCsv;
        private System.Windows.Forms.TableLayoutPanel pnlKpis;
        private System.Windows.Forms.Panel cardTotalVendido;
        private System.Windows.Forms.Label lblTituloTotalVendido;
        private System.Windows.Forms.Label lblValorTotalVendido;
        private System.Windows.Forms.Label lblSubTotalVendido;
        private System.Windows.Forms.Panel cardTicketMedio;
        private System.Windows.Forms.Label lblTituloTicketMedio;
        private System.Windows.Forms.Label lblValorTicketMedio;
        private System.Windows.Forms.Label lblSubTicketMedio;
        private System.Windows.Forms.Panel cardPendente;
        private System.Windows.Forms.Label lblTituloPendente;
        private System.Windows.Forms.Label lblValorPendente;
        private System.Windows.Forms.Label lblSubPendente;
        private System.Windows.Forms.Panel cardOrcamentos;
        private System.Windows.Forms.Label lblTituloOrcamentos;
        private System.Windows.Forms.Label lblValorOrcamentos;
        private System.Windows.Forms.Label lblSubOrcamentos;
        private System.Windows.Forms.Panel pnlFiltros;
        private System.Windows.Forms.Label lblPeriodo;
        private System.Windows.Forms.Panel pnlDtpInicio;
        private System.Windows.Forms.Label lblDtpInicio;
        private System.Windows.Forms.DateTimePicker dtpInicio;
        private System.Windows.Forms.Label lblAte;
        private System.Windows.Forms.Panel pnlDtpFim;
        private System.Windows.Forms.Label lblDtpFim;
        private System.Windows.Forms.DateTimePicker dtpFim;
        private System.Windows.Forms.Button btnHoje;
        private System.Windows.Forms.Button btn7Dias;
        private System.Windows.Forms.Button btn30Dias;
        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.ComboBox cmbTipo;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblBusca;
        private System.Windows.Forms.TextBox txtBusca;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.SplitContainer splitPrincipal;
        private System.Windows.Forms.DataGridView dgvVendas;
        private System.Windows.Forms.Panel pnlDetalhesCard;
        private System.Windows.Forms.Panel pnlDetalhesHeader;
        private System.Windows.Forms.Label lblDetalheTitulo;
        private System.Windows.Forms.Label lblDetalheMeta;
        private System.Windows.Forms.Label lblDetalheNotas;
        private System.Windows.Forms.Label lblItensTitulo;
        private System.Windows.Forms.DataGridView dgvItens;
        private System.Windows.Forms.Label lblPagamentosTitulo;
        private System.Windows.Forms.DataGridView dgvPagamentos;
        private System.Windows.Forms.TableLayoutPanel pnlAcoes;
        private System.Windows.Forms.Button btnReimprimir;
        private System.Windows.Forms.Button btnEfetivar;
        private System.Windows.Forms.Button btnQuitar;
        private System.Windows.Forms.Button btnCancelarVenda;
    }
}
