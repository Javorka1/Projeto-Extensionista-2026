namespace ProjetoExtensionista2026_2
{
    partial class formVenda
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer_menu = new System.Windows.Forms.SplitContainer();
            this.splitContainer_catalogo = new System.Windows.Forms.SplitContainer();
            this.panelConteudo = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnLimparCarrinho = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.txtDesconto = new System.Windows.Forms.TextBox();
            this.lblSubtotal = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnAvancarPagamento = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDescontoPorcentagem = new System.Windows.Forms.TextBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnConsultarCliente = new System.Windows.Forms.Button();
            this.lblNomeCliente = new System.Windows.Forms.Label();
            this.btnCadastrarCliente = new System.Windows.Forms.Button();
            this.lblTelefoneCliente = new System.Windows.Forms.Label();
            this.lblLimiteCliente = new System.Windows.Forms.Label();
            this.lblStatusCliente = new System.Windows.Forms.Label();
            this.lblDocumentoCliente = new System.Windows.Forms.Label();
            this.lblEnderecoCliente = new System.Windows.Forms.Label();
            this.btnLimparCliente = new System.Windows.Forms.Button();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbTipoVenda = new System.Windows.Forms.ComboBox();
            this.panelCarrinho = new System.Windows.Forms.Panel();
            this.lblQtdItens = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_menu)).BeginInit();
            this.splitContainer_menu.Panel2.SuspendLayout();
            this.splitContainer_menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_catalogo)).BeginInit();
            this.splitContainer_catalogo.Panel1.SuspendLayout();
            this.splitContainer_catalogo.Panel2.SuspendLayout();
            this.splitContainer_catalogo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer_menu
            // 
            this.splitContainer_menu.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.splitContainer_menu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_menu.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_menu.Name = "splitContainer_menu";
            // 
            // splitContainer_menu.Panel1
            // 
            this.splitContainer_menu.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer_menu.Panel1Collapsed = true;
            // 
            // splitContainer_menu.Panel2
            // 
            this.splitContainer_menu.Panel2.Controls.Add(this.splitContainer_catalogo);
            this.splitContainer_menu.Size = new System.Drawing.Size(1904, 1041);
            this.splitContainer_menu.SplitterDistance = 247;
            this.splitContainer_menu.SplitterWidth = 1;
            this.splitContainer_menu.TabIndex = 2;
            this.splitContainer_menu.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_menu_SplitterMoved);
            // 
            // splitContainer_catalogo
            // 
            this.splitContainer_catalogo.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.splitContainer_catalogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_catalogo.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer_catalogo.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_catalogo.Name = "splitContainer_catalogo";
            // 
            // splitContainer_catalogo.Panel1
            // 
            this.splitContainer_catalogo.Panel1.Controls.Add(this.panelConteudo);
            // 
            // splitContainer_catalogo.Panel2
            // 
            this.splitContainer_catalogo.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer_catalogo.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer_catalogo.Size = new System.Drawing.Size(1904, 1041);
            this.splitContainer_catalogo.SplitterDistance = 1513;
            this.splitContainer_catalogo.SplitterWidth = 1;
            this.splitContainer_catalogo.TabIndex = 2;
            this.splitContainer_catalogo.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_catalogo_SplitterMoved);
            // 
            // panelConteudo
            // 
            this.panelConteudo.BackColor = System.Drawing.SystemColors.Control;
            this.panelConteudo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelConteudo.Location = new System.Drawing.Point(0, 0);
            this.panelConteudo.Name = "panelConteudo";
            this.panelConteudo.Size = new System.Drawing.Size(1513, 1041);
            this.panelConteudo.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel7, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 148F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 175F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(390, 1041);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 869);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(384, 169);
            this.panel3.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.Control;
            this.panel4.Controls.Add(this.btnLimparCarrinho);
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Controls.Add(this.txtDesconto);
            this.panel4.Controls.Add(this.lblSubtotal);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.lblTotal);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.btnAvancarPagamento);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.txtDescontoPorcentagem);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(15, 0, 15, 10);
            this.panel4.Size = new System.Drawing.Size(384, 169);
            this.panel4.TabIndex = 6;
            // 
            // btnLimparCarrinho
            // 
            this.btnLimparCarrinho.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLimparCarrinho.BackColor = System.Drawing.Color.Red;
            this.btnLimparCarrinho.FlatAppearance.BorderSize = 0;
            this.btnLimparCarrinho.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimparCarrinho.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimparCarrinho.ForeColor = System.Drawing.Color.White;
            this.btnLimparCarrinho.Location = new System.Drawing.Point(15, 115);
            this.btnLimparCarrinho.Name = "btnLimparCarrinho";
            this.btnLimparCarrinho.Size = new System.Drawing.Size(100, 44);
            this.btnLimparCarrinho.TabIndex = 12;
            this.btnLimparCarrinho.Text = "Limpar [F4]";
            this.btnLimparCarrinho.UseVisualStyleBackColor = false;
            this.btnLimparCarrinho.Click += new System.EventHandler(this.btnLimparCarrinho_Click);
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.Black;
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(15, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(354, 1);
            this.panel6.TabIndex = 4;
            // 
            // txtDesconto
            // 
            this.txtDesconto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDesconto.BackColor = System.Drawing.SystemColors.Control;
            this.txtDesconto.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDesconto.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDesconto.Location = new System.Drawing.Point(171, 32);
            this.txtDesconto.Name = "txtDesconto";
            this.txtDesconto.ShortcutsEnabled = false;
            this.txtDesconto.Size = new System.Drawing.Size(115, 18);
            this.txtDesconto.TabIndex = 10;
            this.txtDesconto.Text = "- R$ 0,00";
            this.txtDesconto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblSubtotal
            // 
            this.lblSubtotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSubtotal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtotal.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblSubtotal.Location = new System.Drawing.Point(171, 8);
            this.lblSubtotal.Name = "lblSubtotal";
            this.lblSubtotal.Size = new System.Drawing.Size(198, 20);
            this.lblSubtotal.TabIndex = 9;
            this.lblSubtotal.Text = "R$ 0,00";
            this.lblSubtotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(15, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Subtotal";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(15, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 20);
            this.label6.TabIndex = 5;
            this.label6.Text = "Desconto";
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTotal.Location = new System.Drawing.Point(141, 68);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(228, 28);
            this.lblTotal.TabIndex = 4;
            this.lblTotal.Text = "R$ 0,00";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.BackColor = System.Drawing.Color.LightGray;
            this.panel5.Location = new System.Drawing.Point(15, 60);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(354, 1);
            this.panel5.TabIndex = 3;
            // 
            // btnAvancarPagamento
            // 
            this.btnAvancarPagamento.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAvancarPagamento.BackColor = System.Drawing.Color.SeaGreen;
            this.btnAvancarPagamento.FlatAppearance.BorderSize = 0;
            this.btnAvancarPagamento.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAvancarPagamento.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAvancarPagamento.ForeColor = System.Drawing.Color.White;
            this.btnAvancarPagamento.Location = new System.Drawing.Point(122, 115);
            this.btnAvancarPagamento.Name = "btnAvancarPagamento";
            this.btnAvancarPagamento.Size = new System.Drawing.Size(247, 44);
            this.btnAvancarPagamento.TabIndex = 2;
            this.btnAvancarPagamento.Text = "Avançar para Pagamento [F12]";
            this.btnAvancarPagamento.UseVisualStyleBackColor = false;
            this.btnAvancarPagamento.Click += new System.EventHandler(this.btnAvancarPagamento_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(15, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 24);
            this.label2.TabIndex = 0;
            this.label2.Text = "Total a Pagar";
            // 
            // txtDescontoPorcentagem
            // 
            this.txtDescontoPorcentagem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescontoPorcentagem.BackColor = System.Drawing.SystemColors.Control;
            this.txtDescontoPorcentagem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDescontoPorcentagem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescontoPorcentagem.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.txtDescontoPorcentagem.Location = new System.Drawing.Point(293, 32);
            this.txtDescontoPorcentagem.Name = "txtDescontoPorcentagem";
            this.txtDescontoPorcentagem.ShortcutsEnabled = false;
            this.txtDescontoPorcentagem.Size = new System.Drawing.Size(76, 18);
            this.txtDescontoPorcentagem.TabIndex = 11;
            this.txtDescontoPorcentagem.Text = "0 %";
            this.txtDescontoPorcentagem.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btnConsultarCliente);
            this.panel7.Controls.Add(this.lblNomeCliente);
            this.panel7.Controls.Add(this.btnCadastrarCliente);
            this.panel7.Controls.Add(this.lblTelefoneCliente);
            this.panel7.Controls.Add(this.lblLimiteCliente);
            this.panel7.Controls.Add(this.lblStatusCliente);
            this.panel7.Controls.Add(this.lblDocumentoCliente);
            this.panel7.Controls.Add(this.lblEnderecoCliente);
            this.panel7.Controls.Add(this.btnLimparCliente);
            this.panel7.Controls.Add(this.panel8);
            this.panel7.Controls.Add(this.panel2);
            this.panel7.Controls.Add(this.label4);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 3);
            this.panel7.Name = "panel7";
            this.panel7.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.panel7.Size = new System.Drawing.Size(384, 142);
            this.panel7.TabIndex = 2;
            // 
            // btnConsultarCliente
            // 
            this.btnConsultarCliente.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConsultarCliente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConsultarCliente.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConsultarCliente.Location = new System.Drawing.Point(174, 104);
            this.btnConsultarCliente.Name = "btnConsultarCliente";
            this.btnConsultarCliente.Size = new System.Drawing.Size(102, 28);
            this.btnConsultarCliente.TabIndex = 14;
            this.btnConsultarCliente.Tag = "primary";
            this.btnConsultarCliente.Text = "Alterar [F2]";
            this.btnConsultarCliente.UseVisualStyleBackColor = true;
            this.btnConsultarCliente.Click += new System.EventHandler(this.btnConsultarCliente_Click);
            // 
            // lblNomeCliente
            // 
            this.lblNomeCliente.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNomeCliente.BackColor = System.Drawing.Color.Transparent;
            this.lblNomeCliente.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNomeCliente.Location = new System.Drawing.Point(15, 32);
            this.lblNomeCliente.Name = "lblNomeCliente";
            this.lblNomeCliente.Size = new System.Drawing.Size(356, 24);
            this.lblNomeCliente.TabIndex = 13;
            this.lblNomeCliente.Text = "Cliente: Consumidor Final";
            // 
            // btnCadastrarCliente
            // 
            this.btnCadastrarCliente.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCadastrarCliente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCadastrarCliente.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCadastrarCliente.Location = new System.Drawing.Point(88, 104);
            this.btnCadastrarCliente.Name = "btnCadastrarCliente";
            this.btnCadastrarCliente.Size = new System.Drawing.Size(80, 28);
            this.btnCadastrarCliente.TabIndex = 12;
            this.btnCadastrarCliente.Text = "Novo";
            this.btnCadastrarCliente.UseVisualStyleBackColor = true;
            this.btnCadastrarCliente.Click += new System.EventHandler(this.btnCadastrarCliente_Click);
            // 
            // lblTelefoneCliente
            // 
            this.lblTelefoneCliente.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTelefoneCliente.Location = new System.Drawing.Point(15, 100);
            this.lblTelefoneCliente.Name = "lblTelefoneCliente";
            this.lblTelefoneCliente.Size = new System.Drawing.Size(80, 20);
            this.lblTelefoneCliente.TabIndex = 11;
            this.lblTelefoneCliente.Visible = false;
            // 
            // lblLimiteCliente
            // 
            this.lblLimiteCliente.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLimiteCliente.Location = new System.Drawing.Point(15, 100);
            this.lblLimiteCliente.Name = "lblLimiteCliente";
            this.lblLimiteCliente.Size = new System.Drawing.Size(80, 20);
            this.lblLimiteCliente.TabIndex = 10;
            this.lblLimiteCliente.Visible = false;
            // 
            // lblStatusCliente
            // 
            this.lblStatusCliente.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatusCliente.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusCliente.Location = new System.Drawing.Point(226, 56);
            this.lblStatusCliente.Name = "lblStatusCliente";
            this.lblStatusCliente.Size = new System.Drawing.Size(145, 18);
            this.lblStatusCliente.TabIndex = 9;
            this.lblStatusCliente.Text = "Status: Ativo";
            this.lblStatusCliente.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDocumentoCliente
            // 
            this.lblDocumentoCliente.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocumentoCliente.Location = new System.Drawing.Point(15, 56);
            this.lblDocumentoCliente.Name = "lblDocumentoCliente";
            this.lblDocumentoCliente.Size = new System.Drawing.Size(215, 18);
            this.lblDocumentoCliente.TabIndex = 8;
            this.lblDocumentoCliente.Text = "CPF/CNPJ: Consumidor Final";
            this.lblDocumentoCliente.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEnderecoCliente
            // 
            this.lblEnderecoCliente.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEnderecoCliente.AutoEllipsis = true;
            this.lblEnderecoCliente.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEnderecoCliente.Location = new System.Drawing.Point(15, 76);
            this.lblEnderecoCliente.Name = "lblEnderecoCliente";
            this.lblEnderecoCliente.Size = new System.Drawing.Size(356, 18);
            this.lblEnderecoCliente.TabIndex = 7;
            this.lblEnderecoCliente.Tag = "subtitulo";
            this.lblEnderecoCliente.Text = "Endereço: Não informado";
            this.lblEnderecoCliente.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnLimparCliente
            // 
            this.btnLimparCliente.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLimparCliente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimparCliente.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimparCliente.Location = new System.Drawing.Point(281, 104);
            this.btnLimparCliente.Name = "btnLimparCliente";
            this.btnLimparCliente.Size = new System.Drawing.Size(90, 28);
            this.btnLimparCliente.TabIndex = 6;
            this.btnLimparCliente.Text = "Limpar";
            this.btnLimparCliente.UseVisualStyleBackColor = true;
            this.btnLimparCliente.Click += new System.EventHandler(this.btnLimparCliente_Click);
            // 
            // panel8
            // 
            this.panel8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel8.BackColor = System.Drawing.Color.LightGray;
            this.panel8.Location = new System.Drawing.Point(15, 28);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(356, 1);
            this.panel8.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightGray;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(15, 141);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(354, 1);
            this.panel2.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(15, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(356, 22);
            this.label4.TabIndex = 0;
            this.label4.Text = "Identificação do Cliente";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cmbTipoVenda);
            this.panel1.Controls.Add(this.panelCarrinho);
            this.panel1.Controls.Add(this.lblQtdItens);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 151);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(12, 5, 12, 5);
            this.panel1.Size = new System.Drawing.Size(384, 712);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(238, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Tipo:";
            // 
            // cmbTipoVenda
            // 
            this.cmbTipoVenda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTipoVenda.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoVenda.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTipoVenda.FormattingEnabled = true;
            this.cmbTipoVenda.Items.AddRange(new object[] {
            "Venda",
            "Orçamento"});
            this.cmbTipoVenda.Location = new System.Drawing.Point(281, 8);
            this.cmbTipoVenda.Name = "cmbTipoVenda";
            this.cmbTipoVenda.Size = new System.Drawing.Size(88, 21);
            this.cmbTipoVenda.TabIndex = 4;
            this.cmbTipoVenda.SelectedIndexChanged += new System.EventHandler(this.cmbTipoVenda_SelectedIndexChanged);
            // 
            // panelCarrinho
            // 
            this.panelCarrinho.AutoScroll = true;
            this.panelCarrinho.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelCarrinho.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCarrinho.Location = new System.Drawing.Point(12, 33);
            this.panelCarrinho.Name = "panelCarrinho";
            this.panelCarrinho.Size = new System.Drawing.Size(360, 674);
            this.panelCarrinho.TabIndex = 0;
            // 
            // lblQtdItens
            // 
            this.lblQtdItens.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblQtdItens.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQtdItens.Location = new System.Drawing.Point(12, 5);
            this.lblQtdItens.Name = "lblQtdItens";
            this.lblQtdItens.Size = new System.Drawing.Size(360, 28);
            this.lblQtdItens.TabIndex = 3;
            this.lblQtdItens.Text = "Itens: 0 item(ns)";
            this.lblQtdItens.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // formVenda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.splitContainer_menu);
            this.KeyPreview = true;
            this.Name = "formVenda";
            this.Text = "formVenda";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.formVenda_Load);
            this.splitContainer_menu.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_menu)).EndInit();
            this.splitContainer_menu.ResumeLayout(false);
            this.splitContainer_catalogo.Panel1.ResumeLayout(false);
            this.splitContainer_catalogo.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_catalogo)).EndInit();
            this.splitContainer_catalogo.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer_menu;
        private System.Windows.Forms.SplitContainer splitContainer_catalogo;
        private System.Windows.Forms.Panel panelConteudo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelCarrinho;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnAvancarPagamento;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDesconto;
        private System.Windows.Forms.Label lblSubtotal;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label lblQtdItens;
        private System.Windows.Forms.TextBox txtDescontoPorcentagem;
        private System.Windows.Forms.Button btnLimparCarrinho;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblStatusCliente;
        private System.Windows.Forms.Label lblDocumentoCliente;
        private System.Windows.Forms.Label lblEnderecoCliente;
        private System.Windows.Forms.Button btnLimparCliente;
        private System.Windows.Forms.Label lblLimiteCliente;
        private System.Windows.Forms.Label lblTelefoneCliente;
        private System.Windows.Forms.Button btnCadastrarCliente;
        private System.Windows.Forms.Label lblNomeCliente;
        private System.Windows.Forms.Button btnConsultarCliente;
        private System.Windows.Forms.ComboBox cmbTipoVenda;
        private System.Windows.Forms.Label label1;
    }
}