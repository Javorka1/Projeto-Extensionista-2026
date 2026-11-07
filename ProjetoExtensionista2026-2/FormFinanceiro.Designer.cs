namespace ProjetoExtensionista2026_2
{
    partial class FormFinanceiro
    {
        /// <summary>Required designer variable.</summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>Clean up any resources being used.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>Required method for Designer support.</summary>
        private void InitializeComponent()
        {
            this.pnlTopo = new System.Windows.Forms.Panel();
            this.lblTituloPrincipal = new System.Windows.Forms.Label();
            this.btnAtualizar = new System.Windows.Forms.Button();
            this.btnExportarCsv = new System.Windows.Forms.Button();

            this.pnlIndicadores = new System.Windows.Forms.Panel();
            this.grpCustoEstoque = new System.Windows.Forms.GroupBox();
            this.lblValorCustoEstoque = new System.Windows.Forms.Label();
            this.lblDescCustoEstoque = new System.Windows.Forms.Label();

            this.grpReceitaPotencial = new System.Windows.Forms.GroupBox();
            this.lblValorReceitaPotencial = new System.Windows.Forms.Label();
            this.lblDescReceitaPotencial = new System.Windows.Forms.Label();

            this.grpLucroEstimado = new System.Windows.Forms.GroupBox();
            this.lblValorLucroEstimado = new System.Windows.Forms.Label();
            this.lblDescLucroEstimado = new System.Windows.Forms.Label();

            this.grpMargemMedia = new System.Windows.Forms.GroupBox();
            this.lblValorMargemMedia = new System.Windows.Forms.Label();
            this.lblDescMargemMedia = new System.Windows.Forms.Label();
            this.tblKpi = new System.Windows.Forms.TableLayoutPanel();

            this.pnlFiltros = new System.Windows.Forms.Panel();
            this.lblFiltroCategoria = new System.Windows.Forms.Label();
            this.cmbCategoria = new System.Windows.Forms.ComboBox();
            this.lblBusca = new System.Windows.Forms.Label();
            this.txtBuscaProduto = new System.Windows.Forms.TextBox();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.btnLimparFiltros = new System.Windows.Forms.Button();

            this.pnlAbas = new System.Windows.Forms.Panel();
            this.btnAbaProdutos = new System.Windows.Forms.Button();
            this.btnAbaCategorias = new System.Windows.Forms.Button();
            this.btnAbaFornecedores = new System.Windows.Forms.Button();

            this.pnlConteudoTabelas = new System.Windows.Forms.Panel();
            this.dgvProdutosFinanceiro = new System.Windows.Forms.DataGridView();
            this.dgvResumoCategoria = new System.Windows.Forms.DataGridView();
            this.dgvResumoFornecedor = new System.Windows.Forms.DataGridView();

            this.pnlTopo.SuspendLayout();
            this.pnlIndicadores.SuspendLayout();
            this.tblKpi.SuspendLayout();
            this.grpCustoEstoque.SuspendLayout();
            this.grpReceitaPotencial.SuspendLayout();
            this.grpLucroEstimado.SuspendLayout();
            this.grpMargemMedia.SuspendLayout();
            this.pnlFiltros.SuspendLayout();
            this.pnlAbas.SuspendLayout();
            this.pnlConteudoTabelas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProdutosFinanceiro)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResumoCategoria)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResumoFornecedor)).BeginInit();
            this.SuspendLayout();

            // 
            // pnlTopo
            // 
            this.pnlTopo.Controls.Add(this.lblTituloPrincipal);
            this.pnlTopo.Controls.Add(this.btnAtualizar);
            this.pnlTopo.Controls.Add(this.btnExportarCsv);
            this.pnlTopo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopo.Location = new System.Drawing.Point(0, 0);
            this.pnlTopo.Name = "pnlTopo";
            this.pnlTopo.Size = new System.Drawing.Size(960, 60);
            this.pnlTopo.TabIndex = 0;

            // 
            // lblTituloPrincipal
            // 
            this.lblTituloPrincipal.AutoSize = true;
            this.lblTituloPrincipal.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloPrincipal.Location = new System.Drawing.Point(20, 18);
            this.lblTituloPrincipal.Name = "lblTituloPrincipal";
            this.lblTituloPrincipal.Size = new System.Drawing.Size(340, 26);
            this.lblTituloPrincipal.TabIndex = 0;
            this.lblTituloPrincipal.Text = "Gestão Financeira e Estoque";

            // 
            // btnAtualizar
            // 
            this.btnAtualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAtualizar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAtualizar.Location = new System.Drawing.Point(640, 14);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(145, 34);
            this.btnAtualizar.TabIndex = 1;
            this.btnAtualizar.Text = "Atualizar Dados";
            this.btnAtualizar.UseVisualStyleBackColor = true;
            this.btnAtualizar.Click += new System.EventHandler(this.btnAtualizar_Click);

            // 
            // btnExportarCsv
            // 
            this.btnExportarCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportarCsv.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportarCsv.Location = new System.Drawing.Point(795, 14);
            this.btnExportarCsv.Name = "btnExportarCsv";
            this.btnExportarCsv.Size = new System.Drawing.Size(145, 34);
            this.btnExportarCsv.TabIndex = 2;
            this.btnExportarCsv.Text = "Exportar CSV";
            this.btnExportarCsv.UseVisualStyleBackColor = true;
            this.btnExportarCsv.Click += new System.EventHandler(this.btnExportarCsv_Click);

            // 
            // pnlIndicadores
            // 
            this.pnlIndicadores.Controls.Add(this.tblKpi);
            this.pnlIndicadores.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlIndicadores.Location = new System.Drawing.Point(0, 60);
            this.pnlIndicadores.Name = "pnlIndicadores";
            this.pnlIndicadores.Padding = new System.Windows.Forms.Padding(16, 4, 16, 4);
            this.pnlIndicadores.Size = new System.Drawing.Size(960, 115);
            this.pnlIndicadores.TabIndex = 1;
            // 
            // tblKpi
            // 
            this.tblKpi.ColumnCount = 4;
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.Controls.Add(this.grpCustoEstoque, 0, 0);
            this.tblKpi.Controls.Add(this.grpReceitaPotencial, 1, 0);
            this.tblKpi.Controls.Add(this.grpLucroEstimado, 2, 0);
            this.tblKpi.Controls.Add(this.grpMargemMedia, 3, 0);
            this.tblKpi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblKpi.Location = new System.Drawing.Point(16, 4);
            this.tblKpi.Name = "tblKpi";
            this.tblKpi.RowCount = 1;
            this.tblKpi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblKpi.Size = new System.Drawing.Size(928, 107);
            this.tblKpi.TabIndex = 0;
            // 
            // grpCustoEstoque
            // 
            this.grpCustoEstoque.Controls.Add(this.lblValorCustoEstoque);
            this.grpCustoEstoque.Controls.Add(this.lblDescCustoEstoque);
            this.grpCustoEstoque.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpCustoEstoque.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpCustoEstoque.Location = new System.Drawing.Point(4, 4);
            this.grpCustoEstoque.Margin = new System.Windows.Forms.Padding(4);
            this.grpCustoEstoque.Name = "grpCustoEstoque";
            this.grpCustoEstoque.Size = new System.Drawing.Size(224, 99);
            this.grpCustoEstoque.TabIndex = 0;
            this.grpCustoEstoque.TabStop = false;
            this.grpCustoEstoque.Text = "Custo em Estoque";
            // 
            // lblValorCustoEstoque
            // 
            this.lblValorCustoEstoque.AutoSize = true;
            this.lblValorCustoEstoque.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorCustoEstoque.Location = new System.Drawing.Point(15, 30);
            this.lblValorCustoEstoque.Name = "lblValorCustoEstoque";
            this.lblValorCustoEstoque.Size = new System.Drawing.Size(78, 25);
            this.lblValorCustoEstoque.TabIndex = 0;
            this.lblValorCustoEstoque.Text = "R$ 0,00";
            // 
            // lblDescCustoEstoque
            // 
            this.lblDescCustoEstoque.AutoSize = true;
            this.lblDescCustoEstoque.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescCustoEstoque.Location = new System.Drawing.Point(15, 62);
            this.lblDescCustoEstoque.Name = "lblDescCustoEstoque";
            this.lblDescCustoEstoque.Size = new System.Drawing.Size(167, 15);
            this.lblDescCustoEstoque.TabIndex = 1;
            this.lblDescCustoEstoque.Text = "Total investido em mercadorias";
            // 
            // grpReceitaPotencial
            // 
            this.grpReceitaPotencial.Controls.Add(this.lblValorReceitaPotencial);
            this.grpReceitaPotencial.Controls.Add(this.lblDescReceitaPotencial);
            this.grpReceitaPotencial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpReceitaPotencial.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpReceitaPotencial.Location = new System.Drawing.Point(236, 4);
            this.grpReceitaPotencial.Margin = new System.Windows.Forms.Padding(4);
            this.grpReceitaPotencial.Name = "grpReceitaPotencial";
            this.grpReceitaPotencial.Size = new System.Drawing.Size(224, 99);
            this.grpReceitaPotencial.TabIndex = 1;
            this.grpReceitaPotencial.TabStop = false;
            this.grpReceitaPotencial.Text = "Receita Potencial";
            // 
            // lblValorReceitaPotencial
            // 
            this.lblValorReceitaPotencial.AutoSize = true;
            this.lblValorReceitaPotencial.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorReceitaPotencial.Location = new System.Drawing.Point(15, 30);
            this.lblValorReceitaPotencial.Name = "lblValorReceitaPotencial";
            this.lblValorReceitaPotencial.Size = new System.Drawing.Size(78, 25);
            this.lblValorReceitaPotencial.TabIndex = 0;
            this.lblValorReceitaPotencial.Text = "R$ 0,00";
            // 
            // lblDescReceitaPotencial
            // 
            this.lblDescReceitaPotencial.AutoSize = true;
            this.lblDescReceitaPotencial.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescReceitaPotencial.Location = new System.Drawing.Point(15, 62);
            this.lblDescReceitaPotencial.Name = "lblDescReceitaPotencial";
            this.lblDescReceitaPotencial.Size = new System.Drawing.Size(142, 15);
            this.lblDescReceitaPotencial.TabIndex = 1;
            this.lblDescReceitaPotencial.Text = "Valor bruto de venda total";
            // 
            // grpLucroEstimado
            // 
            this.grpLucroEstimado.Controls.Add(this.lblValorLucroEstimado);
            this.grpLucroEstimado.Controls.Add(this.lblDescLucroEstimado);
            this.grpLucroEstimado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpLucroEstimado.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpLucroEstimado.Location = new System.Drawing.Point(468, 4);
            this.grpLucroEstimado.Margin = new System.Windows.Forms.Padding(4);
            this.grpLucroEstimado.Name = "grpLucroEstimado";
            this.grpLucroEstimado.Size = new System.Drawing.Size(224, 99);
            this.grpLucroEstimado.TabIndex = 2;
            this.grpLucroEstimado.TabStop = false;
            this.grpLucroEstimado.Text = "Lucro Projetado";
            // 
            // lblValorLucroEstimado
            // 
            this.lblValorLucroEstimado.AutoSize = true;
            this.lblValorLucroEstimado.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorLucroEstimado.Location = new System.Drawing.Point(15, 30);
            this.lblValorLucroEstimado.Name = "lblValorLucroEstimado";
            this.lblValorLucroEstimado.Size = new System.Drawing.Size(78, 25);
            this.lblValorLucroEstimado.TabIndex = 0;
            this.lblValorLucroEstimado.Text = "R$ 0,00";
            // 
            // lblDescLucroEstimado
            // 
            this.lblDescLucroEstimado.AutoSize = true;
            this.lblDescLucroEstimado.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescLucroEstimado.Location = new System.Drawing.Point(15, 62);
            this.lblDescLucroEstimado.Name = "lblDescLucroEstimado";
            this.lblDescLucroEstimado.Size = new System.Drawing.Size(166, 15);
            this.lblDescLucroEstimado.TabIndex = 1;
            this.lblDescLucroEstimado.Text = "Receita potencial menos custo";
            // 
            // grpMargemMedia
            // 
            this.grpMargemMedia.Controls.Add(this.lblValorMargemMedia);
            this.grpMargemMedia.Controls.Add(this.lblDescMargemMedia);
            this.grpMargemMedia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpMargemMedia.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpMargemMedia.Location = new System.Drawing.Point(700, 4);
            this.grpMargemMedia.Margin = new System.Windows.Forms.Padding(4);
            this.grpMargemMedia.Name = "grpMargemMedia";
            this.grpMargemMedia.Size = new System.Drawing.Size(224, 99);
            this.grpMargemMedia.TabIndex = 3;
            this.grpMargemMedia.TabStop = false;
            this.grpMargemMedia.Text = "Margem Média";
            // 
            // lblValorMargemMedia
            // 
            this.lblValorMargemMedia.AutoSize = true;
            this.lblValorMargemMedia.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorMargemMedia.Location = new System.Drawing.Point(15, 30);
            this.lblValorMargemMedia.Name = "lblValorMargemMedia";
            this.lblValorMargemMedia.Size = new System.Drawing.Size(59, 25);
            this.lblValorMargemMedia.TabIndex = 0;
            this.lblValorMargemMedia.Text = "0,0%";
            // 
            // lblDescMargemMedia
            // 
            this.lblDescMargemMedia.AutoSize = true;
            this.lblDescMargemMedia.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescMargemMedia.Location = new System.Drawing.Point(15, 62);
            this.lblDescMargemMedia.Name = "lblDescMargemMedia";
            this.lblDescMargemMedia.Size = new System.Drawing.Size(175, 15);
            this.lblDescMargemMedia.TabIndex = 1;
            this.lblDescMargemMedia.Text = "Rentabilidade média sobre custo";

            // 
            // pnlFiltros
            // 
            this.pnlFiltros.Controls.Add(this.lblFiltroCategoria);
            this.pnlFiltros.Controls.Add(this.cmbCategoria);
            this.pnlFiltros.Controls.Add(this.lblBusca);
            this.pnlFiltros.Controls.Add(this.txtBuscaProduto);
            this.pnlFiltros.Controls.Add(this.btnFiltrar);
            this.pnlFiltros.Controls.Add(this.btnLimparFiltros);
            this.pnlFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFiltros.Location = new System.Drawing.Point(0, 175);
            this.pnlFiltros.Name = "pnlFiltros";
            this.pnlFiltros.Size = new System.Drawing.Size(960, 50);
            this.pnlFiltros.TabIndex = 2;

            // 
            // lblFiltroCategoria
            // 
            this.lblFiltroCategoria.AutoSize = true;
            this.lblFiltroCategoria.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFiltroCategoria.Location = new System.Drawing.Point(20, 15);
            this.lblFiltroCategoria.Name = "lblFiltroCategoria";
            this.lblFiltroCategoria.Size = new System.Drawing.Size(68, 16);
            this.lblFiltroCategoria.TabIndex = 0;
            this.lblFiltroCategoria.Text = "Categoria:";

            // 
            // cmbCategoria
            // 
            this.cmbCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategoria.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCategoria.FormattingEnabled = true;
            this.cmbCategoria.Location = new System.Drawing.Point(94, 12);
            this.cmbCategoria.Name = "cmbCategoria";
            this.cmbCategoria.Size = new System.Drawing.Size(180, 24);
            this.cmbCategoria.TabIndex = 1;

            // 
            // lblBusca
            // 
            this.lblBusca.AutoSize = true;
            this.lblBusca.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBusca.Location = new System.Drawing.Point(294, 15);
            this.lblBusca.Name = "lblBusca";
            this.lblBusca.Size = new System.Drawing.Size(55, 16);
            this.lblBusca.TabIndex = 2;
            this.lblBusca.Text = "Produto:";

            // 
            // txtBuscaProduto
            // 
            this.txtBuscaProduto.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBuscaProduto.Location = new System.Drawing.Point(356, 12);
            this.txtBuscaProduto.Name = "txtBuscaProduto";
            this.txtBuscaProduto.Size = new System.Drawing.Size(230, 25);
            this.txtBuscaProduto.TabIndex = 3;

            // 
            // btnFiltrar
            // 
            this.btnFiltrar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFiltrar.Location = new System.Drawing.Point(600, 10);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(105, 28);
            this.btnFiltrar.TabIndex = 4;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = true;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);

            // 
            // btnLimparFiltros
            // 
            this.btnLimparFiltros.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimparFiltros.Location = new System.Drawing.Point(714, 10);
            this.btnLimparFiltros.Name = "btnLimparFiltros";
            this.btnLimparFiltros.Size = new System.Drawing.Size(105, 28);
            this.btnLimparFiltros.TabIndex = 5;
            this.btnLimparFiltros.Text = "Limpar";
            this.btnLimparFiltros.UseVisualStyleBackColor = true;
            this.btnLimparFiltros.Click += new System.EventHandler(this.btnLimparFiltros_Click);

            // 
            // pnlAbas
            // 
            this.pnlAbas.Controls.Add(this.btnAbaProdutos);
            this.pnlAbas.Controls.Add(this.btnAbaCategorias);
            this.pnlAbas.Controls.Add(this.btnAbaFornecedores);
            this.pnlAbas.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAbas.Location = new System.Drawing.Point(0, 225);
            this.pnlAbas.Name = "pnlAbas";
            this.pnlAbas.Size = new System.Drawing.Size(960, 42);
            this.pnlAbas.TabIndex = 3;

            // 
            // btnAbaProdutos
            // 
            this.btnAbaProdutos.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbaProdutos.Location = new System.Drawing.Point(20, 4);
            this.btnAbaProdutos.Name = "btnAbaProdutos";
            this.btnAbaProdutos.Size = new System.Drawing.Size(160, 34);
            this.btnAbaProdutos.TabIndex = 0;
            this.btnAbaProdutos.Tag = "aba_ativa";
            this.btnAbaProdutos.Text = "Por Produto";
            this.btnAbaProdutos.UseVisualStyleBackColor = true;
            this.btnAbaProdutos.Click += new System.EventHandler(this.btnAbaProdutos_Click);

            // 
            // btnAbaCategorias
            // 
            this.btnAbaCategorias.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbaCategorias.Location = new System.Drawing.Point(186, 4);
            this.btnAbaCategorias.Name = "btnAbaCategorias";
            this.btnAbaCategorias.Size = new System.Drawing.Size(160, 34);
            this.btnAbaCategorias.TabIndex = 1;
            this.btnAbaCategorias.Text = "Por Categoria";
            this.btnAbaCategorias.UseVisualStyleBackColor = true;
            this.btnAbaCategorias.Click += new System.EventHandler(this.btnAbaCategorias_Click);

            // 
            // btnAbaFornecedores
            // 
            this.btnAbaFornecedores.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbaFornecedores.Location = new System.Drawing.Point(352, 4);
            this.btnAbaFornecedores.Name = "btnAbaFornecedores";
            this.btnAbaFornecedores.Size = new System.Drawing.Size(160, 34);
            this.btnAbaFornecedores.TabIndex = 2;
            this.btnAbaFornecedores.Text = "Por Fornecedor";
            this.btnAbaFornecedores.UseVisualStyleBackColor = true;
            this.btnAbaFornecedores.Click += new System.EventHandler(this.btnAbaFornecedores_Click);

            // 
            // pnlConteudoTabelas
            // 
            this.pnlConteudoTabelas.Controls.Add(this.dgvProdutosFinanceiro);
            this.pnlConteudoTabelas.Controls.Add(this.dgvResumoCategoria);
            this.pnlConteudoTabelas.Controls.Add(this.dgvResumoFornecedor);
            this.pnlConteudoTabelas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlConteudoTabelas.Location = new System.Drawing.Point(0, 267);
            this.pnlConteudoTabelas.Name = "pnlConteudoTabelas";
            this.pnlConteudoTabelas.Padding = new System.Windows.Forms.Padding(20, 5, 20, 15);
            this.pnlConteudoTabelas.Size = new System.Drawing.Size(960, 383);
            this.pnlConteudoTabelas.TabIndex = 4;

            // 
            // dgvProdutosFinanceiro
            // 
            this.dgvProdutosFinanceiro.AllowUserToAddRows = false;
            this.dgvProdutosFinanceiro.AllowUserToDeleteRows = false;
            this.dgvProdutosFinanceiro.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProdutosFinanceiro.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProdutosFinanceiro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProdutosFinanceiro.Location = new System.Drawing.Point(20, 5);
            this.dgvProdutosFinanceiro.Name = "dgvProdutosFinanceiro";
            this.dgvProdutosFinanceiro.ReadOnly = true;
            this.dgvProdutosFinanceiro.RowHeadersVisible = false;
            this.dgvProdutosFinanceiro.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProdutosFinanceiro.Size = new System.Drawing.Size(920, 363);
            this.dgvProdutosFinanceiro.TabIndex = 0;

            // 
            // dgvResumoCategoria
            // 
            this.dgvResumoCategoria.AllowUserToAddRows = false;
            this.dgvResumoCategoria.AllowUserToDeleteRows = false;
            this.dgvResumoCategoria.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvResumoCategoria.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResumoCategoria.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResumoCategoria.Location = new System.Drawing.Point(20, 5);
            this.dgvResumoCategoria.Name = "dgvResumoCategoria";
            this.dgvResumoCategoria.ReadOnly = true;
            this.dgvResumoCategoria.RowHeadersVisible = false;
            this.dgvResumoCategoria.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResumoCategoria.Size = new System.Drawing.Size(920, 363);
            this.dgvResumoCategoria.TabIndex = 1;
            this.dgvResumoCategoria.Visible = false;

            // 
            // dgvResumoFornecedor
            // 
            this.dgvResumoFornecedor.AllowUserToAddRows = false;
            this.dgvResumoFornecedor.AllowUserToDeleteRows = false;
            this.dgvResumoFornecedor.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvResumoFornecedor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResumoFornecedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResumoFornecedor.Location = new System.Drawing.Point(20, 5);
            this.dgvResumoFornecedor.Name = "dgvResumoFornecedor";
            this.dgvResumoFornecedor.ReadOnly = true;
            this.dgvResumoFornecedor.RowHeadersVisible = false;
            this.dgvResumoFornecedor.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResumoFornecedor.Size = new System.Drawing.Size(920, 363);
            this.dgvResumoFornecedor.TabIndex = 2;
            this.dgvResumoFornecedor.Visible = false;

            // 
            // FormFinanceiro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 650);
            this.Controls.Add(this.pnlConteudoTabelas);
            this.Controls.Add(this.pnlAbas);
            this.Controls.Add(this.pnlFiltros);
            this.Controls.Add(this.pnlIndicadores);
            this.Controls.Add(this.pnlTopo);
            this.Name = "FormFinanceiro";
            this.Text = "Financeiro";
            this.Load += new System.EventHandler(this.FormFinanceiro_Load);

            this.pnlTopo.ResumeLayout(false);
            this.pnlTopo.PerformLayout();
            this.pnlIndicadores.ResumeLayout(false);
            this.grpCustoEstoque.ResumeLayout(false);
            this.grpCustoEstoque.PerformLayout();
            this.grpReceitaPotencial.ResumeLayout(false);
            this.grpReceitaPotencial.PerformLayout();
            this.grpLucroEstimado.ResumeLayout(false);
            this.grpLucroEstimado.PerformLayout();
            this.grpMargemMedia.ResumeLayout(false);
            this.grpMargemMedia.PerformLayout();
            this.pnlFiltros.ResumeLayout(false);
            this.pnlFiltros.PerformLayout();
            this.tblKpi.ResumeLayout(false);
            this.pnlAbas.ResumeLayout(false);
            this.pnlConteudoTabelas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProdutosFinanceiro)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResumoCategoria)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResumoFornecedor)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlTopo;
        private System.Windows.Forms.Label lblTituloPrincipal;
        private System.Windows.Forms.Button btnAtualizar;
        private System.Windows.Forms.Button btnExportarCsv;

        private System.Windows.Forms.Panel pnlIndicadores;
        private System.Windows.Forms.TableLayoutPanel tblKpi;
        private System.Windows.Forms.GroupBox grpCustoEstoque;
        private System.Windows.Forms.Label lblValorCustoEstoque;
        private System.Windows.Forms.Label lblDescCustoEstoque;

        private System.Windows.Forms.GroupBox grpReceitaPotencial;
        private System.Windows.Forms.Label lblValorReceitaPotencial;
        private System.Windows.Forms.Label lblDescReceitaPotencial;

        private System.Windows.Forms.GroupBox grpLucroEstimado;
        private System.Windows.Forms.Label lblValorLucroEstimado;
        private System.Windows.Forms.Label lblDescLucroEstimado;

        private System.Windows.Forms.GroupBox grpMargemMedia;
        private System.Windows.Forms.Label lblValorMargemMedia;
        private System.Windows.Forms.Label lblDescMargemMedia;

        private System.Windows.Forms.Panel pnlFiltros;
        private System.Windows.Forms.Label lblFiltroCategoria;
        private System.Windows.Forms.ComboBox cmbCategoria;
        private System.Windows.Forms.Label lblBusca;
        private System.Windows.Forms.TextBox txtBuscaProduto;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.Button btnLimparFiltros;

        private System.Windows.Forms.Panel pnlAbas;
        private System.Windows.Forms.Button btnAbaProdutos;
        private System.Windows.Forms.Button btnAbaCategorias;
        private System.Windows.Forms.Button btnAbaFornecedores;

        private System.Windows.Forms.Panel pnlConteudoTabelas;
        private System.Windows.Forms.DataGridView dgvProdutosFinanceiro;
        private System.Windows.Forms.DataGridView dgvResumoCategoria;
        private System.Windows.Forms.DataGridView dgvResumoFornecedor;
    }
}
