namespace ProjetoExtensionista2026_2
{
    partial class UcCardProduto
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
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblDescricao = new System.Windows.Forms.Label();
            this.btnAdicionar = new System.Windows.Forms.Button();
            this.UpDownProduto = new System.Windows.Forms.NumericUpDown();
            this.lblPrecoVenda = new System.Windows.Forms.Label();
            this.picFoto = new System.Windows.Forms.PictureBox();
            this.lblEstoque = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.lblOculto = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownProduto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFoto)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoEllipsis = true;
            this.lblTitulo.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(122, 27);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Padding = new System.Windows.Forms.Padding(2);
            this.lblTitulo.Size = new System.Drawing.Size(155, 78);
            this.lblTitulo.TabIndex = 1;
            this.lblTitulo.Text = "Produto ...";
            // 
            // lblDescricao
            // 
            this.lblDescricao.AutoEllipsis = true;
            this.lblDescricao.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescricao.Location = new System.Drawing.Point(3, 150);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Padding = new System.Windows.Forms.Padding(2);
            this.lblDescricao.Size = new System.Drawing.Size(274, 62);
            this.lblDescricao.TabIndex = 2;
            this.lblDescricao.Text = "Descricao ...";
            // 
            // btnAdicionar
            // 
            this.btnAdicionar.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnAdicionar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdicionar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdicionar.Location = new System.Drawing.Point(84, 212);
            this.btnAdicionar.Name = "btnAdicionar";
            this.btnAdicionar.Size = new System.Drawing.Size(190, 29);
            this.btnAdicionar.TabIndex = 3;
            this.btnAdicionar.Text = "Adicionar";
            this.btnAdicionar.UseVisualStyleBackColor = false;
            this.btnAdicionar.Click += new System.EventHandler(this.btnAdicionar_Click);
            // 
            // UpDownProduto
            // 
            this.UpDownProduto.BackColor = System.Drawing.SystemColors.ControlLight;
            this.UpDownProduto.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpDownProduto.Location = new System.Drawing.Point(6, 212);
            this.UpDownProduto.Name = "UpDownProduto";
            this.UpDownProduto.Size = new System.Drawing.Size(72, 29);
            this.UpDownProduto.TabIndex = 4;
            this.UpDownProduto.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblPrecoVenda
            // 
            this.lblPrecoVenda.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrecoVenda.ForeColor = System.Drawing.Color.LimeGreen;
            this.lblPrecoVenda.Location = new System.Drawing.Point(129, 105);
            this.lblPrecoVenda.Name = "lblPrecoVenda";
            this.lblPrecoVenda.Size = new System.Drawing.Size(148, 18);
            this.lblPrecoVenda.TabIndex = 5;
            this.lblPrecoVenda.Text = "R$ 00,00";
            this.lblPrecoVenda.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picFoto
            // 
            this.picFoto.ErrorImage = null;
            this.picFoto.Location = new System.Drawing.Point(6, 30);
            this.picFoto.Name = "picFoto";
            this.picFoto.Size = new System.Drawing.Size(110, 110);
            this.picFoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picFoto.TabIndex = 0;
            this.picFoto.TabStop = false;
            // 
            // lblEstoque
            // 
            this.lblEstoque.BackColor = System.Drawing.Color.PaleGreen;
            this.lblEstoque.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstoque.Location = new System.Drawing.Point(168, 0);
            this.lblEstoque.Name = "lblEstoque";
            this.lblEstoque.Size = new System.Drawing.Size(113, 24);
            this.lblEstoque.TabIndex = 7;
            this.lblEstoque.Text = "Estoque: ";
            this.lblEstoque.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel1.Controls.Add(this.lblCodigo);
            this.panel1.Controls.Add(this.lblEstoque);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(280, 24);
            this.panel1.TabIndex = 8;
            // 
            // lblCodigo
            // 
            this.lblCodigo.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.lblCodigo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodigo.Location = new System.Drawing.Point(-1, 0);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(169, 24);
            this.lblCodigo.TabIndex = 8;
            this.lblCodigo.Text = "Código";
            this.lblCodigo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOculto
            // 
            this.lblOculto.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblOculto.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOculto.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblOculto.Location = new System.Drawing.Point(129, 127);
            this.lblOculto.Name = "lblOculto";
            this.lblOculto.Size = new System.Drawing.Size(148, 20);
            this.lblOculto.TabIndex = 9;
            this.lblOculto.Text = "Mostrar ocultados...";
            this.lblOculto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblOculto.Click += new System.EventHandler(this.btnOculto_Click);
            // 
            // UcCardProduto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblOculto);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblPrecoVenda);
            this.Controls.Add(this.UpDownProduto);
            this.Controls.Add(this.btnAdicionar);
            this.Controls.Add(this.lblDescricao);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.picFoto);
            this.Name = "UcCardProduto";
            this.Size = new System.Drawing.Size(280, 250);
            ((System.ComponentModel.ISupportInitialize)(this.UpDownProduto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFoto)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblDescricao;
        private System.Windows.Forms.Button btnAdicionar;
        private System.Windows.Forms.NumericUpDown UpDownProduto;
        private System.Windows.Forms.Label lblPrecoVenda;
        private System.Windows.Forms.PictureBox picFoto;
        private System.Windows.Forms.Label lblEstoque;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.Label lblOculto;
    }
}
