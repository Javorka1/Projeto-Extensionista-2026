using Npgsql;
using NpgsqlTypes;
using ProjetoExtensionista2026_2.DTOs;
using ProjetoExtensionista2026_2.SQL;
using System;
using System.IO;
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
        private static readonly HttpClient ClienteHttp = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        public long? ClienteIdEmEdicao { get; private set; }
        public event Action AoConcluirOperacao;

        public UcCadastroCliente()
        {
            InitializeComponent();
        }

        public void CarregarDadosParaEdicao(ClienteDTO c)
        {
            if (c == null) return;

            ClienteIdEmEdicao = c.Id;

            txtCodigo.Text = c.Id.ToString();
            txtNome.Text = c.Nome ?? string.Empty;
            txtApelido.Text = c.Apelido ?? string.Empty;
            txtCpfCnpj.Text = c.CpfCnpj ?? string.Empty;
            txtRgIe.Text = c.RgIe ?? string.Empty;
            txtTelefone.Text = c.Telefone ?? string.Empty;
            txtEmail.Text = c.Email ?? string.Empty;
            txtCep.Text = c.Cep ?? string.Empty;
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
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            string nome = txtNome.Text.Trim();
            if (string.IsNullOrWhiteSpace(nome))
            {
                MessageBox.Show("Informe o nome do cliente.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                return;
            }

            try
            {
                using (var conexaoDb = new Connection())
                {
                    conexaoDb.IniciarConexao();

                    string sql = ClienteIdEmEdicao.HasValue
                        ? @"UPDATE public.customer SET
                                name = @name,
                                nickname = @nickname,
                                cpf_cnpj = @cpf_cnpj,
                                rg_ie = @rg_ie,
                                phone = @phone,
                                email = @email,
                                cep = @cep,
                                street = @street,
                                number_address = @number_address,
                                neighborhood = @neighborhood,
                                city = @city,
                                uf = @uf,
                                notes = @notes,
                                is_active = @is_active,
                                updated_at = NOW()
                            WHERE id_customer = @id;"
                        : @"INSERT INTO public.customer
                            (
                                name, nickname, cpf_cnpj, rg_ie, phone, email, cep,
                                street, number_address, neighborhood, complement, city,
                                uf, notes, is_active
                            )
                            VALUES
                            (
                                @name, @nickname, @cpf_cnpj, @rg_ie, @phone, @email, @cep,
                                @street, @number_address, @neighborhood, @complement, @city,
                                @uf, @notes, @is_active
                            )
                            RETURNING id_customer;";

                    using (var comando = new NpgsqlCommand(sql, conexaoDb.Conexao))
                    {
                        if (ClienteIdEmEdicao.HasValue)
                            comando.Parameters.AddWithValue("@id", NpgsqlDbType.Bigint, ClienteIdEmEdicao.Value);
                        else
                            comando.Parameters.AddWithValue("@complement", DBNull.Value);

                        comando.Parameters.AddWithValue("@name", nome);
                        comando.Parameters.AddWithValue("@nickname", ValorOuNulo(txtApelido.Text));
                        comando.Parameters.AddWithValue("@cpf_cnpj", ValorOuNulo(txtCpfCnpj.Text));
                        comando.Parameters.AddWithValue("@rg_ie", ValorOuNulo(txtRgIe.Text));
                        comando.Parameters.AddWithValue("@phone", ValorOuNulo(txtTelefone.Text));
                        comando.Parameters.AddWithValue("@email", ValorOuNulo(txtEmail.Text));
                        comando.Parameters.AddWithValue("@cep", ValorOuNulo(txtCep.Text));
                        comando.Parameters.AddWithValue("@street", ValorOuNulo(txtRua.Text));
                        comando.Parameters.AddWithValue("@number_address", ValorOuNulo(txtNumero.Text));
                        comando.Parameters.AddWithValue("@neighborhood", ValorOuNulo(txtBairro.Text));
                        comando.Parameters.AddWithValue("@city", ValorOuNulo(txtCidade.Text));
                        comando.Parameters.AddWithValue("@uf", ValorOuNulo(cboUf.Text));
                        comando.Parameters.AddWithValue("@notes", ValorOuNulo(txtDescricao.Text));
                        comando.Parameters.AddWithValue("@is_active", rbAtivo.Checked);

                        if (ClienteIdEmEdicao.HasValue)
                        {
                            comando.ExecuteNonQuery();
                        }
                        else
                        {
                            long idCliente = Convert.ToInt64(comando.ExecuteScalar());
                            txtCodigo.Text = idCliente.ToString();
                        }
                    }
                }

                string mensagem = ClienteIdEmEdicao.HasValue
                    ? "Cliente atualizado com sucesso!"
                    : $"Cliente cadastrado com sucesso. Código: {txtCodigo.Text}";

                MessageBox.Show(mensagem, "Cadastro", MessageBoxButtons.OK, MessageBoxIcon.Information);

                AoConcluirOperacao?.Invoke();

                Form formulario = FindForm();
                if (formulario != null)
                {
                    formulario.DialogResult = DialogResult.OK;
                    formulario.Close();
                }
            }
            catch (PostgresException ex) when (ex.SqlState == "23505")
            {
                MessageBox.Show("O CPF/CNPJ informado já está cadastrado.", "Cadastro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar cliente: {ex.Message}", "Falha de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static object ValorOuNulo(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? (object)DBNull.Value : valor.Trim();
        }

        private async void txtCep_Leave(object sender, EventArgs e)
        {
            string cep = ExtrairDigitos(txtCep.Text);
            if (cep.Length != 8)
                return;

            try
            {
                string url = "https://brasilapi.com.br/api/cep/v2/" + cep;
                using (HttpResponseMessage resposta = await ClienteHttp.GetAsync(url))
                {
                    if (!resposta.IsSuccessStatusCode)
                    {
                        MessageBox.Show("CEP não encontrado.", "Consulta de CEP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string json = await resposta.Content.ReadAsStringAsync();
                    BrasilApiCep endereco = DesserializarCep(json);
                    if (endereco == null)
                        return;

                    if (!string.IsNullOrWhiteSpace(endereco.Street))
                        txtRua.Text = endereco.Street;
                    if (!string.IsNullOrWhiteSpace(endereco.Neighborhood))
                        txtBairro.Text = endereco.Neighborhood;
                    if (!string.IsNullOrWhiteSpace(endereco.City))
                        txtCidade.Text = endereco.City;
                    if (!string.IsNullOrWhiteSpace(endereco.State))
                        cboUf.SelectedItem = endereco.State.ToUpperInvariant();
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
            {
                return serializer.ReadObject(stream) as BrasilApiCep;
            }
        }

        private static string ExtrairDigitos(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            var resultado = new StringBuilder();
            foreach (char caractere in texto)
            {
                if (char.IsDigit(caractere))
                    resultado.Append(caractere);
            }

            return resultado.ToString();
        }

        private void btnLimpar_Click(object sender, EventArgs e) => LimparFormulario();

        private void btnCancelar_Click(object sender, EventArgs e) => FindForm()?.Close();

        private void LimparFormulario()
        {
            ClienteIdEmEdicao = null;
            btnSalvar.Text = "Salvar";

            foreach (Control controle in Controls)
            {
                LimparControle(controle);
            }

            rbAtivo.Checked = true;
            cboUf.SelectedIndex = -1;
            txtNome.Focus();
        }

        private void LimparControle(Control controle)
        {
            if (controle is TextBox textBox && textBox != txtCodigo)
            {
                textBox.Clear();
            }
            else if (controle is ComboBox comboBox)
            {
                comboBox.SelectedIndex = -1;
            }
            else if (controle.HasChildren)
            {
                foreach (Control filho in controle.Controls)
                {
                    LimparControle(filho);
                }
            }
        }

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