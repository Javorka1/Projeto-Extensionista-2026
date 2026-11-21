using MySql.Data.MySqlClient;
using ProjetoExtensionista2026_2.DTOs;
using ProjetoExtensionista2026_2.SQL;
using ProjetoExtensionista2026_2.SQL.SQL_functions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public partial class UcCadastroProduto : UserControl
    {
        private static readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(8) };
        private static readonly ConcurrentDictionary<string, byte[]> _imageBytesCache = new ConcurrentDictionary<string, byte[]>();

        private ProdutoDTO _produtoRef;
        private bool _atualizandoValores;
        private bool _margemManual;

        public int? ProdutoIdEmEdicao { get; private set; }
        public event Action AoConcluirOperacao;

        public class ItemCombo
        {
            public long Id { get; set; }
            public string Nome { get; set; }
            public ItemCombo(long id, string nome) { Id = id; Nome = nome; }
            public override string ToString() => Nome;
        }

        public UcCadastroProduto()
        {
            InitializeComponent();

            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            if (cboUnidade.Items.Count > 0)
                cboUnidade.SelectedIndex = 0;

            AtualizarDataAtualizacao(null);
            _ = InicializarDadosAsync();
        }

        private async Task InicializarDadosAsync()
        {
            if (ProdutoIdEmEdicao.HasValue) return;

            await Task.WhenAll(
                CarregarCategoriasBancoAsync(),
                CarregarFornecedoresBancoAsync(),
                GerarProximoCodigoAsync()
            );
        }

        public Task CarregarCategoriasBancoAsync(long? idSelecionar = null) =>
            CarregarComboAsync(cboCategoria, "SELECT id_category, name_category FROM category ORDER BY name_category ASC;", idSelecionar);

        public Task CarregarFornecedoresBancoAsync(long? idSelecionar = null) =>
            CarregarComboAsync(cboFornecedor, "SELECT id_supplier, name FROM supplier WHERE is_active = TRUE ORDER BY name ASC;", idSelecionar);

        private async Task CarregarComboAsync(ComboBox combo, string sql, long? idSelecionar)
        {
            try
            {
                var lista = await Task.Run(() =>
                {
                    var itens = new List<ItemCombo>();
                    using (var db = new Connection())
                    {
                        db.IniciarConexao();
                        using (var cmd = new MySqlCommand(sql, db.Conexao))
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                itens.Add(new ItemCombo(reader.GetInt64(0), reader.GetString(1)));
                        }
                    }
                    return itens;
                });

                combo.DataSource = null;
                combo.DisplayMember = "Nome";
                combo.ValueMember = "Id";
                combo.DataSource = lista;
                combo.SelectedIndex = -1;

                if (idSelecionar.HasValue)
                    combo.SelectedValue = idSelecionar.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar lista: {ex.Message}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnAdicionarCategoria_Click(object sender, EventArgs e)
        {
            string nome = PromptDialog.Mostrar("Digite o nome da nova categoria:", "Nova Categoria", 150).Trim();
            if (string.IsNullOrWhiteSpace(nome)) return;

            try
            {
                long id = await Task.Run(() =>
                {
                    using (var db = new Connection())
                    {
                        db.IniciarConexao();
                        const string sql = "INSERT IGNORE INTO category (name_category) VALUES (@cat);";
                        using (var cmd = new MySqlCommand(sql, db.Conexao))
                        {
                            cmd.Parameters.AddWithValue("@cat", nome);
                            cmd.ExecuteNonQuery();
                        }
                        using (var cmd = new MySqlCommand("SELECT id_category FROM category WHERE name_category = @cat LIMIT 1;", db.Conexao))
                        {
                            cmd.Parameters.AddWithValue("@cat", nome);
                            return Convert.ToInt64(cmd.ExecuteScalar());
                        }
                    }
                });

                await CarregarCategoriasBancoAsync(id);
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

            try
            {
                long id = await Task.Run(() =>
                {
                    using (var db = new Connection())
                    {
                        db.IniciarConexao();
                        using (var cmd = new MySqlCommand("SELECT id_supplier FROM supplier WHERE LOWER(TRIM(name)) = LOWER(@nome) LIMIT 1;", db.Conexao))
                        {
                            cmd.Parameters.AddWithValue("@nome", dados.Nome.Trim());
                            object existente = cmd.ExecuteScalar();
                            if (existente != null && existente != DBNull.Value)
                                return Convert.ToInt64(existente);
                        }
                        using (var cmd = new MySqlCommand("INSERT INTO supplier (name, document, phone, is_active) VALUES (@nome, @doc, @tel, TRUE);", db.Conexao))
                        {
                            cmd.Parameters.AddWithValue("@nome", dados.Nome.Trim());
                            cmd.Parameters.AddWithValue("@doc", dados.Documento ?? string.Empty);
                            cmd.Parameters.AddWithValue("@tel", string.IsNullOrWhiteSpace(dados.Telefone) ? (object)DBNull.Value : dados.Telefone);
                            cmd.ExecuteNonQuery();
                            return Convert.ToInt64(cmd.LastInsertedId);
                        }
                    }
                });

                await CarregarFornecedoresBancoAsync(id);
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
                object res = await Task.Run(async () =>
                {
                    using (var db = new Connection())
                    {
                        db.IniciarConexao();
                        using (var cmd = new MySqlCommand("SELECT COALESCE(MAX(id_product), 0) + 1 FROM product;", db.Conexao))
                            return await cmd.ExecuteScalarAsync();
                    }
                });

                if (!IsDisposed && !ProdutoIdEmEdicao.HasValue)
                    txtCodigo.Text = res?.ToString() ?? "1";
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
            _atualizandoValores = true;
            try
            {
                _produtoRef = p;
                ProdutoIdEmEdicao = p.Id;
                _margemManual = false;

                txtCodigo.Text = p.Id.ToString();
                txtNome.Text = p.Nome ?? string.Empty;
                txtDescricao.Text = p.Descricao ?? string.Empty;
                txtLocalizacao.Text = p.Localizacao ?? string.Empty;
                txtFotoUrl.Text = p.FotoUrl ?? string.Empty;

                numPrecoCompra.Value = Math.Max(numPrecoCompra.Minimum, Math.Min(numPrecoCompra.Maximum, p.PrecoCompra));
                numPrecoVenda.Value = Math.Max(numPrecoVenda.Minimum, Math.Min(numPrecoVenda.Maximum, p.PrecoVenda));
                numEstoque.Value = Math.Max(numEstoque.Minimum, Math.Min(numEstoque.Maximum, (decimal)p.Estoque));
                numEstoqueMinimo.Value = Math.Max(numEstoqueMinimo.Minimum, Math.Min(numEstoqueMinimo.Maximum, (decimal)p.EstoqueMinimo));

                string unidade = string.IsNullOrWhiteSpace(p.Unidade) || p.Unidade.Trim().Equals("UN", StringComparison.OrdinalIgnoreCase)
                    ? "UNID"
                    : p.Unidade.Trim().ToUpper();
                if (!cboUnidade.Items.Contains(unidade)) cboUnidade.Items.Add(unidade);
                cboUnidade.SelectedItem = unidade;

                DefinirStatusProduto(p.Ativo);
                DefinirImagemPreview(Properties.Resources.produto_ausente);
                btnSalvar.Text = "Atualizar";
            }
            finally { _atualizandoValores = false; }

            CalcularMargem();
            ResumeLayout();

            var tarefas = new List<Task> { CarregarDataAtualizacaoAsync(p.Id) };
            if (cboCategoria.Items.Count == 0) tarefas.Add(CarregarCategoriasBancoAsync());
            if (cboFornecedor.Items.Count == 0) tarefas.Add(CarregarFornecedoresBancoAsync());
            await Task.WhenAll(tarefas);

            SelecionarComboPorTexto(cboCategoria, p.Categoria);
            SelecionarComboPorTexto(cboFornecedor, p.Fornecedor);

            if (!string.IsNullOrWhiteSpace(p.FotoUrl))
                _ = CarregarImagemPorUrlAsync(p.FotoUrl, false);
        }

        private async Task CarregarDataAtualizacaoAsync(int idProduto)
        {
            try
            {
                DateTime? atualizado = await Task.Run(async () =>
                {
                    using (var db = new Connection())
                    {
                        db.IniciarConexao();
                        using (var cmd = new MySqlCommand("SELECT updated_at FROM product WHERE id_product = @id;", db.Conexao))
                        {
                            cmd.Parameters.AddWithValue("@id", idProduto);
                            object res = await cmd.ExecuteScalarAsync();
                            return res != null && res != DBNull.Value ? Convert.ToDateTime(res) : (DateTime?)null;
                        }
                    }
                });

                if (!IsDisposed && ProdutoIdEmEdicao == idProduto)
                    AtualizarDataAtualizacao(atualizado);
            }
            catch { }
        }

        private void AtualizarDataAtualizacao(DateTime? atualizado)
        {
            if (lblAtualizado != null)
                lblAtualizado.Text = atualizado.HasValue ? $"Atualizado em: {atualizado.Value.ToLocalTime():dd/MM/yyyy HH:mm}" : "Atualizado em: -";
        }

        private static void SelecionarComboPorTexto(ComboBox combo, string alvo) =>
            combo.SelectedIndex = string.IsNullOrWhiteSpace(alvo) ? -1 : combo.FindStringExact(alvo.Trim());

        private void numPreco_ValueChanged(object sender, EventArgs e)
        {
            if (_atualizandoValores) return;

            if (sender == numPrecoCompra && (_margemManual || numPrecoVenda.Value == 0m) && TryObterMargem(out decimal m) && m != 0m)
            {
                AplicarMargemNaVenda(m, false);
                return;
            }

            if (sender == numPrecoVenda) _margemManual = false;
            CalcularMargem();
        }

        private void txtMargem_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool separador = (e.KeyChar == ',' || e.KeyChar == '.') && !txtMargem.Text.Contains(",") && !txtMargem.Text.Contains(".");
            bool sinal = e.KeyChar == '-' && txtMargem.SelectionStart == 0 && !txtMargem.Text.Contains("-");
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !separador && !sinal)
                e.Handled = true;
        }

        private void txtMargem_TextChanged(object sender, EventArgs e)
        {
            if (_atualizandoValores) return;
            _margemManual = true;
            if (TryObterMargem(out decimal m)) AplicarMargemNaVenda(m, false);
        }

        private void txtMargem_Leave(object sender, EventArgs e)
        {
            if (_atualizandoValores) return;
            if (TryObterMargem(out decimal m)) AplicarMargemNaVenda(m, true);
            else { _margemManual = false; CalcularMargem(); }
        }

        private void txtMargem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; txtMargem_Leave(sender, EventArgs.Empty); }
        }

        private bool TryObterMargem(out decimal margem)
        {
            margem = 0m;
            string limpo = txtMargem?.Text.Replace("%", "").Trim() ?? string.Empty;
            return decimal.TryParse(limpo, NumberStyles.Number, CultureInfo.CurrentCulture, out margem) ||
                   decimal.TryParse(limpo.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out margem);
        }

        private void AplicarMargemNaVenda(decimal margem, bool formatarTexto)
        {
            _atualizandoValores = true;
            try
            {
                if (numPrecoCompra.Value > 0m)
                {
                    decimal venda = Math.Round(numPrecoCompra.Value * (1m + margem / 100m), 2);
                    numPrecoVenda.Value = Math.Max(numPrecoVenda.Minimum, Math.Min(numPrecoVenda.Maximum, venda));
                }
                if (formatarTexto) txtMargem.Text = $"{margem:N2}%";
            }
            finally { _atualizandoValores = false; }
        }

        private void CalcularMargem()
        {
            if (_atualizandoValores) return;
            _atualizandoValores = true;
            try
            {
                txtMargem.Text = numPrecoCompra.Value > 0m
                    ? $"{((numPrecoVenda.Value - numPrecoCompra.Value) / numPrecoCompra.Value) * 100m:N2}%"
                    : "0,00%";
            }
            finally { _atualizandoValores = false; }
        }

        private async void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text)) { MessageBox.Show("O nome do produto é obrigatório.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNome.Focus(); return; }
            if (string.IsNullOrWhiteSpace(cboUnidade.Text)) { MessageBox.Show("Selecione a unidade de medida.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning); cboUnidade.Focus(); return; }
            if (numPrecoVenda.Value <= 0m) { MessageBox.Show("Informe um preço de venda maior que zero.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning); numPrecoVenda.Focus(); return; }

            btnSalvar.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            bool ehEdicao = ProdutoIdEmEdicao.HasValue;
            string nome = txtNome.Text.Trim();
            string descricao = string.IsNullOrWhiteSpace(txtDescricao.Text) ? null : txtDescricao.Text.Trim();
            string localizacao = string.IsNullOrWhiteSpace(txtLocalizacao.Text) ? null : txtLocalizacao.Text.Trim();
            string fotoUrl = string.IsNullOrWhiteSpace(txtFotoUrl.Text) ? null : txtFotoUrl.Text.Trim();
            long? idCat = (cboCategoria.SelectedItem as ItemCombo)?.Id;
            long? idForn = (cboFornecedor.SelectedItem as ItemCombo)?.Id;

            try
            {
                var res = await Task.Run(async () =>
                {
                    using (var db = new Connection())
                    {
                        db.IniciarConexao();

                        string sql = ehEdicao
                            ? @"UPDATE product SET name=@nome, description=@desc, purchase_price=@compra, sale_price=@venda,
                                location=@loc, stock_quantity=@est, min_stock=@min, unit=@un, id_category=@cat, id_supplier=@forn,
                                is_active=@ativo, image_url=@foto, updated_at=NOW() WHERE id_product=@id;"
                            : @"INSERT INTO product (name, description, purchase_price, sale_price, location, stock_quantity,
                                min_stock, unit, id_category, id_supplier, is_active, image_url) VALUES (@nome, @desc, @compra, @venda,
                                @loc, @est, @min, @un, @cat, @forn, @ativo, @foto);";

                        using (var cmd = new MySqlCommand(sql, db.Conexao))
                        {
                            if (ehEdicao) cmd.Parameters.AddWithValue("@id", ProdutoIdEmEdicao.Value);
                            cmd.Parameters.AddWithValue("@nome", nome);
                            cmd.Parameters.AddWithValue("@desc", (object)descricao ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@compra", numPrecoCompra.Value);
                            cmd.Parameters.AddWithValue("@venda", numPrecoVenda.Value);
                            cmd.Parameters.AddWithValue("@loc", (object)localizacao ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@est", numEstoque.Value);
                            cmd.Parameters.AddWithValue("@min", (int)numEstoqueMinimo.Value);
                            cmd.Parameters.AddWithValue("@un", cboUnidade.Text.Trim());
                            cmd.Parameters.AddWithValue("@cat", (object)idCat ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@forn", (object)idForn ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@ativo", ObterStatusProduto());
                            cmd.Parameters.AddWithValue("@foto", (object)fotoUrl ?? DBNull.Value);

                            await cmd.ExecuteNonQueryAsync();
                            int id = ehEdicao ? ProdutoIdEmEdicao.Value : Convert.ToInt32(cmd.LastInsertedId);
                            using (var cmdData = new MySqlCommand("SELECT updated_at FROM product WHERE id_product = @id;", db.Conexao))
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

                AtualizarDataAtualizacao(res.Item2);

                if (_produtoRef != null)
                {
                    _produtoRef.Id = res.Item1;
                    _produtoRef.Nome = nome;
                    _produtoRef.Descricao = descricao ?? string.Empty;
                    _produtoRef.PrecoCompra = numPrecoCompra.Value;
                    _produtoRef.PrecoVenda = numPrecoVenda.Value;
                    _produtoRef.Localizacao = localizacao ?? string.Empty;
                    _produtoRef.Estoque = (int)numEstoque.Value;
                    _produtoRef.EstoqueMinimo = (int)numEstoqueMinimo.Value;
                    _produtoRef.Unidade = cboUnidade.Text.Trim();
                    _produtoRef.Categoria = cboCategoria.Text;
                    _produtoRef.Fornecedor = cboFornecedor.Text;
                    _produtoRef.Ativo = ObterStatusProduto();
                    _produtoRef.FotoUrl = fotoUrl;
                }

                string msg = ProdutoIdEmEdicao.HasValue
                    ? "Produto atualizado com sucesso!"
                    : $"Produto cadastrado com sucesso! (Código: {res.Item1})";

                AuditLogSQL.Register(
                    (ProdutoIdEmEdicao.HasValue ? "Produto atualizado: " : "Produto cadastrado: ") + nome + " (Código: " + res.Item1 + ")",
                    LoginSQL.SessaoUsuario.Nome);

                MessageBox.Show(msg, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (!ProdutoIdEmEdicao.HasValue)
                    await LimparCamposAsync();

                if (!ehEdicao) await LimparCamposAsync();
                AoConcluirOperacao?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar:\n{ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed) { btnSalvar.Enabled = true; Cursor.Current = Cursors.Default; }
            }
        }

        private async void btnLimpar_Click(object sender, EventArgs e) => await LimparCamposAsync();
        public async void LimparCampos() => await LimparCamposAsync();

        public async Task LimparCamposAsync()
        {
            SuspendLayout();
            _atualizandoValores = true;
            try
            {
                _produtoRef = null;
                ProdutoIdEmEdicao = null;
                _margemManual = false;

                txtCodigo.Text = "...";
                txtNome.Clear();
                txtDescricao.Clear();
                txtLocalizacao.Clear();
                txtFotoUrl.Clear();
                txtMargem.Text = "0,00%";

                numPrecoCompra.Value = numPrecoVenda.Value = numEstoque.Value = numEstoqueMinimo.Value = 0;
                if (cboUnidade.Items.Count > 0) cboUnidade.SelectedIndex = 0;
                cboCategoria.SelectedIndex = cboFornecedor.SelectedIndex = -1;

                DefinirStatusProduto(true);
                DefinirImagemPreview(Properties.Resources.produto_ausente);
                btnSalvar.Text = "Salvar";
                AtualizarDataAtualizacao(null);
            }
            finally { _atualizandoValores = false; }

            ResumeLayout();
            txtNome.Focus();
            await GerarProximoCodigoAsync();
        }

        private void btnCancelar_Click(object sender, EventArgs e) { FindForm()?.Close(); AoConcluirOperacao?.Invoke(); }
        private void btnExcluirFoto_Click(object sender, EventArgs e) { DefinirImagemPreview(Properties.Resources.produto_ausente); txtFotoUrl.Clear(); }

        private void btnEscolherArquivo_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog { Title = "Selecione a imagem", Filter = "Imagens (*.jpg;*.jpeg;*.png;*.webp)|*.jpg;*.jpeg;*.png;*.webp" })
            {
                if (ofd.ShowDialog() != DialogResult.OK) return;
                using (var ms = new MemoryStream(File.ReadAllBytes(ofd.FileName)))
                using (var img = Image.FromStream(ms))
                    DefinirImagemPreview(new Bitmap(img));

                txtFotoUrl.Text = ofd.FileName;
            }
        }

        private async void btnCarregarFoto_Click(object sender, EventArgs e) => await CarregarImagemPorUrlAsync(txtFotoUrl.Text.Trim(), true);

        private async void txtFotoUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; await CarregarImagemPorUrlAsync(txtFotoUrl.Text.Trim(), true); }
        }

        private async Task CarregarImagemPorUrlAsync(string url, bool exibirErros)
        {
            if (string.IsNullOrWhiteSpace(url)) return;

            try
            {
                if (!_imageBytesCache.TryGetValue(url, out byte[] bytes))
                {
                    bytes = File.Exists(url)
                        ? await Task.Run(() => File.ReadAllBytes(url))
                        : await _httpClient.GetByteArrayAsync(url);

                    if (bytes?.Length > 0) _imageBytesCache.TryAdd(url, bytes);
                }

                if (bytes != null && !IsDisposed)
                {
                    using (var ms = new MemoryStream(bytes))
                    using (var img = Image.FromStream(ms))
                        DefinirImagemPreview(new Bitmap(img));
                }
            }
            catch (Exception ex)
            {
                if (!IsDisposed)
                {
                    DefinirImagemPreview(Properties.Resources.produto_ausente);
                    if (exibirErros) MessageBox.Show($"Falha ao carregar imagem:\n{ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DefinirImagemPreview(Image nova)
        {
            if (picFotoPreview.Image != null && !ReferenceEquals(picFotoPreview.Image, Properties.Resources.produto_ausente))
                picFotoPreview.Image.Dispose();
            picFotoPreview.Image = nova;
        }
    }

    public static class PromptDialog
    {
        public static string Mostrar(string texto, string titulo, int maxLength = 150)
        {
            using (var form = new Form { ClientSize = new Size(370, 130), FormBorderStyle = FormBorderStyle.FixedDialog, Text = titulo, StartPosition = FormStartPosition.CenterParent, MaximizeBox = false, MinimizeBox = false })
            {
                var lbl = new Label { Left = 16, Top = 16, Width = 338, Text = texto };
                var txt = new TextBox { Left = 16, Top = 40, Width = 338, MaxLength = maxLength, Tag = "obrigatorio" };
                var btnOk = new Button { Text = "Salvar", Left = 180, Width = 84, Top = 82, Tag = "sucesso", DialogResult = DialogResult.OK };
                var btnCancel = new Button { Text = "Cancelar", Left = 270, Width = 84, Top = 82, DialogResult = DialogResult.Cancel };

                form.Controls.AddRange(new Control[] { lbl, txt, btnOk, btnCancel });
                form.AcceptButton = btnOk;
                form.CancelButton = btnCancel;
                GerenciadorTema.AplicarTema(form);

                return form.ShowDialog() == DialogResult.OK ? txt.Text : string.Empty;
            }
        }

        public static (bool Confirmado, string Nome, string Documento, string Telefone) MostrarFornecedor()
        {
            using (var form = new Form { ClientSize = new Size(390, 235), FormBorderStyle = FormBorderStyle.FixedDialog, Text = "Novo Fornecedor", StartPosition = FormStartPosition.CenterParent, MaximizeBox = false, MinimizeBox = false })
            {
                var lblNome = new Label { Left = 16, Top = 16, Width = 358, Text = "Nome / Razão Social *:" };
                var txtNome = new TextBox { Left = 16, Top = 38, Width = 358, MaxLength = 150, Tag = "obrigatorio" };
                var lblDoc = new Label { Left = 16, Top = 72, Width = 358, Text = "Documento (CPF / CNPJ):" };
                var txtDoc = new TextBox { Left = 16, Top = 94, Width = 358, MaxLength = 18 };
                var lblTel = new Label { Left = 16, Top = 128, Width = 358, Text = "Telefone:" };
                var txtTel = new TextBox { Left = 16, Top = 150, Width = 358, MaxLength = 15 };
                var btnOk = new Button { Text = "Salvar", Left = 200, Width = 84, Top = 190, Tag = "sucesso" };
                var btnCancel = new Button { Text = "Cancelar", Left = 290, Width = 84, Top = 190, DialogResult = DialogResult.Cancel };

                AplicarMascara(txtDoc, 14, FormatarDocumento);
                AplicarMascara(txtTel, 11, FormatarTelefone);

                btnOk.Click += (s, e) =>
                {
                    string doc = Digitos(txtDoc.Text), tel = Digitos(txtTel.Text);
                    if (string.IsNullOrWhiteSpace(txtNome.Text)) { MessageBox.Show("O nome do fornecedor é obrigatório.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNome.Focus(); return; }
                    if (doc.Length > 0 && doc.Length != 11 && doc.Length != 14) { MessageBox.Show("CPF ou CNPJ inválido.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtDoc.Focus(); return; }
                    if (tel.Length > 0 && (tel.Length < 10 || tel.Length > 11)) { MessageBox.Show("Telefone inválido.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtTel.Focus(); return; }
                    form.DialogResult = DialogResult.OK;
                    form.Close();
                };

                form.Controls.AddRange(new Control[] { lblNome, txtNome, lblDoc, txtDoc, lblTel, txtTel, btnOk, btnCancel });
                form.AcceptButton = btnOk;
                form.CancelButton = btnCancel;
                GerenciadorTema.AplicarTema(form);

                bool ok = form.ShowDialog() == DialogResult.OK;
                return (ok, txtNome.Text, Digitos(txtDoc.Text), Digitos(txtTel.Text));
            }
        }

        private static string Digitos(string s) => string.IsNullOrEmpty(s) ? string.Empty : new string(s.Where(char.IsDigit).ToArray());

        private static void AplicarMascara(TextBox txt, int max, Func<string, string> fmt)
        {
            bool travado = false;
            txt.KeyPress += (s, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true; };
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
    }
}