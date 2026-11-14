using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ProjetoExtensionista2026_2.SQL;
using ProjetoExtensionista2026_2.SQL.SQL_functions;
using Npgsql;

namespace ProjetoExtensionista2026_2
{
    public partial class formAdmin : Form
    {
        public formAdmin()
        {
            InitializeComponent();
            ConfigurarControles();
            GerenciadorTema.AplicarTema(this);
            Load += formAdmin_Load;
        }

        private void ConfigurarControles()
        {
            StartPosition = FormStartPosition.CenterScreen;

            ConfigurarGrid(gridLogs);
            gridLogs.AutoGenerateColumns = false;
            gridLogs.Columns.Add(new DataGridViewTextBoxColumn { Name = "occurred_at", HeaderText = "Horário", DataPropertyName = "occurred_at", Width = 125, AutoSizeMode = DataGridViewAutoSizeColumnMode.None, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
            gridLogs.Columns.Add(new DataGridViewTextBoxColumn { Name = "action", HeaderText = "Ação", DataPropertyName = "action", Width = 145, AutoSizeMode = DataGridViewAutoSizeColumnMode.None });
            gridLogs.Columns.Add(new DataGridViewTextBoxColumn { Name = "username", HeaderText = "Usuário", DataPropertyName = "username", Width = 95, AutoSizeMode = DataGridViewAutoSizeColumnMode.None });

            ConfigurarGrid(gridUsers);
            gridUsers.AutoGenerateColumns = false;
            gridUsers.Columns.Add(new DataGridViewTextBoxColumn { Name = "id", HeaderText = "ID", DataPropertyName = "id", Width = 42, AutoSizeMode = DataGridViewAutoSizeColumnMode.None });
            gridUsers.Columns.Add(new DataGridViewTextBoxColumn { Name = "username", HeaderText = "Nome", DataPropertyName = "username", Width = 145, AutoSizeMode = DataGridViewAutoSizeColumnMode.None });
            gridUsers.Columns.Add(new DataGridViewTextBoxColumn { Name = "role", HeaderText = "Cargo", DataPropertyName = "role", Width = 90, AutoSizeMode = DataGridViewAutoSizeColumnMode.None });
            gridUsers.Columns.Add(new DataGridViewTextBoxColumn { Name = "created_at", HeaderText = "Criação", DataPropertyName = "created_at", Width = 125, AutoSizeMode = DataGridViewAutoSizeColumnMode.None, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
            gridUsers.Columns.Add(new DataGridViewCheckBoxColumn { Name = "active", HeaderText = "Ativo", DataPropertyName = "active", Width = 55, AutoSizeMode = DataGridViewAutoSizeColumnMode.None });
            gridUsers.Columns.Add(new DataGridViewButtonColumn { Name = "edit", HeaderText = "", Text = "Editar", UseColumnTextForButtonValue = true, Width = 65, AutoSizeMode = DataGridViewAutoSizeColumnMode.None });
            gridUsers.Columns.Add(new DataGridViewButtonColumn { Name = "delete", HeaderText = "", Text = "Apagar", UseColumnTextForButtonValue = true, Width = 65, AutoSizeMode = DataGridViewAutoSizeColumnMode.None });
            gridUsers.CellContentClick += GridUsers_CellContentClick;
        }

        private void ConfigurarGrid(DataGridView grid)
        {
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.FixedSingle;
            grid.ForeColor = Color.DarkGray;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AllowUserToResizeColumns = false;
            grid.ReadOnly = true;
            grid.MultiSelect = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.RowHeadersVisible = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            grid.ColumnHeadersHeight = 36;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            grid.DefaultCellStyle = new DataGridViewCellStyle { Padding = new Padding(5, 2, 5, 2) };
        }

        private void formAdmin_Load(object sender, EventArgs e)
        {
            try
            {
                AuditLogSQL.EnsureSchema();
                CarregarDados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível carregar a administração: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarDados()
        {
            gridLogs.DataSource = AuditLogSQL.GetLogs();
            var users = new DataTable();
            using (var connection = new Connection())
            {
                connection.IniciarConexao();
                using (var command = connection.Conexao.CreateCommand())
                {
                    command.CommandText = @"SELECT id, username, role, created_at, active FROM ""Users"" ORDER BY id;";
                    using (var adapter = new NpgsqlDataAdapter(command))
                    {
                        adapter.Fill(users);
                    }
                }
            }
            gridUsers.DataSource = users;
            lblStatus.Text = users.Rows.Count + " usuário(s) encontrado(s).";
        }

        private void GridUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var row = gridUsers.Rows[e.RowIndex];
            var id = Convert.ToInt32(row.Cells["id"].Value);
            if (gridUsers.Columns[e.ColumnIndex].Name == "edit")
                EditarUsuario(id, Convert.ToString(row.Cells["username"].Value), Convert.ToString(row.Cells["role"].Value), Convert.ToBoolean(row.Cells["active"].Value));
            else if (gridUsers.Columns[e.ColumnIndex].Name == "delete")
                ApagarUsuario(id, Convert.ToString(row.Cells["username"].Value));
        }

        private void EditarUsuario(int id, string username, string role, bool active)
        {
            using (var dialog = new UserEditDialog(username, role, active))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                using (var connection = new Connection())
                {
                    connection.IniciarConexao();
                    using (var command = connection.Conexao.CreateCommand())
                    {
                        command.CommandText = @"UPDATE ""Users"" SET username = @username, role = @role, active = @active WHERE id = @id;";
                        command.Parameters.AddWithValue("id", id);
                        command.Parameters.AddWithValue("username", dialog.UsernameValue);
                        command.Parameters.AddWithValue("role", dialog.RoleValue);
                        command.Parameters.AddWithValue("active", dialog.ActiveValue);
                        command.ExecuteNonQuery();
                    }
                }
                AuditLogSQL.Register("Usuário editado: " + dialog.UsernameValue, LoginSQL.SessaoUsuario.Nome);
                CarregarDados();
            }
        }

        private void ApagarUsuario(int id, string username)
        {
            if (MessageBox.Show("Apagar o usuário '" + username + "'?", "Confirmar exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            using (var connection = new Connection())
            {
                connection.IniciarConexao();
                using (var command = connection.Conexao.CreateCommand())
                {
                    command.CommandText = @"DELETE FROM ""Users"" WHERE id = @id;";
                    command.Parameters.AddWithValue("id", id);
                    command.ExecuteNonQuery();
                }
            }
            AuditLogSQL.Register("Usuário apagado: " + username, LoginSQL.SessaoUsuario.Nome);
            CarregarDados();
        }

        private sealed class UserEditDialog : Form
        {
            private readonly TextBox _username = new TextBox();
            private readonly TextBox _role = new TextBox();
            private readonly CheckBox _active = new CheckBox();

            public string UsernameValue { get { return _username.Text.Trim(); } }
            public string RoleValue { get { return _role.Text.Trim(); } }
            public bool ActiveValue { get { return _active.Checked; } }

            public UserEditDialog(string username, string role, bool active)
            {
                Text = "Editar usuário";
                StartPosition = FormStartPosition.CenterParent;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox = false;
                MinimizeBox = false;
                ClientSize = new Size(330, 205);

                Controls.Add(new Label { Text = "Nome de usuário", Location = new Point(20, 18), AutoSize = true });
                _username.Location = new Point(20, 40); _username.Width = 285; _username.Text = username;
                Controls.Add(_username);
                Controls.Add(new Label { Text = "Cargo", Location = new Point(20, 75), AutoSize = true });
                _role.Location = new Point(20, 97); _role.Width = 285; _role.Text = role;
                Controls.Add(_role);
                _active.Text = "Usuário ativo"; _active.Location = new Point(20, 132); _active.Checked = active;
                Controls.Add(_active);

                var save = new Button { Text = "Salvar", DialogResult = DialogResult.OK, Location = new Point(145, 165), Width = 75 };
                var cancel = new Button { Text = "Cancelar", DialogResult = DialogResult.Cancel, Location = new Point(230, 165), Width = 75 };
                save.Click += delegate { if (string.IsNullOrWhiteSpace(UsernameValue) || string.IsNullOrWhiteSpace(RoleValue)) { MessageBox.Show("Nome e cargo são obrigatórios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); DialogResult = DialogResult.None; } };
                Controls.Add(save); Controls.Add(cancel);
                AcceptButton = save; CancelButton = cancel;
            }
        }
    }
}
