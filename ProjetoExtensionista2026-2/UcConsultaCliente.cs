using Npgsql;
using ProjetoExtensionista2026_2.DTOs;
using ProjetoExtensionista2026_2.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public partial class UcConsultaCliente : UserControl
    {
        public event Action<ClienteDTO> AoSelecionarCliente;

        private List<ClienteDTO> _listaClientes = new List<ClienteDTO>();
        private CancellationTokenSource _ctsBusca;

        private string _colunaOrdenacao = "Id";
        private bool _ordemCrescente = true;

        private ContextMenuStrip _menuContextoGrid;
        private string _valorCelulaClicada = string.Empty;

        private bool _precisaAtualizar;
        private bool _ignorandoProprioEvento;

        private bool EhModoPopup => this.FindForm()?.Modal ?? false;

        public UcConsultaCliente()
        {
            InitializeComponent();

            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);

            if (dgwClientes != null)
                dgwClientes.AutoGenerateColumns = false;

            ConfigurarEventos();
            ConfigurarMenuContexto();
            ConfigurarSincronizacaoEventos();
        }

        private void ConfigurarSincronizacaoEventos()
        {
            Action aoMudarClientes = async () =>
            {
                if (_ignorandoProprioEvento || IsDisposed || !IsHandleCreated)
                    return;

                if (InvokeRequired)
                {
                    BeginInvoke(new Action(async () =>
                    {
                        if (Visible)
                        {
                            _precisaAtualizar = false;
                            await BuscarClientesBancoAsync(txtPesquisa?.Text.Trim() ?? string.Empty);
                        }
                        else
                        {
                            _precisaAtualizar = true;
                        }
                    }));
                    return;
                }

                if (Visible)
                {
                    _precisaAtualizar = false;
                    await BuscarClientesBancoAsync(txtPesquisa?.Text.Trim() ?? string.Empty);
                }
                else
                {
                    _precisaAtualizar = true;
                }
            };

            EventosSistema.ClienteAtualizado += aoMudarClientes;

            VisibleChanged += async (s, e) =>
            {
                if (Visible && _precisaAtualizar && !IsDisposed && IsHandleCreated)
                {
                    _precisaAtualizar = false;
                    await BuscarClientesBancoAsync(txtPesquisa?.Text.Trim() ?? string.Empty);
                }
            };

            HandleDestroyed += (s, e) =>
            {
                EventosSistema.ClienteAtualizado -= aoMudarClientes;
                _ctsBusca?.Cancel();
                _ctsBusca?.Dispose();
                _menuContextoGrid?.Dispose();
            };
        }

        private void NotificarAlteracaoPropria()
        {
            _ignorandoProprioEvento = true;
            try
            {
                EventosSistema.NotificarClienteAtualizado();
            }
            finally
            {
                _ignorandoProprioEvento = false;
            }
        }

        private void ConfigurarEventos()
        {
            this.Load += async (sender, e) =>
            {
                ConfigurarModoExibicao();
                await BuscarClientesBancoAsync(string.Empty);
                txtPesquisa?.Focus();
            };

            if (txtPesquisa != null)
            {
                txtPesquisa.TextChanged += txtPesquisa_TextChanged;
                txtPesquisa.KeyDown += txtPesquisa_KeyDown;
            }

            if (dgwClientes != null)
            {
                dgwClientes.ColumnHeaderMouseClick += dgwClientes_ColumnHeaderMouseClick;
                dgwClientes.CellFormatting += dgwClientes_CellFormatting;

                dgwClientes.CellMouseDown += (sender, e) =>
                {
                    if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
                    {
                        dgwClientes.ClearSelection();
                        dgwClientes.Rows[e.RowIndex].Selected = true;
                        dgwClientes.CurrentCell = dgwClientes.Rows[e.RowIndex].Cells[e.ColumnIndex];
                        _valorCelulaClicada = dgwClientes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? string.Empty;
                    }
                };

                dgwClientes.CellDoubleClick += (sender, e) =>
                {
                    if (e.RowIndex >= 0)
                        ExecutarAcaoLinha(e.RowIndex);
                };

                dgwClientes.KeyDown += dgwClientes_KeyDown;
            }
        }

        private void ConfigurarMenuContexto()
        {
            if (dgwClientes == null) return;

            _menuContextoGrid = new ContextMenuStrip();

            var itemHistorico = new ToolStripMenuItem("Ver Histórico de Vendas");
            var sepHistorico = new ToolStripSeparator();
            var itemCopiarCelula = new ToolStripMenuItem("Copiar Célula");
            var itemCopiarNome = new ToolStripMenuItem("Copiar Nome");
            var itemCopiarDoc = new ToolStripMenuItem("Copiar CPF/CNPJ");
            var itemCopiarTel = new ToolStripMenuItem("Copiar Telefone");
            var itemCopiarEnd = new ToolStripMenuItem("Copiar Endereço");

            itemHistorico.Click += (s, e) => AbrirHistoricoDoClienteSelecionado();
            itemCopiarCelula.Click += (s, e) => CopiarParaAreaTransferencia(_valorCelulaClicada);

            itemCopiarNome.Click += (s, e) =>
            {
                var cli = ObterClienteSelecionado();
                if (cli != null) CopiarParaAreaTransferencia(cli.Nome);
            };

            itemCopiarDoc.Click += (s, e) =>
            {
                var cli = ObterClienteSelecionado();
                if (cli != null && !string.IsNullOrWhiteSpace(cli.CpfCnpj))
                    CopiarParaAreaTransferencia(FormatarCpfCnpj(cli.CpfCnpj));
            };

            itemCopiarTel.Click += (s, e) =>
            {
                var cli = ObterClienteSelecionado();
                if (cli != null && !string.IsNullOrWhiteSpace(cli.Telefone))
                    CopiarParaAreaTransferencia(FormatarTelefone(cli.Telefone));
            };

            itemCopiarEnd.Click += (s, e) =>
            {
                var cli = ObterClienteSelecionado();
                if (cli != null)
                {
                    string end = FormatarEnderecoResumido(cli);
                    if (end != "Não informado") CopiarParaAreaTransferencia(end);
                }
            };

            _menuContextoGrid.Items.AddRange(new ToolStripItem[]
            {
                itemHistorico,
                sepHistorico,
                itemCopiarCelula,
                new ToolStripSeparator(),
                itemCopiarNome,
                itemCopiarDoc,
                itemCopiarTel,
                itemCopiarEnd
            });

            _menuContextoGrid.Opening += (s, e) =>
            {
                Point posMouse = dgwClientes.PointToClient(Cursor.Position);
                var hit = dgwClientes.HitTest(posMouse.X, posMouse.Y);

                if (hit.Type != DataGridViewHitTestType.Cell || hit.RowIndex < 0 || hit.RowIndex >= _listaClientes.Count)
                {
                    e.Cancel = true;
                    return;
                }

                var cli = _listaClientes[hit.RowIndex];

                bool mostrarHistorico = !EhModoPopup;
                itemHistorico.Visible = mostrarHistorico;
                sepHistorico.Visible = mostrarHistorico;

                bool celulaValida = !string.IsNullOrWhiteSpace(_valorCelulaClicada) && _valorCelulaClicada != "-";
                string preview = _valorCelulaClicada.Length > 28
                    ? _valorCelulaClicada.Substring(0, 28) + "..."
                    : _valorCelulaClicada;

                itemCopiarCelula.Enabled = celulaValida;
                itemCopiarCelula.Text = celulaValida ? $"Copiar \"{preview}\"" : "Copiar Célula";

                itemCopiarNome.Enabled = !string.IsNullOrWhiteSpace(cli.Nome);
                itemCopiarDoc.Enabled = !string.IsNullOrWhiteSpace(cli.CpfCnpj);
                itemCopiarTel.Enabled = !string.IsNullOrWhiteSpace(cli.Telefone);
                itemCopiarEnd.Enabled = FormatarEnderecoResumido(cli) != "Não informado";

                Color fundo = GerenciadorTema.ModoEscuro ? GerenciadorTema.FundoCardEscuro : GerenciadorTema.FundoCardClaro;
                Color texto = GerenciadorTema.ModoEscuro ? GerenciadorTema.TextoEscuro : GerenciadorTema.TextoClaro;
                _menuContextoGrid.BackColor = fundo;
                _menuContextoGrid.ForeColor = texto;
                foreach (ToolStripItem item in _menuContextoGrid.Items)
                {
                    item.BackColor = fundo;
                    item.ForeColor = texto;
                }
            };

            dgwClientes.ContextMenuStrip = _menuContextoGrid;
        }

        private ClienteDTO ObterClienteSelecionado()
        {
            if (dgwClientes?.CurrentRow == null) return null;
            int idx = dgwClientes.CurrentRow.Index;
            return (idx >= 0 && idx < _listaClientes.Count) ? _listaClientes[idx] : null;
        }

        private static void CopiarParaAreaTransferencia(string texto)
        {
            if (!string.IsNullOrWhiteSpace(texto) && texto != "-")
                Clipboard.SetText(texto);
        }

        private void ConfigurarModoExibicao()
        {
            bool popup = EhModoPopup;

            if (btnCancelar != null)
                btnCancelar.Visible = popup;

            if (btnHistorico != null)
                btnHistorico.Visible = !popup;

            if (btnSelecionar != null)
            {
                if (popup)
                {
                    btnSelecionar.Text = "Selecionar [Enter]";
                }
                else
                {
                    btnSelecionar.Text = "Cadastrar";
                    btnSelecionar.Tag = "primary";
                    GerenciadorTema.AplicarTema(btnSelecionar);
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape && EhModoPopup)
            {
                btnCancelar?.PerformClick();
                return true;
            }

            if (keyData == Keys.F2)
            {
                btnEditarCliente?.PerformClick();
                return true;
            }

            if (keyData == Keys.F3 && !EhModoPopup)
            {
                AbrirHistoricoDoClienteSelecionado();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnHistorico_Click(object sender, EventArgs e)
        {
            AbrirHistoricoDoClienteSelecionado();
        }

        private void AbrirHistoricoDoClienteSelecionado()
        {
            if (EhModoPopup) return;

            var cliente = ObterClienteSelecionado();
            if (cliente == null)
            {
                MessageBox.Show("Selecione um cliente na lista para visualizar o histórico.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var principal = Application.OpenForms.OfType<formPrincipal>().FirstOrDefault();
            principal?.AbrirHistoricoCliente(cliente);
        }

        private void dgwClientes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || dgwClientes == null) return;

            if (ObterChaveColuna(dgwClientes.Columns[e.ColumnIndex]) == "Status" && e.Value != null)
            {
                bool ativo = e.Value.ToString().Equals("Ativo", StringComparison.OrdinalIgnoreCase);
                Color corStatus = ativo ? GerenciadorTema.CorSucesso : GerenciadorTema.CorPerigo;

                e.CellStyle.ForeColor = corStatus;
                e.CellStyle.SelectionForeColor = corStatus;
                e.CellStyle.Font = new Font(dgwClientes.Font, FontStyle.Bold);
            }
        }

        private void dgwClientes_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 || dgwClientes == null) return;

            string chaveClicada = ObterChaveColuna(dgwClientes.Columns[e.ColumnIndex]);

            if (_colunaOrdenacao == chaveClicada)
            {
                _ordemCrescente = !_ordemCrescente;
            }
            else
            {
                _colunaOrdenacao = chaveClicada;
                _ordemCrescente = true;
            }

            AtualizarExibicaoGrid();
        }

        private static string ObterChaveColuna(DataGridViewColumn coluna)
        {
            if (!string.IsNullOrWhiteSpace(coluna.DataPropertyName))
                return coluna.DataPropertyName;

            string nomeOuTitulo = $"{coluna.Name} {coluna.HeaderText}".ToLowerInvariant();
            if (nomeOuTitulo.Contains("limite")) return "LimiteCredito";
            if (nomeOuTitulo.Contains("status") || nomeOuTitulo.Contains("situacao") || nomeOuTitulo.Contains("situação")) return "Status";
            if (nomeOuTitulo.Contains("tel")) return "Telefone";
            if (nomeOuTitulo.Contains("end")) return "Endereco";
            if (nomeOuTitulo.Contains("cpf") || nomeOuTitulo.Contains("cnpj") || nomeOuTitulo.Contains("doc")) return "Documento";
            if (nomeOuTitulo.Contains("nome")) return "Nome";

            return "Id";
        }

        private void AtualizarExibicaoGrid()
        {
            if (dgwClientes == null || IsDisposed) return;

            IEnumerable<ClienteDTO> consultaOrdenada;

            switch (_colunaOrdenacao)
            {
                case "Nome":
                    consultaOrdenada = _ordemCrescente
                        ? _listaClientes.OrderBy(c => c.Nome, StringComparer.CurrentCultureIgnoreCase)
                        : _listaClientes.OrderByDescending(c => c.Nome, StringComparer.CurrentCultureIgnoreCase);
                    break;

                case "Documento":
                    consultaOrdenada = _ordemCrescente
                        ? _listaClientes.OrderBy(c => ExtrairDigitos(c.CpfCnpj))
                        : _listaClientes.OrderByDescending(c => ExtrairDigitos(c.CpfCnpj));
                    break;

                case "Endereco":
                    consultaOrdenada = _ordemCrescente
                        ? _listaClientes.OrderBy(c => FormatarEnderecoResumido(c), StringComparer.CurrentCultureIgnoreCase)
                        : _listaClientes.OrderByDescending(c => FormatarEnderecoResumido(c), StringComparer.CurrentCultureIgnoreCase);
                    break;

                case "Telefone":
                    consultaOrdenada = _ordemCrescente
                        ? _listaClientes.OrderBy(c => ExtrairDigitos(c.Telefone))
                        : _listaClientes.OrderByDescending(c => ExtrairDigitos(c.Telefone));
                    break;

                case "LimiteCredito":
                    consultaOrdenada = _ordemCrescente
                        ? _listaClientes.OrderBy(c => c.LimiteCredito).ThenBy(c => c.Id)
                        : _listaClientes.OrderByDescending(c => c.LimiteCredito).ThenBy(c => c.Id);
                    break;

                case "Status":
                    consultaOrdenada = _ordemCrescente
                        ? _listaClientes.OrderByDescending(c => c.Ativo).ThenBy(c => c.Id)
                        : _listaClientes.OrderBy(c => c.Ativo).ThenBy(c => c.Id);
                    break;

                default:
                    consultaOrdenada = _ordemCrescente
                        ? _listaClientes.OrderBy(c => c.Id)
                        : _listaClientes.OrderByDescending(c => c.Id);
                    break;
            }

            _listaClientes = consultaOrdenada.ToList();

            dgwClientes.DataSource = _listaClientes.Select(clienteItem => new
            {
                clienteItem.Id,
                clienteItem.Nome,
                Documento = string.IsNullOrWhiteSpace(clienteItem.CpfCnpj) ? "-" : FormatarCpfCnpj(clienteItem.CpfCnpj),
                Endereco = FormatarEnderecoResumido(clienteItem),
                Telefone = string.IsNullOrWhiteSpace(clienteItem.Telefone) ? "-" : FormatarTelefone(clienteItem.Telefone),
                LimiteCredito = clienteItem.LimiteCredito.ToString("C2"),
                Status = clienteItem.Ativo ? "Ativo" : "Inativo"
            }).ToList();

            foreach (DataGridViewColumn col in dgwClientes.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Programmatic;
                col.HeaderCell.SortGlyphDirection = ObterChaveColuna(col) == _colunaOrdenacao
                    ? (_ordemCrescente ? SortOrder.Ascending : SortOrder.Descending)
                    : SortOrder.None;
            }
        }

        private async void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            _ctsBusca?.Cancel();
            _ctsBusca?.Dispose();
            _ctsBusca = new CancellationTokenSource();
            var token = _ctsBusca.Token;

            string termo = txtPesquisa.Text.Trim();

            try
            {
                await Task.Delay(200, token);

                if (!token.IsCancellationRequested)
                    await BuscarClientesBancoAsync(termo, token);
            }
            catch (TaskCanceledException)
            {

            }
        }

        private async Task BuscarClientesBancoAsync(string termo, CancellationToken token = default)
        {
            try
            {
                var resultados = await Task.Run(async () =>
                {
                    var lista = new List<ClienteDTO>();

                    using (var conexaoDb = new Connection())
                    {
                        conexaoDb.IniciarConexao();

                        string termoOriginal = termo.Trim();
                        string digitos = ExtrairDigitos(termoOriginal);

                        const string sql = @"
                            SELECT
                                customer.id_customer,
                                customer.name,
                                customer.nickname,
                                customer.cpf_cnpj,
                                customer.rg_ie,
                                customer.phone,
                                customer.email,
                                customer.cep,
                                customer.street,
                                customer.number_address,
                                customer.neighborhood,
                                customer.complement,
                                customer.city,
                                customer.uf,
                                customer.credit_limit,
                                customer.is_active,
                                customer.notes
                            FROM public.customer
                            WHERE (
                                @termoOriginal = ''
                                OR customer.name ILIKE @termoContem
                                OR customer.nickname ILIKE @termoContem
                                OR customer.cpf_cnpj ILIKE @termoContem
                                OR customer.phone ILIKE @termoContem
                                OR customer.cpf_cnpj ILIKE @apenasDigitos
                                OR customer.phone ILIKE @apenasDigitos
                                OR customer.id_customer::TEXT = @termoOriginal
                            )
                            ORDER BY
                                CASE
                                    WHEN @termoOriginal = '' THEN 0
                                    WHEN customer.id_customer::TEXT = @termoOriginal THEN 1
                                    WHEN customer.name ILIKE @termoComeca THEN 2
                                    WHEN customer.nickname ILIKE @termoComeca THEN 3
                                    ELSE 4
                                END,
                                CASE WHEN @termoOriginal = '' THEN customer.id_customer END ASC,
                                customer.name ASC
                            LIMIT 50;";

                        using (var comando = new NpgsqlCommand(sql, conexaoDb.Conexao))
                        {
                            comando.Parameters.AddWithValue("@termoOriginal", termoOriginal);
                            comando.Parameters.AddWithValue("@termoComeca", termoOriginal + "%");
                            comando.Parameters.AddWithValue("@termoContem", "%" + termoOriginal + "%");
                            comando.Parameters.AddWithValue("@apenasDigitos", string.IsNullOrEmpty(digitos) ? "@@@SEM_DIGITOS@@@" : "%" + digitos + "%");

                            using (var reader = await comando.ExecuteReaderAsync(token))
                            {
                                while (await reader.ReadAsync(token))
                                {
                                    lista.Add(new ClienteDTO
                                    {
                                        Id = reader["id_customer"] != DBNull.Value ? Convert.ToInt64(reader["id_customer"]) : 0,
                                        Nome = reader["name"]?.ToString() ?? string.Empty,
                                        Apelido = reader["nickname"]?.ToString() ?? string.Empty,
                                        CpfCnpj = reader["cpf_cnpj"]?.ToString() ?? string.Empty,
                                        RgIe = reader["rg_ie"]?.ToString() ?? string.Empty,
                                        Telefone = reader["phone"]?.ToString() ?? string.Empty,
                                        Email = reader["email"]?.ToString() ?? string.Empty,
                                        Cep = reader["cep"]?.ToString() ?? string.Empty,
                                        Rua = reader["street"]?.ToString() ?? string.Empty,
                                        Numero = reader["number_address"]?.ToString() ?? string.Empty,
                                        Bairro = reader["neighborhood"]?.ToString() ?? string.Empty,
                                        Complemento = reader["complement"]?.ToString() ?? string.Empty,
                                        Cidade = reader["city"]?.ToString() ?? string.Empty,
                                        Uf = reader["uf"]?.ToString() ?? string.Empty,
                                        LimiteCredito = reader["credit_limit"] != DBNull.Value ? Convert.ToDecimal(reader["credit_limit"]) : 0m,
                                        Ativo = reader["is_active"] != DBNull.Value && Convert.ToBoolean(reader["is_active"]),
                                        Observacoes = reader["notes"]?.ToString() ?? string.Empty
                                    });
                                }
                            }
                        }
                    }

                    return lista;
                }, token);

                if (token.IsCancellationRequested || dgwClientes == null || IsDisposed) return;

                _listaClientes = resultados ?? new List<ClienteDTO>();
                AtualizarExibicaoGrid();
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception ex)
            {
                if (!IsDisposed)
                    MessageBox.Show($"Erro ao consultar clientes: {ex.Message}", "Falha de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExecutarAcaoLinha(int indiceLinha)
        {
            if (indiceLinha < 0 || indiceLinha >= _listaClientes.Count) return;

            ClienteDTO cliente = _listaClientes[indiceLinha];

            if (EhModoPopup)
            {
                if (!cliente.Ativo)
                {
                    MessageBox.Show("Este cliente está inativo e não pode ser selecionado para a venda.", "Cliente Inativo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                AoSelecionarCliente?.Invoke(cliente);
            }
            else
            {
                btnEditarCliente?.PerformClick();
            }
        }

        private async void btnSelecionar_Click(object sender, EventArgs e)
        {
            if (!EhModoPopup)
            {
                await AbrirModalClienteAsync(null);
                return;
            }

            if (dgwClientes.CurrentRow != null && dgwClientes.CurrentRow.Index >= 0)
            {
                ExecutarAcaoLinha(dgwClientes.CurrentRow.Index);
            }
            else
            {
                MessageBox.Show("Selecione um cliente na lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnEditarCliente_Click(object sender, EventArgs e)
        {
            if (dgwClientes.CurrentRow == null || dgwClientes.CurrentRow.Index < 0)
            {
                MessageBox.Show("Selecione um cliente na lista para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int indice = dgwClientes.CurrentRow.Index;
            if (indice >= _listaClientes.Count) return;

            ClienteDTO clienteSelecionado = _listaClientes[indice];

            if (clienteSelecionado.Id == 1)
            {
                MessageBox.Show("O cliente padrão 'Cliente não informado' não pode ser editado.", "Operação não permitida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            await AbrirModalClienteAsync(clienteSelecionado);
        }

        private async Task AbrirModalClienteAsync(ClienteDTO cliente)
        {
            bool atualizado = false;
            bool ehEdicao = cliente != null;

            using (var formModal = new Form())
            {
                var ucCadastro = new UcCadastroCliente();

                formModal.Text = ehEdicao ? $"Editar Cliente - {cliente.Nome}" : "Cadastrar Novo Cliente";
                formModal.ClientSize = ucCadastro.Size;
                formModal.StartPosition = FormStartPosition.CenterParent;
                formModal.FormBorderStyle = FormBorderStyle.FixedDialog;
                formModal.MaximizeBox = false;
                formModal.MinimizeBox = false;

                ucCadastro.Dock = DockStyle.Fill;
                if (ehEdicao)
                    ucCadastro.CarregarDadosParaEdicao(cliente);

                ucCadastro.AoConcluirOperacao += () =>
                {
                    atualizado = true;
                    formModal.DialogResult = DialogResult.OK;
                    formModal.Close();
                };

                formModal.Controls.Add(ucCadastro);
                GerenciadorTema.AplicarTema(formModal);

                formModal.ShowDialog(this.FindForm());
            }

            if (atualizado)
            {
                NotificarAlteracaoPropria();

                string termoAtual = txtPesquisa?.Text.Trim() ?? string.Empty;
                await BuscarClientesBancoAsync(termoAtual);

                if (ehEdicao)
                {
                    int novoIndice = _listaClientes.FindIndex(c => c.Id == cliente.Id);
                    if (novoIndice >= 0 && novoIndice < dgwClientes.Rows.Count)
                    {
                        dgwClientes.ClearSelection();
                        dgwClientes.Rows[novoIndice].Selected = true;
                        dgwClientes.CurrentCell = dgwClientes.Rows[novoIndice].Cells[0];
                    }
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (!EhModoPopup) return;
            this.FindForm()?.Close();
        }

        private void txtPesquisa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && dgwClientes.Rows.Count > 0)
            {
                e.Handled = true;
                dgwClientes.Focus();
                dgwClientes.Rows[0].Selected = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                if (dgwClientes.CurrentRow != null && dgwClientes.CurrentRow.Index >= 0)
                    ExecutarAcaoLinha(dgwClientes.CurrentRow.Index);
            }
        }

        private void dgwClientes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                if (dgwClientes.CurrentRow != null && dgwClientes.CurrentRow.Index >= 0)
                    ExecutarAcaoLinha(dgwClientes.CurrentRow.Index);
            }
        }

        private static string FormatarEnderecoResumido(ClienteDTO clienteItem)
        {
            if (string.IsNullOrWhiteSpace(clienteItem.Rua) && string.IsNullOrWhiteSpace(clienteItem.Cidade))
                return "Não informado";

            var partes = new List<string>();
            if (!string.IsNullOrWhiteSpace(clienteItem.Rua))
            {
                string logradouro = clienteItem.Rua.Trim();
                if (!string.IsNullOrWhiteSpace(clienteItem.Numero))
                    logradouro += $", {clienteItem.Numero.Trim()}";
                partes.Add(logradouro);
            }

            if (!string.IsNullOrWhiteSpace(clienteItem.Bairro))
                partes.Add(clienteItem.Bairro.Trim());

            if (!string.IsNullOrWhiteSpace(clienteItem.Cidade))
            {
                string cidadeUf = clienteItem.Cidade.Trim();
                if (!string.IsNullOrWhiteSpace(clienteItem.Uf))
                    cidadeUf += $" - {clienteItem.Uf.Trim()}";
                partes.Add(cidadeUf);
            }

            return string.Join(" - ", partes);
        }

        private static string FormatarCpfCnpj(string doc)
        {
            string digitos = ExtrairDigitos(doc);
            if (digitos.Length == 11)
                return Convert.ToUInt64(digitos).ToString(@"000\.000\.000\-00");
            if (digitos.Length == 14)
                return Convert.ToUInt64(digitos).ToString(@"00\.000\.000\/0000\-00");

            return doc?.Trim() ?? string.Empty;
        }

        private static string FormatarTelefone(string fone)
        {
            string digitos = ExtrairDigitos(fone);
            if (digitos.Length == 11)
                return Convert.ToUInt64(digitos).ToString(@"(00) 00000\-0000");
            if (digitos.Length == 10)
                return Convert.ToUInt64(digitos).ToString(@"(00) 0000\-0000");

            return fone?.Trim() ?? string.Empty;
        }

        private static string ExtrairDigitos(string texto)
        {
            return string.IsNullOrEmpty(texto) ? string.Empty : Regex.Replace(texto, @"\D", string.Empty);
        }
    }
}