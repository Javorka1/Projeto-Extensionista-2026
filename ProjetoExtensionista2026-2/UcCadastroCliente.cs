using Npgsql;
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

        public UcCadastroCliente()
        {
            InitializeComponent();

            btnSalvar.Click += btnSalvar_Click;
            btnLimpar.Click += btnLimpar_Click;
            btnCancelar.Click += btnCancelar_Click;
            textBox7.Leave += textBox7_Leave;
            rbAtivo.Checked = true;
            comboBox1.Items.AddRange(new object[]
            {
                "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA",
                "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN",
                "RS", "RO", "RR", "SC", "SP", "SE", "TO"
            });
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

                    const string sql = @"
                        INSERT INTO public.customer
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

                    long idCliente;
                    using (var comando = new NpgsqlCommand(sql, conexaoDb.Conexao))
                    {
                        comando.Parameters.AddWithValue("@name", nome);
                        comando.Parameters.AddWithValue("@nickname", ValorOuNulo(textBox1.Text));
                        comando.Parameters.AddWithValue("@cpf_cnpj", ValorOuNulo(textBox2.Text));
                        comando.Parameters.AddWithValue("@rg_ie", ValorOuNulo(textBox4.Text));
                        comando.Parameters.AddWithValue("@phone", ValorOuNulo(textBox5.Text));
                        comando.Parameters.AddWithValue("@email", ValorOuNulo(textBox3.Text));
                        comando.Parameters.AddWithValue("@cep", ValorOuNulo(textBox7.Text));
                        comando.Parameters.AddWithValue("@street", ValorOuNulo(textBox10.Text));
                        comando.Parameters.AddWithValue("@number_address", ValorOuNulo(textBox8.Text));
                        comando.Parameters.AddWithValue("@neighborhood", ValorOuNulo(textBox6.Text));
                        comando.Parameters.AddWithValue("@complement", DBNull.Value);
                        comando.Parameters.AddWithValue("@city", ValorOuNulo(textBox12.Text));
                        comando.Parameters.AddWithValue("@uf", ValorOuNulo(comboBox1.Text));
                        comando.Parameters.AddWithValue("@notes", ValorOuNulo(txtDescricao.Text));
                        comando.Parameters.AddWithValue("@is_active", rbAtivo.Checked);
                        idCliente = Convert.ToInt64(comando.ExecuteScalar());

                        txtCodigo.Text = idCliente.ToString();
                    }
                }

                MessageBox.Show($"Cliente cadastrado com sucesso. Código: {txtCodigo.Text}", "Cadastro", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                MessageBox.Show($"Erro ao cadastrar cliente: {ex.Message}", "Falha de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static object ValorOuNulo(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? (object)DBNull.Value : valor.Trim();
        }

        private async void textBox7_Leave(object sender, EventArgs e)
        {
            string cep = ExtrairDigitos(textBox7.Text);
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
                        textBox10.Text = endereco.Street;
                    if (!string.IsNullOrWhiteSpace(endereco.Neighborhood))
                        textBox6.Text = endereco.Neighborhood;
                    if (!string.IsNullOrWhiteSpace(endereco.City))
                        textBox12.Text = endereco.City;
                    if (!string.IsNullOrWhiteSpace(endereco.State))
                        comboBox1.SelectedItem = endereco.State.ToUpperInvariant();
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

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparFormulario();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            FindForm()?.Close();
        }

        private void LimparFormulario()
        {
            foreach (Control controle in Controls)
            {
                LimparControle(controle);
            }

            rbAtivo.Checked = true;
            comboBox1.SelectedIndex = -1;
            txtNome.Focus();
        }

        private static void LimparControle(Control controle)
        {
            if (controle is TextBox textBox && textBox.Name != "textBox9")
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
