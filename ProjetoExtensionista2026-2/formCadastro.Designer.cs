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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_cadastrarCliente = new System.Windows.Forms.Button();
            this.btn_cadastarProduto = new System.Windows.Forms.Button();
            this.panelConteudo = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.btn_cadastrarCliente);
            this.panel1.Controls.Add(this.btn_cadastarProduto);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(834, 40);
            this.panel1.TabIndex = 0;
            // 
            // btn_cadastrarCliente
            // 
            this.btn_cadastrarCliente.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btn_cadastrarCliente.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btn_cadastrarCliente.FlatAppearance.BorderSize = 0;
            this.btn_cadastrarCliente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_cadastrarCliente.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_cadastrarCliente.Location = new System.Drawing.Point(92, -3);
            this.btn_cadastrarCliente.Name = "btn_cadastrarCliente";
            this.btn_cadastrarCliente.Size = new System.Drawing.Size(96, 42);
            this.btn_cadastrarCliente.TabIndex = 1;
            this.btn_cadastrarCliente.Text = "Cliente";
            this.btn_cadastrarCliente.UseVisualStyleBackColor = false;
            this.btn_cadastrarCliente.Click += new System.EventHandler(this.btn_cadastrarCliente_Click);
            // 
            // btn_cadastarProduto
            // 
            this.btn_cadastarProduto.BackColor = System.Drawing.SystemColors.Control;
            this.btn_cadastarProduto.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btn_cadastarProduto.FlatAppearance.BorderSize = 0;
            this.btn_cadastarProduto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_cadastarProduto.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_cadastarProduto.Location = new System.Drawing.Point(0, -3);
            this.btn_cadastarProduto.Name = "btn_cadastarProduto";
            this.btn_cadastarProduto.Size = new System.Drawing.Size(96, 42);
            this.btn_cadastarProduto.TabIndex = 0;
            this.btn_cadastarProduto.Text = "Produto";
            this.btn_cadastarProduto.UseVisualStyleBackColor = false;
            this.btn_cadastarProduto.Click += new System.EventHandler(this.btn_cadastarProduto_Click);
            // 
            // panelConteudo
            // 
            this.panelConteudo.BackColor = System.Drawing.SystemColors.Control;
            this.panelConteudo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelConteudo.Location = new System.Drawing.Point(0, 38);
            this.panelConteudo.Name = "panelConteudo";
            this.panelConteudo.Size = new System.Drawing.Size(834, 443);
            this.panelConteudo.TabIndex = 1;
            // 
            // formCadastro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(834, 481);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelConteudo);
            this.Name = "formCadastro";
            this.Text = "Cadastrar";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_cadastarProduto;
        private System.Windows.Forms.Button btn_cadastrarCliente;
        private System.Windows.Forms.Panel panelConteudo;
    }
}