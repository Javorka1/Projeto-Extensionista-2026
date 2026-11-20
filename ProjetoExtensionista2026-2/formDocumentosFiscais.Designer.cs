namespace ProjetoExtensionista2026_2
{
    partial class formDocumentosFiscais
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkCupom = new System.Windows.Forms.CheckBox();
            this.chkPromissoria = new System.Windows.Forms.CheckBox();
            this.lblAviso = new System.Windows.Forms.Label();
            this.cmbImpressoras = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAtualizarImpressoras = new System.Windows.Forms.Button();
            this.btnVerPromissoria = new System.Windows.Forms.Button();
            this.btnVerCupom = new System.Windows.Forms.Button();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnSalvarPdf = new System.Windows.Forms.Button();
            this.picPromissoria = new System.Windows.Forms.PictureBox();
            this.picCupom = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPromissoria)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCupom)).BeginInit();
            this.SuspendLayout();



            this.groupBox1.Controls.Add(this.chkCupom);
            this.groupBox1.Controls.Add(this.chkPromissoria);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(660, 59);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Documentos Fiscais";



            this.chkCupom.AutoSize = true;
            this.chkCupom.Location = new System.Drawing.Point(480, 31);
            this.chkCupom.Name = "chkCupom";
            this.chkCupom.Size = new System.Drawing.Size(104, 21);
            this.chkCupom.TabIndex = 1;
            this.chkCupom.Text = "Cupom Fiscal";
            this.chkCupom.UseVisualStyleBackColor = true;



            this.chkPromissoria.AutoSize = true;
            this.chkPromissoria.Location = new System.Drawing.Point(54, 31);
            this.chkPromissoria.Name = "chkPromissoria";
            this.chkPromissoria.Size = new System.Drawing.Size(129, 21);
            this.chkPromissoria.TabIndex = 0;
            this.chkPromissoria.Text = "Nota Promissoria";
            this.chkPromissoria.UseVisualStyleBackColor = true;



            this.lblAviso.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAviso.Location = new System.Drawing.Point(12, 328);
            this.lblAviso.Name = "lblAviso";
            this.lblAviso.Size = new System.Drawing.Size(660, 44);
            this.lblAviso.TabIndex = 1;
            this.lblAviso.Text = "Aviso:";
            this.lblAviso.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;



            this.cmbImpressoras.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbImpressoras.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbImpressoras.FormattingEnabled = true;
            this.cmbImpressoras.Location = new System.Drawing.Point(95, 12);
            this.cmbImpressoras.Name = "cmbImpressoras";
            this.cmbImpressoras.Size = new System.Drawing.Size(471, 25);
            this.cmbImpressoras.TabIndex = 2;



            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Impressora:";



            this.btnAtualizarImpressoras.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAtualizarImpressoras.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAtualizarImpressoras.Location = new System.Drawing.Point(572, 12);
            this.btnAtualizarImpressoras.Name = "btnAtualizarImpressoras";
            this.btnAtualizarImpressoras.Size = new System.Drawing.Size(100, 25);
            this.btnAtualizarImpressoras.TabIndex = 4;
            this.btnAtualizarImpressoras.Text = "Atualizar";
            this.btnAtualizarImpressoras.UseVisualStyleBackColor = true;



            this.btnVerPromissoria.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnVerPromissoria.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVerPromissoria.Location = new System.Drawing.Point(175, 285);
            this.btnVerPromissoria.Name = "btnVerPromissoria";
            this.btnVerPromissoria.Size = new System.Drawing.Size(30, 30);
            this.btnVerPromissoria.TabIndex = 7;
            this.btnVerPromissoria.Text = "";
            this.btnVerPromissoria.UseVisualStyleBackColor = true;



            this.btnVerCupom.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnVerCupom.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVerCupom.Location = new System.Drawing.Point(586, 285);
            this.btnVerCupom.Name = "btnVerCupom";
            this.btnVerCupom.Size = new System.Drawing.Size(30, 30);
            this.btnVerCupom.TabIndex = 8;
            this.btnVerCupom.Text = "";
            this.btnVerCupom.UseVisualStyleBackColor = true;



            this.btnImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImprimir.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImprimir.Location = new System.Drawing.Point(492, 389);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(180, 30);
            this.btnImprimir.TabIndex = 9;
            this.btnImprimir.Text = "Imprimir";
            this.btnImprimir.UseVisualStyleBackColor = true;



            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.Location = new System.Drawing.Point(15, 389);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(124, 30);
            this.btnCancelar.TabIndex = 10;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;



            this.btnSalvarPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalvarPdf.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalvarPdf.Location = new System.Drawing.Point(359, 389);
            this.btnSalvarPdf.Name = "btnSalvarPdf";
            this.btnSalvarPdf.Size = new System.Drawing.Size(124, 30);
            this.btnSalvarPdf.TabIndex = 11;
            this.btnSalvarPdf.Text = "Salvar";
            this.btnSalvarPdf.UseVisualStyleBackColor = true;



            this.picPromissoria.Location = new System.Drawing.Point(41, 119);
            this.picPromissoria.Name = "picPromissoria";
            this.picPromissoria.Size = new System.Drawing.Size(174, 206);
            this.picPromissoria.TabIndex = 12;
            this.picPromissoria.TabStop = false;



            this.picCupom.Location = new System.Drawing.Point(455, 119);
            this.picCupom.Name = "picCupom";
            this.picCupom.Size = new System.Drawing.Size(174, 206);
            this.picCupom.TabIndex = 13;
            this.picCupom.TabStop = false;



            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 431);
            this.Controls.Add(this.btnSalvarPdf);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnImprimir);
            this.Controls.Add(this.btnVerCupom);
            this.Controls.Add(this.btnVerPromissoria);
            this.Controls.Add(this.btnAtualizarImpressoras);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbImpressoras);
            this.Controls.Add(this.lblAviso);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.picPromissoria);
            this.Controls.Add(this.picCupom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "formDocumentosFiscais";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Documentos Fiscais";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPromissoria)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCupom)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkCupom;
        private System.Windows.Forms.CheckBox chkPromissoria;
        private System.Windows.Forms.Label lblAviso;
        private System.Windows.Forms.ComboBox cmbImpressoras;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAtualizarImpressoras;
        private System.Windows.Forms.Button btnVerPromissoria;
        private System.Windows.Forms.Button btnVerCupom;
        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnSalvarPdf;
        private System.Windows.Forms.PictureBox picPromissoria;
        private System.Windows.Forms.PictureBox picCupom;
    }
}