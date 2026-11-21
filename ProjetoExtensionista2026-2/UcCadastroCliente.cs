using MySql.Data.MySqlClient;
using ProjetoExtensionista2026_2.DTOs;
using ProjetoExtensionista2026_2.SQL;
using ProjetoExtensionista2026_2.SQL.SQL_functions;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public partial class UcCadastroCliente : UserControl
    {
        private static readonly HttpClient ClienteHttp = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };

        public long? ClienteIdEmEdicao { get; private set; }
        public event Action AoConcluirOperacao;

        public UcCadastroCliente()
        {
            InitializeComponent();

            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            ConfigurarCampos();
            AtualizarDataAtualizacao(null);
            _ = GerarProximoCodigoAsync();
        }

        private void ConfigurarCampos()
        {

            txtNome.Tag = txtCpfCnpj.Tag = txtTelefone.Tag = "obrigatorio";


            txtNome.MaxLength = 150;
            txtApelido.MaxLength = 150;
            txtCpfCnpj.MaxLength = 18;
            txtRgIe.MaxLength = 20;
            txtTelefone.MaxLength = 15;
            txtEmail.MaxLength = 150;
            txtCep.MaxLength = 9;
            txtRua.MaxLength = 150;
            txtNumero.MaxLength = 10;
            txtBairro.MaxLength = 60;
            txtCidade.MaxLength = 80;
            txtDescricao.MaxLength = 1000;

            AplicarMascara(txtCpfCnpj, 14, FormatarDocumento);
            AplicarMascara(txtTelefone, 11, FormatarTelefone);
            AplicarMascara(txtCep, 8, FormatarCep);

            txtNumero.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != '/')
                    e.Handled = true;
            };
        }

        public async Task GerarProximoCodigoAsync()
        {
            try
            {
                object res = await Task.Run(async () =>
                {
                    using (var db = new Connection())
                    {
                        db.IniciarConexao();
                        using (var cmd = new MySqlCommand("SELECT COALESCE(MAX(id_customer), 0) + 1 FROM customer;", db.Conexao))
                            return await cmd.ExecuteScalarAsync();
                    }
                });

                if (!IsDisposed && !ClienteIdEmEdicao.HasValue)
                    txtCodigo.Text = res?.ToString() ?? "1";
            }
            catch
            {
                if (!IsDisposed && !ClienteIdEmEdicao.HasValue)
                    txtCodigo.Text = "1";
            }
        }

        public void CarregarDadosParaEdicao(ClienteDTO c)
        {
            if (c == null) return;

            ClienteIdEmEdicao = c.Id;

            txtCodigo.Text = c.Id.ToString();
            txtNome.Text = c.Nome ?? string.Empty;
            txtApelido.Text = c.Apelido ?? string.Empty;
            txtCpfCnpj.Text = Digitos(c.CpfCnpj);
            txtRgIe.Text = c.RgIe ?? string.Empty;
            txtTelefone.Text = Digitos(c.Telefone);
            txtEmail.Text = c.Email ?? string.Empty;
            txtCep.Text = Digitos(c.Cep);
            txtRua.Text = c.Rua ?? string.Empty;
            txtNumero.Text = c.Numero ?? string.Empty;
            txtBairro.Text = c.Bairro ?? string.Empty;
            txtCidade.Text = c.Cidade ?? string.Empty;
            txtDescricao.Text = c.Observacoes ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(c.Uf))
                cboUf.SelectedItem = c.Uf.Trim().ToUpperInvariant();
            else
                cboUf.SelectedIndex = -1;

            rbAtivo.Checked = c.Ativo;
            btnSalvar.Text = "Atualizar";

            _ = CarregarDataAtualizacaoAsync(c.Id);
        }

        private async Task CarregarDataAtualizacaoAsync(long idCliente)
        {
            try
            {
                DateTime? atualizado = await Task.Run(async () =>
                {
                    using (var db = new Connection())
                    {
                        db.IniciarConexao();
                        using (var cmd = new MySqlCommand("SELECT updated_at FROM customer WHERE id_customer = @id;", db.Conexao))
                        {
                            cmd.Parameters.AddWithValue("@id", idCliente);
                            object res = await cmd.ExecuteScalarAsync();
                            return res != null && res != DBNull.Value ? Convert.ToDateTime(res) : (DateTime?)null;
                        }
                    }
                });

                if (!IsDisposed && ClienteIdEmEdicao == idCliente)
                    AtualizarDataAtualizacao(atualizado);
            }
            catch { }
        }

        private void AtualizarDataAtualizacao(DateTime? atualizado)
        {
            if (lblAtualizado != null)
                lblAtualizado.Text = atualizado.HasValue
                    ? $"Atualizado em: {atualizado.Value.ToLocalTime():dd/MM/yyyy HH:mm}"
                    : "Atualizado em: -";
        }

        private async void btnSalvar_Click(object sender, EventArgs e)
        {
            string nome = txtNome.Text.Trim();
            if (string.IsNullOrWhiteSpace(nome))
            {
                MessageBox.Show("Informe o nome do cliente.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                return;
            }

            string docDigitos = Digitos(txtCpfCnpj.Text);
            if (docDigitos.Length != 11 && docDigitos.Length != 14)
            {
                MessageBox.Show("Informe um CPF (11 dígitos) ou CNPJ (14 dígitos) válido.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCpfCnpj.Focus();
                return;
            }

            string foneDigitos = Digitos(txtTelefone.Text);
            if (foneDigitos.Length < 10 || foneDigitos.Length > 11)
            {
                MessageBox.Show("Informe um telefone válido com DDD (10 ou 11 dígitos).", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTelefone.Focus();
                return;
            }

            string cepDigitos = Digitos(txtCep.Text);
            if (cepDigitos.Length > 0 && cepDigitos.Length != 8)
            {
                MessageBox.Show("O CEP deve conter 8 dígitos ou ficar em branco.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCep.Focus();
                return;
            }

            btnSalvar.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            bool ehEdicao = ClienteIdEmEdicao.HasValue;
            object apelido = ValorOuNulo(txtApelido.Text);
            object rgIe = ValorOuNulo(txtRgIe.Text);
            object email = ValorOuNulo(txtEmail.Text);
            object cep = ValorOuNulo(cepDigitos);
            object rua = ValorOuNulo(txtRua.Text);
            object numero = ValorOuNulo(txtNumero.Text);
            object bairro = ValorOuNulo(txtBairro.Text);
            object cidade = ValorOuNulo(txtCidade.Text);
            object uf = ValorOuNulo(cboUf.Text);
            object notas = ValorOuNulo(txtDescricao.Text);
            bool ativo = rbAtivo.Checked;

            try
            {
                var res = await Task.Run(async () =>
                {
                    using (var db = new Connection())
                    {
                        db.IniciarConexao();

                        string sql = ehEdicao
                            ? @"UPDATE customer SET
                                    name = @name, nickname = @nickname, cpf_cnpj = @cpf_cnpj, rg_ie = @rg_ie,
                                    phone = @phone, email = @email, cep = @cep, street = @street,
                                    number_address = @number_address, neighborhood = @neighborhood, city = @city,
                                    uf = @uf, notes = @notes, is_active = @is_active, updated_at = NOW()
                                WHERE id_customer = @id;"
                            : @"INSERT INTO customer (
                                    name, nickname, cpf_cnpj, rg_ie, phone, email, cep,
                                    street, number_address, neighborhood, complement, city,
                                    uf, notes, is_active
                                ) VALUES (
                                    @name, @nickname, @cpf_cnpj, @rg_ie, @phone, @email, @cep,
                                    @street, @number_address, @neighborhood, NULL, @city,
                                    @uf, @notes, @is_active
                                );";

                        using (var cmd = new MySqlCommand(sql, db.Conexao))
                        {
                            if (ehEdicao)
                                cmd.Parameters.AddWithValue("@id", ClienteIdEmEdicao.Value);

                            cmd.Parameters.AddWithValue("@name", nome);
                            cmd.Parameters.AddWithValue("@nickname", apelido);
                            cmd.Parameters.AddWithValue("@cpf_cnpj", docDigitos);
                            cmd.Parameters.AddWithValue("@rg_ie", rgIe);
                            cmd.Parameters.AddWithValue("@phone", foneDigitos);
                            cmd.Parameters.AddWithValue("@email", email);
                            cmd.Parameters.AddWithValue("@cep", cep);
                            cmd.Parameters.AddWithValue("@street", rua);
                            cmd.Parameters.AddWithValue("@number_address", numero);
                            cmd.Parameters.AddWithValue("@neighborhood", bairro);
                            cmd.Parameters.AddWithValue("@city", cidade);
                            cmd.Parameters.AddWithValue("@uf", uf);
                            cmd.Parameters.AddWithValue("@notes", notas);
                            cmd.Parameters.AddWithValue("@is_active", ativo);

                            await cmd.ExecuteNonQueryAsync();
                            long id = ehEdicao ? ClienteIdEmEdicao.Value : Convert.ToInt64(cmd.LastInsertedId);
                            using (var cmdData = new MySqlCommand("SELECT updated_at FROM customer WHERE id_customer = @id;", db.Conexao))
                            {
                                cmdData.Parameters.AddWithValue("@id", id);
                                object atualizado = await cmdData.ExecuteScalarAsync();
                                DateTime? dataAtualizacao = atualizado != null && atualizado != DBNull.Value
                                    ? Convert.ToDateTime(atualizado)
                                    : (DateTime?)null;
                                return (id, dataAtualizacao);
                            }
                        }
                    }
                });

                txtCodigo.Text = res.id.ToString();
                AtualizarDataAtualizacao(res.dataAtualizacao);

                string mensagem = ehEdicao
                    ? "Cliente atualizado com sucesso!"
                    : $"Cliente cadastrado com sucesso. Código: {res.id}";

                AuditLogSQL.Register(
                    (ClienteIdEmEdicao.HasValue ? "Cliente atualizado: " : "Cliente cadastrado: ") + nome,
                    LoginSQL.SessaoUsuario.Nome);

                MessageBox.Show(mensagem, "Cadastro", MessageBoxButtons.OK, MessageBoxIcon.Information);

                AoConcluirOperacao?.Invoke();

                Form formulario = FindForm();
                if (formulario != null)
                {
                    formulario.DialogResult = DialogResult.OK;
                    formulario.Close();
                }
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                MessageBox.Show("O CPF/CNPJ informado já está cadastrado.", "Cadastro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar cliente: {ex.Message}", "Falha de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed)
                {
                    btnSalvar.Enabled = true;
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private static object ValorOuNulo(string valor) =>
            string.IsNullOrWhiteSpace(valor) ? (object)DBNull.Value : valor.Trim();

        private async void txtCep_Leave(object sender, EventArgs e)
        {
            string cep = Digitos(txtCep.Text);
            if (cep.Length != 8) return;

            try
            {
                using (HttpResponseMessage resposta = await ClienteHttp.GetAsync("https://brasilapi.com.br/api/cep/v2/" + cep))
                {
                    if (!resposta.IsSuccessStatusCode)
                    {
                        MessageBox.Show("CEP não encontrado.", "Consulta de CEP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string json = await resposta.Content.ReadAsStringAsync();
                    BrasilApiCep endereco = DesserializarCep(json);
                    if (endereco == null) return;

                    if (!string.IsNullOrWhiteSpace(endereco.Street)) txtRua.Text = endereco.Street;
                    if (!string.IsNullOrWhiteSpace(endereco.Neighborhood)) txtBairro.Text = endereco.Neighborhood;
                    if (!string.IsNullOrWhiteSpace(endereco.City)) txtCidade.Text = endereco.City;
                    if (!string.IsNullOrWhiteSpace(endereco.State)) cboUf.SelectedItem = endereco.State.ToUpperInvariant();
                }
            }
            catch (TaskCanceledException)
            {
                MessageBox.Show("A consulta do CEP demorou muito e foi cancelada.", "Consulta de CEP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Não foi possível consultar o CEP: {ex.Message}", "Consulta de CEP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static BrasilApiCep DesserializarCep(string json)
        {
            var serializer = new DataContractJsonSerializer(typeof(BrasilApiCep));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                return serializer.ReadObject(stream) as BrasilApiCep;
        }

        private async void btnLimpar_Click(object sender, EventArgs e) => await LimparFormularioAsync();

        private void btnCancelar_Click(object sender, EventArgs e) => FindForm()?.Close();

        private async Task LimparFormularioAsync()
        {
            ClienteIdEmEdicao = null;
            btnSalvar.Text = "Salvar";
            txtCodigo.Text = "...";

            foreach (Control controle in Controls)
                LimparControle(controle);

            rbAtivo.Checked = true;
            cboUf.SelectedIndex = -1;
            AtualizarDataAtualizacao(null);
            txtNome.Focus();

            await GerarProximoCodigoAsync();
        }

        private void LimparControle(Control controle)
        {
            if (controle is TextBox textBox && textBox != txtCodigo)
                textBox.Clear();
            else if (controle is ComboBox comboBox)
                comboBox.SelectedIndex = -1;
            else if (controle.HasChildren)
                foreach (Control filho in controle.Controls)
                    LimparControle(filho);
        }

        private static string Digitos(string s) =>
            string.IsNullOrEmpty(s) ? string.Empty : new string(s.Where(char.IsDigit).ToArray());

        private static void AplicarMascara(TextBox txt, int max, Func<string, string> fmt)
        {
            if (txt == null) return;
            bool travado = false;

            txt.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            };

            txt.TextChanged += (s, e) =>
            {
                if (travado) return;
                travado = true;
                string d = Digitos(txt.Text);
                txt.Text = fmt(d.Length > max ? d.Substring(0, max) : d);
                txt.SelectionStart = txt.Text.Length;
                travado = false;
            };
        }

        private static string FormatarDocumento(string d) => d.Length <= 11
            ? (d.Length > 9 ? $"{d.Substring(0, 3)}.{d.Substring(3, 3)}.{d.Substring(6, 3)}-{d.Substring(9)}"
             : d.Length > 6 ? $"{d.Substring(0, 3)}.{d.Substring(3, 3)}.{d.Substring(6)}"
             : d.Length > 3 ? $"{d.Substring(0, 3)}.{d.Substring(3)}" : d)
            : (d.Length > 12 ? $"{d.Substring(0, 2)}.{d.Substring(2, 3)}.{d.Substring(5, 3)}/{d.Substring(8, 4)}-{d.Substring(12)}"
             : $"{d.Substring(0, 2)}.{d.Substring(2, 3)}.{d.Substring(5, 3)}/{d.Substring(8)}");

        private static string FormatarTelefone(string d) => d.Length > 10
            ? $"({d.Substring(0, 2)}) {d.Substring(2, 5)}-{d.Substring(7)}"
            : d.Length > 6 ? $"({d.Substring(0, 2)}) {d.Substring(2, 4)}-{d.Substring(6)}"
            : d.Length > 2 ? $"({d.Substring(0, 2)}) {d.Substring(2)}"
            : d.Length > 0 ? $"({d}" : string.Empty;

        private static string FormatarCep(string d) =>
            d.Length > 5 ? $"{d.Substring(0, 5)}-{d.Substring(5)}" : d;

        [DataContract]
        private sealed class BrasilApiCep
        {
            [DataMember(Name = "street")]
            public string Street { get; set; }

            [DataMember(Name = "neighborhood")]
            public string Neighborhood { get; set; }

            [DataMember(Name = "city")]
            public string City { get; set; }

            [DataMember(Name = "state")]
            public string State { get; set; }
        }
    }
}