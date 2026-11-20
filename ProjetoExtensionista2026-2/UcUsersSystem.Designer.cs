namespace ProjetoExtensionista2026_2
{
    partial class UcUsersSystem
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

        #region Código gerado pelo Designer de Componentes





        private void InitializeComponent()
        {
            this.imgUser = new System.Windows.Forms.PictureBox();
            this.userName = new System.Windows.Forms.Label();
            this.lblRole = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imgUser)).BeginInit();
            this.SuspendLayout();



            this.imgUser.Location = new System.Drawing.Point(10, 10);
            this.imgUser.Name = "imgUser";
            this.imgUser.Size = new System.Drawing.Size(60, 60);
            this.imgUser.TabIndex = 0;
            this.imgUser.TabStop = false;



            this.userName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userName.Location = new System.Drawing.Point(90, 28);
            this.userName.Name = "userName";
            this.userName.Size = new System.Drawing.Size(225, 24);
            this.userName.TabIndex = 1;
            this.userName.Text = "Placheholder";
            this.userName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;



            this.lblRole.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRole.Location = new System.Drawing.Point(321, 28);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(225, 24);
            this.lblRole.TabIndex = 2;
            this.lblRole.Text = "Placheholder";
            this.lblRole.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;



            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblRole);
            this.Controls.Add(this.userName);
            this.Controls.Add(this.imgUser);
            this.Name = "UcUsersSystem";
            this.Size = new System.Drawing.Size(800, 80);
            ((System.ComponentModel.ISupportInitialize)(this.imgUser)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox imgUser;
        private System.Windows.Forms.Label userName;
        private System.Windows.Forms.Label lblRole;
    }
}
