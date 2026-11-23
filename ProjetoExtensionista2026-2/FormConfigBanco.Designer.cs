namespace ProjetoExtensionista2026_2
{
    partial class FormConfigBanco
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
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblServidor = new System.Windows.Forms.Label();
            this.txtServidor = new System.Windows.Forms.TextBox();
            this.lblPorta = new System.Windows.Forms.Label();
            this.txtPorta = new System.Windows.Forms.TextBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblSenha = new System.Windows.Forms.Label();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.chkMostrarSenha = new System.Windows.Forms.CheckBox();
            this.lblBanco = new System.Windows.Forms.Label();
            this.txtBanco = new System.Windows.Forms.TextBox();
            this.lblCaminho = new System.Windows.Forms.Label();
            this.txtCaminhoJson = new System.Windows.Forms.TextBox();
            this.btnAlterarCaminho = new System.Windows.Forms.Button();
            this.lblResultadoTeste = new System.Windows.Forms.Label();
            this.btnTestar = new System.Windows.Forms.Button();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // lblTitulo
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 16);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(260, 21);
            this.lblTitulo.Text = "Configuração do Banco de Dados";

            // lblServidor
            this.lblServidor.AutoSize = true;
            this.lblServidor.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblServidor.Location = new System.Drawing.Point(20, 52);
            this.lblServidor.Name = "lblServidor";
            this.lblServidor.Text = "IP / Servidor:";

            // txtServidor
            this.txtServidor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtServidor.Location = new System.Drawing.Point(20, 73);
            this.txtServidor.Name = "txtServidor";
            this.txtServidor.Size = new System.Drawing.Size(280, 25);
            this.txtServidor.TabIndex = 0;

            // lblPorta
            this.lblPorta.AutoSize = true;
            this.lblPorta.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblPorta.Location = new System.Drawing.Point(312, 52);
            this.lblPorta.Name = "lblPorta";
            this.lblPorta.Text = "Porta:";

            // txtPorta
            this.txtPorta.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPorta.Location = new System.Drawing.Point(312, 73);
            this.txtPorta.Name = "txtPorta";
            this.txtPorta.Size = new System.Drawing.Size(98, 25);
            this.txtPorta.TabIndex = 1;

            // lblUsuario
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblUsuario.Location = new System.Drawing.Point(20, 110);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Text = "Usuário:";

            // txtUsuario
            this.txtUsuario.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtUsuario.Location = new System.Drawing.Point(20, 131);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(390, 25);
            this.txtUsuario.TabIndex = 2;

            // lblSenha
            this.lblSenha.AutoSize = true;
            this.lblSenha.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblSenha.Location = new System.Drawing.Point(20, 168);
            this.lblSenha.Name = "lblSenha";
            this.lblSenha.Text = "Senha:";

            // txtSenha
            this.txtSenha.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSenha.Location = new System.Drawing.Point(20, 189);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.Size = new System.Drawing.Size(390, 25);
            this.txtSenha.TabIndex = 3;
            this.txtSenha.UseSystemPasswordChar = true;

            // chkMostrarSenha
            this.chkMostrarSenha.AutoSize = true;
            this.chkMostrarSenha.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkMostrarSenha.Location = new System.Drawing.Point(20, 220);
            this.chkMostrarSenha.Name = "chkMostrarSenha";
            this.chkMostrarSenha.Text = "Mostrar senha";
            this.chkMostrarSenha.TabIndex = 4;
            this.chkMostrarSenha.UseVisualStyleBackColor = true;
            this.chkMostrarSenha.CheckedChanged += new System.EventHandler(this.chkMostrarSenha_CheckedChanged);

            // lblBanco
            this.lblBanco.AutoSize = true;
            this.lblBanco.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblBanco.Location = new System.Drawing.Point(20, 248);
            this.lblBanco.Name = "lblBanco";
            this.lblBanco.Text = "Banco de Dados:";

            // txtBanco
            this.txtBanco.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtBanco.Location = new System.Drawing.Point(20, 269);
            this.txtBanco.Name = "txtBanco";
            this.txtBanco.Size = new System.Drawing.Size(390, 25);
            this.txtBanco.TabIndex = 5;

            // lblCaminho
            this.lblCaminho.AutoSize = true;
            this.lblCaminho.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCaminho.Location = new System.Drawing.Point(20, 306);
            this.lblCaminho.Name = "lblCaminho";
            this.lblCaminho.Text = "Arquivo JSON de configuração:";

            // txtCaminhoJson
            this.txtCaminhoJson.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtCaminhoJson.Location = new System.Drawing.Point(20, 327);
            this.txtCaminhoJson.Name = "txtCaminhoJson";
            this.txtCaminhoJson.ReadOnly = true;
            this.txtCaminhoJson.Size = new System.Drawing.Size(295, 23);
            this.txtCaminhoJson.TabIndex = 6;

            // btnAlterarCaminho
            this.btnAlterarCaminho.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAlterarCaminho.Location = new System.Drawing.Point(323, 326);
            this.btnAlterarCaminho.Name = "btnAlterarCaminho";
            this.btnAlterarCaminho.Size = new System.Drawing.Size(87, 25);
            this.btnAlterarCaminho.TabIndex = 7;
            this.btnAlterarCaminho.Text = "Alterar...";
            this.btnAlterarCaminho.UseVisualStyleBackColor = true;
            this.btnAlterarCaminho.Click += new System.EventHandler(this.btnAlterarCaminho_Click);

            // lblResultadoTeste
            this.lblResultadoTeste.AutoSize = true;
            this.lblResultadoTeste.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblResultadoTeste.Location = new System.Drawing.Point(20, 364);
            this.lblResultadoTeste.Name = "lblResultadoTeste";
            this.lblResultadoTeste.Text = "";

            // btnTestar
            this.btnTestar.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnTestar.Location = new System.Drawing.Point(20, 394);
            this.btnTestar.Name = "btnTestar";
            this.btnTestar.Size = new System.Drawing.Size(135, 36);
            this.btnTestar.TabIndex = 8;
            this.btnTestar.Text = "Testar Conexão";
            this.btnTestar.UseVisualStyleBackColor = true;
            this.btnTestar.Click += new System.EventHandler(this.btnTestar_Click);

            // btnSalvar
            this.btnSalvar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnSalvar.Location = new System.Drawing.Point(202, 394);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(110, 36);
            this.btnSalvar.TabIndex = 9;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);

            // btnCancelar
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnCancelar.Location = new System.Drawing.Point(320, 394);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(90, 36);
            this.btnCancelar.TabIndex = 10;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;

            // FormConfigBanco
            this.AcceptButton = this.btnSalvar;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(430, 450);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblServidor);
            this.Controls.Add(this.txtServidor);
            this.Controls.Add(this.lblPorta);
            this.Controls.Add(this.txtPorta);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.lblSenha);
            this.Controls.Add(this.txtSenha);
            this.Controls.Add(this.chkMostrarSenha);
            this.Controls.Add(this.lblBanco);
            this.Controls.Add(this.txtBanco);
            this.Controls.Add(this.lblCaminho);
            this.Controls.Add(this.txtCaminhoJson);
            this.Controls.Add(this.btnAlterarCaminho);
            this.Controls.Add(this.lblResultadoTeste);
            this.Controls.Add(this.btnTestar);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.btnCancelar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConfigBanco";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configurar Banco de Dados";
            this.Load += new System.EventHandler(this.FormConfigBanco_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblServidor;
        private System.Windows.Forms.TextBox txtServidor;
        private System.Windows.Forms.Label lblPorta;
        private System.Windows.Forms.TextBox txtPorta;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label lblSenha;
        private System.Windows.Forms.TextBox txtSenha;
        private System.Windows.Forms.CheckBox chkMostrarSenha;
        private System.Windows.Forms.Label lblBanco;
        private System.Windows.Forms.TextBox txtBanco;
        private System.Windows.Forms.Label lblCaminho;
        private System.Windows.Forms.TextBox txtCaminhoJson;
        private System.Windows.Forms.Button btnAlterarCaminho;
        private System.Windows.Forms.Label lblResultadoTeste;
        private System.Windows.Forms.Button btnTestar;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
