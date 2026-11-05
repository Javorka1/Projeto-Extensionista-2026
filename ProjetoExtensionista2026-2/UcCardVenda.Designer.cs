namespace ProjetoExtensionista2026_2
{
    partial class UcCardVenda
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDiminuir = new System.Windows.Forms.Button();
            this.btnAumentar = new System.Windows.Forms.Button();
            this.btnExcluir = new System.Windows.Forms.Button();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.lblNomeProduto = new System.Windows.Forms.Label();
            this.picFoto = new System.Windows.Forms.PictureBox();
            this.lblQuantidade = new System.Windows.Forms.Label();
            this.lblPrecoTotal = new System.Windows.Forms.Label();
            this.lblPrecoUnitario = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFoto)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.btnDiminuir);
            this.panel1.Controls.Add(this.btnAumentar);
            this.panel1.Controls.Add(this.btnExcluir);
            this.panel1.Controls.Add(this.lblCodigo);
            this.panel1.Controls.Add(this.lblNomeProduto);
            this.panel1.Controls.Add(this.picFoto);
            this.panel1.Controls.Add(this.lblQuantidade);
            this.panel1.Controls.Add(this.lblPrecoTotal);
            this.panel1.Controls.Add(this.lblPrecoUnitario);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.panel1.Size = new System.Drawing.Size(546, 60);
            this.panel1.TabIndex = 0;
            // 
            // btnDiminuir
            // 
            this.btnDiminuir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDiminuir.Location = new System.Drawing.Point(313, 27);
            this.btnDiminuir.Name = "btnDiminuir";
            this.btnDiminuir.Size = new System.Drawing.Size(26, 20);
            this.btnDiminuir.TabIndex = 9;
            this.btnDiminuir.Text = "-";
            this.btnDiminuir.UseVisualStyleBackColor = true;
            this.btnDiminuir.Click += new System.EventHandler(this.btnDiminuir_Click);
            // 
            // btnAumentar
            // 
            this.btnAumentar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAumentar.Location = new System.Drawing.Point(383, 27);
            this.btnAumentar.Name = "btnAumentar";
            this.btnAumentar.Size = new System.Drawing.Size(26, 20);
            this.btnAumentar.TabIndex = 8;
            this.btnAumentar.Text = "+";
            this.btnAumentar.UseVisualStyleBackColor = true;
            this.btnAumentar.Click += new System.EventHandler(this.btnAumentar_Click);
            // 
            // btnExcluir
            // 
            this.btnExcluir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcluir.FlatAppearance.BorderSize = 0;
            this.btnExcluir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcluir.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExcluir.Location = new System.Drawing.Point(524, 3);
            this.btnExcluir.Name = "btnExcluir";
            this.btnExcluir.Size = new System.Drawing.Size(24, 25);
            this.btnExcluir.TabIndex = 4;
            this.btnExcluir.Text = "X";
            this.btnExcluir.UseVisualStyleBackColor = true;
            this.btnExcluir.Click += new System.EventHandler(this.btnExcluir_Click);
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodigo.Location = new System.Drawing.Point(63, 29);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(51, 16);
            this.lblCodigo.TabIndex = 3;
            this.lblCodigo.Text = "Código";
            // 
            // lblNomeProduto
            // 
            this.lblNomeProduto.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNomeProduto.AutoEllipsis = true;
            this.lblNomeProduto.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNomeProduto.Location = new System.Drawing.Point(62, 5);
            this.lblNomeProduto.MaximumSize = new System.Drawing.Size(230, 20);
            this.lblNomeProduto.MinimumSize = new System.Drawing.Size(140, 20);
            this.lblNomeProduto.Name = "lblNomeProduto";
            this.lblNomeProduto.Size = new System.Drawing.Size(165, 20);
            this.lblNomeProduto.TabIndex = 2;
            this.lblNomeProduto.Text = "Produto ...";
            // 
            // picFoto
            // 
            this.picFoto.ErrorImage = null;
            this.picFoto.Location = new System.Drawing.Point(5, 5);
            this.picFoto.Name = "picFoto";
            this.picFoto.Size = new System.Drawing.Size(56, 48);
            this.picFoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picFoto.TabIndex = 1;
            this.picFoto.TabStop = false;
            // 
            // lblQuantidade
            // 
            this.lblQuantidade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblQuantidade.BackColor = System.Drawing.Color.Transparent;
            this.lblQuantidade.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuantidade.Location = new System.Drawing.Point(336, 29);
            this.lblQuantidade.Name = "lblQuantidade";
            this.lblQuantidade.Size = new System.Drawing.Size(51, 16);
            this.lblQuantidade.TabIndex = 10;
            this.lblQuantidade.Text = "123";
            this.lblQuantidade.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPrecoTotal
            // 
            this.lblPrecoTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPrecoTotal.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrecoTotal.Location = new System.Drawing.Point(406, 27);
            this.lblPrecoTotal.Name = "lblPrecoTotal";
            this.lblPrecoTotal.Size = new System.Drawing.Size(106, 20);
            this.lblPrecoTotal.TabIndex = 5;
            this.lblPrecoTotal.Text = "R$ 00,00";
            this.lblPrecoTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPrecoUnitario
            // 
            this.lblPrecoUnitario.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPrecoUnitario.BackColor = System.Drawing.Color.Transparent;
            this.lblPrecoUnitario.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrecoUnitario.Location = new System.Drawing.Point(211, 27);
            this.lblPrecoUnitario.Name = "lblPrecoUnitario";
            this.lblPrecoUnitario.Size = new System.Drawing.Size(106, 20);
            this.lblPrecoUnitario.TabIndex = 7;
            this.lblPrecoUnitario.Text = "R$ 00,00";
            this.lblPrecoUnitario.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlText;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(5, 59);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(536, 1);
            this.panel2.TabIndex = 11;
            // 
            // UcCardVenda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "UcCardVenda";
            this.Size = new System.Drawing.Size(546, 60);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFoto)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblNomeProduto;
        private System.Windows.Forms.PictureBox picFoto;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.Label lblPrecoTotal;
        private System.Windows.Forms.Button btnExcluir;
        private System.Windows.Forms.Label lblPrecoUnitario;
        private System.Windows.Forms.Button btnDiminuir;
        private System.Windows.Forms.Button btnAumentar;
        private System.Windows.Forms.Label lblQuantidade;
        private System.Windows.Forms.Panel panel2;
    }
}
