namespace ProjetoExtensionista2026_2
{
    partial class formCadastro
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
            this.pnlAbasTopo = new System.Windows.Forms.Panel();
            this.btn_cadastrarCliente = new System.Windows.Forms.Button();
            this.btn_cadastarProduto = new System.Windows.Forms.Button();
            this.panelConteudo = new System.Windows.Forms.Panel();
            this.pnlAbasTopo.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlAbasTopo
            // 
            this.pnlAbasTopo.Controls.Add(this.btn_cadastrarCliente);
            this.pnlAbasTopo.Controls.Add(this.btn_cadastarProduto);
            this.pnlAbasTopo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAbasTopo.Location = new System.Drawing.Point(0, 0);
            this.pnlAbasTopo.Name = "pnlAbasTopo";
            this.pnlAbasTopo.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.pnlAbasTopo.Size = new System.Drawing.Size(960, 52);
            this.pnlAbasTopo.TabIndex = 0;
            // 
            // btn_cadastrarCliente
            // 
            this.btn_cadastrarCliente.FlatAppearance.BorderSize = 0;
            this.btn_cadastrarCliente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_cadastrarCliente.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_cadastrarCliente.Location = new System.Drawing.Point(162, 8);
            this.btn_cadastrarCliente.Name = "btn_cadastrarCliente";
            this.btn_cadastrarCliente.Size = new System.Drawing.Size(140, 36);
            this.btn_cadastrarCliente.TabIndex = 1;
            this.btn_cadastrarCliente.Text = "Cadastro de Clientes";
            this.btn_cadastrarCliente.UseVisualStyleBackColor = true;
            this.btn_cadastrarCliente.Click += new System.EventHandler(this.btn_cadastrarCliente_Click);
            // 
            // btn_cadastarProduto
            // 
            this.btn_cadastarProduto.FlatAppearance.BorderSize = 0;
            this.btn_cadastarProduto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_cadastarProduto.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_cadastarProduto.Location = new System.Drawing.Point(12, 8);
            this.btn_cadastarProduto.Name = "btn_cadastarProduto";
            this.btn_cadastarProduto.Size = new System.Drawing.Size(140, 36);
            this.btn_cadastarProduto.TabIndex = 0;
            this.btn_cadastarProduto.Tag = "aba_ativa";
            this.btn_cadastarProduto.Text = "Cadastro de Produtos";
            this.btn_cadastarProduto.UseVisualStyleBackColor = true;
            this.btn_cadastarProduto.Click += new System.EventHandler(this.btn_cadastarProduto_Click);
            // 
            // panelConteudo
            // 
            this.panelConteudo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelConteudo.Location = new System.Drawing.Point(0, 52);
            this.panelConteudo.Name = "panelConteudo";
            this.panelConteudo.Size = new System.Drawing.Size(960, 589);
            this.panelConteudo.TabIndex = 1;
            // 
            // formCadastro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 641);
            this.Controls.Add(this.panelConteudo);
            this.Controls.Add(this.pnlAbasTopo);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "formCadastro";
            this.Text = "Cadastro de Entidades";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlAbasTopo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlAbasTopo;
        private System.Windows.Forms.Button btn_cadastarProduto;
        private System.Windows.Forms.Button btn_cadastrarCliente;
        private System.Windows.Forms.Panel panelConteudo;
    }
}
