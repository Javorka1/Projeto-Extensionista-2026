namespace ProjetoExtensionista2026_2
{
    partial class FormConfig
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
            this.lblTituloPrincipal = new System.Windows.Forms.Label();
            this.grpAparencia = new System.Windows.Forms.GroupBox();
            this.btnAlternarTema = new System.Windows.Forms.Button();
            this.lblDescAparencia = new System.Windows.Forms.Label();
            this.grpBanco = new System.Windows.Forms.GroupBox();
            this.lblDescBanco = new System.Windows.Forms.Label();
            this.btnTestarBanco = new System.Windows.Forms.Button();
            this.lblStatusBanco = new System.Windows.Forms.Label();
            this.grpSeguranca = new System.Windows.Forms.GroupBox();
            this.lblUsuarioLogado = new System.Windows.Forms.Label();
            this.lblDescSeguranca = new System.Windows.Forms.Label();
            this.btnAlterarSenha = new System.Windows.Forms.Button();
            this.grpSobre = new System.Windows.Forms.GroupBox();
            this.tblConfigGrid = new System.Windows.Forms.TableLayoutPanel();
            this.lblInfoSobre = new System.Windows.Forms.Label();
            this.lnkGitHub = new System.Windows.Forms.LinkLabel();
            this.grpAparencia.SuspendLayout();
            this.grpBanco.SuspendLayout();
            this.grpSeguranca.SuspendLayout();
            this.grpSobre.SuspendLayout();
            this.tblConfigGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTituloPrincipal
            // 
            this.lblTituloPrincipal.AutoSize = true;
            this.lblTituloPrincipal.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloPrincipal.Location = new System.Drawing.Point(30, 20);
            this.lblTituloPrincipal.Name = "lblTituloPrincipal";
            this.lblTituloPrincipal.Size = new System.Drawing.Size(280, 26);
            this.lblTituloPrincipal.TabIndex = 0;
            this.lblTituloPrincipal.Text = "Configurações do Sistema";
            // 
            // grpAparencia
            // 
            this.grpAparencia.Controls.Add(this.btnAlternarTema);
            this.grpAparencia.Controls.Add(this.lblDescAparencia);
            this.grpAparencia.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpAparencia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpAparencia.Margin = new System.Windows.Forms.Padding(10);
            this.grpAparencia.Name = "grpAparencia";
            this.grpAparencia.Size = new System.Drawing.Size(350, 185);
            this.grpAparencia.TabIndex = 1;
            this.grpAparencia.TabStop = false;
            this.grpAparencia.Text = "Aparência";
            // 
            // lblDescAparencia
            // 
            this.lblDescAparencia.AutoSize = true;
            this.lblDescAparencia.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescAparencia.Location = new System.Drawing.Point(20, 30);
            this.lblDescAparencia.Name = "lblDescAparencia";
            this.lblDescAparencia.Size = new System.Drawing.Size(295, 32);
            this.lblDescAparencia.TabIndex = 0;
            this.lblDescAparencia.Text = "Alterne entre o tema claro e escuro.\r\nSua preferência é salva automaticamente.";
            // 
            // btnAlternarTema
            // 
            this.btnAlternarTema.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAlternarTema.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAlternarTema.Location = new System.Drawing.Point(23, 85);
            this.btnAlternarTema.Name = "btnAlternarTema";
            this.btnAlternarTema.Size = new System.Drawing.Size(295, 45);
            this.btnAlternarTema.TabIndex = 1;
            this.btnAlternarTema.Text = "Mudar para Modo Escuro";
            this.btnAlternarTema.UseVisualStyleBackColor = true;
            this.btnAlternarTema.Click += new System.EventHandler(this.btnAlternarTema_Click);
            // 
            // grpBanco
            // 
            this.grpBanco.Controls.Add(this.lblDescBanco);
            this.grpBanco.Controls.Add(this.btnTestarBanco);
            this.grpBanco.Controls.Add(this.lblStatusBanco);
            this.grpBanco.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBanco.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBanco.Margin = new System.Windows.Forms.Padding(10);
            this.grpBanco.Name = "grpBanco";
            this.grpBanco.Size = new System.Drawing.Size(350, 200);
            this.grpBanco.TabIndex = 2;
            this.grpBanco.TabStop = false;
            this.grpBanco.Text = "Banco de Dados";
            // 
            // lblDescBanco
            // 
            this.lblDescBanco.AutoSize = true;
            this.lblDescBanco.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescBanco.Location = new System.Drawing.Point(20, 30);
            this.lblDescBanco.Name = "lblDescBanco";
            this.lblDescBanco.Size = new System.Drawing.Size(262, 32);
            this.lblDescBanco.TabIndex = 0;
            this.lblDescBanco.Text = "Supabase PostgreSQL (Nuvem)\r\nVerifique a conectividade com o servidor.";
            // 
            // btnTestarBanco
            // 
            this.btnTestarBanco.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestarBanco.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestarBanco.Location = new System.Drawing.Point(23, 80);
            this.btnTestarBanco.Name = "btnTestarBanco";
            this.btnTestarBanco.Size = new System.Drawing.Size(295, 45);
            this.btnTestarBanco.TabIndex = 1;
            this.btnTestarBanco.Text = "Testar Conexão";
            this.btnTestarBanco.UseVisualStyleBackColor = true;
            this.btnTestarBanco.Click += new System.EventHandler(this.btnTestarBanco_Click);
            // 
            // lblStatusBanco
            // 
            this.lblStatusBanco.AutoSize = true;
            this.lblStatusBanco.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusBanco.Location = new System.Drawing.Point(20, 145);
            this.lblStatusBanco.Name = "lblStatusBanco";
            this.lblStatusBanco.Size = new System.Drawing.Size(130, 16);
            this.lblStatusBanco.TabIndex = 2;
            this.lblStatusBanco.Text = "Status: Não testado";
            // 
            // grpSeguranca
            // 
            this.grpSeguranca.Controls.Add(this.lblUsuarioLogado);
            this.grpSeguranca.Controls.Add(this.lblDescSeguranca);
            this.grpSeguranca.Controls.Add(this.btnAlterarSenha);
            this.grpSeguranca.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSeguranca.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSeguranca.Margin = new System.Windows.Forms.Padding(10);
            this.grpSeguranca.Name = "grpSeguranca";
            this.grpSeguranca.Size = new System.Drawing.Size(350, 185);
            this.grpSeguranca.TabIndex = 3;
            this.grpSeguranca.TabStop = false;
            this.grpSeguranca.Text = "Segurança e Acesso";
            // 
            // lblUsuarioLogado
            // 
            this.lblUsuarioLogado.AutoSize = true;
            this.lblUsuarioLogado.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuarioLogado.Location = new System.Drawing.Point(20, 30);
            this.lblUsuarioLogado.Name = "lblUsuarioLogado";
            this.lblUsuarioLogado.Size = new System.Drawing.Size(160, 16);
            this.lblUsuarioLogado.TabIndex = 0;
            this.lblUsuarioLogado.Text = "Usuário: Administrador";
            // 
            // lblDescSeguranca
            // 
            this.lblDescSeguranca.AutoSize = true;
            this.lblDescSeguranca.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescSeguranca.Location = new System.Drawing.Point(20, 55);
            this.lblDescSeguranca.Name = "lblDescSeguranca";
            this.lblDescSeguranca.Size = new System.Drawing.Size(275, 16);
            this.lblDescSeguranca.TabIndex = 1;
            this.lblDescSeguranca.Text = "Altere a sua senha de acesso ao sistema.";
            // 
            // btnAlterarSenha
            // 
            this.btnAlterarSenha.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAlterarSenha.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAlterarSenha.Location = new System.Drawing.Point(23, 85);
            this.btnAlterarSenha.Name = "btnAlterarSenha";
            this.btnAlterarSenha.Size = new System.Drawing.Size(295, 45);
            this.btnAlterarSenha.TabIndex = 2;
            this.btnAlterarSenha.Text = "Alterar Senha";
            this.btnAlterarSenha.UseVisualStyleBackColor = true;
            this.btnAlterarSenha.Click += new System.EventHandler(this.btnAlterarSenha_Click);
            // 
            // grpSobre
            // 
            this.grpSobre.Controls.Add(this.lblInfoSobre);
            this.grpSobre.Controls.Add(this.lnkGitHub);
            this.grpSobre.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSobre.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSobre.Margin = new System.Windows.Forms.Padding(10);
            this.grpSobre.Name = "grpSobre";
            this.grpSobre.Size = new System.Drawing.Size(350, 200);
            this.grpSobre.TabIndex = 4;
            this.grpSobre.TabStop = false;
            this.grpSobre.Text = "Sobre o Sistema";
            // 
            // lblInfoSobre
            // 
            this.lblInfoSobre.AutoSize = true;
            this.lblInfoSobre.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoSobre.Location = new System.Drawing.Point(20, 30);
            this.lblInfoSobre.Name = "lblInfoSobre";
            this.lblInfoSobre.Size = new System.Drawing.Size(298, 48);
            this.lblInfoSobre.TabIndex = 0;
            this.lblInfoSobre.Text = "Projeto Extensionista 2026-2\r\nVersão: 1.0.0 (Beta)\r\nDesenvolvido pela turma de Programação 2";
            // 
            // lnkGitHub
            // 
            this.lnkGitHub.AutoSize = true;
            this.lnkGitHub.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkGitHub.Location = new System.Drawing.Point(20, 100);
            this.lnkGitHub.Name = "lnkGitHub";
            this.lnkGitHub.Size = new System.Drawing.Size(165, 16);
            this.lnkGitHub.TabIndex = 1;
            this.lnkGitHub.TabStop = true;
            this.lnkGitHub.Text = "Ver repositório no GitHub";
            this.lnkGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGitHub_LinkClicked);
            // 
            // FormConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 568);
            this.tblConfigGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tblConfigGrid.ColumnCount = 2;
            this.tblConfigGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblConfigGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblConfigGrid.Controls.Add(this.grpAparencia, 0, 0);
            this.tblConfigGrid.Controls.Add(this.grpSeguranca, 1, 0);
            this.tblConfigGrid.Controls.Add(this.grpBanco, 0, 1);
            this.tblConfigGrid.Controls.Add(this.grpSobre, 1, 1);
            this.tblConfigGrid.Location = new System.Drawing.Point(30, 60);
            this.tblConfigGrid.Name = "tblConfigGrid";
            this.tblConfigGrid.RowCount = 2;
            this.tblConfigGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 210F));
            this.tblConfigGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 210F));
            this.tblConfigGrid.Size = new System.Drawing.Size(860, 430);
            this.tblConfigGrid.TabIndex = 5;

            this.Controls.Add(this.lblTituloPrincipal);
            this.Controls.Add(this.tblConfigGrid);
            this.Name = "FormConfig";
            this.Text = "Configurações";
            this.Load += new System.EventHandler(this.formConfig_Load);
            this.grpAparencia.ResumeLayout(false);
            this.grpAparencia.PerformLayout();
            this.grpBanco.ResumeLayout(false);
            this.grpBanco.PerformLayout();
            this.grpSeguranca.ResumeLayout(false);
            this.grpSeguranca.PerformLayout();
            this.grpSobre.ResumeLayout(false);
            this.tblConfigGrid.ResumeLayout(false);
            this.grpSobre.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTituloPrincipal;
        private System.Windows.Forms.TableLayoutPanel tblConfigGrid;
        private System.Windows.Forms.GroupBox grpAparencia;
        private System.Windows.Forms.Label lblDescAparencia;
        private System.Windows.Forms.Button btnAlternarTema;
        private System.Windows.Forms.GroupBox grpBanco;
        private System.Windows.Forms.Label lblDescBanco;
        private System.Windows.Forms.Button btnTestarBanco;
        private System.Windows.Forms.Label lblStatusBanco;
        private System.Windows.Forms.GroupBox grpSeguranca;
        private System.Windows.Forms.Label lblUsuarioLogado;
        private System.Windows.Forms.Label lblDescSeguranca;
        private System.Windows.Forms.Button btnAlterarSenha;
        private System.Windows.Forms.GroupBox grpSobre;
        private System.Windows.Forms.Label lblInfoSobre;
        private System.Windows.Forms.LinkLabel lnkGitHub;
    }
}
