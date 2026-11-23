namespace ProjetoExtensionista2026_2
{
    partial class FormInicio
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

        private void InitializeComponent()
        {
            this.pnlRoot = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlHeaderAcoes = new System.Windows.Forms.Panel();
            this.lblSaudacao = new System.Windows.Forms.Label();
            this.lblSubtitulo = new System.Windows.Forms.Label();
            this.btnAtualizar = new System.Windows.Forms.Button();
            this.pnlAtalhos = new System.Windows.Forms.Panel();
            this.lblTituloAtalhos = new System.Windows.Forms.Label();
            this.tblAtalhos = new System.Windows.Forms.TableLayoutPanel();
            this.tblPaineis = new System.Windows.Forms.TableLayoutPanel();
            this.grpUltimasVendas = new System.Windows.Forms.GroupBox();
            this.dgvUltimasVendas = new System.Windows.Forms.DataGridView();
            this.grpEstoqueBaixo = new System.Windows.Forms.GroupBox();
            this.dgvEstoqueBaixo = new System.Windows.Forms.DataGridView();

            this.pnlRoot.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlHeaderAcoes.SuspendLayout();
            this.pnlAtalhos.SuspendLayout();
            this.tblPaineis.SuspendLayout();
            this.grpUltimasVendas.SuspendLayout();
            this.grpEstoqueBaixo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUltimasVendas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEstoqueBaixo)).BeginInit();
            this.SuspendLayout();

            // pnlRoot
            this.pnlRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRoot.Padding = new System.Windows.Forms.Padding(22, 18, 22, 18);
            this.pnlRoot.Controls.Add(this.tblPaineis);
            this.pnlRoot.Controls.Add(this.pnlAtalhos);
            this.pnlRoot.Controls.Add(this.pnlHeader);
            this.pnlRoot.Name = "pnlRoot";

            // pnlHeader
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 68;
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Controls.Add(this.lblSubtitulo);
            this.pnlHeader.Controls.Add(this.lblSaudacao);
            this.pnlHeader.Controls.Add(this.pnlHeaderAcoes);

            // pnlHeaderAcoes
            this.pnlHeaderAcoes.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlHeaderAcoes.Width = 130;
            this.pnlHeaderAcoes.Name = "pnlHeaderAcoes";
            this.pnlHeaderAcoes.Controls.Add(this.btnAtualizar);

            // lblSaudacao
            this.lblSaudacao.AutoSize = true;
            this.lblSaudacao.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblSaudacao.Location = new System.Drawing.Point(0, 2);
            this.lblSaudacao.Name = "lblSaudacao";
            this.lblSaudacao.Text = "Olá!";

            // lblSubtitulo
            this.lblSubtitulo.AutoSize = true;
            this.lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular);
            this.lblSubtitulo.Location = new System.Drawing.Point(2, 39);
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.Text = "";

            // btnAtualizar
            this.btnAtualizar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnAtualizar.Location = new System.Drawing.Point(8, 12);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(120, 38);
            this.btnAtualizar.TabIndex = 0;
            this.btnAtualizar.Text = "Atualizar";
            this.btnAtualizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAtualizar.UseVisualStyleBackColor = true;
            this.btnAtualizar.Click += new System.EventHandler(this.btnAtualizar_Click);

            // pnlAtalhos
            this.pnlAtalhos.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAtalhos.Height = 190;
            this.pnlAtalhos.Padding = new System.Windows.Forms.Padding(0, 8, 0, 14);
            this.pnlAtalhos.Name = "pnlAtalhos";
            this.pnlAtalhos.Controls.Add(this.tblAtalhos);
            this.pnlAtalhos.Controls.Add(this.lblTituloAtalhos);

            // lblTituloAtalhos
            this.lblTituloAtalhos.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTituloAtalhos.Height = 24;
            this.lblTituloAtalhos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTituloAtalhos.Name = "lblTituloAtalhos";
            this.lblTituloAtalhos.Text = "ACESSO RÁPIDO";

            // tblAtalhos
            this.tblAtalhos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblAtalhos.Name = "tblAtalhos";
            this.tblAtalhos.Margin = new System.Windows.Forms.Padding(0);

            // tblPaineis
            this.tblPaineis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblPaineis.ColumnCount = 2;
            this.tblPaineis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblPaineis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblPaineis.RowCount = 1;
            this.tblPaineis.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblPaineis.Controls.Add(this.grpUltimasVendas, 0, 0);
            this.tblPaineis.Controls.Add(this.grpEstoqueBaixo, 1, 0);
            this.tblPaineis.Name = "tblPaineis";

            // grpUltimasVendas
            this.grpUltimasVendas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpUltimasVendas.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.grpUltimasVendas.Margin = new System.Windows.Forms.Padding(0, 4, 10, 0);
            this.grpUltimasVendas.Padding = new System.Windows.Forms.Padding(12, 10, 12, 12);
            this.grpUltimasVendas.Name = "grpUltimasVendas";
            this.grpUltimasVendas.Text = "Últimas Vendas";
            this.grpUltimasVendas.Controls.Add(this.dgvUltimasVendas);

            // dgvUltimasVendas
            this.dgvUltimasVendas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUltimasVendas.AllowUserToAddRows = false;
            this.dgvUltimasVendas.AllowUserToDeleteRows = false;
            this.dgvUltimasVendas.AllowUserToResizeRows = false;
            this.dgvUltimasVendas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUltimasVendas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvUltimasVendas.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvUltimasVendas.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvUltimasVendas.ColumnHeadersHeight = 38;
            this.dgvUltimasVendas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvUltimasVendas.MultiSelect = false;
            this.dgvUltimasVendas.Name = "dgvUltimasVendas";
            this.dgvUltimasVendas.ReadOnly = true;
            this.dgvUltimasVendas.RowHeadersVisible = false;
            this.dgvUltimasVendas.RowTemplate.Height = 36;
            this.dgvUltimasVendas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // grpEstoqueBaixo
            this.grpEstoqueBaixo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpEstoqueBaixo.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.grpEstoqueBaixo.Margin = new System.Windows.Forms.Padding(10, 4, 0, 0);
            this.grpEstoqueBaixo.Padding = new System.Windows.Forms.Padding(12, 10, 12, 12);
            this.grpEstoqueBaixo.Name = "grpEstoqueBaixo";
            this.grpEstoqueBaixo.Text = "Produtos com Estoque Baixo";
            this.grpEstoqueBaixo.Controls.Add(this.dgvEstoqueBaixo);

            // dgvEstoqueBaixo
            this.dgvEstoqueBaixo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEstoqueBaixo.AllowUserToAddRows = false;
            this.dgvEstoqueBaixo.AllowUserToDeleteRows = false;
            this.dgvEstoqueBaixo.AllowUserToResizeRows = false;
            this.dgvEstoqueBaixo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEstoqueBaixo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvEstoqueBaixo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvEstoqueBaixo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvEstoqueBaixo.ColumnHeadersHeight = 38;
            this.dgvEstoqueBaixo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvEstoqueBaixo.MultiSelect = false;
            this.dgvEstoqueBaixo.Name = "dgvEstoqueBaixo";
            this.dgvEstoqueBaixo.ReadOnly = true;
            this.dgvEstoqueBaixo.RowHeadersVisible = false;
            this.dgvEstoqueBaixo.RowTemplate.Height = 36;
            this.dgvEstoqueBaixo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // FormInicio
            this.ClientSize = new System.Drawing.Size(1020, 640);
            this.Controls.Add(this.pnlRoot);
            this.Name = "FormInicio";
            this.Text = "Principal";
            this.Load += new System.EventHandler(this.FormInicio_Load);

            this.pnlRoot.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlHeaderAcoes.ResumeLayout(false);
            this.pnlAtalhos.ResumeLayout(false);
            this.tblPaineis.ResumeLayout(false);
            this.grpUltimasVendas.ResumeLayout(false);
            this.grpEstoqueBaixo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUltimasVendas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEstoqueBaixo)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlRoot;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlHeaderAcoes;
        private System.Windows.Forms.Label lblSaudacao;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Button btnAtualizar;
        private System.Windows.Forms.Panel pnlAtalhos;
        private System.Windows.Forms.Label lblTituloAtalhos;
        private System.Windows.Forms.TableLayoutPanel tblAtalhos;
        private System.Windows.Forms.TableLayoutPanel tblPaineis;
        private System.Windows.Forms.GroupBox grpUltimasVendas;
        private System.Windows.Forms.DataGridView dgvUltimasVendas;
        private System.Windows.Forms.GroupBox grpEstoqueBaixo;
        private System.Windows.Forms.DataGridView dgvEstoqueBaixo;
    }
}
