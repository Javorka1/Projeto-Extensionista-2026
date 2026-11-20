namespace ProjetoExtensionista2026_2
{
    partial class miniFormSignIn
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
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.pwdTextBox = new System.Windows.Forms.TextBox();
            this.RegisterBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();



            this.nameTextBox.Location = new System.Drawing.Point(300, 160);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(200, 20);
            this.nameTextBox.TabIndex = 0;
            this.nameTextBox.Enter += new System.EventHandler(this.TextBox_Enter);
            this.nameTextBox.Leave += new System.EventHandler(this.TxtSenha_Leave);



            this.pwdTextBox.Location = new System.Drawing.Point(300, 197);
            this.pwdTextBox.Name = "pwdTextBox";
            this.pwdTextBox.Size = new System.Drawing.Size(200, 20);
            this.pwdTextBox.TabIndex = 1;
            this.pwdTextBox.Enter += new System.EventHandler(this.TextBox_Enter);
            this.pwdTextBox.Leave += new System.EventHandler(this.TxtSenha_Leave);



            this.RegisterBtn.Location = new System.Drawing.Point(325, 229);
            this.RegisterBtn.Name = "RegisterBtn";
            this.RegisterBtn.Size = new System.Drawing.Size(150, 25);
            this.RegisterBtn.TabIndex = 2;
            this.RegisterBtn.Text = "Registre-se";
            this.RegisterBtn.UseVisualStyleBackColor = true;
            this.RegisterBtn.Click += new System.EventHandler(this.RegisterBtn_Click);



            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 411);
            this.Controls.Add(this.RegisterBtn);
            this.Controls.Add(this.pwdTextBox);
            this.Controls.Add(this.nameTextBox);
            this.Name = "miniFormSignIn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "miniFormSignIn";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.TextBox pwdTextBox;
        private System.Windows.Forms.Button RegisterBtn;
    }
}