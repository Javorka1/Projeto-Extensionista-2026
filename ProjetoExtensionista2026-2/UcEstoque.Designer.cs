namespace ProjetoExtensionista2026_2
{
    partial class UcEstoque
    {
        /// <summary> 
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer de Componentes

        /// <summary> 
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanelProdutos = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.searchBarPanel = new System.Windows.Forms.Panel();
            this.filterBtn = new System.Windows.Forms.Button();
            this.searchBtn = new System.Windows.Forms.Button();
            this.searchTxtBox = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCadastrar = new System.Windows.Forms.Button();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.searchBarPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanelProdutos
            // 
            this.flowLayoutPanelProdutos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelProdutos.Location = new System.Drawing.Point(3, 111);
            this.flowLayoutPanelProdutos.Name = "flowLayoutPanelProdutos";
            this.flowLayoutPanelProdutos.Padding = new System.Windows.Forms.Padding(25, 0, 25, 0);
            this.flowLayoutPanelProdutos.Size = new System.Drawing.Size(1914, 858);
            this.flowLayoutPanelProdutos.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanelProdutos, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1920, 1080);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.searchBarPanel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(25, 20, 25, 20);
            this.panel1.Size = new System.Drawing.Size(1914, 102);
            this.panel1.TabIndex = 1;
            // 
            // searchBarPanel
            // 
            this.searchBarPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchBarPanel.Controls.Add(this.filterBtn);
            this.searchBarPanel.Controls.Add(this.searchBtn);
            this.searchBarPanel.Controls.Add(this.searchTxtBox);
            this.searchBarPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchBarPanel.Location = new System.Drawing.Point(25, 20);
            this.searchBarPanel.Name = "searchBarPanel";
            this.searchBarPanel.Size = new System.Drawing.Size(1864, 51);
            this.searchBarPanel.TabIndex = 4;
            // 
            // filterBtn
            // 
            this.filterBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.filterBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.filterBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterBtn.Location = new System.Drawing.Point(1728, 0);
            this.filterBtn.Name = "filterBtn";
            this.filterBtn.Size = new System.Drawing.Size(66, 49);
            this.filterBtn.TabIndex = 1;
            this.filterBtn.Text = "Filtros";
            this.filterBtn.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterBtn.UseVisualStyleBackColor = true;
            // 
            // searchBtn
            // 
            this.searchBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.searchBtn.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchBtn.Location = new System.Drawing.Point(1794, 0);
            this.searchBtn.Name = "searchBtn";
            this.searchBtn.Size = new System.Drawing.Size(68, 49);
            this.searchBtn.TabIndex = 2;
            this.searchBtn.Text = "Buscar";
            this.searchBtn.UseVisualStyleBackColor = true;
            // 
            // searchTxtBox
            // 
            this.searchTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchTxtBox.Location = new System.Drawing.Point(0, 0);
            this.searchTxtBox.Multiline = true;
            this.searchTxtBox.Name = "searchTxtBox";
            this.searchTxtBox.Size = new System.Drawing.Size(1722, 49);
            this.searchTxtBox.TabIndex = 0;
            this.searchTxtBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchTxtBox_KeyDown);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCadastrar);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 975);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1914, 102);
            this.panel2.TabIndex = 2;
            // 
            // btnCadastrar
            // 
            this.btnCadastrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCadastrar.Location = new System.Drawing.Point(1738, 13);
            this.btnCadastrar.Name = "btnCadastrar";
            this.btnCadastrar.Size = new System.Drawing.Size(150, 75);
            this.btnCadastrar.TabIndex = 0;
            this.btnCadastrar.Text = "Cadastrar";
            this.btnCadastrar.UseVisualStyleBackColor = true;
            this.btnCadastrar.Click += new System.EventHandler(this.btnCadastrar_Click);
            // 
            // UcEstoque
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "UcEstoque";
            this.Size = new System.Drawing.Size(1920, 1080);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.searchBarPanel.ResumeLayout(false);
            this.searchBarPanel.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelProdutos;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel searchBarPanel;
        private System.Windows.Forms.Button filterBtn;
        private System.Windows.Forms.Button searchBtn;
        private System.Windows.Forms.TextBox searchTxtBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCadastrar;
    }
}
