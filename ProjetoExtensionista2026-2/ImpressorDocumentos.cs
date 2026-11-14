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
        public bool EhOrcamento { get; set; }
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
            !NomeCliente.Trim().Equals("Consumidor Não Identificado", StringComparison.OrdinalIgnoreCase) &&
            !NomeCliente.Trim().Equals("Cliente não informado", StringComparison.OrdinalIgnoreCase);
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

        // =========================================================================
        // 1. LAYOUT DO CUPOM TÉRMICO (80 mm)
        // =========================================================================
        private static bool DesenharPaginaCupom(Graphics g, float limiteAltura, DadosDocumentoVenda d, ref int indiceItem, out float yFinal)
        {
            var doc = new CursorDocumento(g, 10f, 12f, 290f);
            using (var t = new TemaDoc())
            {
                if (indiceItem == 0)
                {
                    doc.Centro(NomeEstabelecimento.ToUpper(), t.Fantasia);
                    doc.Centro(SubtituloEstabelecimento.ToUpper(), t.Peq);
                    doc.Centro($"{EnderecoEstabelecimento}\n{CidadeUfEstabelecimento}\n{TelefoneEstabelecimento}", t.Peq, 3f);
                    doc.Par($"CNPJ : {CnpjEstabelecimento}", $"IE : {IeEstabelecimento}", t.Peq, 5f);

                    string cliente = d.PossuiClienteIdentificado ? d.NomeCliente.ToUpper() : "CONSUMIDOR FINAL";
                    doc.Esq($"CLIENTE : {cliente}", t.Peq);
                    if (d.PossuiClienteIdentificado && !string.IsNullOrWhiteSpace(d.DocumentoCliente))
                        doc.Esq($"CPF/CNPJ : {d.DocumentoCliente}", t.Peq, 2f);

                    doc.Dir("COMPROVANTE DE VENDA", t.Micro, 0f);
                    doc.Par($"{d.DataEmissao:dd/MM/yyyy   HH:mm}", $"Nº {d.NumeroVenda:D6}", t.Peq, 4f, t.NumComp);
                }

                doc.Sep();
                doc.Colunas(new[] { 65f, 155f, 70f }, new[] { StringAlignment.Near, StringAlignment.Center, StringAlignment.Far });
                doc.LinhaTexto(t.Peq, 13f, "CODIGO", "DESCRIÇÃO", "UNID.");
                doc.LinhaTexto(t.Peq, 13f, "", "QTD x UNIT", "R$ VALOR");
                doc.Sep(2f, 5f);

                while (indiceItem < d.Itens.Count)
                {
                    if (!doc.Cabe(28f, limiteAltura)) { yFinal = doc.Y; return true; }
                    var it = d.Itens[indiceItem++];
                    string desc = it.Descricao.Length > 30 ? it.Descricao.Substring(0, 30).ToUpper() : it.Descricao.ToUpper();

                    doc.Par($"{it.IdProduto:D5}  {desc}", "UN", t.Item, 0f);
                    doc.LinhaTexto(t.Item, 14f, "", $"{it.Quantidade}  x  {it.PrecoUnitario:N2}", it.TotalItem.ToString("N2"));
                }

                decimal sub = d.Itens.Sum(i => i.TotalItem);
                decimal tot = Math.Max(0m, sub - d.Desconto);
                decimal rec = d.ValorPago > 0m ? d.ValorPago : tot;
                var pgs = ObterLinhasPagamentoCupom(d, tot);

                if (!doc.Cabe(180f + pgs.Count * 15f + (string.IsNullOrWhiteSpace(d.Observacoes) ? 0f : 34f), limiteAltura))
                { yFinal = doc.Y; return true; }

                doc.Sep(2f, 6f);
                if (d.Desconto > 0m)
                {
                    doc.Par("Subtotal R$", sub.ToString("N2"), t.Peq);
                    doc.Par("Desconto R$", $"- {d.Desconto:N2}", t.Peq, 3f);
                }
                doc.Par("Total da Nota R$", tot.ToString("N2"), t.Total);
                doc.Par("Valor Recebido R$", rec.ToString("N2"), t.Total);
                doc.Par("Troco R$", d.Troco.ToString("N2"), t.Total, 6f);

                bool prazo = d.Pagamentos.Any(p => p.EhAPrazo), vista = d.Pagamentos.Any(p => !p.EhAPrazo);
                string cond = (prazo && vista) ? "MISTA (VISTA + PRAZO)" : d.Pagamentos.Count > 1 ? "MÚLTIPLAS FORMAS" : prazo ? "A PRAZO" : "À VISTA";
                doc.Esq($"FORMA DE PGTO. : {cond}", t.PeqNeg, 2f);

                doc.Colunas(new[] { 72f, 144f, 74f }, new[] { StringAlignment.Near, StringAlignment.Near, StringAlignment.Far });
                doc.LinhaTexto(t.PeqNeg, 14f, "DATA PGTO", "TIPO PGTO", "R$ VALOR");
                foreach (var pg in pgs)
                    doc.LinhaTexto(t.Peq, 14f, pg.Data, pg.DescricaoMetodo.Length > 22 ? pg.DescricaoMetodo.Substring(0, 22) : pg.DescricaoMetodo, pg.Valor.ToString("N2"));

                doc.Sep(4f, 5f);
                doc.Esq($"VENDEDOR(A) : {(string.IsNullOrWhiteSpace(d.NomeVendedor) ? "VENDEDOR 1" : d.NomeVendedor.ToUpper())}", t.Peq);
                doc.Sep(3f, 8f);

                if (!string.IsNullOrWhiteSpace(d.Observacoes))
                {
                    doc.Esq("Observações :", t.PeqNeg, 0f);
                    doc.Esq(d.Observacoes.Trim(), t.Peq, 6f);
                }

                doc.Centro("Recebi a(s) mercadoria(s) acima descrita(s), concordando\nplenamente com os prazos e condições de garantia.", t.Aceite, 6f);
                doc.Sep(2f, 5f);
                doc.Centro("* OBRIGADO E VOLTE SEMPRE *", t.PeqNeg);

                yFinal = doc.Y;
                return false;
            }
        }

        // =========================================================================
        // 2. LAYOUT DA NOTA PROMISSÓRIA / ORÇAMENTO ENXUTO
        // =========================================================================
        private static float DesenharPromissoriaEnxuta(Graphics g, DadosDocumentoVenda d, ParcelaPromissoria parcela, float x, float y, float w)
        {
            bool ehOrcamento = d.EhOrcamento;
            bool temCliente = d.PossuiClienteIdentificado;
            decimal sub = d.Itens.Sum(i => i.TotalItem);
            decimal tot = Math.Max(0m, sub - d.Desconto);

            using (var t = new TemaDoc())
            {
                g.FillRectangle(t.BrushFaixa, x, y, w, 46f);
                g.DrawRectangle(t.PenInterna, x, y, w, 46f);

                string titulo = ehOrcamento
                    ? "ORÇAMENTO DE VENDA"
                    : (temCliente ? "NOTA PROMISSÓRIA" : "COMPROVANTE DE VENDA (SEM VALOR DE PROMISSÓRIA)");

                string numDir = ehOrcamento
                    ? $"ORÇAMENTO Nº {d.NumeroVenda:D6}"
                    : $"Nº {d.NumeroVenda:D6}  ({parcela.NumeroParcela}/{parcela.TotalParcelas})";

                string valDir = ehOrcamento
                    ? $"TOTAL: {tot:C2}   |   DATA: {d.DataEmissao:dd/MM/yyyy}"
                    : $"VALOR: {parcela.Valor:C2}   |   VENC.: {parcela.DataVencimento:dd/MM/yyyy}";

                g.DrawString(titulo, t.TituloProm, t.BrushAzul, new RectangleF(x + 12, y + 4, 460, 22), t.EsqCentro);
                g.DrawString($"{NomeEstabelecimento}  •  CNPJ: {CnpjEstabelecimento}  •  {CidadeUfEstabelecimento}", t.SubProm, t.BrushSuave, new RectangleF(x + 12, y + 25, 460, 16), t.EsqCentro);
                g.DrawString(numDir, t.RotuloProm, t.BrushTexto, new RectangleF(x + w - 230, y + 4, 218, 17), t.DirCentro);
                g.DrawString(valDir, t.DestaqueProm, t.BrushAzul, new RectangleF(x + w - 270, y + 22, 258, 20), t.DirCentro);

                var doc = new CursorDocumento(g, x + 12f, y + 54f, w - 24f);

                string clausula = ehOrcamento
                    ? "ORÇAMENTO DE PRODUTOS: Documento emitido exclusivamente para cotação de preços e condições, sem valor fiscal e sem obrigação de pagamento."
                    : temCliente
                        ? $"Aos {parcela.DataVencimento:dd/MM/yyyy}, pagarei(emos) por esta única via de NOTA PROMISSÓRIA a {NomeEstabelecimento} (CNPJ {CnpjEstabelecimento}), ou à sua ordem, a quantia de {parcela.Valor:C2} em moeda corrente deste país, pagável em {CidadeUfEstabelecimento}."
                        : "DOCUMENTO SEM VALOR DE NOTA PROMISSÓRIA: Emitido sem identificação de emitente/cliente, servindo exclusivamente como comprovante simples da operação, itens adquiridos e formas de pagamento.";
                doc.Esq(clausula, t.TextoProm, 6f, t.BrushTexto);

                string rotuloCli = ehOrcamento ? "CLIENTE" : "EMITENTE";
                doc.Colunas(new[] { 460f, doc.Largura - 460f }, new[] { StringAlignment.Near, StringAlignment.Far });
                doc.LinhaGrade(t.PenInterna, null, t.RotuloProm, t.BrushTexto, 22f, 0f,
                    $"{rotuloCli}: {(temCliente ? d.NomeCliente.ToUpper() : "Não informado")}",
                    $"CPF/CNPJ: {(temCliente && !string.IsNullOrWhiteSpace(d.DocumentoCliente) ? d.DocumentoCliente : "Não informado")}");

                doc.Colunas(new[] { 520f, doc.Largura - 520f }, new[] { StringAlignment.Near, StringAlignment.Far });
                doc.LinhaGrade(t.PenInterna, null, t.SubProm, t.BrushTexto, 22f, 6f,
                    $"ENDEREÇO: {(temCliente && !string.IsNullOrWhiteSpace(d.EnderecoCliente) ? d.EnderecoCliente : "Não informado")}",
                    $"TEL: {(temCliente && !string.IsNullOrWhiteSpace(d.TelefoneCliente) ? d.TelefoneCliente : "-")}");

                doc.Colunas(new[] { 56f, 50f, doc.Largura - 306f, 100f, 100f },
                    new[] { StringAlignment.Center, StringAlignment.Center, StringAlignment.Near, StringAlignment.Far, StringAlignment.Far });
                doc.LinhaGrade(t.PenInterna, t.BrushFaixa, t.RotuloProm, t.BrushTexto, 20f, 0f, "CÓD.", "QTD.", "PRODUTO / DESCRIÇÃO", "UNIT. (R$)", "TOTAL (R$)");

                if (d.Itens != null && d.Itens.Count > 0)
                {
                    foreach (var it in d.Itens)
                    {
                        string desc = it.Descricao.Length > 55 ? it.Descricao.Substring(0, 55).ToUpper() + "..." : it.Descricao.ToUpper();
                        doc.LinhaGrade(t.PenInterna, null, t.SubProm, t.BrushTexto, 17f, 0f,
                            it.IdProduto.ToString("D4"), it.Quantidade.ToString("00"), desc, it.PrecoUnitario.ToString("N2"), it.TotalItem.ToString("N2"));
                    }
                }
                else
                {
                    doc.Colunas(new[] { doc.Largura }, new[] { StringAlignment.Near });
                    doc.LinhaGrade(t.PenInterna, null, t.SubProm, t.BrushSuave, 17f, 0f, "Nenhum item registrado");
                }

                doc.Colunas(new[] { 430f, doc.Largura - 430f }, new[] { StringAlignment.Near, StringAlignment.Far });
                doc.LinhaGrade(t.PenInterna, t.BrushSubFaixa, t.RotuloProm, t.BrushTexto, 22f, 6f,
                    $"Subtotal: {sub:C2}{(d.Desconto > 0m ? $"   |   Desconto: - {d.Desconto:C2}" : "")}",
                    ehOrcamento ? $"TOTAL DO ORÇAMENTO: {tot:C2}" : $"TOTAL DA VENDA: {tot:C2}");

                bool temTroco = !ehOrcamento && d.Troco > 0m;
                string textoPagto = ehOrcamento
                    ? "Condição a definir no fechamento da venda."
                    : ObterResumoPagamentosTexto(d, tot);

                if (temTroco)
                {
                    string infoRecebido = d.ValorPago > 0m ? $"Valor Recebido: {d.ValorPago:C2}   |   " : string.Empty;
                    textoPagto += $"\n{infoRecebido}Troco: {d.Troco:C2}";
                }

                doc.Caixa("FORMAS DE PAGAMENTO:", textoPagto, t.RotuloProm, t.TextoProm, t.PenInterna, t.BrushSuave, t.BrushTexto, temTroco ? 48f : 40f, 6f);

                bool temObs = !string.IsNullOrWhiteSpace(d.Observacoes);
                doc.Caixa("OBSERVAÇÕES:", temObs ? d.Observacoes.Trim() : "Nenhuma observação registrada.",
                    t.RotuloProm, t.TextoProm, t.PenInterna, t.BrushSuave, temObs ? t.BrushTexto : t.BrushSuave, 42f, 10f);

                bool exibirAssinatura = temCliente && !ehOrcamento;
                float yRodape = doc.Y + (exibirAssinatura ? 12f : 4f);
                g.DrawString($"{CidadeUfEstabelecimento}, {d.DataEmissao:dd 'de' MMMM 'de' yyyy}", t.TextoProm, t.BrushTexto, new RectangleF(x + 14, yRodape - 8f, 330, 18f), t.EsqCentro);

                if (exibirAssinatura)
                {
                    g.DrawLine(t.PenExterna, x + w - 330f, yRodape, x + w - 16f, yRodape);
                    g.DrawString($"Assinatura do Emitente: {d.NomeCliente}", t.RotuloProm, t.BrushSuave, new RectangleF(x + w - 330f, yRodape + 2f, 314f, 16f), t.Centro);
                    doc.Y = yRodape + 24f;
                }
                else doc.Y = yRodape + 16f;

                float altura = doc.Y - y;
                g.DrawRectangle(t.PenExterna, x, y, w, altura);
                return altura;
            }
        }

        // =========================================================================
        // 3. MOTOR DE RENDERIZAÇÃO, PDF E IMPRESSÃO
        // =========================================================================
        public static List<Bitmap> RenderizarPaginasCupom(DadosDocumentoVenda dados)
        {
            var paginas = new List<Bitmap>();
            int indiceItem = 0;
            bool mais = true;

            while (mais)
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
                        mais = DesenharPaginaCupom(g, 830f, dados, ref indiceItem, out yFinal);
                    }
                    int alturaUtil = Math.Min(1700, Math.Max(420, (int)((yFinal + 20f) * 2.0f)));
                    paginas.Add(bmpBruto.Clone(new Rectangle(0, 0, 620, alturaUtil), bmpBruto.PixelFormat));
                }
            }
            return paginas;
        }

        public static List<Bitmap> RenderizarPaginasPromissoria(DadosDocumentoVenda dados)
        {
            if (dados.Parcelas == null || dados.Parcelas.Count == 0)
            {
                decimal tot = Math.Max(0m, dados.Itens.Sum(i => i.TotalItem) - dados.Desconto);
                dados.Parcelas = new List<ParcelaPromissoria>
                {
                    new ParcelaPromissoria { NumeroParcela = 1, TotalParcelas = 1, DataVencimento = dados.DataEmissao.AddDays(30), Valor = tot }
                };
            }

            var paginas = new List<Bitmap>();
            int idx = 0;
            while (idx < dados.Parcelas.Count)
            {
                var bmp = new Bitmap(1230, 1740);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                    g.ScaleTransform(1.5f, 1.5f);

                    float y = 28f;
                    int naPagina = 0;
                    bool exibirAssinatura = dados.PossuiClienteIdentificado && !dados.EhOrcamento;
                    float extraTroco = (!dados.EhOrcamento && dados.Troco > 0m) ? 8f : 0f;
                    float est = 300f + Math.Max(1, dados.Itens?.Count ?? 0) * 17f + (exibirAssinatura ? 20f : 0f) + extraTroco;

                    while (idx < dados.Parcelas.Count && naPagina < 2)
                    {
                        if (naPagina > 0 && (y + est > 1125f)) break;
                        float alt = DesenharPromissoriaEnxuta(g, dados, dados.Parcelas[idx], 40f, y, 740f);

                        if (naPagina == 0 && idx + 1 < dados.Parcelas.Count && (y + alt + 24f + est <= 1125f))
                            using (var penCorte = new Pen(Color.Gray, 1f) { DashStyle = DashStyle.Dash })
                                g.DrawLine(penCorte, 40f, y + alt + 12f, 780f, y + alt + 12f);

                        y += alt + 24f;
                        idx++;
                        naPagina++;
                    }
                }
                paginas.Add(bmp);
            }
            return paginas;
        }

        private static List<(string Data, string DescricaoMetodo, decimal Valor)> ObterLinhasPagamentoCupom(DadosDocumentoVenda d, decimal totalFinal)
        {
            var linhas = new List<(string, string, decimal)>();
            if (d.Pagamentos != null)
            {
                foreach (var pg in d.Pagamentos.Where(p => p.Valor > 0m))
                {
                    string m = (pg.Metodo ?? "DINHEIRO").ToUpper();
                    if (pg.EhAPrazo && pg.Parcelas >= 1)
                    {
                        decimal baseParc = Math.Floor((pg.Valor / pg.Parcelas) * 100m) / 100m;
                        decimal resto = pg.Valor - (baseParc * pg.Parcelas);
                        for (int i = 1; i <= pg.Parcelas; i++)
                            linhas.Add((d.DataEmissao.AddMonths(i).ToString("dd/MM/yyyy"), pg.Parcelas > 1 ? $"{m} ({i}/{pg.Parcelas})" : m, i == pg.Parcelas ? baseParc + resto : baseParc));
                    }
                    else linhas.Add((d.DataEmissao.ToString("dd/MM/yyyy"), pg.Parcelas > 1 ? $"{m} ({pg.Parcelas}x)" : m, pg.Valor));
                }
            }
            if (linhas.Count == 0)
                linhas.Add((d.DataEmissao.ToString("dd/MM/yyyy"), string.IsNullOrWhiteSpace(d.FormaPagamento) ? "DINHEIRO" : d.FormaPagamento.ToUpper(), totalFinal));
            return linhas;
        }

        private static string ObterResumoPagamentosTexto(DadosDocumentoVenda d, decimal totalFinal)
        {
            if (d.Pagamentos != null && d.Pagamentos.Any(p => p.Valor > 0m))
                return string.Join("   |   ", d.Pagamentos.Where(p => p.Valor > 0m).Select(p => $"{p.Metodo}{(p.Parcelas > 1 ? $" ({p.Parcelas}x)" : "")}: {p.Valor:C2}"));
            return !string.IsNullOrWhiteSpace(d.FormaPagamento) ? $"{d.FormaPagamento} ({totalFinal:C2})" : $"À Vista ({totalFinal:C2})";
        }

        public static void SalvarComoPdf(string caminhoArquivo, List<Bitmap> paginas)
        {
            if (paginas == null || paginas.Count == 0) throw new InvalidOperationException("Nenhuma página disponível.");
            using (var fs = new FileStream(caminhoArquivo, FileMode.Create, FileAccess.Write))
            {
                var offsets = new List<long> { 0 };
                void Escrever(string txt) { var b = Encoding.ASCII.GetBytes(txt); fs.Write(b, 0, b.Length); }

                Escrever("%PDF-1.4\n");
                offsets.Add(fs.Position); Escrever("1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n");
                offsets.Add(fs.Position); Escrever($"2 0 obj\n<< /Type /Pages /Kids [ {string.Join(" ", Enumerable.Range(0, paginas.Count).Select(i => $"{3 + i * 3} 0 R"))} ] /Count {paginas.Count} >>\nendobj\n");

                for (int i = 0; i < paginas.Count; i++)
                {
                    byte[] jpg;
                    using (var ms = new MemoryStream()) { paginas[i].Save(ms, ImageFormat.Jpeg); jpg = ms.ToArray(); }
                    string wPt = (paginas[i].Width * 72.0 / 150.0).ToString("0.##", CultureInfo.InvariantCulture);
                    string hPt = (paginas[i].Height * 72.0 / 150.0).ToString("0.##", CultureInfo.InvariantCulture);
                    int pId = 3 + i * 3, imId = 4 + i * 3, cId = 5 + i * 3;

                    offsets.Add(fs.Position); Escrever($"{pId} 0 obj\n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 {wPt} {hPt}] /Resources << /XObject << /Im0 {imId} 0 R >> >> /Contents {cId} 0 R >>\nendobj\n");
                    offsets.Add(fs.Position); Escrever($"{imId} 0 obj\n<< /Type /XObject /Subtype /Image /Width {paginas[i].Width} /Height {paginas[i].Height} /ColorSpace /DeviceRGB /BitsPerComponent 8 /Filter /DCTDecode /Length {jpg.Length} >>\nstream\n");
                    fs.Write(jpg, 0, jpg.Length); Escrever("\nendstream\nendobj\n");

                    string cmd = $"q {wPt} 0 0 {hPt} 0 0 cm /Im0 Do Q";
                    offsets.Add(fs.Position); Escrever($"{cId} 0 obj\n<< /Length {cmd.Length} >>\nstream\n{cmd}\nendstream\nendobj\n");
                }

                long xref = fs.Position;
                Escrever($"xref\n0 {offsets.Count}\n0000000000 65535 f \n" + string.Concat(offsets.Skip(1).Select(o => $"{o:D10} 00000 n \n")));
                Escrever($"trailer\n<< /Size {offsets.Count} /Root 1 0 R >>\nstartxref\n{xref}\n%%EOF");
            }
        }

        public static void ImprimirPaginasNaImpressora(List<Bitmap> paginas, string nomeImpressora, string nomeDocumento, bool formatoCupom80mm)
        {
            if (paginas == null || paginas.Count == 0) return;
            int idx = 0;
            using (var doc = new PrintDocument { DocumentName = nomeDocumento, PrintController = new StandardPrintController() })
            {
                doc.PrinterSettings.PrinterName = nomeImpressora;
                doc.DefaultPageSettings.Margins = formatoCupom80mm ? new Margins(5, 5, 5, 5) : new Margins(20, 20, 20, 20);
                if (formatoCupom80mm) doc.DefaultPageSettings.PaperSize = new PaperSize("Cupom80mm", 315, 850);

                doc.PrintPage += (s, e) =>
                {
                    var bmp = paginas[idx++];
                    float esc = Math.Min((float)e.MarginBounds.Width / bmp.Width, (float)e.MarginBounds.Height / bmp.Height);
                    e.Graphics.DrawImage(bmp, e.MarginBounds.Left, e.MarginBounds.Top, (int)(bmp.Width * esc), (int)(bmp.Height * esc));
                    e.HasMorePages = idx < paginas.Count;
                };
                doc.Print();
            }
        }

        private sealed class TemaDoc : IDisposable
        {
            public readonly Pen PenLinha = new Pen(Color.Black, 1f);
            public readonly Pen PenExterna = new Pen(Color.FromArgb(95, 105, 118), 1.4f);
            public readonly Pen PenInterna = new Pen(Color.FromArgb(165, 172, 182), 0.9f);
            public readonly SolidBrush BrushFaixa = new SolidBrush(Color.FromArgb(236, 243, 253));
            public readonly SolidBrush BrushSubFaixa = new SolidBrush(Color.FromArgb(246, 248, 252));
            public readonly SolidBrush BrushAzul = new SolidBrush(Color.FromArgb(15, 70, 165));
            public readonly SolidBrush BrushTexto = new SolidBrush(Color.FromArgb(28, 32, 38));
            public readonly SolidBrush BrushSuave = new SolidBrush(Color.FromArgb(85, 92, 102));

            public readonly Font Fantasia = new Font("Arial", 11.5f, FontStyle.Bold);
            public readonly Font NumComp = new Font("Arial", 10.5f, FontStyle.Bold);
            public readonly Font Total = new Font("Arial", 9.8f, FontStyle.Bold);
            public readonly Font Peq = new Font("Arial", 8f, FontStyle.Regular);
            public readonly Font PeqNeg = new Font("Arial", 8f, FontStyle.Bold);
            public readonly Font Item = new Font("Arial", 7.8f, FontStyle.Bold);
            public readonly Font Aceite = new Font("Arial", 7.2f, FontStyle.Regular);
            public readonly Font Micro = new Font("Arial", 6f, FontStyle.Bold);

            public readonly Font TituloProm = new Font("Segoe UI", 12f, FontStyle.Bold);
            public readonly Font DestaqueProm = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            public readonly Font TextoProm = new Font("Segoe UI", 8.2f, FontStyle.Regular);
            public readonly Font SubProm = new Font("Segoe UI", 8f, FontStyle.Regular);
            public readonly Font RotuloProm = new Font("Segoe UI", 7.8f, FontStyle.Bold);

            public readonly StringFormat EsqCentro = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
            public readonly StringFormat DirCentro = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };
            public readonly StringFormat Centro = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            public void Dispose()
            {
                PenLinha.Dispose(); PenExterna.Dispose(); PenInterna.Dispose();
                BrushFaixa.Dispose(); BrushSubFaixa.Dispose(); BrushAzul.Dispose(); BrushTexto.Dispose(); BrushSuave.Dispose();
                Fantasia.Dispose(); NumComp.Dispose(); Total.Dispose(); Peq.Dispose(); PeqNeg.Dispose(); Item.Dispose(); Aceite.Dispose(); Micro.Dispose();
                TituloProm.Dispose(); DestaqueProm.Dispose(); TextoProm.Dispose(); SubProm.Dispose(); RotuloProm.Dispose();
            }
        }

        private sealed class CursorDocumento
        {
            private readonly Graphics _g;
            private readonly float _x;
            private float[] _larguras;
            private StringAlignment[] _alinhamentos;
            public float Largura { get; }
            public float Y { get; set; }

            public CursorDocumento(Graphics g, float x, float y, float largura) { _g = g; _x = x; Y = y; Largura = largura; }
            public bool Cabe(float alt, float limite) => (Y + alt) <= limite;
            public void Sep(float antes = 2f, float depois = 4f) { Y += antes; _g.DrawLine(Pens.Black, _x, Y, _x + Largura, Y); Y += depois; }
            public void Centro(string txt, Font f, float dy = 1f, Brush b = null) => Texto(txt, f, b ?? Brushes.Black, StringAlignment.Center, dy);
            public void Esq(string txt, Font f, float dy = 1f, Brush b = null) => Texto(txt, f, b ?? Brushes.Black, StringAlignment.Near, dy);
            public void Dir(string txt, Font f, float dy = 1f, Brush b = null) => Texto(txt, f, b ?? Brushes.Black, StringAlignment.Far, dy);

            private void Texto(string txt, Font f, Brush b, StringAlignment al, float dy)
            {
                float h = _g.MeasureString(txt, f, (int)Largura).Height;
                _g.DrawString(txt, f, b, new RectangleF(_x, Y, Largura, h + 2f), new StringFormat { Alignment = al });
                Y += h + dy;
            }

            public void Par(string esq, string dir, Font fEsq, float dy = 1f, Font fDir = null)
            {
                fDir = fDir ?? fEsq;
                float h = Math.Max(fEsq.GetHeight(_g), fDir.GetHeight(_g));
                _g.DrawString(esq, fEsq, Brushes.Black, new RectangleF(_x, Y, Largura, h + 2f), new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                _g.DrawString(dir, fDir, Brushes.Black, new RectangleF(_x, Y, Largura, h + 2f), new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });
                Y += h + dy;
            }

            public void Colunas(float[] larguras, StringAlignment[] alinhamentos) { _larguras = larguras; _alinhamentos = alinhamentos; }

            public void LinhaTexto(Font f, float h, params string[] textos)
            {
                float x = _x;
                for (int i = 0; i < Math.Min(textos.Length, _larguras.Length); i++)
                {
                    if (!string.IsNullOrEmpty(textos[i]))
                        _g.DrawString(textos[i], f, Brushes.Black, new RectangleF(x, Y, _larguras[i], h), new StringFormat { Alignment = _alinhamentos[i], LineAlignment = StringAlignment.Center });
                    x += _larguras[i];
                }
                Y += h;
            }

            public void LinhaGrade(Pen borda, Brush fundo, Font f, Brush bTxt, float h, float dy, params string[] textos)
            {
                if (fundo != null) _g.FillRectangle(fundo, _x, Y, Largura, h);
                float x = _x;
                for (int i = 0; i < Math.Min(textos.Length, _larguras.Length); i++)
                {
                    _g.DrawRectangle(borda, x, Y, _larguras[i], h);
                    float pad = _alinhamentos[i] == StringAlignment.Center ? 2f : 6f;
                    if (!string.IsNullOrEmpty(textos[i]))
                        _g.DrawString(textos[i], f, bTxt, new RectangleF(x + pad, Y, Math.Max(4f, _larguras[i] - pad * 2f), h), new StringFormat { Alignment = _alinhamentos[i], LineAlignment = StringAlignment.Center });
                    x += _larguras[i];
                }
                Y += h + dy;
            }

            public void Caixa(string titulo, string conteudo, Font fTit, Font fCont, Pen borda, Brush bTit, Brush bCont, float h, float dy = 6f)
            {
                _g.DrawRectangle(borda, _x, Y, Largura, h);
                _g.DrawString(titulo, fTit, bTit, new RectangleF(_x + 6f, Y + 2f, Largura - 12f, 14f));
                _g.DrawString(conteudo, fCont, bCont, new RectangleF(_x + 6f, Y + 16f, Largura - 12f, h - 18f));
                Y += h + dy;
            }
        }
    }
}