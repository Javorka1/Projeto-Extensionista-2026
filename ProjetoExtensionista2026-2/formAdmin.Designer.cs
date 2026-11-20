namespace ProjetoExtensionista2026_2
{
    partial class formAdmin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel areaPrincipal;
        private System.Windows.Forms.Panel quadro;
        private System.Windows.Forms.SplitContainer splitAdmin;
        private System.Windows.Forms.Panel painelLogs;
        private System.Windows.Forms.Panel painelUsuarios;
        private System.Windows.Forms.Panel cabecalhoLogs;
        private System.Windows.Forms.Panel cabecalhoUsuarios;
        private System.Windows.Forms.Label lblTituloLogs;
        private System.Windows.Forms.Label lblSubtituloLogs;
        private System.Windows.Forms.Label lblTituloUsuarios;
        private System.Windows.Forms.Label lblSubtituloUsuarios;
        private System.Windows.Forms.DataGridView gridLogs;
        private System.Windows.Forms.DataGridView gridUsers;
        private System.Windows.Forms.Label lblStatus;

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
            this.areaPrincipal = new System.Windows.Forms.Panel();
            this.quadro = new System.Windows.Forms.Panel();
            this.splitAdmin = new System.Windows.Forms.SplitContainer();
            this.painelLogs = new System.Windows.Forms.Panel();
            this.gridLogs = new System.Windows.Forms.DataGridView();
            this.cabecalhoLogs = new System.Windows.Forms.Panel();
            this.lblSubtituloLogs = new System.Windows.Forms.Label();
            this.lblTituloLogs = new System.Windows.Forms.Label();
            this.painelUsuarios = new System.Windows.Forms.Panel();
            this.gridUsers = new System.Windows.Forms.DataGridView();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cabecalhoUsuarios = new System.Windows.Forms.Panel();
            this.lblSubtituloUsuarios = new System.Windows.Forms.Label();
            this.lblTituloUsuarios = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.DataDG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActionDG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserDG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.IdDG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserDG2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RoleDG = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.areaPrincipal.SuspendLayout();
            this.quadro.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitAdmin)).BeginInit();
            this.splitAdmin.Panel1.SuspendLayout();
            this.splitAdmin.Panel2.SuspendLayout();
            this.splitAdmin.SuspendLayout();
            this.painelLogs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLogs)).BeginInit();
            this.cabecalhoLogs.SuspendLayout();
            this.painelUsuarios.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridUsers)).BeginInit();
            this.cabecalhoUsuarios.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // areaPrincipal
            // 
            this.areaPrincipal.Controls.Add(this.quadro);
            this.areaPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.areaPrincipal.Location = new System.Drawing.Point(0, 0);
            this.areaPrincipal.Name = "areaPrincipal";
            this.areaPrincipal.Padding = new System.Windows.Forms.Padding(16);
            this.areaPrincipal.Size = new System.Drawing.Size(1120, 700);
            this.areaPrincipal.TabIndex = 0;
            this.areaPrincipal.Resize += new System.EventHandler(this.areaPrincipal_Resize);
            // 
            // quadro
            // 
            this.quadro.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.quadro.Controls.Add(this.splitAdmin);
            this.quadro.Location = new System.Drawing.Point(20, 20);
            this.quadro.Name = "quadro";
            this.quadro.Padding = new System.Windows.Forms.Padding(16);
            this.quadro.Size = new System.Drawing.Size(1080, 650);
            this.quadro.TabIndex = 0;
            this.quadro.Tag = "card";
            // 
            // splitAdmin
            // 
            this.splitAdmin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitAdmin.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitAdmin.IsSplitterFixed = true;
            this.splitAdmin.Location = new System.Drawing.Point(16, 16);
            this.splitAdmin.Name = "splitAdmin";
            // 
            // splitAdmin.Panel1
            // 
            this.splitAdmin.Panel1.Controls.Add(this.painelLogs);
            this.splitAdmin.Panel1.Padding = new System.Windows.Forms.Padding(0, 0, 8, 0);
            // 
            // splitAdmin.Panel2
            // 
            this.splitAdmin.Panel2.Controls.Add(this.painelUsuarios);
            this.splitAdmin.Panel2.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.splitAdmin.Size = new System.Drawing.Size(1048, 618);
            this.splitAdmin.SplitterDistance = 121;
            this.splitAdmin.TabIndex = 0;
            // 
            // painelLogs
            // 
            this.painelLogs.Controls.Add(this.gridLogs);
            this.painelLogs.Controls.Add(this.cabecalhoLogs);
            this.painelLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.painelLogs.Location = new System.Drawing.Point(0, 0);
            this.painelLogs.Name = "painelLogs";
            this.painelLogs.Padding = new System.Windows.Forms.Padding(8);
            this.painelLogs.Size = new System.Drawing.Size(113, 618);
            this.painelLogs.TabIndex = 0;
            // 
            // gridLogs
            // 
            this.gridLogs.AllowUserToAddRows = false;
            this.gridLogs.AllowUserToDeleteRows = false;
            this.gridLogs.AllowUserToResizeColumns = false;
            this.gridLogs.AllowUserToResizeRows = false;
            this.gridLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridLogs.Location = new System.Drawing.Point(8, 66);
            this.gridLogs.Name = "gridLogs";
            this.gridLogs.ReadOnly = true;
            this.gridLogs.RowHeadersVisible = false;
            this.gridLogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridLogs.Size = new System.Drawing.Size(97, 544);
            this.gridLogs.TabIndex = 1;
            // 
            // cabecalhoLogs
            // 
            this.cabecalhoLogs.Controls.Add(this.lblSubtituloLogs);
            this.cabecalhoLogs.Controls.Add(this.lblTituloLogs);
            this.cabecalhoLogs.Dock = System.Windows.Forms.DockStyle.Top;
            this.cabecalhoLogs.Location = new System.Drawing.Point(8, 8);
            this.cabecalhoLogs.Name = "cabecalhoLogs";
            this.cabecalhoLogs.Size = new System.Drawing.Size(97, 58);
            this.cabecalhoLogs.TabIndex = 0;
            // 
            // lblSubtituloLogs
            // 
            this.lblSubtituloLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubtituloLogs.Location = new System.Drawing.Point(0, 30);
            this.lblSubtituloLogs.Name = "lblSubtituloLogs";
            this.lblSubtituloLogs.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.lblSubtituloLogs.Size = new System.Drawing.Size(97, 28);
            this.lblSubtituloLogs.TabIndex = 1;
            this.lblSubtituloLogs.Tag = "subtitulo";
            this.lblSubtituloLogs.Text = "Ações realizadas pelos usuários";
            // 
            // lblTituloLogs
            // 
            this.lblTituloLogs.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTituloLogs.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTituloLogs.Location = new System.Drawing.Point(0, 0);
            this.lblTituloLogs.Name = "lblTituloLogs";
            this.lblTituloLogs.Padding = new System.Windows.Forms.Padding(4, 2, 0, 0);
            this.lblTituloLogs.Size = new System.Drawing.Size(97, 30);
            this.lblTituloLogs.TabIndex = 0;
            this.lblTituloLogs.Text = "Histórico de logs";
            // 
            // painelUsuarios
            // 
            this.painelUsuarios.Controls.Add(this.gridUsers);
            this.painelUsuarios.Controls.Add(this.lblStatus);
            this.painelUsuarios.Controls.Add(this.cabecalhoUsuarios);
            this.painelUsuarios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.painelUsuarios.Location = new System.Drawing.Point(8, 0);
            this.painelUsuarios.Name = "painelUsuarios";
            this.painelUsuarios.Padding = new System.Windows.Forms.Padding(8);
            this.painelUsuarios.Size = new System.Drawing.Size(915, 618);
            this.painelUsuarios.TabIndex = 0;
            // 
            // gridUsers
            // 
            this.gridUsers.AllowUserToAddRows = false;
            this.gridUsers.AllowUserToDeleteRows = false;
            this.gridUsers.AllowUserToResizeColumns = false;
            this.gridUsers.AllowUserToResizeRows = false;
            this.gridUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridUsers.Location = new System.Drawing.Point(8, 66);
            this.gridUsers.Name = "gridUsers";
            this.gridUsers.ReadOnly = true;
            this.gridUsers.RowHeadersVisible = false;
            this.gridUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridUsers.Size = new System.Drawing.Size(899, 519);
            this.gridUsers.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Location = new System.Drawing.Point(8, 585);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(4, 4, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(899, 25);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "0 usuário(s) encontrado(s).";
            // 
            // cabecalhoUsuarios
            // 
            this.cabecalhoUsuarios.Controls.Add(this.lblSubtituloUsuarios);
            this.cabecalhoUsuarios.Controls.Add(this.lblTituloUsuarios);
            this.cabecalhoUsuarios.Dock = System.Windows.Forms.DockStyle.Top;
            this.cabecalhoUsuarios.Location = new System.Drawing.Point(8, 8);
            this.cabecalhoUsuarios.Name = "cabecalhoUsuarios";
            this.cabecalhoUsuarios.Size = new System.Drawing.Size(899, 58);
            this.cabecalhoUsuarios.TabIndex = 0;
            // 
            // lblSubtituloUsuarios
            // 
            this.lblSubtituloUsuarios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubtituloUsuarios.Location = new System.Drawing.Point(0, 30);
            this.lblSubtituloUsuarios.Name = "lblSubtituloUsuarios";
            this.lblSubtituloUsuarios.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.lblSubtituloUsuarios.Size = new System.Drawing.Size(899, 28);
            this.lblSubtituloUsuarios.TabIndex = 1;
            this.lblSubtituloUsuarios.Tag = "subtitulo";
            this.lblSubtituloUsuarios.Text = "Gerencie acesso, cargo e status";
            // 
            // lblTituloUsuarios
            // 
            this.lblTituloUsuarios.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTituloUsuarios.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTituloUsuarios.Location = new System.Drawing.Point(0, 0);
            this.lblTituloUsuarios.Name = "lblTituloUsuarios";
            this.lblTituloUsuarios.Padding = new System.Windows.Forms.Padding(4, 2, 0, 0);
            this.lblTituloUsuarios.Size = new System.Drawing.Size(899, 30);
            this.lblTituloUsuarios.TabIndex = 0;
            this.lblTituloUsuarios.Text = "Usuários";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DataDG,
            this.ActionDG,
            this.UserDG});
            this.dataGridView1.Location = new System.Drawing.Point(44, 112);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(726, 864);
            this.dataGridView1.TabIndex = 0;
            // 
            // DataDG
            // 
            this.DataDG.HeaderText = "Data";
            this.DataDG.Name = "DataDG";
            this.DataDG.ReadOnly = true;
            this.DataDG.Width = 50;
            // 
            // ActionDG
            // 
            this.ActionDG.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ActionDG.HeaderText = "Ação";
            this.ActionDG.Name = "ActionDG";
            this.ActionDG.ReadOnly = true;
            // 
            // UserDG
            // 
            this.UserDG.HeaderText = "Usuario";
            this.UserDG.Name = "UserDG";
            this.UserDG.ReadOnly = true;
            this.UserDG.Width = 150;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IdDG,
            this.UserDG2,
            this.RoleDG});
            this.dataGridView2.Location = new System.Drawing.Point(919, 113);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(628, 864);
            this.dataGridView2.TabIndex = 1;
            // 
            // IdDG
            // 
            this.IdDG.HeaderText = "ID";
            this.IdDG.Name = "IdDG";
            this.IdDG.Width = 40;
            // 
            // UserDG2
            // 
            this.UserDG2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.UserDG2.HeaderText = "Usuario";
            this.UserDG2.Name = "UserDG2";
            // 
            // RoleDG
            // 
            this.RoleDG.HeaderText = "Cargo";
            this.RoleDG.Name = "RoleDG";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(52, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 40);
            this.label1.TabIndex = 2;
            this.label1.Text = "Logs";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(924, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 40);
            this.label2.TabIndex = 3;
            this.label2.Text = "Users";
            // 
            // formAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1612, 1041);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.dataGridView1);
            this.MinimumSize = new System.Drawing.Size(900, 560);
            this.Name = "formAdmin";
            this.Text = "Administração do sistema";
            this.areaPrincipal.ResumeLayout(false);
            this.quadro.ResumeLayout(false);
            this.splitAdmin.Panel1.ResumeLayout(false);
            this.splitAdmin.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitAdmin)).EndInit();
            this.splitAdmin.ResumeLayout(false);
            this.painelLogs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridLogs)).EndInit();
            this.cabecalhoLogs.ResumeLayout(false);
            this.painelUsuarios.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridUsers)).EndInit();
            this.cabecalhoUsuarios.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private void areaPrincipal_Resize(object sender, System.EventArgs e)
        {
            int largura = System.Math.Min(1160, System.Math.Max(0, this.areaPrincipal.ClientSize.Width - 32));
            int altura = System.Math.Min(680, System.Math.Max(0, this.areaPrincipal.ClientSize.Height - 32));
            if (largura <= 0 || altura <= 0)
                return;

            this.quadro.Size = new System.Drawing.Size(largura, altura);
            this.quadro.Location = new System.Drawing.Point(
                System.Math.Max(0, (this.areaPrincipal.ClientSize.Width - this.quadro.Width) / 2),
                System.Math.Max(0, (this.areaPrincipal.ClientSize.Height - this.quadro.Height) / 2));
        }

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataDG;
        private System.Windows.Forms.DataGridViewTextBoxColumn ActionDG;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserDG;
        private System.Data.Common.DataAdapter dataAdapter1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdDG;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserDG2;
        private System.Windows.Forms.DataGridViewComboBoxColumn RoleDG;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}