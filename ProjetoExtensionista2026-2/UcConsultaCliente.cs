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

        public UcConsultaCliente()
        {
            InitializeComponent();

            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);

            ConfigurarGrid();
            ConfigurarEventos();
        }

        private void ConfigurarGrid()
        {
            if (dgwClientes == null) return;

            dgwClientes.AutoGenerateColumns = false;
            dgwClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgwClientes.MultiSelect = false;
            dgwClientes.ReadOnly = true;
            dgwClientes.RowHeadersVisible = false;
            dgwClientes.AllowUserToAddRows = false;
            dgwClientes.AllowUserToDeleteRows = false;
        }

        private void ConfigurarEventos()
        {
            this.Load += async (sender, e) =>
            {
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
                dgwClientes.CellDoubleClick += (sender, e) =>
                {
                    if (e.RowIndex >= 0)
                        ConfirmarSelecaoLinha(e.RowIndex);
                };

                dgwClientes.KeyDown += dgwClientes_KeyDown;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                btnCancelar.PerformClick();
                return true; // Informa ao Windows Forms que a tecla foi tratada
            }

            return base.ProcessCmdKey(ref msg, keyData);
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
                {
                    await BuscarClientesBancoAsync(termo, token);
                }
            }
            catch (TaskCanceledException)
            {
                // Ignorado intencionalmente durante digitação rápida
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
                            WHERE customer.is_active = TRUE
                              AND (
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
                                    WHEN customer.id_customer::TEXT = @termoOriginal THEN 1
                                    WHEN customer.name ILIKE @termoComeca THEN 2
                                    WHEN customer.nickname ILIKE @termoComeca THEN 3
                                    ELSE 4
                                END,
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

                if (token.IsCancellationRequested || dgwClientes == null) return;

                _listaClientes = resultados ?? new List<ClienteDTO>();

                dgwClientes.DataSource = _listaClientes.Select(clienteItem => new
                {
                    clienteItem.Id,
                    clienteItem.Nome,
                    Documento = string.IsNullOrWhiteSpace(clienteItem.CpfCnpj) ? "-" : FormatarCpfCnpj(clienteItem.CpfCnpj),
                    Endereco = FormatarEnderecoResumido(clienteItem),
                    Telefone = string.IsNullOrWhiteSpace(clienteItem.Telefone) ? "-" : FormatarTelefone(clienteItem.Telefone),
                    Status = clienteItem.Ativo ? "Ativo" : "Inativo"
                }).ToList();
            }
            catch (OperationCanceledException)
            {
                // Cancelamento esperado
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao consultar clientes: {ex.Message}", "Falha de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfirmarSelecaoLinha(int indiceLinha)
        {
            if (indiceLinha < 0 || indiceLinha >= _listaClientes.Count) return;

            ClienteDTO cliente = _listaClientes[indiceLinha];
            AoSelecionarCliente?.Invoke(cliente);
        }

        private void btnSelecionar_Click(object sender, EventArgs e)
        {
            if (dgwClientes.CurrentRow != null && dgwClientes.CurrentRow.Index >= 0)
            {
                ConfirmarSelecaoLinha(dgwClientes.CurrentRow.Index);
            }
            else
            {
                MessageBox.Show("Selecione um cliente na lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
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
                {
                    ConfirmarSelecaoLinha(dgwClientes.CurrentRow.Index);
                }
            }
        }

        private void dgwClientes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                if (dgwClientes.CurrentRow != null && dgwClientes.CurrentRow.Index >= 0)
                {
                    ConfirmarSelecaoLinha(dgwClientes.CurrentRow.Index);
                }
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