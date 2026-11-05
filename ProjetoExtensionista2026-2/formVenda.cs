using ProjetoExtensionista2026_2.DTOs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public partial class formVenda : Form
    {
        private UcEstoque _ucEstoque;

        // Controle numérico dos descontos
        private long _descontoCentavos = 0;
        private int _descontoPorcentagem = 0;
        private bool _atualizandoDesconto = false;
        private decimal DescontoAtual => _descontoCentavos / 100m;

        // Cliente ativo na venda
        private ClienteDTO _clienteSelecionado;
        public long IdClienteSelecionado => _clienteSelecionado?.Id ?? 1;

        public formVenda()
        {
            InitializeComponent();

            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();

            KeyPreview = true;

            FormBorderStyle = FormBorderStyle.Sizable;
            WindowState = FormWindowState.Maximized;
        }

        private void formVenda_Load(object sender, EventArgs e)
        {
            _ucEstoque = new UcEstoque
            {
                Dock = DockStyle.Fill
            };

            _ucEstoque.ConfigurarPerfilVendedor(true);
            _ucEstoque.AoAdicionarAoCarrinho += AdicionarOuIncrementarCarrinho;

            panelConteudo.Controls.Clear();
            panelConteudo.Controls.Add(_ucEstoque);
            _ucEstoque.BringToFront();

            _ucEstoque.BuscarProdutosNoSupabase(string.Empty);

            if (panelCarrinho != null)
                panelCarrinho.AutoScroll = true;

            AjustarLarguraCarrinho();
            ConfigurarCamposDesconto();
            DefinirClientePadrao();
            AtualizarTotaisCarrinho();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AjustarLarguraCarrinho();
        }

        private void AjustarLarguraCarrinho()
        {
            if (splitContainer_catalogo != null && splitContainer_catalogo.Width > 650)
            {
                const int larguraDesejada = 455;
                splitContainer_catalogo.SplitterDistance = Math.Max(250, splitContainer_catalogo.Width - larguraDesejada);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F2:
                    btnConsultarCliente.PerformClick();
                    return true;

                case Keys.F3:
                    _ucEstoque?.FocarCampoBusca();
                    return true;

                case Keys.F4:
                    btnLimparCarrinho.PerformClick();
                    return true;

                case Keys.F12:
                    btnAvancarPagamento.PerformClick();
                    return true;

                case Keys.Delete:
                    if (!(ActiveControl is TextBox))
                    {
                        var ultimoCard = panelCarrinho.Controls.OfType<UcCardVenda>().FirstOrDefault();
                        if (ultimoCard != null)
                        {
                            panelCarrinho.Controls.Remove(ultimoCard);
                            ultimoCard.Dispose();
                            AtualizarTotaisCarrinho();
                            return true;
                        }
                    }
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnConsultarCliente_Click(object sender, EventArgs e)
        {
            using (var formConsulta = new formConsultaCliente())
            {
                if (formConsulta.ShowDialog(this) == DialogResult.OK && formConsulta.ClienteSelecionado != null)
                {
                    SelecionarCliente(formConsulta.ClienteSelecionado);
                }
            }
        }

        private void btnLimparCliente_Click(object sender, EventArgs e)
        {
            DefinirClientePadrao();
        }

        private void DefinirClientePadrao()
        {
            SelecionarCliente(new ClienteDTO
            {
                Id = 1,
                Nome = "Cliente não informado",
                Apelido = "Cliente",
                CpfCnpj = string.Empty,
                Telefone = string.Empty,
                LimiteCredito = 0m,
                Ativo = true
            });
        }

        private void SelecionarCliente(ClienteDTO cliente)
        {
            if (cliente == null) return;

            _clienteSelecionado = cliente;
            AtualizarExibicaoCliente();
        }

        private void AtualizarExibicaoCliente()
        {
            if (_clienteSelecionado == null) return;

            if (lblNomeCliente != null)
                lblNomeCliente.Text = _clienteSelecionado.Nome;

            if (lblDocumentoCliente != null)
                lblDocumentoCliente.Text = FormatarCpfCnpj(_clienteSelecionado.CpfCnpj);

            if (lblTelefoneCliente != null)
                lblTelefoneCliente.Text = FormatarTelefone(_clienteSelecionado.Telefone);

            if (lblEnderecoCliente != null)
                lblEnderecoCliente.Text = FormatarEndereco(_clienteSelecionado);

            if (lblStatusCliente != null)
            {
                lblStatusCliente.Text = _clienteSelecionado.Ativo ? "Status: Ativo" : "Status: Inativo";
                lblStatusCliente.ForeColor = _clienteSelecionado.Ativo
                    ? Color.FromArgb(40, 167, 69)
                    : Color.FromArgb(220, 53, 69);
            }

            if (lblLimiteCliente != null)
                lblLimiteCliente.Text = $"Limite de Crédito: {_clienteSelecionado.LimiteCredito:C2}";
        }

        private static string FormatarEndereco(ClienteDTO c)
        {
            if (c == null) return "Endereço: Não informado";

            var partes = new List<string>();

            if (!string.IsNullOrWhiteSpace(c.Rua))
                partes.Add(c.Rua.Trim());

            if (!string.IsNullOrWhiteSpace(c.Numero))
                partes.Add(c.Numero.Trim());

            if (!string.IsNullOrWhiteSpace(c.Complemento))
                partes.Add(c.Complemento.Trim());

            if (!string.IsNullOrWhiteSpace(c.Bairro))
                partes.Add(c.Bairro.Trim());

            string cidade = c.Cidade?.Trim();
            string uf = c.Uf?.Trim();

            if (!string.IsNullOrWhiteSpace(cidade) && !string.IsNullOrWhiteSpace(uf))
                partes.Add($"{cidade} - {uf}");
            else if (!string.IsNullOrWhiteSpace(cidade))
                partes.Add(cidade);
            else if (!string.IsNullOrWhiteSpace(uf))
                partes.Add(uf);

            if (!string.IsNullOrWhiteSpace(c.Cep))
                partes.Add($"CEP: {FormatarCep(c.Cep)}");

            return partes.Count > 0 ? string.Join(", ", partes) : "Endereço: Não informado";
        }

        private static string FormatarCep(string cep)
        {
            string digitos = ExtrairDigitos(cep);
            return digitos.Length == 8
                ? $"{digitos.Substring(0, 5)}-{digitos.Substring(5, 3)}"
                : cep.Trim();
        }

        private static string FormatarCpfCnpj(string doc)
        {
            if (string.IsNullOrWhiteSpace(doc)) return "CPF/CNPJ: Não informado";

            string digitos = ExtrairDigitos(doc);
            if (digitos.Length == 11)
                return Convert.ToUInt64(digitos).ToString(@"CPF: 000\.000\.000\-00");
            if (digitos.Length == 14)
                return Convert.ToUInt64(digitos).ToString(@"CNPJ: 00\.000\.000\/0000\-00");

            return doc.Trim();
        }

        private static string FormatarTelefone(string fone)
        {
            if (string.IsNullOrWhiteSpace(fone)) return "Telefone: Não informado";

            string digitos = ExtrairDigitos(fone);
            if (digitos.Length == 11)
                return Convert.ToUInt64(digitos).ToString(@"Telefone: \(00\) 00000\-0000");
            if (digitos.Length == 10)
                return Convert.ToUInt64(digitos).ToString(@"Telefone: \(00\) 0000\-0000");

            return fone.Trim();
        }

        private static string ExtrairDigitos(string texto)
        {
            return string.IsNullOrEmpty(texto) ? string.Empty : Regex.Replace(texto, @"\D", string.Empty);
        }

        private void AdicionarOuIncrementarCarrinho(int idProduto, string nomeProduto, decimal precoVenda, int quantidade, string fotoUrl, int estoqueDisponivel)
        {
            if (panelCarrinho == null) return;

            if (estoqueDisponivel <= 0)
            {
                MessageBox.Show(
                    $"O produto '{nomeProduto}' está com o estoque esgotado!",
                    "Estoque Indisponível",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            var cardExistente = panelCarrinho.Controls
                .OfType<UcCardVenda>()
                .FirstOrDefault(c => c.ProdutoId == idProduto);

            if (cardExistente != null)
            {
                int limiteEstoque = cardExistente.EstoqueMaximo;
                int qtdAtual = cardExistente.Quantidade;

                if (qtdAtual >= limiteEstoque)
                {
                    MessageBox.Show(
                        $"Limite atingido! O carrinho já possui todas as {limiteEstoque} unidade(s) disponíveis no estoque para '{cardExistente.NomeProduto}'.",
                        "Estoque Máximo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                if (qtdAtual + quantidade > limiteEstoque)
                {
                    int restantePermitido = limiteEstoque - qtdAtual;
                    MessageBox.Show(
                        $"Não é possível adicionar mais {quantidade} unidade(s). Você já tem {qtdAtual} no carrinho e restam apenas {restantePermitido} no estoque.",
                        "Estoque Insuficiente",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                cardExistente.CarregarItem(
                    idProduto,
                    cardExistente.NomeProduto,
                    precoVenda,
                    qtdAtual + quantidade,
                    limiteEstoque,
                    cardExistente.FotoOrigem ?? fotoUrl
                );

                AtualizarTotaisCarrinho();
                return;
            }

            if (quantidade > estoqueDisponivel)
            {
                MessageBox.Show(
                    $"Não é possível adicionar {quantidade} unidades de '{nomeProduto}'. O estoque atual possui apenas {estoqueDisponivel} unidade(s).",
                    "Estoque Insuficiente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            panelCarrinho.SuspendLayout();
            try
            {
                var novoItem = new UcCardVenda
                {
                    Dock = DockStyle.Top,
                    Margin = new Padding(0, 0, 0, 4)
                };

                novoItem.CarregarItem(idProduto, nomeProduto, precoVenda, quantidade, estoqueDisponivel, fotoUrl);
                novoItem.AoAlterarQuantidade += (id, novaQtd, novoTotalItem) => AtualizarTotaisCarrinho();
                novoItem.AoRemoverItem += (id, cardRemover) =>
                {
                    panelCarrinho.Controls.Remove(cardRemover);
                    cardRemover.Dispose();
                    AtualizarTotaisCarrinho();
                };

                panelCarrinho.Controls.Add(novoItem);
                novoItem.BringToFront();
            }
            finally
            {
                panelCarrinho.ResumeLayout();
            }

            AtualizarTotaisCarrinho();
        }

        private (decimal Subtotal, int TotalItens) CalcularMetricasCarrinho()
        {
            decimal subtotal = 0m;
            int totalItens = 0;

            if (panelCarrinho != null)
            {
                foreach (UcCardVenda item in panelCarrinho.Controls.OfType<UcCardVenda>())
                {
                    subtotal += item.PrecoTotal;
                    totalItens += item.Quantidade;
                }
            }

            return (subtotal, totalItens);
        }

        private decimal ObterSubtotalAtual() => CalcularMetricasCarrinho().Subtotal;

        private void AtualizarTotaisCarrinho()
        {
            var (subtotal, totalItens) = CalcularMetricasCarrinho();

            if (DescontoAtual > subtotal)
            {
                _descontoCentavos = (long)Math.Round(subtotal * 100m);
                SincronizarCamposDesconto(subtotal, originadoPorValor: true);
            }

            decimal totalFinal = Math.Max(0m, subtotal - DescontoAtual);

            if (lblSubtotal != null)
                lblSubtotal.Text = subtotal.ToString("C2");

            if (lblTotal != null)
                lblTotal.Text = totalFinal.ToString("C2");

            if (lblQtdItens != null)
                lblQtdItens.Text = $"Itens do Pedido: {totalItens} item(ns)";
        }

        private void ConfigurarCamposDesconto()
        {
            if (txtDesconto != null)
            {
                txtDesconto.ShortcutsEnabled = false;
                txtDesconto.Text = "- R$ 0,00";
                txtDesconto.KeyPress += txtDesconto_KeyPress;
                txtDesconto.KeyDown += txtDesconto_KeyDown;
                txtDesconto.MouseDown += (s, ev) => txtDesconto.Select(txtDesconto.Text.Length, 0);
                txtDesconto.GotFocus += (s, ev) => txtDesconto.Select(txtDesconto.Text.Length, 0);
            }

            if (txtDescontoPorcentagem != null)
            {
                txtDescontoPorcentagem.ShortcutsEnabled = false;
                txtDescontoPorcentagem.Text = "0 %";
                txtDescontoPorcentagem.KeyPress += txtDescontoPorcentagem_KeyPress;
                txtDescontoPorcentagem.KeyDown += txtDescontoPorcentagem_KeyDown;
                txtDescontoPorcentagem.MouseDown += (s, ev) => txtDescontoPorcentagem.Select(txtDescontoPorcentagem.Text.Length, 0);
                txtDescontoPorcentagem.GotFocus += (s, ev) => txtDescontoPorcentagem.Select(txtDescontoPorcentagem.Text.Length, 0);
            }
        }

        private void txtDesconto_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            if (!char.IsDigit(e.KeyChar)) return;

            int digito = e.KeyChar - '0';
            long proximoCentavos = (_descontoCentavos * 10) + digito;
            decimal subtotal = ObterSubtotalAtual();
            long subtotalCentavos = (long)Math.Round(subtotal * 100m);

            _descontoCentavos = Math.Min(proximoCentavos, subtotalCentavos);
            SincronizarCamposDesconto(subtotal, originadoPorValor: true);
            AtualizarTotaisCarrinho();
        }

        private void txtDesconto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                e.SuppressKeyPress = true;
                _descontoCentavos /= 10;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                e.SuppressKeyPress = true;
                _descontoCentavos = 0;
            }
            else return;

            SincronizarCamposDesconto(ObterSubtotalAtual(), originadoPorValor: true);
            AtualizarTotaisCarrinho();
        }

        private void txtDescontoPorcentagem_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            if (!char.IsDigit(e.KeyChar)) return;

            int digito = e.KeyChar - '0';
            _descontoPorcentagem = Math.Min(100, (_descontoPorcentagem * 10) + digito);

            SincronizarCamposDesconto(ObterSubtotalAtual(), originadoPorValor: false);
            AtualizarTotaisCarrinho();
        }

        private void txtDescontoPorcentagem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                e.SuppressKeyPress = true;
                _descontoPorcentagem /= 10;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                e.SuppressKeyPress = true;
                _descontoPorcentagem = 0;
            }
            else return;

            SincronizarCamposDesconto(ObterSubtotalAtual(), originadoPorValor: false);
            AtualizarTotaisCarrinho();
        }

        private void SincronizarCamposDesconto(decimal subtotal, bool originadoPorValor)
        {
            if (_atualizandoDesconto) return;
            _atualizandoDesconto = true;

            try
            {
                if (originadoPorValor)
                {
                    if (DescontoAtual > subtotal)
                        _descontoCentavos = (long)Math.Round(subtotal * 100m);

                    int pctCalculada = subtotal > 0m ? (int)Math.Round((DescontoAtual / subtotal) * 100m) : 0;
                    _descontoPorcentagem = Math.Min(100, Math.Max(0, pctCalculada));
                }
                else
                {
                    _descontoPorcentagem = Math.Min(100, Math.Max(0, _descontoPorcentagem));
                    decimal valorCalculado = Math.Min(subtotal, Math.Round(subtotal * (_descontoPorcentagem / 100m), 2));
                    _descontoCentavos = (long)Math.Round(valorCalculado * 100m);
                }

                if (txtDesconto != null)
                {
                    txtDesconto.Text = $"- R$ {DescontoAtual:N2}";
                    txtDesconto.SelectionStart = txtDesconto.Text.Length;
                }

                if (txtDescontoPorcentagem != null)
                {
                    txtDescontoPorcentagem.Text = $"{_descontoPorcentagem} %";
                    txtDescontoPorcentagem.SelectionStart = txtDescontoPorcentagem.Text.Length;
                }
            }
            finally
            {
                _atualizandoDesconto = false;
            }
        }

        private void splitContainer_menu_SplitterMoved(object sender, SplitterEventArgs e)
        {
            LimitarSplitter(splitContainer_menu, 0.18, 0.25);
        }

        private void splitContainer_catalogo_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (splitContainer_catalogo == null || splitContainer_catalogo.Width <= 650) return;
            int dist = splitContainer_catalogo.SplitterDistance;
            int minDistance = splitContainer_catalogo.Width - 600;
            int maxDistance = splitContainer_catalogo.Width - 455;
            if (dist < minDistance) splitContainer_catalogo.SplitterDistance = minDistance;
            else if (dist > maxDistance) splitContainer_catalogo.SplitterDistance = maxDistance;
        }

        private static void LimitarSplitter(SplitContainer split, double percMin, double percMax)
        {
            if (split == null || split.Width <= 0) return;

            int min = (int)(split.Width * percMin);
            int max = (int)(split.Width * percMax);

            if (split.SplitterDistance < min)
                split.SplitterDistance = min;
            else if (split.SplitterDistance > max)
                split.SplitterDistance = max;
        }

        private void btnLimparCarrinho_Click(object sender, EventArgs e)
        {
            if (panelCarrinho == null || panelCarrinho.Controls.Count == 0) return;

            var confirmacao = MessageBox.Show(
                "Deseja realmente esvaziar o carrinho de compras?",
                "Limpar Carrinho",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmacao == DialogResult.Yes)
            {
                panelCarrinho.SuspendLayout();
                try
                {
                    while (panelCarrinho.Controls.Count > 0)
                    {
                        Control c = panelCarrinho.Controls[0];
                        panelCarrinho.Controls.Remove(c);
                        c.Dispose();
                    }
                }
                finally
                {
                    panelCarrinho.ResumeLayout();
                }

                _descontoCentavos = 0;
                _descontoPorcentagem = 0;
                SincronizarCamposDesconto(0m, originadoPorValor: true);
                AtualizarTotaisCarrinho();
            }
        }

        private void btnAvancarPagamento_Click(object sender, EventArgs e)
        {
            var (subtotal, totalItens) = CalcularMetricasCarrinho();
            if (totalItens <= 0 || subtotal <= 0m)
            {
                MessageBox.Show(
                    "Adicione ao menos um produto ao carrinho antes de avançar para o pagamento.",
                    "Carrinho Vazio",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            decimal desconto = DescontoAtual;
            int descontoPct = _descontoPorcentagem;
            decimal totalFinal = Math.Max(0m, subtotal - desconto);

            using (var formPagto = new formPagamento(subtotal, desconto, descontoPct, totalFinal))
            {
                if (formPagto.ShowDialog(this) == DialogResult.OK)
                {
                    panelCarrinho.SuspendLayout();
                    try
                    {
                        while (panelCarrinho.Controls.Count > 0)
                        {
                            Control c = panelCarrinho.Controls[0];
                            panelCarrinho.Controls.Remove(c);
                            c.Dispose();
                        }
                    }
                    finally
                    {
                        panelCarrinho.ResumeLayout();
                    }

                    _descontoCentavos = 0;
                    _descontoPorcentagem = 0;
                    SincronizarCamposDesconto(0m, originadoPorValor: true);
                    DefinirClientePadrao();
                    AtualizarTotaisCarrinho();

                    _ucEstoque?.FocarCampoBusca();
                }
            }
        }
    }
}