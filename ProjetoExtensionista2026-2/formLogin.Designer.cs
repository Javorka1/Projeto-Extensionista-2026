namespace ProjetoExtensionista2026_2
{
    partial class formLogin
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

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formLogin));
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlCardLogin = new System.Windows.Forms.Panel();
            this.labelSignIn = new System.Windows.Forms.Label();
            this.loginButton = new System.Windows.Forms.Button();
            this.pwdTxtBox = new System.Windows.Forms.TextBox();
            this.lblSenha = new System.Windows.Forms.Label();
            this.userTxtBox = new System.Windows.Forms.TextBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.lblSubtituloLogin = new System.Windows.Forms.Label();
            this.lblTituloLogin = new System.Windows.Forms.Label();
            this.lockIcon = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.pnlCardLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lockIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlCardLogin);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(684, 641);
            this.panel1.TabIndex = 0;
            // 
            // pnlCardLogin
            // 
            this.pnlCardLogin.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlCardLogin.Controls.Add(this.labelSignIn);
            this.pnlCardLogin.Controls.Add(this.loginButton);
            this.pnlCardLogin.Controls.Add(this.pwdTxtBox);
            this.pnlCardLogin.Controls.Add(this.lblSenha);
            this.pnlCardLogin.Controls.Add(this.userTxtBox);
            this.pnlCardLogin.Controls.Add(this.lblUsuario);
            this.pnlCardLogin.Controls.Add(this.lblSubtituloLogin);
            this.pnlCardLogin.Controls.Add(this.lblTituloLogin);
            this.pnlCardLogin.Controls.Add(this.lockIcon);
            this.pnlCardLogin.Location = new System.Drawing.Point(142, 55);
            this.pnlCardLogin.Name = "pnlCardLogin";
            this.pnlCardLogin.Padding = new System.Windows.Forms.Padding(30, 24, 30, 24);
            this.pnlCardLogin.Size = new System.Drawing.Size(400, 520);
            this.pnlCardLogin.TabIndex = 0;
            this.pnlCardLogin.Tag = "card";
            // 
            // labelSignIn
            // 
            this.labelSignIn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelSignIn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSignIn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.labelSignIn.Location = new System.Drawing.Point(30, 460);
            this.labelSignIn.Name = "labelSignIn";
            this.labelSignIn.Size = new System.Drawing.Size(340, 24);
            this.labelSignIn.TabIndex = 8;
            this.labelSignIn.Text = "Nao possui cadastro? Cadastre-se aqui";
            this.labelSignIn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelSignIn.Click += new System.EventHandler(this.labelSignIn_Click);
            // 
            // loginButton
            // 
            this.loginButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.loginButton.FlatAppearance.BorderSize = 0;
            this.loginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loginButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginButton.ForeColor = System.Drawing.Color.White;
            this.loginButton.Location = new System.Drawing.Point(30, 395);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(340, 44);
            this.loginButton.TabIndex = 7;
            this.loginButton.Tag = "primary";
            this.loginButton.Text = "Entrar no Sistema";
            this.loginButton.UseVisualStyleBackColor = false;
            this.loginButton.Click += new System.EventHandler(this.AlternarCadeado);
            // 
            // pwdTxtBox
            // 
            this.pwdTxtBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pwdTxtBox.Location = new System.Drawing.Point(30, 335);
            this.pwdTxtBox.Name = "pwdTxtBox";
            this.pwdTxtBox.Size = new System.Drawing.Size(340, 25);
            this.pwdTxtBox.TabIndex = 6;
            this.pwdTxtBox.Enter += new System.EventHandler(this.TextBox_Enter);
            this.pwdTxtBox.Leave += new System.EventHandler(this.TxtSenha_Leave);
            // 
            // lblSenha
            // 
            this.lblSenha.AutoSize = true;
            this.lblSenha.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSenha.Location = new System.Drawing.Point(30, 315);
            this.lblSenha.Name = "lblSenha";
            this.lblSenha.Size = new System.Drawing.Size(99, 15);
            this.lblSenha.TabIndex = 5;
            this.lblSenha.Text = "Senha de Acesso";
            // 
            // userTxtBox
            // 
            this.userTxtBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userTxtBox.Location = new System.Drawing.Point(30, 265);
            this.userTxtBox.Name = "userTxtBox";
            this.userTxtBox.Size = new System.Drawing.Size(340, 25);
            this.userTxtBox.TabIndex = 4;
            this.userTxtBox.Enter += new System.EventHandler(this.TextBox_Enter);
            this.userTxtBox.Leave += new System.EventHandler(this.TxtSenha_Leave);
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuario.Location = new System.Drawing.Point(30, 245);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(103, 15);
            this.lblUsuario.TabIndex = 3;
            this.lblUsuario.Text = "Nome de Usuario";
            // 
            // lblSubtituloLogin
            // 
            this.lblSubtituloLogin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtituloLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(150)))), ((int)(((byte)(165)))));
            this.lblSubtituloLogin.Location = new System.Drawing.Point(30, 195);
            this.lblSubtituloLogin.Name = "lblSubtituloLogin";
            this.lblSubtituloLogin.Size = new System.Drawing.Size(340, 22);
            this.lblSubtituloLogin.TabIndex = 2;
            this.lblSubtituloLogin.Tag = "subtitulo";
            this.lblSubtituloLogin.Text = "Informe suas credenciais para acessar o sistema";
            this.lblSubtituloLogin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTituloLogin
            // 
            this.lblTituloLogin.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloLogin.Location = new System.Drawing.Point(30, 160);
            this.lblTituloLogin.Name = "lblTituloLogin";
            this.lblTituloLogin.Size = new System.Drawing.Size(340, 32);
            this.lblTituloLogin.TabIndex = 1;
            this.lblTituloLogin.Text = "Acesso ao Sistema";
            this.lblTituloLogin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lockIcon
            // 
            this.lockIcon.InitialImage = ((System.Drawing.Image)(resources.GetObject("lockIcon.InitialImage")));
            this.lockIcon.Location = new System.Drawing.Point(160, 45);
            this.lockIcon.Name = "lockIcon";
            this.lockIcon.Size = new System.Drawing.Size(80, 80);
            this.lockIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.lockIcon.TabIndex = 0;
            this.lockIcon.TabStop = false;
            // 
            // formLogin
            // 
            this.AcceptButton = this.loginButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 641);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "formLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NexStore 2026 - Autenticacao";
            this.Load += new System.EventHandler(this.formLogin_Load);
            this.Resize += new System.EventHandler(this.formLogin_Resize);
            this.panel1.ResumeLayout(false);
            this.pnlCardLogin.ResumeLayout(false);
            this.pnlCardLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lockIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlCardLogin;
        private System.Windows.Forms.PictureBox lockIcon;
        private System.Windows.Forms.Label lblTituloLogin;
        private System.Windows.Forms.Label lblSubtituloLogin;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.TextBox userTxtBox;
        private System.Windows.Forms.Label lblSenha;
        private System.Windows.Forms.TextBox pwdTxtBox;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Label labelSignIn;
    }
}
