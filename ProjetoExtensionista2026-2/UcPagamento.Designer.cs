namespace ProjetoExtensionista2026_2
{
    partial class UcPagamento
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
            this.btnDinheiro = new System.Windows.Forms.Button();
            this.btnDebito = new System.Windows.Forms.Button();
            this.btnCredito = new System.Windows.Forms.Button();
            this.btnPix = new System.Windows.Forms.Button();
            this.btnCarne = new System.Windows.Forms.Button();
            this.gpbLancamentoValores = new System.Windows.Forms.GroupBox();
            this.btn1000 = new System.Windows.Forms.Button();
            this.btn100 = new System.Windows.Forms.Button();
            this.btn10 = new System.Windows.Forms.Button();
            this.btnExato = new System.Windows.Forms.Button();
            this.txtPagamento = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblParcelas = new System.Windows.Forms.Label();
            this.cmbParcelas = new System.Windows.Forms.ComboBox();
            this.btnAdicionarPagamento = new System.Windows.Forms.Button();
            this.gpbLancamentoValores.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDinheiro
            // 
            this.btnDinheiro.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDinheiro.Location = new System.Drawing.Point(0, 19);
            this.btnDinheiro.Name = "btnDinheiro";
            this.btnDinheiro.Size = new System.Drawing.Size(170, 60);
            this.btnDinheiro.TabIndex = 0;
            this.btnDinheiro.Tag = "Dinheiro";
            this.btnDinheiro.Text = "💸 Dinheiro";
            this.btnDinheiro.UseVisualStyleBackColor = true;
            // 
            // btnDebito
            // 
            this.btnDebito.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDebito.Location = new System.Drawing.Point(0, 85);
            this.btnDebito.Name = "btnDebito";
            this.btnDebito.Size = new System.Drawing.Size(170, 60);
            this.btnDebito.TabIndex = 1;
            this.btnDebito.Tag = "Cartão de Débito";
            this.btnDebito.Text = "💳 Débito";
            this.btnDebito.UseVisualStyleBackColor = true;
            // 
            // btnCredito
            // 
            this.btnCredito.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnCredito.Location = new System.Drawing.Point(0, 283);
            this.btnCredito.Name = "btnCredito";
            this.btnCredito.Size = new System.Drawing.Size(170, 60);
            this.btnCredito.TabIndex = 2;
            this.btnCredito.Tag = "Cartão de Crédito";
            this.btnCredito.Text = "💳 Crédito";
            this.btnCredito.UseVisualStyleBackColor = true;
            // 
            // btnPix
            // 
            this.btnPix.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnPix.Location = new System.Drawing.Point(0, 151);
            this.btnPix.Name = "btnPix";
            this.btnPix.Size = new System.Drawing.Size(170, 60);
            this.btnPix.TabIndex = 3;
            this.btnPix.Tag = "Pix";
            this.btnPix.Text = "❖ Pix";
            this.btnPix.UseVisualStyleBackColor = true;
            // 
            // btnCarne
            // 
            this.btnCarne.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnCarne.Location = new System.Drawing.Point(0, 217);
            this.btnCarne.Name = "btnCarne";
            this.btnCarne.Size = new System.Drawing.Size(170, 60);
            this.btnCarne.TabIndex = 4;
            this.btnCarne.Tag = "Carnê";
            this.btnCarne.Text = "📋 Carnê";
            this.btnCarne.UseVisualStyleBackColor = true;
            // 
            // gpbLancamentoValores
            // 
            this.gpbLancamentoValores.Controls.Add(this.btnAdicionarPagamento);
            this.gpbLancamentoValores.Controls.Add(this.cmbParcelas);
            this.gpbLancamentoValores.Controls.Add(this.lblParcelas);
            this.gpbLancamentoValores.Controls.Add(this.btn1000);
            this.gpbLancamentoValores.Controls.Add(this.btn100);
            this.gpbLancamentoValores.Controls.Add(this.btn10);
            this.gpbLancamentoValores.Controls.Add(this.btnExato);
            this.gpbLancamentoValores.Controls.Add(this.txtPagamento);
            this.gpbLancamentoValores.Controls.Add(this.label1);
            this.gpbLancamentoValores.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpbLancamentoValores.Location = new System.Drawing.Point(205, 19);
            this.gpbLancamentoValores.Name = "gpbLancamentoValores";
            this.gpbLancamentoValores.Size = new System.Drawing.Size(307, 324);
            this.gpbLancamentoValores.TabIndex = 5;
            this.gpbLancamentoValores.TabStop = false;
            this.gpbLancamentoValores.Text = "Lançamento de Valores";
            // 
            // btn1000
            // 
            this.btn1000.Font = new System.Drawing.Font("Segoe UI Light", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn1000.Location = new System.Drawing.Point(223, 103);
            this.btn1000.Name = "btn1000";
            this.btn1000.Size = new System.Drawing.Size(54, 23);
            this.btn1000.TabIndex = 7;
            this.btn1000.Tag = "1000";
            this.btn1000.Text = "R$ 1000";
            this.btn1000.UseVisualStyleBackColor = true;
            // 
            // btn100
            // 
            this.btn100.Font = new System.Drawing.Font("Segoe UI Light", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn100.Location = new System.Drawing.Point(163, 103);
            this.btn100.Name = "btn100";
            this.btn100.Size = new System.Drawing.Size(54, 23);
            this.btn100.TabIndex = 4;
            this.btn100.Tag = "100";
            this.btn100.Text = "R$ 100";
            this.btn100.UseVisualStyleBackColor = true;
            // 
            // btn10
            // 
            this.btn10.Font = new System.Drawing.Font("Segoe UI Light", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn10.Location = new System.Drawing.Point(112, 103);
            this.btn10.Name = "btn10";
            this.btn10.Size = new System.Drawing.Size(45, 23);
            this.btn10.TabIndex = 3;
            this.btn10.Tag = "10";
            this.btn10.Text = "R$ 10";
            this.btn10.UseVisualStyleBackColor = true;
            // 
            // btnExato
            // 
            this.btnExato.Font = new System.Drawing.Font("Segoe UI Light", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExato.Location = new System.Drawing.Point(10, 103);
            this.btnExato.Name = "btnExato";
            this.btnExato.Size = new System.Drawing.Size(96, 23);
            this.btnExato.TabIndex = 2;
            this.btnExato.Text = "Valor Exato";
            this.btnExato.UseVisualStyleBackColor = true;
            // 
            // txtPagamento
            // 
            this.txtPagamento.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPagamento.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPagamento.Location = new System.Drawing.Point(10, 66);
            this.txtPagamento.Name = "txtPagamento";
            this.txtPagamento.ShortcutsEnabled = false;
            this.txtPagamento.Size = new System.Drawing.Size(267, 29);
            this.txtPagamento.TabIndex = 1;
            this.txtPagamento.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Valor Pago (R$):";
            // 
            // lblParcelas
            // 
            this.lblParcelas.AutoSize = true;
            this.lblParcelas.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParcelas.Location = new System.Drawing.Point(6, 152);
            this.lblParcelas.Name = "lblParcelas";
            this.lblParcelas.Size = new System.Drawing.Size(69, 21);
            this.lblParcelas.TabIndex = 8;
            this.lblParcelas.Text = "Parcelas:";
            // 
            // cmbParcelas
            // 
            this.cmbParcelas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParcelas.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cmbParcelas.FormattingEnabled = true;
            this.cmbParcelas.Items.AddRange(new object[] {
            "1X",
            "2X",
            "3X",
            "4X",
            "5X",
            "6X",
            "7X",
            "8X",
            "9X",
            "10X",
            "11X",
            "12X"});
            this.cmbParcelas.Location = new System.Drawing.Point(10, 176);
            this.cmbParcelas.Name = "cmbParcelas";
            this.cmbParcelas.Size = new System.Drawing.Size(96, 29);
            this.cmbParcelas.TabIndex = 9;
            // 
            // btnAdicionarPagamento
            // 
            this.btnAdicionarPagamento.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdicionarPagamento.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnAdicionarPagamento.FlatAppearance.BorderSize = 0;
            this.btnAdicionarPagamento.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdicionarPagamento.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdicionarPagamento.ForeColor = System.Drawing.Color.White;
            this.btnAdicionarPagamento.Location = new System.Drawing.Point(45, 264);
            this.btnAdicionarPagamento.Name = "btnAdicionarPagamento";
            this.btnAdicionarPagamento.Size = new System.Drawing.Size(212, 50);
            this.btnAdicionarPagamento.TabIndex = 10;
            this.btnAdicionarPagamento.Text = "Adicionar [Enter]";
            this.btnAdicionarPagamento.UseVisualStyleBackColor = false;
            // 
            // UcPagamento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gpbLancamentoValores);
            this.Controls.Add(this.btnCarne);
            this.Controls.Add(this.btnPix);
            this.Controls.Add(this.btnCredito);
            this.Controls.Add(this.btnDebito);
            this.Controls.Add(this.btnDinheiro);
            this.Name = "UcPagamento";
            this.Size = new System.Drawing.Size(527, 368);
            this.gpbLancamentoValores.ResumeLayout(false);
            this.gpbLancamentoValores.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDinheiro;
        private System.Windows.Forms.Button btnDebito;
        private System.Windows.Forms.Button btnCredito;
        private System.Windows.Forms.Button btnPix;
        private System.Windows.Forms.Button btnCarne;
        private System.Windows.Forms.GroupBox gpbLancamentoValores;
        private System.Windows.Forms.TextBox txtPagamento;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn100;
        private System.Windows.Forms.Button btn10;
        private System.Windows.Forms.Button btnExato;
        private System.Windows.Forms.Button btn1000;
        private System.Windows.Forms.ComboBox cmbParcelas;
        private System.Windows.Forms.Label lblParcelas;
        private System.Windows.Forms.Button btnAdicionarPagamento;
    }
}
