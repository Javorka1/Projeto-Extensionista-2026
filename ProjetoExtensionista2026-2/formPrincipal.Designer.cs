namespace ProjetoExtensionista2026_2
{
    partial class formPrincipal
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
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.pnlNav = new System.Windows.Forms.Panel();
            this.btnNavConfig = new System.Windows.Forms.Button();
            this.btnNavCadastro = new System.Windows.Forms.Button();
            this.btnNavFinanceiro = new System.Windows.Forms.Button();
            this.btnNavEstoque = new System.Windows.Forms.Button();
            this.btnNavVendas = new System.Windows.Forms.Button();
            this.btnNavPrincipal = new System.Windows.Forms.Button();
            this.pnlUserFooter = new System.Windows.Forms.Panel();
            this.btnSair = new System.Windows.Forms.Button();
            this.btnToggleTema = new System.Windows.Forms.Button();
            this.pnlCardUsuario = new System.Windows.Forms.Panel();
            this.lblCargoUsuario = new System.Windows.Forms.Label();
            this.lblNomeUsuario = new System.Windows.Forms.Label();
            this.lblAvatar = new System.Windows.Forms.Label();
            this.pnlHeaderLogo = new System.Windows.Forms.Panel();
            this.lblLogoSubtitulo = new System.Windows.Forms.Label();
            this.lblLogoTitulo = new System.Windows.Forms.Label();
            this.lblLogoIcone = new System.Windows.Forms.Label();
            this.listBoxSidebar = new System.Windows.Forms.ListBox();
            this.panelWorkspace = new System.Windows.Forms.Panel();
            this.panelSidebar.SuspendLayout();
            this.pnlNav.SuspendLayout();
            this.pnlUserFooter.SuspendLayout();
            this.pnlCardUsuario.SuspendLayout();
            this.pnlHeaderLogo.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSidebar
            // 
            this.panelSidebar.Controls.Add(this.pnlNav);
            this.panelSidebar.Controls.Add(this.pnlUserFooter);
            this.panelSidebar.Controls.Add(this.pnlHeaderLogo);
            this.panelSidebar.Controls.Add(this.listBoxSidebar);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(240, 681);
            this.panelSidebar.TabIndex = 0;
            // 
            // pnlNav
            // 
            this.pnlNav.Controls.Add(this.btnNavConfig);
            this.pnlNav.Controls.Add(this.btnNavCadastro);
            this.pnlNav.Controls.Add(this.btnNavFinanceiro);
            this.pnlNav.Controls.Add(this.btnNavEstoque);
            this.pnlNav.Controls.Add(this.btnNavVendas);
            this.pnlNav.Controls.Add(this.btnNavPrincipal);
            this.pnlNav.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlNav.Location = new System.Drawing.Point(0, 75);
            this.pnlNav.Name = "pnlNav";
            this.pnlNav.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.pnlNav.Size = new System.Drawing.Size(240, 476);
            this.pnlNav.TabIndex = 1;
            // 
            // btnNavConfig
            // 
            this.btnNavConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavConfig.FlatAppearance.BorderSize = 0;
            this.btnNavConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavConfig.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNavConfig.Location = new System.Drawing.Point(10, 218);
            this.btnNavConfig.Name = "btnNavConfig";
            this.btnNavConfig.Size = new System.Drawing.Size(220, 42);
            this.btnNavConfig.TabIndex = 5;
            this.btnNavConfig.Tag = "nav_item";
            this.btnNavConfig.Text = "  Configuracoes";
            this.btnNavConfig.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavConfig.UseVisualStyleBackColor = true;
            this.btnNavConfig.Click += new System.EventHandler(this.btnNavConfig_Click);
            // 
            // btnNavCadastro
            // 
            this.btnNavCadastro.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavCadastro.FlatAppearance.BorderSize = 0;
            this.btnNavCadastro.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavCadastro.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNavCadastro.Location = new System.Drawing.Point(10, 176);
            this.btnNavCadastro.Name = "btnNavCadastro";
            this.btnNavCadastro.Size = new System.Drawing.Size(220, 42);
            this.btnNavCadastro.TabIndex = 4;
            this.btnNavCadastro.Tag = "nav_item";
            this.btnNavCadastro.Text = "  Clientes e Produtos";
            this.btnNavCadastro.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavCadastro.UseVisualStyleBackColor = true;
            this.btnNavCadastro.Click += new System.EventHandler(this.btnNavCadastro_Click);
            // 
            // btnNavFinanceiro
            // 
            this.btnNavFinanceiro.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavFinanceiro.FlatAppearance.BorderSize = 0;
            this.btnNavFinanceiro.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavFinanceiro.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNavFinanceiro.Location = new System.Drawing.Point(10, 134);
            this.btnNavFinanceiro.Name = "btnNavFinanceiro";
            this.btnNavFinanceiro.Size = new System.Drawing.Size(220, 42);
            this.btnNavFinanceiro.TabIndex = 3;
            this.btnNavFinanceiro.Tag = "nav_item";
            this.btnNavFinanceiro.Text = "  Painel Financeiro";
            this.btnNavFinanceiro.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavFinanceiro.UseVisualStyleBackColor = true;
            this.btnNavFinanceiro.Click += new System.EventHandler(this.btnNavFinanceiro_Click);
            // 
            // btnNavEstoque
            // 
            this.btnNavEstoque.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavEstoque.FlatAppearance.BorderSize = 0;
            this.btnNavEstoque.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavEstoque.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNavEstoque.Location = new System.Drawing.Point(10, 92);
            this.btnNavEstoque.Name = "btnNavEstoque";
            this.btnNavEstoque.Size = new System.Drawing.Size(220, 42);
            this.btnNavEstoque.TabIndex = 2;
            this.btnNavEstoque.Tag = "nav_item";
            this.btnNavEstoque.Text = "  Gestao de Estoque";
            this.btnNavEstoque.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavEstoque.UseVisualStyleBackColor = true;
            this.btnNavEstoque.Click += new System.EventHandler(this.btnNavEstoque_Click);
            // 
            // btnNavVendas
            // 
            this.btnNavVendas.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavVendas.FlatAppearance.BorderSize = 0;
            this.btnNavVendas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavVendas.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNavVendas.Location = new System.Drawing.Point(10, 50);
            this.btnNavVendas.Name = "btnNavVendas";
            this.btnNavVendas.Size = new System.Drawing.Size(220, 42);
            this.btnNavVendas.TabIndex = 1;
            this.btnNavVendas.Tag = "nav_ativa";
            this.btnNavVendas.Text = "  Frente de Caixa [F1]";
            this.btnNavVendas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavVendas.UseVisualStyleBackColor = true;
            this.btnNavVendas.Click += new System.EventHandler(this.btnNavVendas_Click);
            // 
            // btnNavPrincipal
            // 
            this.btnNavPrincipal.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavPrincipal.FlatAppearance.BorderSize = 0;
            this.btnNavPrincipal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavPrincipal.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNavPrincipal.Location = new System.Drawing.Point(10, 8);
            this.btnNavPrincipal.Name = "btnNavPrincipal";
            this.btnNavPrincipal.Size = new System.Drawing.Size(220, 42);
            this.btnNavPrincipal.TabIndex = 0;
            this.btnNavPrincipal.Tag = "nav_item";
            this.btnNavPrincipal.Text = "  Principal";
            this.btnNavPrincipal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavPrincipal.UseVisualStyleBackColor = true;
            this.btnNavPrincipal.Click += new System.EventHandler(this.btnNavPrincipal_Click);
            // 
            // pnlUserFooter
            // 
            this.pnlUserFooter.Controls.Add(this.btnSair);
            this.pnlUserFooter.Controls.Add(this.btnToggleTema);
            this.pnlUserFooter.Controls.Add(this.pnlCardUsuario);
            this.pnlUserFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlUserFooter.Location = new System.Drawing.Point(0, 551);
            this.pnlUserFooter.Name = "pnlUserFooter";
            this.pnlUserFooter.Padding = new System.Windows.Forms.Padding(10, 6, 10, 8);
            this.pnlUserFooter.Size = new System.Drawing.Size(240, 130);
            this.pnlUserFooter.TabIndex = 2;
            // 
            // btnSair
            // 
            this.btnSair.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSair.FlatAppearance.BorderSize = 1;
            this.btnSair.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSair.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSair.Location = new System.Drawing.Point(10, 92);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(220, 30);
            this.btnSair.TabIndex = 2;
            this.btnSair.Tag = "danger";
            this.btnSair.Text = "Encerrar Sessao";
            this.btnSair.UseVisualStyleBackColor = true;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // btnToggleTema
            // 
            this.btnToggleTema.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnToggleTema.FlatAppearance.BorderSize = 1;
            this.btnToggleTema.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleTema.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToggleTema.Location = new System.Drawing.Point(10, 56);
            this.btnToggleTema.Name = "btnToggleTema";
            this.btnToggleTema.Size = new System.Drawing.Size(220, 32);
            this.btnToggleTema.TabIndex = 1;
            this.btnToggleTema.Tag = "tema_toggle";
            this.btnToggleTema.Text = "Alternar Modo Claro / Escuro";
            this.btnToggleTema.UseVisualStyleBackColor = true;
            this.btnToggleTema.Click += new System.EventHandler(this.btnToggleTema_Click);
            // 
            // pnlCardUsuario
            // 
            this.pnlCardUsuario.Controls.Add(this.lblCargoUsuario);
            this.pnlCardUsuario.Controls.Add(this.lblNomeUsuario);
            this.pnlCardUsuario.Controls.Add(this.lblAvatar);
            this.pnlCardUsuario.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCardUsuario.Location = new System.Drawing.Point(10, 6);
            this.pnlCardUsuario.Name = "pnlCardUsuario";
            this.pnlCardUsuario.Padding = new System.Windows.Forms.Padding(4);
            this.pnlCardUsuario.Size = new System.Drawing.Size(220, 48);
            this.pnlCardUsuario.TabIndex = 0;
            this.pnlCardUsuario.Tag = "card";
            // 
            // lblCargoUsuario
            // 
            this.lblCargoUsuario.AutoSize = true;
            this.lblCargoUsuario.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCargoUsuario.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblCargoUsuario.Location = new System.Drawing.Point(48, 26);
            this.lblCargoUsuario.Name = "lblCargoUsuario";
            this.lblCargoUsuario.Size = new System.Drawing.Size(81, 13);
            this.lblCargoUsuario.TabIndex = 2;
            this.lblCargoUsuario.Text = "Administrador";
            // 
            // lblNomeUsuario
            // 
            this.lblNomeUsuario.AutoSize = true;
            this.lblNomeUsuario.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNomeUsuario.Location = new System.Drawing.Point(48, 7);
            this.lblNomeUsuario.Name = "lblNomeUsuario";
            this.lblNomeUsuario.Size = new System.Drawing.Size(55, 17);
            this.lblNomeUsuario.TabIndex = 1;
            this.lblNomeUsuario.Text = "Usuario";
            // 
            // lblAvatar
            // 
            this.lblAvatar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.lblAvatar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvatar.ForeColor = System.Drawing.Color.White;
            this.lblAvatar.Location = new System.Drawing.Point(4, 5);
            this.lblAvatar.Name = "lblAvatar";
            this.lblAvatar.Size = new System.Drawing.Size(36, 36);
            this.lblAvatar.TabIndex = 0;
            this.lblAvatar.Text = "US";
            this.lblAvatar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlHeaderLogo
            // 
            this.pnlHeaderLogo.Controls.Add(this.lblLogoSubtitulo);
            this.pnlHeaderLogo.Controls.Add(this.lblLogoTitulo);
            this.pnlHeaderLogo.Controls.Add(this.lblLogoIcone);
            this.pnlHeaderLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeaderLogo.Location = new System.Drawing.Point(0, 0);
            this.pnlHeaderLogo.Name = "pnlHeaderLogo";
            this.pnlHeaderLogo.Size = new System.Drawing.Size(240, 75);
            this.pnlHeaderLogo.TabIndex = 0;
            // 
            // lblLogoSubtitulo
            // 
            this.lblLogoSubtitulo.AutoSize = true;
            this.lblLogoSubtitulo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogoSubtitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(160)))), ((int)(((byte)(175)))));
            this.lblLogoSubtitulo.Location = new System.Drawing.Point(54, 43);
            this.lblLogoSubtitulo.Name = "lblLogoSubtitulo";
            this.lblLogoSubtitulo.Size = new System.Drawing.Size(126, 13);
            this.lblLogoSubtitulo.TabIndex = 2;
            this.lblLogoSubtitulo.Tag = "subtitulo";
            this.lblLogoSubtitulo.Text = "Ponto de Venda e ERP";
            // 
            // lblLogoTitulo
            // 
            this.lblLogoTitulo.AutoSize = true;
            this.lblLogoTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogoTitulo.Location = new System.Drawing.Point(54, 20);
            this.lblLogoTitulo.Name = "lblLogoTitulo";
            this.lblLogoTitulo.Size = new System.Drawing.Size(119, 21);
            this.lblLogoTitulo.TabIndex = 1;
            this.lblLogoTitulo.Text = "NexStore 2026";
            // 
            // lblLogoIcone
            // 
            this.lblLogoIcone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.lblLogoIcone.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogoIcone.ForeColor = System.Drawing.Color.White;
            this.lblLogoIcone.Location = new System.Drawing.Point(12, 18);
            this.lblLogoIcone.Name = "lblLogoIcone";
            this.lblLogoIcone.Size = new System.Drawing.Size(36, 36);
            this.lblLogoIcone.TabIndex = 0;
            this.lblLogoIcone.Text = "N";
            this.lblLogoIcone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listBoxSidebar
            // 
            this.listBoxSidebar.FormattingEnabled = true;
            this.listBoxSidebar.Location = new System.Drawing.Point(0, 0);
            this.listBoxSidebar.Name = "listBoxSidebar";
            this.listBoxSidebar.Size = new System.Drawing.Size(10, 10);
            this.listBoxSidebar.TabIndex = 3;
            this.listBoxSidebar.Visible = false;
            // 
            // panelWorkspace
            // 
            this.panelWorkspace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWorkspace.Location = new System.Drawing.Point(240, 0);
            this.panelWorkspace.Name = "panelWorkspace";
            this.panelWorkspace.Size = new System.Drawing.Size(1024, 681);
            this.panelWorkspace.TabIndex = 1;
            // 
            // formPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.panelWorkspace);
            this.Controls.Add(this.panelSidebar);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "formPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NexStore 2026 - Ponto de Venda e Gestao Comercial";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.formPrincipal_Load);
            this.panelSidebar.ResumeLayout(false);
            this.pnlNav.ResumeLayout(false);
            this.pnlUserFooter.ResumeLayout(false);
            this.pnlCardUsuario.ResumeLayout(false);
            this.pnlCardUsuario.PerformLayout();
            this.pnlHeaderLogo.ResumeLayout(false);
            this.pnlHeaderLogo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Panel pnlHeaderLogo;
        private System.Windows.Forms.Label lblLogoIcone;
        private System.Windows.Forms.Label lblLogoTitulo;
        private System.Windows.Forms.Label lblLogoSubtitulo;
        private System.Windows.Forms.Panel pnlNav;
        public System.Windows.Forms.Button btnNavPrincipal;
        public System.Windows.Forms.Button btnNavVendas;
        public System.Windows.Forms.Button btnNavEstoque;
        public System.Windows.Forms.Button btnNavFinanceiro;
        public System.Windows.Forms.Button btnNavCadastro;
        public System.Windows.Forms.Button btnNavConfig;
        private System.Windows.Forms.Panel pnlUserFooter;
        private System.Windows.Forms.Panel pnlCardUsuario;
        private System.Windows.Forms.Label lblAvatar;
        private System.Windows.Forms.Label lblNomeUsuario;
        private System.Windows.Forms.Label lblCargoUsuario;
        private System.Windows.Forms.Button btnToggleTema;
        public System.Windows.Forms.Button btnSair;
        public System.Windows.Forms.ListBox listBoxSidebar;
        private System.Windows.Forms.Panel panelWorkspace;
    }
}
