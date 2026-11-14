using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjetoExtensionista2026_2
{
    public class ParcelaPromissoria
    {
        public int NumeroParcela { get; set; }
        public int TotalParcelas { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal Valor { get; set; }
    }

    public class PagamentoDocumento
    {
        public string Metodo { get; set; }
        public decimal Valor { get; set; }
        public int Parcelas { get; set; } = 1;
        public bool EhAPrazo { get; set; }
    }

    public class DadosDocumentoVenda
    {
        public int NumeroVenda { get; set; }
        public DateTime DataEmissao { get; set; } = DateTime.Now;
        public string NomeCliente { get; set; }
        public string DocumentoCliente { get; set; }
        public string EnderecoCliente { get; set; }
        public string CidadeCliente { get; set; }
        public string UfCliente { get; set; }
        public string CepCliente { get; set; }
        public string TelefoneCliente { get; set; }
        public string FormaPagamento { get; set; }
        public string NomeVendedor { get; set; } = "VENDEDOR 1";
        public decimal Desconto { get; set; }
        public decimal ValorPago { get; set; }
        public decimal Troco { get; set; }
        public string Observacoes { get; set; }
        public List<ItemVenda> Itens { get; set; } = new List<ItemVenda>();
        public List<ParcelaPromissoria> Parcelas { get; set; } = new List<ParcelaPromissoria>();
        public List<PagamentoDocumento> Pagamentos { get; set; } = new List<PagamentoDocumento>();

        public bool PossuiClienteIdentificado =>
            !string.IsNullOrWhiteSpace(NomeCliente) &&
            !NomeCliente.Trim().Equals("Consumidor Não Identificado", StringComparison.OrdinalIgnoreCase);
    }

    public static class ImpressorDocumentos
    {
        private const string NomeEstabelecimento = "Estabelecimento";
        private const string SubtituloEstabelecimento = "Loja de Produtos";
        private const string EnderecoEstabelecimento = "Rua Principal, 100 - Centro";
        private const string CidadeUfEstabelecimento = "Frutal - MG";
        private const string TelefoneEstabelecimento = "(00) 0000-0000";
        private const string CnpjEstabelecimento = "00.000.000/0001-00";
        private const string IeEstabelecimento = "ISENTO";

        public static List<Bitmap> RenderizarPaginasCupom(DadosDocumentoVenda dados)
        {
            var paginas = new List<Bitmap>();
            int indiceItem = 0;
            bool temMaisPaginas = true;

            while (temMaisPaginas)
            {
                float yFinal;
                using (var bmpBruto = new Bitmap(620, 1700))
                {
                    using (var g = Graphics.FromImage(bmpBruto))
                    {
                        g.Clear(Color.White);
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                        g.ScaleTransform(2.0f, 2.0f);

                        temMaisPaginas = DesenharPaginaCupom(g, 830f, dados, ref indiceItem, out yFinal);
                    }

                    int alturaUtil = Math.Min(1700, Math.Max(420, (int)((yFinal + 20f) * 2.0f)));
                    var bmpRecortado = bmpBruto.Clone(new Rectangle(0, 0, 620, alturaUtil), bmpBruto.PixelFormat);
                    paginas.Add(bmpRecortado);
                }
            }

            return paginas;
        }

        public static List<Bitmap> RenderizarPaginasPromissoria(DadosDocumentoVenda dados)
        {
            GarantirParcelaUnicaSeVazio(dados);

            var paginas = new List<Bitmap>();
            int indiceParcela = 0;
            float alturaCartao = CalcularAlturaPromissoria(dados);
            const float limitePaginaLogico = 1125f;

            while (indiceParcela < dados.Parcelas.Count)
            {
                var bmp = new Bitmap(1230, 1740);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                    g.ScaleTransform(1.5f, 1.5f);

                    float yAtual = 28f;
                    int desenhadasNaPagina = 0;

                    while (indiceParcela < dados.Parcelas.Count && desenhadasNaPagina < 2)
                    {
                        if (desenhadasNaPagina > 0 && (yAtual + alturaCartao > limitePaginaLogico))
                            break;

                        DesenharPromissoriaEnxuta(g, dados, dados.Parcelas[indiceParcela], 40f, yAtual, 740f, alturaCartao);

                        if (desenhadasNaPagina == 0 && indiceParcela + 1 < dados.Parcelas.Count &&
                            (yAtual + alturaCartao + 24f + alturaCartao <= limitePaginaLogico))
                        {
                            using (var penCorte = new Pen(Color.Gray, 1f) { DashStyle = DashStyle.Dash })
                            {
                                g.DrawLine(penCorte, 40f, yAtual + alturaCartao + 12f, 780f, yAtual + alturaCartao + 12f);
                            }
                        }

                        yAtual += alturaCartao + 24f;
                        indiceParcela++;
                        desenhadasNaPagina++;
                    }
                }
                paginas.Add(bmp);
            }

            return paginas;
        }

        private static float CalcularAlturaPromissoria(DadosDocumentoVenda d)
        {
            int qtdItens = Math.Max(1, d.Itens?.Count ?? 0);
            float alturaTabelaItens = 20f + (qtdItens * 17f) + 22f;
            float alturaRodape = d.PossuiClienteIdentificado ? 42f : 26f;

            return 246f + alturaTabelaItens + alturaRodape;
        }

        public static void SalvarComoPdf(string caminhoArquivo, List<Bitmap> paginas)
        {
            if (paginas == null || paginas.Count == 0)
                throw new InvalidOperationException("Nenhuma página disponível para salvar.");

            using (var fs = new FileStream(caminhoArquivo, FileMode.Create, FileAccess.Write))
            {
                var offsets = new List<long> { 0 };

                void EscreverTexto(string texto)
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(texto);
                    fs.Write(bytes, 0, bytes.Length);
                }

                EscreverTexto("%PDF-1.4\n");

                offsets.Add(fs.Position);
                EscreverTexto("1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n");

                var kids = new StringBuilder();
                for (int i = 0; i < paginas.Count; i++)
                    kids.Append($"{3 + i * 3} 0 R ");

                offsets.Add(fs.Position);
                EscreverTexto($"2 0 obj\n<< /Type /Pages /Kids [ {kids}] /Count {paginas.Count} >>\nendobj\n");

                for (int i = 0; i < paginas.Count; i++)
                {
                    Bitmap bmp = paginas[i];
                    byte[] jpegBytes;
                    using (var ms = new MemoryStream())
                    {
                        bmp.Save(ms, ImageFormat.Jpeg);
                        jpegBytes = ms.ToArray();
                    }

                    string wPt = (bmp.Width * 72.0 / 150.0).ToString("0.##", CultureInfo.InvariantCulture);
                    string hPt = (bmp.Height * 72.0 / 150.0).ToString("0.##", CultureInfo.InvariantCulture);

                    int idObjPagina = 3 + i * 3;
                    int idObjImagem = 4 + i * 3;
                    int idObjConteudo = 5 + i * 3;

                    offsets.Add(fs.Position);
                    EscreverTexto(
                        $"{idObjPagina} 0 obj\n" +
                        $"<< /Type /Page /Parent 2 0 R /MediaBox [0 0 {wPt} {hPt}] " +
                        $"/Resources << /XObject << /Im0 {idObjImagem} 0 R >> >> /Contents {idObjConteudo} 0 R >>\n" +
                        "endobj\n");

                    offsets.Add(fs.Position);
                    EscreverTexto(
                        $"{idObjImagem} 0 obj\n" +
                        $"<< /Type /XObject /Subtype /Image /Width {bmp.Width} /Height {bmp.Height} " +
                        $"/ColorSpace /DeviceRGB /BitsPerComponent 8 /Filter /DCTDecode /Length {jpegBytes.Length} >>\nstream\n");
                    fs.Write(jpegBytes, 0, jpegBytes.Length);
                    EscreverTexto("\nendstream\nendobj\n");

                    string cmdDesenho = $"q {wPt} 0 0 {hPt} 0 0 cm /Im0 Do Q";
                    offsets.Add(fs.Position);
                    EscreverTexto(
                        $"{idObjConteudo} 0 obj\n<< /Length {cmdDesenho.Length} >>\nstream\n{cmdDesenho}\nendstream\nendobj\n");
                }

                long posicaoXref = fs.Position;
                EscreverTexto($"xref\n0 {offsets.Count}\n");
                EscreverTexto("0000000000 65535 f \n");
                for (int i = 1; i < offsets.Count; i++)
                {
                    EscreverTexto($"{offsets[i]:D10} 00000 n \n");
                }

                EscreverTexto(
                    $"trailer\n<< /Size {offsets.Count} /Root 1 0 R >>\n" +
                    $"startxref\n{posicaoXref}\n%%EOF");
            }
        }

        public static void ImprimirPaginasNaImpressora(List<Bitmap> paginas, string nomeImpressora, string nomeDocumento, bool formatoCupom80mm)
        {
            if (paginas == null || paginas.Count == 0) return;

            int indicePagina = 0;
            using (var doc = new PrintDocument())
            {
                doc.DocumentName = nomeDocumento;
                doc.PrinterSettings.PrinterName = nomeImpressora;
                doc.PrintController = new StandardPrintController();

                if (formatoCupom80mm)
                {
                    doc.DefaultPageSettings.PaperSize = new PaperSize("Cupom80mm", 315, 850);
                    doc.DefaultPageSettings.Margins = new Margins(5, 5, 5, 5);
                }
                else
                {
                    doc.DefaultPageSettings.Margins = new Margins(20, 20, 20, 20);
                }

                doc.PrintPage += (s, e) =>
                {
                    Bitmap bmp = paginas[indicePagina];
                    Rectangle area = e.MarginBounds;

                    float escala = Math.Min((float)area.Width / bmp.Width, (float)area.Height / bmp.Height);
                    int larguraFinal = (int)(bmp.Width * escala);
                    int alturaFinal = (int)(bmp.Height * escala);

                    e.Graphics.DrawImage(bmp, area.Left, area.Top, larguraFinal, alturaFinal);

                    indicePagina++;
                    e.HasMorePages = indicePagina < paginas.Count;
                };

                doc.Print();
            }
        }

        private static void GarantirParcelaUnicaSeVazio(DadosDocumentoVenda dados)
        {
            if (dados.Parcelas != null && dados.Parcelas.Count > 0) return;

            decimal totalLiquido = Math.Max(0m, dados.Itens.Sum(i => i.TotalItem) - dados.Desconto);

            dados.Parcelas = new List<ParcelaPromissoria>
            {
                new ParcelaPromissoria
                {
                    NumeroParcela = 1,
                    TotalParcelas = 1,
                    DataVencimento = dados.DataEmissao.AddDays(30),
                    Valor = totalLiquido
                }
            };
        }

        private static List<(string Data, string DescricaoMetodo, decimal Valor)> ObterLinhasPagamentoCupom(DadosDocumentoVenda d, decimal totalFinal)
        {
            var linhas = new List<(string Data, string DescricaoMetodo, decimal Valor)>();

            if (d.Pagamentos != null && d.Pagamentos.Count > 0)
            {
                foreach (var pg in d.Pagamentos)
                {
                    if (pg.Valor <= 0m) continue;

                    string nomeMetodo = (pg.Metodo ?? "DINHEIRO").ToUpper();

                    if (pg.EhAPrazo && pg.Parcelas >= 1)
                    {
                        decimal baseParc = Math.Floor((pg.Valor / pg.Parcelas) * 100m) / 100m;
                        decimal resto = pg.Valor - (baseParc * pg.Parcelas);

                        for (int i = 1; i <= pg.Parcelas; i++)
                        {
                            decimal valParc = (i == pg.Parcelas) ? baseParc + resto : baseParc;
                            string rotulo = pg.Parcelas > 1 ? $"{nomeMetodo} ({i}/{pg.Parcelas})" : nomeMetodo;
                            linhas.Add((d.DataEmissao.AddMonths(i).ToString("dd/MM/yyyy"), rotulo, valParc));
                        }
                    }
                    else
                    {
                        string rotulo = pg.Parcelas > 1 ? $"{nomeMetodo} ({pg.Parcelas}x)" : nomeMetodo;
                        linhas.Add((d.DataEmissao.ToString("dd/MM/yyyy"), rotulo, pg.Valor));
                    }
                }

                if (linhas.Count > 0)
                    return linhas;
            }

            string tipoFallback = string.IsNullOrWhiteSpace(d.FormaPagamento) ? "DINHEIRO" : d.FormaPagamento.ToUpper();
            linhas.Add((d.DataEmissao.ToString("dd/MM/yyyy"), tipoFallback, totalFinal));
            return linhas;
        }

        private static string ObterResumoPagamentosTexto(DadosDocumentoVenda d, decimal totalFinal)
        {
            if (d.Pagamentos != null && d.Pagamentos.Count > 0)
            {
                var partes = d.Pagamentos
                    .Where(p => p.Valor > 0m)
                    .Select(p =>
                    {
                        string parc = p.Parcelas > 1 ? $" ({p.Parcelas}x)" : string.Empty;
                        return $"{p.Metodo}{parc}: {p.Valor:C2}";
                    });

                string resumo = string.Join("   |   ", partes);
                if (!string.IsNullOrWhiteSpace(resumo))
                    return resumo;
            }

            return !string.IsNullOrWhiteSpace(d.FormaPagamento)
                ? $"{d.FormaPagamento} ({totalFinal:C2})"
                : $"À Vista ({totalFinal:C2})";
        }

        // 1. CUPOM TÉRMICO (80 mm) - Sem assinatura do cliente e com múltiplas formas de pagamento
        private static bool DesenharPaginaCupom(Graphics g, float limiteAltura, DadosDocumentoVenda d, ref int indiceItem, out float yFinal)
        {
            float x = 10f;
            float largura = 290f;
            float y = 12f;

            using (var penLinha = new Pen(Color.Black, 1f))
            using (var fFantasia = new Font("Arial", 11.5f, FontStyle.Bold))
            using (var fCabecalho = new Font("Arial", 8f, FontStyle.Regular))
            using (var fRotuloComp = new Font("Arial", 6f, FontStyle.Bold))
            using (var fNumeroComp = new Font("Arial", 10.5f, FontStyle.Bold))
            using (var fColunaHeader = new Font("Arial", 7.8f, FontStyle.Regular))
            using (var fItemLinha = new Font("Arial", 7.8f, FontStyle.Bold))
            using (var fTotalGrande = new Font("Arial", 9.8f, FontStyle.Bold))
            using (var fTextoNormal = new Font("Arial", 8f, FontStyle.Regular))
            using (var fTextoNegrito = new Font("Arial", 8f, FontStyle.Bold))
            using (var fAceite = new Font("Arial", 7.2f, FontStyle.Regular))
            {
                var centro = new StringFormat { Alignment = StringAlignment.Center };
                var esquerda = new StringFormat { Alignment = StringAlignment.Near };
                var direita = new StringFormat { Alignment = StringAlignment.Far };

                if (indiceItem == 0)
                {
                    g.DrawString(NomeEstabelecimento.ToUpper(), fFantasia, Brushes.Black, new RectangleF(x, y, largura, 18), centro);
                    y += 18f;

                    g.DrawString(SubtituloEstabelecimento.ToUpper(), fCabecalho, Brushes.Black, new RectangleF(x, y, largura, 14), centro);
                    y += 13f;

                    g.DrawString(EnderecoEstabelecimento, fCabecalho, Brushes.Black, new RectangleF(x, y, largura, 14), centro);
                    y += 13f;

                    g.DrawString(CidadeUfEstabelecimento, fCabecalho, Brushes.Black, new RectangleF(x, y, largura, 14), centro);
                    y += 13f;

                    g.DrawString(TelefoneEstabelecimento, fCabecalho, Brushes.Black, new RectangleF(x, y, largura, 14), centro);
                    y += 15f;

                    g.DrawString($"CNPJ : {CnpjEstabelecimento}", fCabecalho, Brushes.Black, new RectangleF(x, y, largura * 0.65f, 14), esquerda);
                    g.DrawString($"IE : {IeEstabelecimento}", fCabecalho, Brushes.Black, new RectangleF(x, y, largura, 14), direita);
                    y += 18f;

                    string nomeCliente = d.PossuiClienteIdentificado
                        ? d.NomeCliente.ToUpper()
                        : "CONSUMIDOR FINAL";
                    g.DrawString($"CLIENTE : {nomeCliente}", fCabecalho, Brushes.Black, new RectangleF(x, y, largura, 14), esquerda);
                    y += 13f;

                    if (d.PossuiClienteIdentificado && !string.IsNullOrWhiteSpace(d.DocumentoCliente))
                    {
                        g.DrawString($"CPF/CNPJ : {d.DocumentoCliente}", fCabecalho, Brushes.Black, new RectangleF(x, y, largura, 14), esquerda);
                        y += 13f;
                    }

                    y += 3f;

                    g.DrawString("COMPROVANTE DE VENDA", fRotuloComp, Brushes.Black, new RectangleF(x, y, largura, 10), direita);
                    g.DrawString($"{d.DataEmissao:dd/MM/yyyy   HH:mm}", fCabecalho, Brushes.Black, new RectangleF(x, y + 6f, largura * 0.55f, 15), esquerda);
                    g.DrawString($"Nº {d.NumeroVenda:D6}", fNumeroComp, Brushes.Black, new RectangleF(x, y + 8f, largura, 17), direita);
                    y += 28f;
                }

                g.DrawLine(penLinha, x, y, x + largura, y);
                y += 3f;

                g.DrawString("CODIGO", fColunaHeader, Brushes.Black, new RectangleF(x, y, 65f, 13), esquerda);
                g.DrawString("DESCRIÇÃO", fColunaHeader, Brushes.Black, new RectangleF(x + 65f, y, 155f, 13), centro);
                g.DrawString("UNID.", fColunaHeader, Brushes.Black, new RectangleF(x + 220f, y, 70f, 13), direita);
                y += 12f;

                g.DrawString("QTD x UNIT", fColunaHeader, Brushes.Black, new RectangleF(x + 65f, y, 155f, 13), centro);
                g.DrawString("R$ VALOR", fColunaHeader, Brushes.Black, new RectangleF(x + 220f, y, 70f, 13), direita);
                y += 14f;

                g.DrawLine(penLinha, x, y, x + largura, y);
                y += 5f;

                while (indiceItem < d.Itens.Count)
                {
                    if (y + 28f > limiteAltura)
                    {
                        yFinal = y;
                        return true;
                    }

                    var item = d.Itens[indiceItem];
                    string desc = item.Descricao.ToUpper();
                    if (desc.Length > 30) desc = desc.Substring(0, 30);

                    g.DrawString($"{item.IdProduto:D5}  {desc}", fItemLinha, Brushes.Black, new RectangleF(x, y, largura - 30f, 13), esquerda);
                    g.DrawString("UN", fItemLinha, Brushes.Black, new RectangleF(x, y, largura, 13), direita);
                    y += 12f;

                    g.DrawString($"{item.Quantidade}  x  {item.PrecoUnitario:N2}", fItemLinha, Brushes.Black, new RectangleF(x + 65f, y, 155f, 13), centro);
                    g.DrawString(item.TotalItem.ToString("N2"), fItemLinha, Brushes.Black, new RectangleF(x, y, largura, 13), direita);
                    y += 15f;

                    indiceItem++;
                }

                decimal subtotal = d.Itens.Sum(i => i.TotalItem);
                decimal totalFinal = Math.Max(0m, subtotal - d.Desconto);
                decimal valorRecebido = d.ValorPago > 0m ? d.ValorPago : totalFinal;
                var linhasPagamento = ObterLinhasPagamentoCupom(d, totalFinal);

                float alturaNecessariaRodape = 185f + (linhasPagamento.Count * 15f) + (string.IsNullOrWhiteSpace(d.Observacoes) ? 0f : 34f);
                if (y + alturaNecessariaRodape > limiteAltura)
                {
                    yFinal = y;
                    return true;
                }

                g.DrawLine(penLinha, x, y, x + largura, y);
                y += 6f;

                if (d.Desconto > 0m)
                {
                    g.DrawString("Subtotal R$", fTextoNormal, Brushes.Black, new RectangleF(x, y, 180f, 15), esquerda);
                    g.DrawString(subtotal.ToString("N2"), fTextoNormal, Brushes.Black, new RectangleF(x, y, largura, 15), direita);
                    y += 15f;

                    g.DrawString("Desconto R$", fTextoNormal, Brushes.Black, new RectangleF(x, y, 180f, 15), esquerda);
                    g.DrawString($"- {d.Desconto:N2}", fTextoNormal, Brushes.Black, new RectangleF(x, y, largura, 15), direita);
                    y += 15f;
                }

                g.DrawString("Total da Nota R$", fTotalGrande, Brushes.Black, new RectangleF(x, y, 185f, 17), esquerda);
                g.DrawString(totalFinal.ToString("N2"), fTotalGrande, Brushes.Black, new RectangleF(x, y, largura, 17), direita);
                y += 18f;

                g.DrawString("Valor Recebido R$", fTotalGrande, Brushes.Black, new RectangleF(x, y, 185f, 17), esquerda);
                g.DrawString(valorRecebido.ToString("N2"), fTotalGrande, Brushes.Black, new RectangleF(x, y, largura, 17), direita);
                y += 18f;

                g.DrawString("Troco R$", fTotalGrande, Brushes.Black, new RectangleF(x, y, 185f, 17), esquerda);
                g.DrawString(d.Troco.ToString("N2"), fTotalGrande, Brushes.Black, new RectangleF(x, y, largura, 17), direita);
                y += 22f;

                bool temPrazo = d.Pagamentos != null && d.Pagamentos.Any(p => p.EhAPrazo);
                bool temVista = d.Pagamentos != null && d.Pagamentos.Any(p => !p.EhAPrazo);
                string condicaoPgto = (temPrazo && temVista) ? "MISTA (VISTA + PRAZO)"
                                    : (d.Pagamentos != null && d.Pagamentos.Count > 1) ? "MÚLTIPLAS FORMAS"
                                    : temPrazo ? "A PRAZO"
                                    : "À VISTA";

                g.DrawString($"FORMA DE PGTO. : {condicaoPgto}", fTextoNegrito, Brushes.Black, new RectangleF(x, y, largura, 15), esquerda);
                y += 16f;

                g.DrawString("DATA PGTO", fTextoNegrito, Brushes.Black, new RectangleF(x, y, 72f, 14), esquerda);
                g.DrawString("TIPO PGTO", fTextoNegrito, Brushes.Black, new RectangleF(x + 74f, y, 142f, 14), esquerda);
                g.DrawString("R$ VALOR", fTextoNegrito, Brushes.Black, new RectangleF(x + 216f, y, 74f, 14), direita);
                y += 14f;

                foreach (var pg in linhasPagamento)
                {
                    string metodoCurto = pg.DescricaoMetodo.Length > 22
                        ? pg.DescricaoMetodo.Substring(0, 22)
                        : pg.DescricaoMetodo;

                    g.DrawString(pg.Data, fTextoNormal, Brushes.Black, new RectangleF(x, y, 72f, 14), esquerda);
                    g.DrawString(metodoCurto, fTextoNormal, Brushes.Black, new RectangleF(x + 74f, y, 142f, 14), esquerda);
                    g.DrawString(pg.Valor.ToString("N2"), fTextoNormal, Brushes.Black, new RectangleF(x + 216f, y, 74f, 14), direita);
                    y += 14f;
                }

                y += 4f;
                g.DrawLine(penLinha, x, y, x + largura, y);
                y += 5f;

                string vendedor = string.IsNullOrWhiteSpace(d.NomeVendedor) ? "VENDEDOR 1" : d.NomeVendedor.ToUpper();
                g.DrawString($"VENDEDOR(A) : {vendedor}", fTextoNormal, Brushes.Black, new RectangleF(x, y, largura, 14), esquerda);
                y += 16f;

                g.DrawLine(penLinha, x, y, x + largura, y);
                y += 8f;

                if (!string.IsNullOrWhiteSpace(d.Observacoes))
                {
                    g.DrawString("Observações :", fTextoNegrito, Brushes.Black, new RectangleF(x, y, largura, 13), esquerda);
                    y += 13f;
                    g.DrawString(d.Observacoes, fTextoNormal, Brushes.Black, new RectangleF(x, y, largura, 28), esquerda);
                    y += 30f;
                }

                g.DrawString(
                    "Recebi a(s) mercadoria(s) acima descrita(s), concordando\nplenamente com os prazos e condições de garantia.",
                    fAceite, Brushes.Black, new RectangleF(x, y, largura, 26), centro);
                y += 30f;

                g.DrawLine(penLinha, x, y, x + largura, y);
                y += 6f;

                g.DrawString("* OBRIGADO E VOLTE SEMPRE *", fTextoNegrito, Brushes.Black, new RectangleF(x, y, largura, 15), centro);
                y += 16f;

                yFinal = y;
                return false;
            }
        }

        // 2. NOTA PROMISSÓRIA ENXUTA COM ITENS COMPRADOS E ASSINATURA APENAS QUANDO HOUVER CLIENTE
        private static void DesenharPromissoriaEnxuta(Graphics g, DadosDocumentoVenda d, ParcelaPromissoria parcela, float x, float y, float w, float h)
        {
            bool temCliente = d.PossuiClienteIdentificado;
            decimal subtotal = d.Itens.Sum(i => i.TotalItem);
            decimal totalVenda = Math.Max(0m, subtotal - d.Desconto);

            Color corAzul = Color.FromArgb(15, 70, 165);
            Color corFaixa = Color.FromArgb(236, 243, 253);
            Color corSubFaixa = Color.FromArgb(246, 248, 252);
            Color corBorda = Color.FromArgb(95, 105, 118);
            Color corTexto = Color.FromArgb(28, 32, 38);
            Color corSuave = Color.FromArgb(85, 92, 102);

            using (var penExterna = new Pen(corBorda, 1.4f))
            using (var penInterna = new Pen(Color.FromArgb(165, 172, 182), 0.9f))
            using (var brushFaixa = new SolidBrush(corFaixa))
            using (var brushSubFaixa = new SolidBrush(corSubFaixa))
            using (var brushAzul = new SolidBrush(corAzul))
            using (var brushTexto = new SolidBrush(corTexto))
            using (var brushSuave = new SolidBrush(corSuave))
            using (var fTitulo = new Font("Segoe UI", 12f, FontStyle.Bold))
            using (var fSubtitulo = new Font("Segoe UI", 8f, FontStyle.Regular))
            using (var fDestaque = new Font("Segoe UI", 9.5f, FontStyle.Bold))
            using (var fRotulo = new Font("Segoe UI", 7.8f, FontStyle.Bold))
            using (var fTexto = new Font("Segoe UI", 8.2f, FontStyle.Regular))
            {
                var esqCentro = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
                var dirCentro = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };
                var centro = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

                g.DrawRectangle(penExterna, x, y, w, h);

                // 1. Cabeçalho compacto
                float hCab = 46f;
                g.FillRectangle(brushFaixa, x, y, w, hCab);
                g.DrawRectangle(penInterna, x, y, w, hCab);

                string tituloDoc = temCliente
                    ? "NOTA PROMISSÓRIA"
                    : "COMPROVANTE DE VENDA (SEM VALOR DE PROMISSÓRIA)";
                g.DrawString(tituloDoc, fTitulo, brushAzul, new RectangleF(x + 12, y + 4, 460, 22), esqCentro);
                g.DrawString($"{NomeEstabelecimento}  •  CNPJ: {CnpjEstabelecimento}  •  {CidadeUfEstabelecimento}",
                    fSubtitulo, brushSuave, new RectangleF(x + 12, y + 25, 460, 16), esqCentro);

                g.DrawString($"Nº {d.NumeroVenda:D6}  ({parcela.NumeroParcela}/{parcela.TotalParcelas})",
                    fRotulo, brushTexto, new RectangleF(x + w - 230, y + 4, 218, 17), dirCentro);
                g.DrawString($"VALOR: {parcela.Valor:C2}   |   VENC.: {parcela.DataVencimento:dd/MM/yyyy}",
                    fDestaque, brushAzul, new RectangleF(x + w - 270, y + 22, 258, 20), dirCentro);

                float yAtual = y + hCab + 8f;

                // 2. Cláusula objetiva de compromisso (ou aviso de conferência)
                string clausula;
                if (temCliente)
                {
                    clausula =
                        $"Aos {parcela.DataVencimento:dd/MM/yyyy}, pagarei(emos) por esta única via de NOTA PROMISSÓRIA a " +
                        $"{NomeEstabelecimento} (CNPJ {CnpjEstabelecimento}), ou à sua ordem, a quantia de {parcela.Valor:C2} " +
                        $"em moeda corrente deste país, pagável em {CidadeUfEstabelecimento}.";
                }
                else
                {
                    clausula =
                        "DOCUMENTO SEM VALOR DE NOTA PROMISSÓRIA: Emitido sem identificação de emitente/cliente, " +
                        "servindo exclusivamente como comprovante simples da operação, itens adquiridos e formas de pagamento.";
                }
                g.DrawString(clausula, fTexto, brushTexto, new RectangleF(x + 12, yAtual, w - 24, 34));
                yAtual += 36f;

                // 3. Quadro enxuto do Emitente / Cliente
                float hCliente = 44f;
                float xBox = x + 12f;
                float wBox = w - 24f;

                g.DrawRectangle(penInterna, xBox, yAtual, wBox, hCliente);
                g.DrawLine(penInterna, xBox, yAtual + 22f, xBox + wBox, yAtual + 22f);

                string nomeCli = temCliente ? d.NomeCliente.ToUpper() : "Não informado";
                string docCli = (temCliente && !string.IsNullOrWhiteSpace(d.DocumentoCliente)) ? d.DocumentoCliente : "Não informado";
                string endCli = (temCliente && !string.IsNullOrWhiteSpace(d.EnderecoCliente)) ? d.EnderecoCliente : "Não informado";
                string telCli = (temCliente && !string.IsNullOrWhiteSpace(d.TelefoneCliente)) ? d.TelefoneCliente : "-";

                g.DrawString($"EMITENTE: {nomeCli}", fRotulo, brushTexto, new RectangleF(xBox + 6, yAtual, 450, 22f), esqCentro);
                g.DrawString($"CPF/CNPJ: {docCli}", fRotulo, brushTexto, new RectangleF(xBox + 460, yAtual, wBox - 466, 22f), dirCentro);

                g.DrawString($"ENDEREÇO: {endCli}", fSubtitulo, brushTexto, new RectangleF(xBox + 6, yAtual + 22f, 510, 22f), esqCentro);
                g.DrawString($"TEL: {telCli}", fSubtitulo, brushTexto, new RectangleF(xBox + 520, yAtual + 22f, wBox - 526, 22f), dirCentro);

                yAtual += hCliente + 6f;

                // 4. Grade compacta dos Itens Comprados
                float cCod = 56f;
                float cQtd = 50f;
                float cDesc = 410f;
                float cUnit = 100f;
                float cTot = wBox - (cCod + cQtd + cDesc + cUnit); // 100f

                float hCabTabela = 20f;
                g.FillRectangle(brushFaixa, xBox, yAtual, wBox, hCabTabela);
                g.DrawRectangle(penInterna, xBox, yAtual, cCod, hCabTabela);
                g.DrawRectangle(penInterna, xBox + cCod, yAtual, cQtd, hCabTabela);
                g.DrawRectangle(penInterna, xBox + cCod + cQtd, yAtual, cDesc, hCabTabela);
                g.DrawRectangle(penInterna, xBox + cCod + cQtd + cDesc, yAtual, cUnit, hCabTabela);
                g.DrawRectangle(penInterna, xBox + cCod + cQtd + cDesc + cUnit, yAtual, cTot, hCabTabela);

                g.DrawString("CÓD.", fRotulo, brushTexto, new RectangleF(xBox, yAtual, cCod, hCabTabela), centro);
                g.DrawString("QTD.", fRotulo, brushTexto, new RectangleF(xBox + cCod, yAtual, cQtd, hCabTabela), centro);
                g.DrawString("PRODUTO / DESCRIÇÃO", fRotulo, brushTexto, new RectangleF(xBox + cCod + cQtd + 6, yAtual, cDesc - 12, hCabTabela), esqCentro);
                g.DrawString("UNIT. (R$)", fRotulo, brushTexto, new RectangleF(xBox + cCod + cQtd + cDesc, yAtual, cUnit - 4, hCabTabela), dirCentro);
                g.DrawString("TOTAL (R$)", fRotulo, brushTexto, new RectangleF(xBox + cCod + cQtd + cDesc + cUnit, yAtual, cTot - 4, hCabTabela), dirCentro);

                yAtual += hCabTabela;

                int qtdLinhasItens = Math.Max(1, d.Itens?.Count ?? 0);
                float hLinhaItem = 17f;

                for (int idx = 0; idx < qtdLinhasItens; idx++)
                {
                    g.DrawRectangle(penInterna, xBox, yAtual, cCod, hLinhaItem);
                    g.DrawRectangle(penInterna, xBox + cCod, yAtual, cQtd, hLinhaItem);
                    g.DrawRectangle(penInterna, xBox + cCod + cQtd, yAtual, cDesc, hLinhaItem);
                    g.DrawRectangle(penInterna, xBox + cCod + cQtd + cDesc, yAtual, cUnit, hLinhaItem);
                    g.DrawRectangle(penInterna, xBox + cCod + cQtd + cDesc + cUnit, yAtual, cTot, hLinhaItem);

                    if (d.Itens != null && idx < d.Itens.Count)
                    {
                        var item = d.Itens[idx];
                        string desc = item.Descricao.ToUpper();
                        if (desc.Length > 55) desc = desc.Substring(0, 55) + "...";

                        g.DrawString(item.IdProduto.ToString("D4"), fSubtitulo, brushTexto, new RectangleF(xBox, yAtual, cCod, hLinhaItem), centro);
                        g.DrawString(item.Quantidade.ToString("00"), fSubtitulo, brushTexto, new RectangleF(xBox + cCod, yAtual, cQtd, hLinhaItem), centro);
                        g.DrawString(desc, fSubtitulo, brushTexto, new RectangleF(xBox + cCod + cQtd + 6, yAtual, cDesc - 12, hLinhaItem), esqCentro);
                        g.DrawString(item.PrecoUnitario.ToString("N2"), fSubtitulo, brushTexto, new RectangleF(xBox + cCod + cQtd + cDesc, yAtual, cUnit - 6, hLinhaItem), dirCentro);
                        g.DrawString(item.TotalItem.ToString("N2"), fSubtitulo, brushTexto, new RectangleF(xBox + cCod + cQtd + cDesc + cUnit, yAtual, cTot - 6, hLinhaItem), dirCentro);
                    }
                    else
                    {
                        g.DrawString("Nenhum item registrado", fSubtitulo, brushSuave, new RectangleF(xBox + cCod + cQtd + 6, yAtual, cDesc - 12, hLinhaItem), esqCentro);
                    }

                    yAtual += hLinhaItem;
                }

                // Rodapé de Totais da Grade de Itens
                float hTotaisTabela = 22f;
                g.FillRectangle(brushSubFaixa, xBox, yAtual, wBox, hTotaisTabela);
                g.DrawRectangle(penInterna, xBox, yAtual, wBox, hTotaisTabela);

                string textoDesconto = d.Desconto > 0m ? $"   |   Desconto: - {d.Desconto:C2}" : string.Empty;
                g.DrawString($"Subtotal: {subtotal:C2}{textoDesconto}", fSubtitulo, brushSuave,
                    new RectangleF(xBox + 6, yAtual, 430, hTotaisTabela), esqCentro);
                g.DrawString($"TOTAL DA VENDA: {totalVenda:C2}", fRotulo, brushTexto,
                    new RectangleF(xBox + 440, yAtual, wBox - 446, hTotaisTabela), dirCentro);

                yAtual += hTotaisTabela + 6f;

                float hPagto = 40f;
                g.DrawRectangle(penInterna, xBox, yAtual, wBox, hPagto);
                g.DrawString("FORMAS DE PAGAMENTO:", fRotulo, brushSuave, new RectangleF(xBox + 6, yAtual + 2f, wBox - 12, 14f), esqCentro);

                string formasTexto = ObterResumoPagamentosTexto(d, totalVenda);
                g.DrawString(formasTexto, fTexto, brushTexto, new RectangleF(xBox + 6, yAtual + 16f, wBox - 12, 22f));

                yAtual += hPagto + 6f;

                float hObs = 42f;
                g.DrawRectangle(penInterna, xBox, yAtual, wBox, hObs);
                g.DrawString("OBSERVAÇÕES DA VENDA:", fRotulo, brushSuave, new RectangleF(xBox + 6, yAtual + 2f, wBox - 12, 14f), esqCentro);

                string obsTexto = string.IsNullOrWhiteSpace(d.Observacoes)
                    ? "Nenhuma observação registrada."
                    : d.Observacoes.Trim();
                Brush brushObs = string.IsNullOrWhiteSpace(d.Observacoes) ? brushSuave : brushTexto;
                g.DrawString(obsTexto, fTexto, brushObs, new RectangleF(xBox + 6, yAtual + 16f, wBox - 12, 24f));

                float yRodape = y + h - (temCliente ? 26f : 22f);
                g.DrawString($"{CidadeUfEstabelecimento}, {d.DataEmissao:dd 'de' MMMM 'de' yyyy}",
                    fTexto, brushTexto, new RectangleF(x + 14, yRodape - 2f, 330, 18f), esqCentro);

                if (temCliente)
                {
                    float xAssIni = x + w - 330f;
                    float xAssFim = x + w - 16f;
                    g.DrawLine(penExterna, xAssIni, yRodape, xAssFim, yRodape);
                    g.DrawString($"Assinatura do Emitente: {d.NomeCliente}", fRotulo, brushSuave,
                        new RectangleF(xAssIni, yRodape + 2f, xAssFim - xAssIni, 16f), centro);
                }
            }
        }
    }
}