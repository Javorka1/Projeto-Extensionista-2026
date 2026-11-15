using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjetoExtensionista2026_2.SQL.SQL_functions;

namespace ProjetoExtensionista2026_2
{
    public partial class FormRegistroVendas : Form
    {
        private DataTable _dtVendasAtual;
        private long? _idVendaSelecionada;
        private string _tipoSelecionado = string.Empty;
        private string _statusSelecionado = string.Empty;

        private static bool PodeCancelarRegistro
        {
            get
            {
                string role = (LoginSQL.SessaoUsuario.Role ?? string.Empty).Trim().ToLowerInvariant();
                return role == "admin" || role == "monetary";
            }
        }

        private readonly DTOs.ClienteDTO _clienteFiltro;

        public FormRegistroVendas()
        {
            InitializeComponent();
        }

        public FormRegistroVendas(DTOs.ClienteDTO clienteFiltro) : this()
        {
            _clienteFiltro = clienteFiltro;
        }

        private async void FormRegistroVendas_Load(object sender, EventArgs e)
        {
            if (_clienteFiltro != null)
            {
                dtpInicio.Value = DateTime.Today.AddYears(-2);
                dtpFim.Value = DateTime.Today;
                txtBusca.Text = _clienteFiltro.Nome;
            }

            cmbTipo.SelectedIndex = 0;
            cmbStatus.SelectedIndex = 0;

            dtpInicio.ValueChanged += (s, ev) => lblDtpInicio.Text = dtpInicio.Value.ToString("dd/MM/yyyy");
            dtpFim.ValueChanged += (s, ev) => lblDtpFim.Text = dtpFim.Value.ToString("dd/MM/yyyy");
            lblDtpInicio.Click += (s, ev) => { dtpInicio.Focus(); SendKeys.Send("{F4}"); };
            lblDtpFim.Click += (s, ev) => { dtpFim.Focus(); SendKeys.Send("{F4}"); };

            dtpFim.Value = DateTime.Today;
            dtpInicio.Value = DateTime.Today.AddDays(-30);
            lblDtpInicio.Text = dtpInicio.Value.ToString("dd/MM/yyyy");
            lblDtpFim.Text = dtpFim.Value.ToString("dd/MM/yyyy");

            btnCancelarVenda.Visible = PodeCancelarRegistro;
            GerenciadorTema.AplicarTema(this);
            AjustarProporcaoSplit();
            AtualizarBotoesAcao();
            await CarregarVendasAsync();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AjustarProporcaoSplit();
        }

        private void AjustarProporcaoSplit()
        {
            if (splitPrincipal != null && splitPrincipal.Width > 500)
            {
                int larguraDetalhes = Math.Max(340, Math.Min(430, (int)(splitPrincipal.Width * 0.34)));
                splitPrincipal.SplitterDistance = Math.Max(300, splitPrincipal.Width - larguraDetalhes);
            }
        }

        private async Task CarregarVendasAsync()
        {
            try
            {
                btnAtualizar.Enabled = false;
                btnFiltrar.Enabled = false;
                btnAtualizar.Text = "Carregando...";

                DateTime inicio = dtpInicio.Value.Date;
                DateTime fim = dtpFim.Value.Date;
                string tipo = cmbTipo.SelectedItem?.ToString() ?? "Todos";
                string status = cmbStatus.SelectedItem?.ToString() ?? "Todos";
                string busca = txtBusca.Text.Trim();

                DataTable dt = await Task.Run(() => VendaSQL.ListarVendas(inicio, fim, tipo, status, busca));
                _dtVendasAtual = dt;

                dgvVendas.DataSource = _dtVendasAtual;
                FormatarGridVendas();
                AtualizarKpis(_dtVendasAtual);

                if (dgvVendas.Rows.Count == 0)
                {
                    LimparDetalhes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar o registro de vendas:\n\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnAtualizar.Text = "Atualizar";
                btnAtualizar.Enabled = true;
                btnFiltrar.Enabled = true;
            }
        }

        private void FormatarGridVendas()
        {
            if (dgvVendas.Columns.Count == 0) return;

            dgvVendas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvVendas.Columns.Contains("ID"))
            {
                dgvVendas.Columns["ID"].FillWeight = 45;
                dgvVendas.Columns["ID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvVendas.Columns.Contains("Data/Hora"))
            {
                dgvVendas.Columns["Data/Hora"].FillWeight = 95;
                dgvVendas.Columns["Data/Hora"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }
            if (dgvVendas.Columns.Contains("Cliente"))
                dgvVendas.Columns["Cliente"].FillWeight = 135;
            if (dgvVendas.Columns.Contains("Vendedor"))
                dgvVendas.Columns["Vendedor"].FillWeight = 85;
            if (dgvVendas.Columns.Contains("Tipo"))
                dgvVendas.Columns["Tipo"].FillWeight = 70;
            if (dgvVendas.Columns.Contains("Pagamento"))
                dgvVendas.Columns["Pagamento"].FillWeight = 110;
            if (dgvVendas.Columns.Contains("Status"))
                dgvVendas.Columns["Status"].FillWeight = 70;

            if (dgvVendas.Columns.Contains("Desconto (R$)"))
            {
                dgvVendas.Columns["Desconto (R$)"].FillWeight = 95;
                dgvVendas.Columns["Desconto (R$)"].DefaultCellStyle.Format = "N2";
                dgvVendas.Columns["Desconto (R$)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dgvVendas.Columns.Contains("Total (R$)"))
            {
                dgvVendas.Columns["Total (R$)"].FillWeight = 90;
                dgvVendas.Columns["Total (R$)"].DefaultCellStyle.Format = "C2";
                dgvVendas.Columns["Total (R$)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvVendas.Columns["Total (R$)"].DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            }

            foreach (DataGridViewRow row in dgvVendas.Rows)
            {
                string st = (row.Cells["Status"]?.Value?.ToString() ?? "").ToUpperInvariant();
                if (st == "PAGO")
                    row.Cells["Status"].Style.ForeColor = GerenciadorTema.CorSucesso;
                else if (st == "PENDENTE")
                    row.Cells["Status"].Style.ForeColor = GerenciadorTema.CorAviso;
                else if (st == "CANCELADA")
                    row.Cells["Status"].Style.ForeColor = GerenciadorTema.CorPerigo;
            }
        }

        private void AtualizarKpis(DataTable dt)
        {
            decimal totalVendido = 0m;
            int qtdVendas = 0;
            decimal totalPendente = 0m;
            int qtdOrcamentos = 0;
            decimal totalOrcamentos = 0m;

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string tipo = (row["Tipo"]?.ToString() ?? "").ToUpperInvariant();
                    string status = (row["Status"]?.ToString() ?? "").ToUpperInvariant();
                    decimal total = row["Total (R$)"] != DBNull.Value ? Convert.ToDecimal(row["Total (R$)"]) : 0m;

                    if (status == "CANCELADA") continue;

                    if (tipo == "ORCAMENTO")
                    {
                        qtdOrcamentos++;
                        totalOrcamentos += total;
                    }
                    else
                    {
                        qtdVendas++;
                        totalVendido += total;
                        if (status == "PENDENTE")
                            totalPendente += total;
                    }
                }
            }

            decimal ticketMedio = qtdVendas > 0 ? totalVendido / qtdVendas : 0m;

            lblValorTotalVendido.Text = totalVendido.ToString("C2");
            lblValorTicketMedio.Text = ticketMedio.ToString("C2");
            lblSubTicketMedio.Text = $"{qtdVendas} venda(s) no período";
            lblValorPendente.Text = totalPendente.ToString("C2");
            lblValorOrcamentos.Text = qtdOrcamentos.ToString();
            lblSubOrcamentos.Text = $"Total orçado: {totalOrcamentos:C2}";
        }

        private async void dgvVendas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvVendas.CurrentRow == null || dgvVendas.CurrentRow.IsNewRow)
                return;

            object valorId = dgvVendas.CurrentRow.Cells["ID"]?.Value;
            if (valorId == null || valorId == DBNull.Value)
                return;

            long idSale = Convert.ToInt64(valorId);
            if (_idVendaSelecionada == idSale)
                return;

            _idVendaSelecionada = idSale;
            _tipoSelecionado = (dgvVendas.CurrentRow.Cells["Tipo"]?.Value?.ToString() ?? "").ToUpperInvariant();
            _statusSelecionado = (dgvVendas.CurrentRow.Cells["Status"]?.Value?.ToString() ?? "").ToUpperInvariant();
            AtualizarBotoesAcao();
            await CarregarDetalhesAsync(idSale);
        }

        private async Task CarregarDetalhesAsync(long idSale)
        {
            try
            {
                DetalhesVendaResult detalhes = await Task.Run(() => VendaSQL.ObterDetalhesVenda(idSale));

                // Garante que a seleção não mudou enquanto aguardava o banco
                if (_idVendaSelecionada != idSale) return;

                lblDetalheTitulo.Text = $"{(detalhes.Tipo == "ORCAMENTO" ? "Orçamento" : "Venda")} #{detalhes.IdVenda}  •  {detalhes.Total:C2}";
                lblDetalheMeta.Text = $"Cliente: {detalhes.ClienteNome}  |  Vendedor: {detalhes.Vendedor}";
                lblDetalheNotas.Text = string.IsNullOrWhiteSpace(detalhes.Observacoes)
                    ? $"Emitido em {detalhes.DataVenda:dd/MM/yyyy HH:mm} — Status: {detalhes.Status}"
                    : $"Obs.: {detalhes.Observacoes}";

                dgvItens.DataSource = detalhes.Itens;
                dgvItens.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                if (dgvItens.Columns.Contains("Cód.")) dgvItens.Columns["Cód."].FillWeight = 48;
                if (dgvItens.Columns.Contains("Produto")) dgvItens.Columns["Produto"].FillWeight = 120;
                if (dgvItens.Columns.Contains("Qtd."))
                {
                    dgvItens.Columns["Qtd."].FillWeight = 55;
                    dgvItens.Columns["Qtd."].DefaultCellStyle.Format = "N0";
                }
                if (dgvItens.Columns.Contains("Unit. (R$)"))
                {
                    dgvItens.Columns["Unit. (R$)"].FillWeight = 68;
                    dgvItens.Columns["Unit. (R$)"].DefaultCellStyle.Format = "N2";
                }
                if (dgvItens.Columns.Contains("Subtotal (R$)"))
                {
                    dgvItens.Columns["Subtotal (R$)"].FillWeight = 78;
                    dgvItens.Columns["Subtotal (R$)"].DefaultCellStyle.Format = "C2";
                }

                dgvPagamentos.DataSource = detalhes.Pagamentos;
                dgvPagamentos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                if (dgvPagamentos.Columns.Contains("Valor (R$)"))
                    dgvPagamentos.Columns["Valor (R$)"].DefaultCellStyle.Format = "C2";
                if (dgvPagamentos.Columns.Contains("Parcelas"))
                    dgvPagamentos.Columns["Parcelas"].FillWeight = 55;
            }
            catch (Exception ex)
            {
                lblDetalheNotas.Text = "Erro ao carregar detalhes: " + ex.Message;
            }
        }

        private void LimparDetalhes()
        {
            _idVendaSelecionada = null;
            _tipoSelecionado = string.Empty;
            _statusSelecionado = string.Empty;
            lblDetalheTitulo.Text = "Nenhuma venda selecionada";
            lblDetalheMeta.Text = "Selecione um registro na tabela ao lado";
            lblDetalheNotas.Text = "";
            dgvItens.DataSource = null;
            dgvPagamentos.DataSource = null;
            AtualizarBotoesAcao();
        }

        private void AtualizarBotoesAcao()
        {
            bool temSelecao = _idVendaSelecionada.HasValue && _idVendaSelecionada.Value > 0;
            bool cancelada = _statusSelecionado == "CANCELADA";
            bool ehOrcamento = _tipoSelecionado == "ORCAMENTO";
            bool pendente = _statusSelecionado == "PENDENTE";

            if (btnReimprimir != null)
                btnReimprimir.Enabled = temSelecao && !cancelada;

            if (btnEfetivar != null)
                btnEfetivar.Enabled = temSelecao && ehOrcamento && !cancelada;

            if (btnQuitar != null)
                btnQuitar.Enabled = temSelecao && !ehOrcamento && pendente;

            if (btnCancelarVenda != null)
                btnCancelarVenda.Enabled = temSelecao && !cancelada && PodeCancelarRegistro;
        }

        private async void btnReimprimir_Click(object sender, EventArgs e)
        {
            if (!_idVendaSelecionada.HasValue) return;
            long idSale = _idVendaSelecionada.Value;

            try
            {
                Cursor = Cursors.WaitCursor;
                DadosDocumentoVenda dados = await Task.Run(() => VendaSQL.ObterDadosDocumento(idSale));
                Cursor = Cursors.Default;

                bool ehAPrazo = dados.Pagamentos.Any(p => p.EhAPrazo);
                using (var modalDocs = new formDocumentosFiscais(dados, ehAPrazo))
                {
                    if (modalDocs.ShowDialog(this) == DialogResult.OK)
                    {
                        await Task.Run(() => VendaSQL.AtualizarFlagsDocumentos(
                            idSale,
                            modalDocs.EmitirCupomFiscal,
                            modalDocs.EmitirNotaPromissoria));
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Erro ao preparar os documentos para reimpressão:\n\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnEfetivar_Click(object sender, EventArgs e)
        {
            if (!_idVendaSelecionada.HasValue || _tipoSelecionado != "ORCAMENTO") return;
            long idOrcamento = _idVendaSelecionada.Value;

            try
            {
                Cursor = Cursors.WaitCursor;
                OrcamentoParaEfetivar orc = await Task.Run(() => VendaSQL.ObterOrcamentoParaEfetivar(idOrcamento));
                Cursor = Cursors.Default;

                if (orc.Itens == null || orc.Itens.Count == 0)
                {
                    MessageBox.Show("Este orçamento não possui itens para efetivar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int descPct = orc.Subtotal > 0m ? (int)Math.Round((orc.Desconto / orc.Subtotal) * 100m) : 0;

                using (var formPagto = new formPagamento(
                    orc.Subtotal,
                    orc.Desconto,
                    descPct,
                    orc.Total,
                    orc.Itens,
                    orc.IdCliente,
                    idOrcamento))
                {
                    if (formPagto.ShowDialog(this) == DialogResult.OK)
                    {
                        _idVendaSelecionada = null;
                        await CarregarVendasAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Erro ao efetivar orçamento:\n\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnQuitar_Click(object sender, EventArgs e)
        {
            if (!_idVendaSelecionada.HasValue || _tipoSelecionado == "ORCAMENTO" || _statusSelecionado != "PENDENTE")
                return;

            long idSale = _idVendaSelecionada.Value;
            var resp = MessageBox.Show(
                $"Confirmar o recebimento/quitação total da venda #{idSale}?",
                "Quitar Pendência",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resp != DialogResult.Yes) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                string usuario = LoginSQL.SessaoUsuario.Nome;
                bool ok = await Task.Run(() => VendaSQL.QuitarPendencia(idSale, usuario));
                Cursor = Cursors.Default;

                if (ok)
                {
                    MessageBox.Show($"Venda #{idSale} marcada como PAGO.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _idVendaSelecionada = null;
                    await CarregarVendasAsync();
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Erro ao quitar pendência:\n\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnCancelarVenda_Click(object sender, EventArgs e)
        {
            if (!_idVendaSelecionada.HasValue || !PodeCancelarRegistro || _statusSelecionado == "CANCELADA")
                return;

            long idSale = _idVendaSelecionada.Value;
            string avisoEstoque = _tipoSelecionado == "ORCAMENTO"
                ? "O orçamento será marcado como CANCELADO."
                : "A venda será marcada como CANCELADA e os itens serão estornados ao estoque.";

            var resp = MessageBox.Show(
                $"Deseja realmente cancelar o registro #{idSale}?\n\n{avisoEstoque}",
                "Confirmar Cancelamento",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (resp != DialogResult.Yes) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                string usuario = LoginSQL.SessaoUsuario.Nome;
                bool ok = await Task.Run(() => VendaSQL.CancelarRegistro(idSale, usuario));
                Cursor = Cursors.Default;

                if (ok)
                {
                    MessageBox.Show($"Registro #{idSale} cancelado com sucesso.", "Cancelado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _idVendaSelecionada = null;
                    await CarregarVendasAsync();
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Erro ao cancelar registro:\n\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAtualizar_Click(object sender, EventArgs e)
        {
            _idVendaSelecionada = null;
            await CarregarVendasAsync();
        }

        private async void btnFiltrar_Click(object sender, EventArgs e)
        {
            _idVendaSelecionada = null;
            await CarregarVendasAsync();
        }

        private async void txtBusca_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                _idVendaSelecionada = null;
                await CarregarVendasAsync();
            }
        }

        private async void btnHoje_Click(object sender, EventArgs e)
        {
            dtpInicio.Value = DateTime.Today;
            dtpFim.Value = DateTime.Today;
            _idVendaSelecionada = null;
            await CarregarVendasAsync();
        }

        private async void btn7Dias_Click(object sender, EventArgs e)
        {
            dtpFim.Value = DateTime.Today;
            dtpInicio.Value = DateTime.Today.AddDays(-7);
            _idVendaSelecionada = null;
            await CarregarVendasAsync();
        }

        private async void btn30Dias_Click(object sender, EventArgs e)
        {
            dtpFim.Value = DateTime.Today;
            dtpInicio.Value = DateTime.Today.AddDays(-30);
            _idVendaSelecionada = null;
            await CarregarVendasAsync();
        }

        private void btnExportarCsv_Click(object sender, EventArgs e)
        {
            ExportadorCsv.ExportarDataGridView(
                dgvVendas,
                $"Registro_Vendas_{DateTime.Now:yyyy-MM-dd_HHmm}.csv",
                "Exportar Registro de Vendas");
        }
    }
}
