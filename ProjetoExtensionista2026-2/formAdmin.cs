using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ProjetoExtensionista2026_2.SQL;
using ProjetoExtensionista2026_2.SQL.SQL_functions;
using MySql.Data.MySqlClient;
using MySql.Data.MySqlClient;

namespace ProjetoExtensionista2026_2
{
    public partial class formAdmin : Form
    {
        public formAdmin()
        {
            InitializeComponent();
            ConfigurarControles();
            GerenciadorTema.AplicarTema(this);
            AplicarEstiloDosDataGridViews();
            Load += formAdmin_Load;
        }

        private void ConfigurarControles()
        {
            StartPosition = FormStartPosition.CenterScreen;

            ConfigurarGrid(gridLogs);
            gridLogs.AutoGenerateColumns = false;
            gridLogs.Columns.Add(new DataGridViewTextBoxColumn { Name = "created_at", HeaderText = "Horário", DataPropertyName = "created_at", Width = 125, AutoSizeMode = DataGridViewAutoSizeColumnMode.None, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
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

            gridLogs.Visible = false;
            gridUsers.Visible = false;

            ConfigurarGrid(dataGridView1);
            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "occurred_at", HeaderText = "Horário", DataPropertyName = "occurred_at", Width = 145, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "action", HeaderText = "Ação", DataPropertyName = "action", Width = 220 });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "username", HeaderText = "Usuário", DataPropertyName = "username", Width = 140 });

            ConfigurarGrid(dataGridView2);
            dataGridView2.Columns.Clear();
            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn { Name = "id", HeaderText = "ID", DataPropertyName = "id", Width = 42 });
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn { Name = "username", HeaderText = "Nome", DataPropertyName = "username", Width = 145 });
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn { Name = "role", HeaderText = "Cargo", DataPropertyName = "role", Width = 90 });
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn { Name = "created_at", HeaderText = "Criação", DataPropertyName = "created_at", Width = 125, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
            dataGridView2.Columns.Add(new DataGridViewCheckBoxColumn { Name = "status", HeaderText = "Status", DataPropertyName = "status", Width = 55 });
            dataGridView2.Columns.Add(new DataGridViewCheckBoxColumn { Name = "active", HeaderText = "Ativo", DataPropertyName = "active", Width = 55 });
            dataGridView2.Columns.Add(new DataGridViewButtonColumn { Name = "edit", HeaderText = "", Text = "Editar", UseColumnTextForButtonValue = true, Width = 65 });
            dataGridView2.Columns.Add(new DataGridViewButtonColumn { Name = "delete", HeaderText = "", Text = "Apagar", UseColumnTextForButtonValue = true, Width = 65 });
            dataGridView2.CellContentClick += GridUsers_CellContentClick;
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

        private void AplicarEstiloDosDataGridViews()
        {
            Color fundo = GerenciadorTema.ModoEscuro ? GerenciadorTema.InputEscuro : GerenciadorTema.InputClaro;
            Color linhaAlternada = GerenciadorTema.ModoEscuro ? GerenciadorTema.LinhaAlternadaEscura : GerenciadorTema.LinhaAlternadaClara;
            Color borda = GerenciadorTema.ModoEscuro ? GerenciadorTema.BordaEscura : GerenciadorTema.BordaClara;
            Color texto = GerenciadorTema.ModoEscuro ? GerenciadorTema.TextoEscuro : GerenciadorTema.TextoClaro;

            foreach (DataGridView grid in new[] { dataGridView1, dataGridView2 })
            {
                grid.BackgroundColor = fundo;
                grid.GridColor = borda;
                grid.ForeColor = texto;
                grid.DefaultCellStyle.BackColor = fundo;
                grid.DefaultCellStyle.ForeColor = texto;
                grid.DefaultCellStyle.SelectionBackColor = GerenciadorTema.CorDestaque;
                grid.DefaultCellStyle.SelectionForeColor = Color.White;
                grid.RowsDefaultCellStyle.BackColor = fundo;
                grid.RowsDefaultCellStyle.ForeColor = texto;
                grid.AlternatingRowsDefaultCellStyle.BackColor = linhaAlternada;
                grid.AlternatingRowsDefaultCellStyle.ForeColor = texto;
                grid.ColumnHeadersDefaultCellStyle.BackColor = GerenciadorTema.CorDestaque;
                grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = GerenciadorTema.CorDestaque;
                grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            }

            dataGridView1.Columns["action"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["username"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns["id"].Width = 55;
            dataGridView2.Columns["username"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns["role"].Width = 80;
            dataGridView2.Columns["created_at"].Width = 115;
            dataGridView2.Columns["status"].Width = 65;
            dataGridView2.Columns["active"].Width = 65;
            dataGridView2.Columns["edit"].Width = 65;
            dataGridView2.Columns["delete"].Width = 65;
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
            dataGridView1.DataSource = AuditLogSQL.GetLogs();
            var users = new DataTable();
            using (var connection = new Connection())
            {
                connection.IniciarConexao();
                using (var command = connection.Conexao.CreateCommand())
                {
                    command.CommandText = "SELECT id, username, role, created_at, status, active FROM `Users` ORDER BY id;";
                    using (var adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(users);
                    }
                }
            }
            dataGridView2.DataSource = users;
            lblStatus.Text = users.Rows.Count + " usuário(s) encontrado(s).";
        }

        private void GridUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var row = dataGridView2.Rows[e.RowIndex];
            var id = Convert.ToInt32(row.Cells["id"].Value);
            if (dataGridView2.Columns[e.ColumnIndex].Name == "edit")
                EditarUsuario(id, Convert.ToString(row.Cells["username"].Value), Convert.ToString(row.Cells["role"].Value), Convert.ToBoolean(row.Cells["status"].Value), Convert.ToBoolean(row.Cells["active"].Value));
            else if (dataGridView2.Columns[e.ColumnIndex].Name == "delete")
                ApagarUsuario(id, Convert.ToString(row.Cells["username"].Value));
        }

        private void EditarUsuario(int id, string username, string role, bool status, bool active)
        {
            using (var dialog = new UserEditDialog(username, role, status, active))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                using (var connection = new Connection())
                {
                    connection.IniciarConexao();
                    using (var command = connection.Conexao.CreateCommand())
                    {
                        command.CommandText = "UPDATE `Users` SET username = @username, role = @role, status = @status, active = @active WHERE id = @id;";
                        command.Parameters.AddWithValue("id", id);
                        command.Parameters.AddWithValue("username", dialog.UsernameValue);
                        command.Parameters.AddWithValue("role", dialog.RoleValue);
                        command.Parameters.AddWithValue("status", dialog.StatusValue);
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
                    command.CommandText = "DELETE FROM `Users` WHERE id = @id;";
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
            private readonly ComboBox _role = new ComboBox();
            private readonly CheckBox _status = new CheckBox();
            private readonly CheckBox _active = new CheckBox();

            public string UsernameValue { get { return _username.Text.Trim(); } }
            public string RoleValue { get { return Convert.ToString(_role.SelectedItem); } }
            public bool StatusValue { get { return _status.Checked; } }
            public bool ActiveValue { get { return _active.Checked; } }

            public UserEditDialog(string username, string role, bool status, bool active)
            {
                Text = "Editar usuário";
                StartPosition = FormStartPosition.CenterParent;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox = false;
                MinimizeBox = false;
                ClientSize = new Size(330, 230);

                Controls.Add(new Label { Text = "Nome de usuário", Location = new Point(20, 18), AutoSize = true });
                _username.Location = new Point(20, 40); _username.Width = 285; _username.Text = username;
                Controls.Add(_username);
                Controls.Add(new Label { Text = "Cargo", Location = new Point(20, 75), AutoSize = true });
                _role.Location = new Point(20, 97); _role.Width = 285; _role.DropDownStyle = ComboBoxStyle.DropDownList;
                _role.Items.AddRange(new object[] { "admin", "seller", "stocker", "monetary", "common" });
                foreach (var item in _role.Items)
                {
                    if (string.Equals(Convert.ToString(item), role, StringComparison.OrdinalIgnoreCase))
                    {
                        _role.SelectedItem = item;
                        break;
                    }
                }
                Controls.Add(_role);
                _status.Text = "Conta habilitada (status)"; _status.Location = new Point(20, 132); _status.Checked = status;
                Controls.Add(_status);
                _active.Text = "Usuário aprovado (active)"; _active.Location = new Point(20, 156); _active.Checked = active;
                Controls.Add(_active);

                var save = new Button { Text = "Salvar", DialogResult = DialogResult.OK, Location = new Point(145, 190), Width = 75 };
                var cancel = new Button { Text = "Cancelar", DialogResult = DialogResult.Cancel, Location = new Point(230, 190), Width = 75 };
                save.Click += delegate { if (string.IsNullOrWhiteSpace(UsernameValue) || string.IsNullOrWhiteSpace(RoleValue)) { MessageBox.Show("Nome e cargo são obrigatórios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); DialogResult = DialogResult.None; } };
                Controls.Add(save); Controls.Add(cancel);
                AcceptButton = save; CancelButton = cancel;
            }
        }
    }
}
