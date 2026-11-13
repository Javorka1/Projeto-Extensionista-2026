using Npgsql;
using NpgsqlTypes;
using ProjetoExtensionista2026_2.DTOs;
using ProjetoExtensionista2026_2.SQL;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public partial class UcCadastroProduto : UserControl
    {
        private static readonly HttpClient _httpClient = CriarHttpClient();
        private static readonly ConcurrentDictionary<string, byte[]> _imageBytesCache = new ConcurrentDictionary<string, byte[]>();

        private ProdutoDTO _produtoRef;
        public int? ProdutoIdEmEdicao { get; private set; }
        public event Action AoConcluirOperacao;

        public UcCadastroProduto()
        {
            InitializeComponent();

            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            if (cboUnidade.Items.Count > 0)
                cboUnidade.SelectedIndex = 0;

            _ = InicializarDadosAsync();
        }

        private static HttpClient CriarHttpClient()
        {
            var client = new HttpClient { Timeout = TimeSpan.FromSeconds(8) };
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
            return client;
        }

        private async Task InicializarDadosAsync()
        {
            if (!ProdutoIdEmEdicao.HasValue)
            {
                await Task.WhenAll(
                    CarregarCategoriasBancoAsync(),
                    CarregarFornecedoresBancoAsync(),
                    GerarProximoCodigoAsync()
                );
            }
        }

        public class ItemCombo
        {
            public long Id { get; set; }
            public string Nome { get; set; }

            public ItemCombo(long id, string nome)
            {
                Id = id;
                Nome = nome;
            }

            public override string ToString() => Nome;
        }

        public async Task CarregarCategoriasBancoAsync(long? idParaSelecionar = null)
        {
            try
            {
                var lista = await Task.Run(() =>
                {
                    var itens = new List<ItemCombo>();
                    using (var db = new Connection())
                    {
                        db.IniciarConexao();
                        const string sql = "SELECT id_category, name_category FROM public.category ORDER BY name_category ASC;";
                        using (var cmd = new NpgsqlCommand(sql, db.Conexao))
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                itens.Add(new ItemCombo(
                                    reader.GetInt64(reader.GetOrdinal("id_category")),
                                    reader.GetString(reader.GetOrdinal("name_category"))
                                ));
                            }
                        }
                    }
                    return itens;
                });

                cboCategoria.DataSource = null;
                cboCategoria.DisplayMember = "Nome";
                cboCategoria.ValueMember = "Id";
                cboCategoria.DataSource = lista;
                cboCategoria.SelectedIndex = idParaSelecionar.HasValue ? -1 : -1;

                if (idParaSelecionar.HasValue)
                    cboCategoria.SelectedValue = idParaSelecionar.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar categorias: {ex.Message}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public async Task CarregarFornecedoresBancoAsync(long? idParaSelecionar = null)
        {
            try
            {
                var lista = await Task.Run(() =>
                {
                    var itens = new List<ItemCombo>();
                    using (var db = new Connection())
                    {
                        db.IniciarConexao();
                        const string sql = "SELECT id_supplier, name FROM public.supplier WHERE is_active = TRUE ORDER BY name ASC;";
                        using (var cmd = new NpgsqlCommand(sql, db.Conexao))
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                itens.Add(new ItemCombo(
                                    reader.GetInt64(reader.GetOrdinal("id_supplier")),
                                    reader.GetString(reader.GetOrdinal("name"))
                                ));
                            }
                        }
                    }
                    return itens;
                });

                cboFornecedor.DataSource = null;
                cboFornecedor.DisplayMember = "Nome";
                cboFornecedor.ValueMember = "Id";
                cboFornecedor.DataSource = lista;
                cboFornecedor.SelectedIndex = -1;

                if (idParaSelecionar.HasValue)
                    cboFornecedor.SelectedValue = idParaSelecionar.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar fornecedores: {ex.Message}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnAdicionarCategoria_Click(object sender, EventArgs e)
        {
            string novaCategoria = PromptDialog.Mostrar("Digite o nome da nova categoria:", "Nova Categoria");
            if (string.IsNullOrWhiteSpace(novaCategoria)) return;

            novaCategoria = novaCategoria.Trim();

            try
            {
                long idCriado = await Task.Run(() =>
                {
                    using (var db = new Connection())
                    {
                        db.IniciarConexao();
                        const string sql = @"
                            INSERT INTO public.category (name_category)
                            SELECT @cat
                            WHERE NOT EXISTS (
                                SELECT 1 FROM public.category WHERE LOWER(TRIM(name_category)) = LOWER(@cat)
                            );

                            SELECT id_category FROM public.category WHERE LOWER(TRIM(name_category)) = LOWER(@cat) LIMIT 1;";

                        using (var cmd = new NpgsqlCommand(sql, db.Conexao))
                        {
                            cmd.Parameters.AddWithValue("@cat", novaCategoria);
                            return Convert.ToInt64(cmd.ExecuteScalar());
                        }
                    }
                });

                await CarregarCategoriasBancoAsync(idCriado);
                MessageBox.Show("Categoria adicionada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao adicionar categoria: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAdicionarFornecedor_Click(object sender, EventArgs e)
        {
            var dados = PromptDialog.MostrarFornecedor();
            if (!dados.Confirmado || string.IsNullOrWhiteSpace(dados.Nome)) return;

            string nome = dados.Nome.Trim();
            string documento = string.IsNullOrWhiteSpace(dados.Documento) ? null : dados.Documento.Trim();
            string telefone = string.IsNullOrWhiteSpace(dados.Telefone) ? null : dados.Telefone.Trim();

            try
            {
                long idCriado = await Task.Run(() =>
                {
                    using (var db = new Connection())
                    {
                        db.IniciarConexao();
                        const string sql = @"
                    INSERT INTO public.supplier (name, document, phone, is_active)
                    SELECT @nome, @doc, @tel, TRUE
                    WHERE NOT EXISTS (
                        SELECT 1 FROM public.supplier WHERE LOWER(TRIM(name)) = LOWER(@nome)
                    );

                    SELECT id_supplier FROM public.supplier WHERE LOWER(TRIM(name)) = LOWER(@nome) LIMIT 1;";

                        using (var cmd = new NpgsqlCommand(sql, db.Conexao))
                        {
                            cmd.Parameters.AddWithValue("@nome", NpgsqlDbType.Varchar, nome);
                            cmd.Parameters.AddWithValue("@doc", NpgsqlDbType.Varchar, (object)documento ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@tel", NpgsqlDbType.Varchar, (object)telefone ?? DBNull.Value);

                            return Convert.ToInt64(cmd.ExecuteScalar());
                        }
                    }
                });

                await CarregarFornecedoresBancoAsync(idCriado);
                MessageBox.Show("Fornecedor adicionado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao adicionar fornecedor: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void DefinirStatusProduto(bool ativo)
        {
            rbAtivo.Checked = ativo;
            rbInativo.Checked = !ativo;
        }

        public bool ObterStatusProduto() => rbAtivo.Checked;

        public async Task GerarProximoCodigoAsync()
        {
            try
            {
                string codigoGerado = await Task.Run(async () =>
                {
                    using (var db = new Connection())
                    {
                        db.IniciarConexao();
                        const string query = "SELECT COALESCE(MAX(id_product), 0) + 1 FROM public.product;";
                        using (var cmd = new NpgsqlCommand(query, db.Conexao))
                        {
                            object resultado = await cmd.ExecuteScalarAsync();
                            return (resultado != null && int.TryParse(resultado.ToString(), out int prox))
                                ? prox.ToString()
                                : "1";
                        }
                    }
                });

                if (!IsDisposed && !ProdutoIdEmEdicao.HasValue)
                    txtCodigo.Text = codigoGerado;
            }
            catch
            {
                if (!IsDisposed && !ProdutoIdEmEdicao.HasValue)
                    txtCodigo.Text = "1";
            }
        }

        public async void CarregarDadosParaEdicao(ProdutoDTO p) => await CarregarDadosParaEdicaoAsync(p);

        public async Task CarregarDadosParaEdicaoAsync(ProdutoDTO p)
        {
            if (p == null) return;

            SuspendLayout();

            _produtoRef = p;
            ProdutoIdEmEdicao = p.Id;

            txtCodigo.Text = p.Id.ToString();
            txtNome.Text = p.Nome ?? string.Empty;
            txtDescricao.Text = p.Descricao ?? string.Empty;
            txtLocalizacao.Text = p.Localizacao ?? string.Empty;

            numPrecoCompra.Value = Math.Max(numPrecoCompra.Minimum, Math.Min(numPrecoCompra.Maximum, p.PrecoCompra));
            numPrecoVenda.Value = Math.Max(numPrecoVenda.Minimum, Math.Min(numPrecoVenda.Maximum, p.PrecoVenda));
            numEstoque.Value = Math.Max(numEstoque.Minimum, Math.Min(numEstoque.Maximum, (decimal)p.Estoque));
            numEstoqueMinimo.Value = Math.Max(numEstoqueMinimo.Minimum, Math.Min(numEstoqueMinimo.Maximum, (decimal)p.EstoqueMinimo));

            string unidade = string.IsNullOrWhiteSpace(p.Unidade) ? "UNID" : p.Unidade.Trim().ToUpper();
            if (unidade == "UN") unidade = "UNID";
            if (!cboUnidade.Items.Contains(unidade))
                cboUnidade.Items.Add(unidade);
            cboUnidade.SelectedItem = unidade;

            DefinirStatusProduto(p.Ativo);
            CalcularMargem();
            txtFotoUrl.Text = p.FotoUrl ?? string.Empty;
            btnSalvar.Text = "Atualizar";

            DefinirImagemPreview(Properties.Resources.produto_ausente);

            ResumeLayout();

            var tasks = new List<Task>();
            if (cboCategoria.Items.Count == 0) tasks.Add(CarregarCategoriasBancoAsync());
            if (cboFornecedor.Items.Count == 0) tasks.Add(CarregarFornecedoresBancoAsync());

            if (tasks.Count > 0)
                await Task.WhenAll(tasks);

            SelecionarItemComboPorNome(cboCategoria, p.Categoria);
            SelecionarItemComboPorNome(cboFornecedor, p.Fornecedor);

            if (!string.IsNullOrWhiteSpace(p.FotoUrl))
                _ = CarregarImagemPorUrlAsync(p.FotoUrl, exibirErros: false);
        }

        private static void SelecionarItemComboPorNome(ComboBox combo, string nomeAlvo)
        {
            if (string.IsNullOrWhiteSpace(nomeAlvo))
            {
                combo.SelectedIndex = -1;
                return;
            }

            string alvo = nomeAlvo.Trim();
            for (int i = 0; i < combo.Items.Count; i++)
            {
                if (combo.Items[i] is ItemCombo item &&
                    string.Equals(item.Nome?.Trim(), alvo, StringComparison.OrdinalIgnoreCase))
                {
                    combo.SelectedIndex = i;
                    return;
                }
            }
            combo.SelectedIndex = -1;
        }

        // Evento vinculado pelo Designer aos dois NumericUpDown (numPrecoCompra e numPrecoVenda)
        private void numPreco_ValueChanged(object sender, EventArgs e) => CalcularMargem();

        private void CalcularMargem()
        {
            decimal compra = numPrecoCompra.Value;
            decimal venda = numPrecoVenda.Value;

            txtMargem.Text = compra > 0
                ? $"{((venda - compra) / compra) * 100m:F2}%"
                : "0,00%";
        }

        private async void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("O nome do produto é obrigatório.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(cboUnidade.Text))
            {
                MessageBox.Show("Selecione a unidade de medida (UNID ou JG).", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboUnidade.Focus();
                return;
            }

            btnSalvar.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            string nome = txtNome.Text.Trim();
            string descricao = string.IsNullOrWhiteSpace(txtDescricao.Text) ? null : txtDescricao.Text.Trim();
            decimal precoCompra = numPrecoCompra.Value;
            decimal precoVenda = numPrecoVenda.Value;
            string localizacao = string.IsNullOrWhiteSpace(txtLocalizacao.Text) ? null : txtLocalizacao.Text.Trim();
            decimal estoque = numEstoque.Value;
            int estoqueMin = (int)numEstoqueMinimo.Value;
            string unidade = cboUnidade.Text.Trim();
            bool statusAtivo = ObterStatusProduto();
            string fotoUrl = string.IsNullOrWhiteSpace(txtFotoUrl.Text) ? null : txtFotoUrl.Text.Trim();

            long? idCategoria = (cboCategoria.SelectedItem as ItemCombo)?.Id;
            long? idFornecedor = (cboFornecedor.SelectedItem as ItemCombo)?.Id;
            string textoCategoria = cboCategoria.Text;
            string textoFornecedor = cboFornecedor.Text;

            try
            {
                int idFinal = await Task.Run(async () =>
                {
                    using (var db = new Connection())
                    {
                        db.IniciarConexao();

                        string query = ProdutoIdEmEdicao.HasValue
                            ? @"UPDATE public.product SET
                                    name = @nome,
                                    description = @descricao,
                                    purchase_price = @preco_compra,
                                    sale_price = @preco_venda,
                                    location = @localizacao,
                                    stock_quantity = @estoque,
                                    min_stock = @estoque_min,
                                    unit = @unidade,
                                    id_category = @id_categoria,
                                    id_supplier = @id_fornecedor,
                                    is_active = @ativo,
                                    image_url = @foto_url,
                                    updated_at = NOW()
                                WHERE id_product = @id;"
                            : @"INSERT INTO public.product (
                                    name, description, purchase_price, sale_price,
                                    location, stock_quantity, min_stock, unit,
                                    id_category, id_supplier, is_active, image_url
                                ) VALUES (
                                    @nome, @descricao, @preco_compra, @preco_venda,
                                    @localizacao, @estoque, @estoque_min, @unidade,
                                    @id_categoria, @id_fornecedor, @ativo, @foto_url
                                ) RETURNING id_product;";

                        using (var cmd = new NpgsqlCommand(query, db.Conexao))
                        {
                            if (ProdutoIdEmEdicao.HasValue)
                                cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, ProdutoIdEmEdicao.Value);

                            cmd.Parameters.AddWithValue("@nome", NpgsqlDbType.Varchar, nome);
                            cmd.Parameters.AddWithValue("@descricao", NpgsqlDbType.Text, (object)descricao ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@preco_compra", NpgsqlDbType.Numeric, precoCompra);
                            cmd.Parameters.AddWithValue("@preco_venda", NpgsqlDbType.Numeric, precoVenda);
                            cmd.Parameters.AddWithValue("@localizacao", NpgsqlDbType.Varchar, (object)localizacao ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@estoque", NpgsqlDbType.Numeric, estoque);
                            cmd.Parameters.AddWithValue("@estoque_min", NpgsqlDbType.Integer, estoqueMin);
                            cmd.Parameters.AddWithValue("@unidade", NpgsqlDbType.Varchar, unidade);
                            cmd.Parameters.AddWithValue("@id_categoria", NpgsqlDbType.Bigint, (object)idCategoria ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@id_fornecedor", NpgsqlDbType.Bigint, (object)idFornecedor ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@ativo", NpgsqlDbType.Boolean, statusAtivo);
                            cmd.Parameters.AddWithValue("@foto_url", NpgsqlDbType.Text, (object)fotoUrl ?? DBNull.Value);

                            if (ProdutoIdEmEdicao.HasValue)
                            {
                                await cmd.ExecuteNonQueryAsync();
                                return ProdutoIdEmEdicao.Value;
                            }

                            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
                        }
                    }
                });

                if (_produtoRef != null)
                {
                    _produtoRef.Id = idFinal;
                    _produtoRef.Nome = nome;
                    _produtoRef.Descricao = descricao ?? string.Empty;
                    _produtoRef.PrecoCompra = precoCompra;
                    _produtoRef.PrecoVenda = precoVenda;
                    _produtoRef.Localizacao = localizacao ?? string.Empty;
                    _produtoRef.Estoque = (int)estoque;
                    _produtoRef.EstoqueMinimo = estoqueMin;
                    _produtoRef.Unidade = unidade;
                    _produtoRef.Categoria = textoCategoria;
                    _produtoRef.Fornecedor = textoFornecedor;
                    _produtoRef.Ativo = statusAtivo;
                    _produtoRef.FotoUrl = fotoUrl;
                }

                string msg = ProdutoIdEmEdicao.HasValue
                    ? "Produto atualizado com sucesso!"
                    : $"Produto cadastrado com sucesso! (Código: {idFinal})";

                MessageBox.Show(msg, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (!ProdutoIdEmEdicao.HasValue)
                    await LimparCamposAsync();

                AoConcluirOperacao?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar no banco de dados:\n{ex.Message}", "Erro de Operação", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private async void btnLimpar_Click(object sender, EventArgs e) => await LimparCamposAsync();
        public async void LimparCampos() => await LimparCamposAsync();

        public async Task LimparCamposAsync()
        {
            SuspendLayout();

            _produtoRef = null;
            ProdutoIdEmEdicao = null;

            txtCodigo.Text = "...";
            txtNome.Clear();
            txtDescricao.Clear();
            txtLocalizacao.Clear();
            txtFotoUrl.Clear();
            txtMargem.Text = "0,00%";

            numPrecoCompra.Value = 0;
            numPrecoVenda.Value = 0;
            numEstoque.Value = 0;
            numEstoqueMinimo.Value = 0;

            if (cboUnidade.Items.Count > 0) cboUnidade.SelectedIndex = 0;
            cboCategoria.SelectedIndex = -1;
            cboFornecedor.SelectedIndex = -1;

            DefinirStatusProduto(true);
            DefinirImagemPreview(Properties.Resources.produto_ausente);
            btnSalvar.Text = "Salvar";

            ResumeLayout();
            txtNome.Focus();

            await GerarProximoCodigoAsync();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            FindForm()?.Close();
            AoConcluirOperacao?.Invoke();
        }

        private void btnExcluirFoto_Click(object sender, EventArgs e)
        {
            DefinirImagemPreview(Properties.Resources.produto_ausente);
            txtFotoUrl.Clear();
        }

        private void btnEscolherArquivo_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Selecione a imagem do produto";
                ofd.Filter = "Imagens (*.jpg;*.jpeg;*.png;*.webp)|*.jpg;*.jpeg;*.png;*.webp|Todos os arquivos (*.*)|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        byte[] bytes = File.ReadAllBytes(ofd.FileName);
                        using (var ms = new MemoryStream(bytes))
                        using (var tempImg = Image.FromStream(ms))
                        {
                            DefinirImagemPreview(new Bitmap(tempImg));
                        }
                        txtFotoUrl.Text = ofd.FileName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao carregar a imagem selecionada: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void btnCarregarFoto_Click(object sender, EventArgs e) =>
            await CarregarImagemPorUrlAsync(txtFotoUrl.Text.Trim(), exibirErros: true);

        private async void txtFotoUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await CarregarImagemPorUrlAsync(txtFotoUrl.Text.Trim(), exibirErros: true);
            }
        }

        private async Task CarregarImagemPorUrlAsync(string url, bool exibirErros = true)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                if (exibirErros)
                {
                    MessageBox.Show("Por favor, informe a URL da imagem.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFotoUrl.Focus();
                }
                return;
            }

            try
            {
                if (!_imageBytesCache.TryGetValue(url, out byte[] dadosImagem))
                {
                    if (File.Exists(url))
                    {
                        dadosImagem = await Task.Run(() => File.ReadAllBytes(url));
                    }
                    else if (Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) &&
                            (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                    {
                        dadosImagem = await _httpClient.GetByteArrayAsync(uriResult);
                    }
                    else
                    {
                        if (exibirErros)
                            MessageBox.Show("Informe uma URL válida iniciada por http:// ou https://", "URL Inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (dadosImagem != null && dadosImagem.Length > 0)
                        _imageBytesCache.TryAdd(url, dadosImagem);
                }

                if (dadosImagem != null && !IsDisposed)
                {
                    using (var ms = new MemoryStream(dadosImagem))
                    using (var imgOriginal = Image.FromStream(ms))
                    {
                        DefinirImagemPreview(new Bitmap(imgOriginal));
                    }
                }
            }
            catch (Exception ex)
            {
                if (!IsDisposed)
                {
                    DefinirImagemPreview(Properties.Resources.produto_ausente);
                    if (exibirErros)
                    {
                        MessageBox.Show($"Não foi possível carregar a imagem do link informado.\n\nDetalhes: {ex.Message}",
                                        "Falha ao Carregar Imagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void DefinirImagemPreview(Image novaImagem)
        {
            if (picFotoPreview.Image != null && !ReferenceEquals(picFotoPreview.Image, Properties.Resources.produto_ausente))
                picFotoPreview.Image.Dispose();

            picFotoPreview.Image = novaImagem;
        }
    }

    public static class PromptDialog
    {
        public static string Mostrar(string texto, string titulo)
        {
            using (Form formPrompt = new Form
            {
                Width = 380,
                Height = 160,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = titulo,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            })
            {
                Label lblTexto = new Label { Left = 16, Top = 16, Width = 330, Text = texto };
                TextBox txtInput = new TextBox { Left = 16, Top = 40, Width = 330 };
                Button btnOk = new Button { Text = "Salvar", Left = 180, Width = 80, Top = 75, DialogResult = DialogResult.OK };
                Button btnCancel = new Button { Text = "Cancelar", Left = 266, Width = 80, Top = 75, DialogResult = DialogResult.Cancel };

                formPrompt.Controls.AddRange(new Control[] { lblTexto, txtInput, btnOk, btnCancel });
                formPrompt.AcceptButton = btnOk;
                formPrompt.CancelButton = btnCancel;

                return formPrompt.ShowDialog() == DialogResult.OK ? txtInput.Text : string.Empty;
            }
        }

        public static (bool Confirmado, string Nome, string Documento, string Telefone) MostrarFornecedor()
        {
            using (Form formPrompt = new Form
            {
                Width = 400,
                Height = 270,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Novo Fornecedor",
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            })
            {
                Label lblNome = new Label { Left = 16, Top = 16, Width = 350, Text = "Nome / Razão Social *:" };
                TextBox txtNome = new TextBox { Left = 16, Top = 38, Width = 350 };

                Label lblDoc = new Label { Left = 16, Top = 72, Width = 350, Text = "Documento (CPF / CNPJ):" };
                TextBox txtDoc = new TextBox { Left = 16, Top = 94, Width = 350, MaxLength = 20 };

                Label lblTel = new Label { Left = 16, Top = 128, Width = 350, Text = "Telefone:" };
                TextBox txtTel = new TextBox { Left = 16, Top = 150, Width = 350, MaxLength = 20 };

                Button btnOk = new Button { Text = "Salvar", Left = 200, Width = 80, Top = 190 };
                Button btnCancel = new Button { Text = "Cancelar", Left = 286, Width = 80, Top = 190, DialogResult = DialogResult.Cancel };

                btnOk.Click += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(txtNome.Text))
                    {
                        MessageBox.Show("O nome do fornecedor é obrigatório.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtNome.Focus();
                        return;
                    }
                    formPrompt.DialogResult = DialogResult.OK;
                    formPrompt.Close();
                };

                formPrompt.Controls.AddRange(new Control[] { lblNome, txtNome, lblDoc, txtDoc, lblTel, txtTel, btnOk, btnCancel });
                formPrompt.AcceptButton = btnOk;
                formPrompt.CancelButton = btnCancel;

                bool ok = formPrompt.ShowDialog() == DialogResult.OK;
                return (ok, txtNome.Text, txtDoc.Text, txtTel.Text);
            }
        }
    }
}