namespace ProjetoExtensionista2026_2
{
    partial class formEstoque
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
            this.searchTxtBox = new System.Windows.Forms.TextBox();
            this.searchBarPanel = new System.Windows.Forms.Panel();
            this.filterBtn = new System.Windows.Forms.Button();
            this.searchBtn = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.searchBarPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // searchTxtBox
            // 
            this.searchTxtBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.searchTxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchTxtBox.Location = new System.Drawing.Point(0, 0);
            this.searchTxtBox.Multiline = true;
            this.searchTxtBox.Name = "searchTxtBox";
            this.searchTxtBox.Size = new System.Drawing.Size(1266, 49);
            this.searchTxtBox.TabIndex = 0;
            // 
            // searchBarPanel
            // 
            this.searchBarPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchBarPanel.Controls.Add(this.filterBtn);
            this.searchBarPanel.Controls.Add(this.searchBtn);
            this.searchBarPanel.Controls.Add(this.searchTxtBox);
            this.searchBarPanel.Location = new System.Drawing.Point(18, 12);
            this.searchBarPanel.Name = "searchBarPanel";
            this.searchBarPanel.Size = new System.Drawing.Size(1509, 51);
            this.searchBarPanel.TabIndex = 1;
            // 
            // filterBtn
            // 
            this.filterBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.filterBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.filterBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterBtn.Location = new System.Drawing.Point(1373, 0);
            this.filterBtn.Name = "filterBtn";
            this.filterBtn.Size = new System.Drawing.Size(66, 49);
            this.filterBtn.TabIndex = 1;
            this.filterBtn.Text = "Filtros";
            this.filterBtn.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterBtn.UseVisualStyleBackColor = true;
            // 
            // searchBtn
            // 
            this.searchBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.searchBtn.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchBtn.Location = new System.Drawing.Point(1439, 0);
            this.searchBtn.Name = "searchBtn";
            this.searchBtn.Size = new System.Drawing.Size(68, 49);
            this.searchBtn.TabIndex = 2;
            this.searchBtn.Text = "Buscar";
            this.searchBtn.UseVisualStyleBackColor = true;
            this.searchBtn.Click += new System.EventHandler(this.searchBtn_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(19, 83);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1510, 586);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // formEstoque
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1541, 681);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.searchBarPanel);
            this.Name = "formEstoque";
            this.Text = "formEstoque";
            this.Load += new System.EventHandler(this.formEstoque_Load);
            this.searchBarPanel.ResumeLayout(false);
            this.searchBarPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox searchTxtBox;
        private System.Windows.Forms.Panel searchBarPanel;
        private System.Windows.Forms.Button filterBtn;
        private System.Windows.Forms.Button searchBtn;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}