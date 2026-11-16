using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace ProjetoExtensionista2026_2.SQL.SQL_functions
{
    internal class OrcamentoParaEfetivar
    {
        public long IdVenda { get; set; }
        public long IdCliente { get; set; } = 1;
        public decimal Subtotal { get; set; }
        public decimal Desconto { get; set; }
        public decimal Total { get; set; }
        public List<ItemVenda> Itens { get; set; } = new List<ItemVenda>();
    }

    internal class DetalhesVendaResult
    {
        public long IdVenda { get; set; }
        public DateTime DataVenda { get; set; }
        public string ClienteNome { get; set; } = "Cliente não informado";
        public string ClienteDocumento { get; set; } = string.Empty;
        public string Vendedor { get; set; } = "Sistema";
        public string Tipo { get; set; } = "VENDA";
        public string Status { get; set; } = "PAGO";
        public string Observacoes { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal Desconto { get; set; }
        public decimal Total { get; set; }
        public DataTable Itens { get; set; } = new DataTable();
        public DataTable Pagamentos { get; set; } = new DataTable();
    }

    internal static class VendaSQL
    {
        private static bool _schemaGarantido = false;

        /// <summary>
        /// Garante que a coluna seller_name exista na tabela public.sale.
        /// </summary>
        public static void EnsureSchema(NpgsqlConnection conexaoExistente = null, NpgsqlTransaction transacao = null)
        {
            if (_schemaGarantido) return;

            const string sql = @"ALTER TABLE public.sale ADD COLUMN IF NOT EXISTS seller_name VARCHAR(120);";

            if (conexaoExistente != null)
            {
                using (var cmd = new NpgsqlCommand(sql, conexaoExistente, transacao))
                {
                    cmd.ExecuteNonQuery();
                }
                _schemaGarantido = true;
                return;
            }

            using (var db = new Connection())
            {
                db.IniciarConexao();
                using (var cmd = new NpgsqlCommand(sql, db.Conexao))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            _schemaGarantido = true;
        }

        public static DataTable ListarVendas(DateTime dataInicio, DateTime dataFim, string tipo, string status, string termoBusca)
        {
            EnsureSchema();

            var tabela = new DataTable();
            DateTime inicio = dataInicio.Date;
            DateTime fimExclusivo = dataFim.Date.AddDays(1);

            string sql = @"
                SELECT
                    s.id_sale AS ""ID"",
                    s.sale_date AS ""Data/Hora"",
                    COALESCE(NULLIF(TRIM(c.name), ''), 'Consumidor Não Identificado') AS ""Cliente"",
                    COALESCE(NULLIF(TRIM(s.seller_name), ''), 'Não informado') AS ""Vendedor"",
                    COALESCE(s.sale_type, 'VENDA') AS ""Tipo"",
                    COALESCE(sp.formas, CASE WHEN s.sale_type = 'ORCAMENTO' THEN 'Orçamento' ELSE '-' END) AS ""Pagamento"",
                    COALESCE(s.payment_status, 'PAGO') AS ""Status"",
                    COALESCE(s.discount, 0) AS ""Desconto (R$)"",
                    COALESCE(s.total, 0) AS ""Total (R$)""
                FROM public.sale s
                LEFT JOIN public.customer c ON c.id_customer = s.id_client
                LEFT JOIN (
                    SELECT
                        id_sale,
                        STRING_AGG(DISTINCT payment_method, ' + ') AS formas
                    FROM public.sale_payment
                    GROUP BY id_sale
                ) sp ON sp.id_sale = s.id_sale
                WHERE s.sale_date >= (@inicio::date AT TIME ZONE 'America/Sao_Paulo')
                  AND s.sale_date < ((@fim::date + INTERVAL '1 day') AT TIME ZONE 'America/Sao_Paulo')
                  AND (@tipo = 'Todos' OR UPPER(COALESCE(s.sale_type, 'VENDA')) = UPPER(@tipo))
                  AND (@status = 'Todos' OR UPPER(COALESCE(s.payment_status, 'PAGO')) = UPPER(@status))
                  AND (
                        @busca = ''
                        OR CAST(s.id_sale AS TEXT) ILIKE @buscaLike
                        OR COALESCE(c.name, '') ILIKE @buscaLike
                        OR COALESCE(c.cpf_cnpj, '') ILIKE @buscaLike
                        OR COALESCE(s.seller_name, '') ILIKE @buscaLike
                  )
                ORDER BY s.sale_date DESC, s.id_sale DESC
                LIMIT 500;";

            string buscaLimpa = (termoBusca ?? string.Empty).Trim();

            using (var db = new Connection())
            {
                db.IniciarConexao();
                using (var cmd = new NpgsqlCommand(sql, db.Conexao))
                {
                    cmd.Parameters.Add(new NpgsqlParameter("inicio", NpgsqlDbType.Timestamp) { Value = inicio });
                    cmd.Parameters.Add(new NpgsqlParameter("fim", NpgsqlDbType.Timestamp) { Value = fimExclusivo });
                    cmd.Parameters.Add(new NpgsqlParameter("tipo", NpgsqlDbType.Text) { Value = string.IsNullOrWhiteSpace(tipo) ? "Todos" : tipo });
                    cmd.Parameters.Add(new NpgsqlParameter("status", NpgsqlDbType.Text) { Value = string.IsNullOrWhiteSpace(status) ? "Todos" : status });
                    cmd.Parameters.Add(new NpgsqlParameter("busca", NpgsqlDbType.Text) { Value = buscaLimpa });
                    cmd.Parameters.Add(new NpgsqlParameter("buscaLike", NpgsqlDbType.Text) { Value = $"%{buscaLimpa}%" });

                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        adapter.Fill(tabela);
                    }
                }
            }

            return tabela;
        }

        public static DetalhesVendaResult ObterDetalhesVenda(long idSale)
        {
            EnsureSchema();

            var detalhes = new DetalhesVendaResult { IdVenda = idSale };

            using (var db = new Connection())
            {
                db.IniciarConexao();

                const string sqlCabecalho = @"
                    SELECT
                        s.id_sale,
                        s.sale_date,
                        COALESCE(NULLIF(TRIM(c.name), ''), 'Consumidor Não Identificado') AS cliente_nome,
                        COALESCE(c.cpf_cnpj, '') AS cliente_doc,
                        COALESCE(NULLIF(TRIM(s.seller_name), ''), 'Não informado') AS vendedor,
                        COALESCE(s.sale_type, 'VENDA') AS tipo,
                        COALESCE(s.payment_status, 'PAGO') AS status,
                        COALESCE(s.notes, '') AS notes,
                        COALESCE(s.subtotal, 0) AS subtotal,
                        COALESCE(s.discount, 0) AS discount,
                        COALESCE(s.total, 0) AS total
                    FROM public.sale s
                    LEFT JOIN public.customer c ON c.id_customer = s.id_client
                    WHERE s.id_sale = @id_sale
                    LIMIT 1;";

                using (var cmd = new NpgsqlCommand(sqlCabecalho, db.Conexao))
                {
                    cmd.Parameters.Add(new NpgsqlParameter("id_sale", NpgsqlDbType.Bigint) { Value = idSale });
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            detalhes.DataVenda = reader.IsDBNull(1) ? DateTime.Now : reader.GetDateTime(1);
                            detalhes.ClienteNome = reader.GetString(2);
                            detalhes.ClienteDocumento = reader.GetString(3);
                            detalhes.Vendedor = reader.GetString(4);
                            detalhes.Tipo = reader.GetString(5);
                            detalhes.Status = reader.GetString(6);
                            detalhes.Observacoes = reader.GetString(7);
                            detalhes.Subtotal = reader.GetDecimal(8);
                            detalhes.Desconto = reader.GetDecimal(9);
                            detalhes.Total = reader.GetDecimal(10);
                        }
                    }
                }

                const string sqlItens = @"
                    SELECT
                        sp.id_product AS ""Cód."",
                        COALESCE(p.name, 'Produto #' || sp.id_product) AS ""Produto"",
                        sp.quantity AS ""Qtd."",
                        sp.unit_price AS ""Unit. (R$)"",
                        sp.subtotal AS ""Subtotal (R$)""
                    FROM public.sale_product sp
                    LEFT JOIN public.product p ON p.id_product = sp.id_product
                    WHERE sp.id_sale = @id_sale
                    ORDER BY sp.id_product ASC;";

                using (var cmdItens = new NpgsqlCommand(sqlItens, db.Conexao))
                {
                    cmdItens.Parameters.Add(new NpgsqlParameter("id_sale", NpgsqlDbType.Bigint) { Value = idSale });
                    using (var adapter = new NpgsqlDataAdapter(cmdItens))
                    {
                        adapter.Fill(detalhes.Itens);
                    }
                }

                const string sqlPagamentos = @"
                    SELECT
                        payment_method AS ""Forma de Pagamento"",
                        amount AS ""Valor (R$)"",
                        installments || 'x' AS ""Parcelas""
                    FROM public.sale_payment
                    WHERE id_sale = @id_sale
                    ORDER BY created_at ASC;";

                using (var cmdPag = new NpgsqlCommand(sqlPagamentos, db.Conexao))
                {
                    cmdPag.Parameters.Add(new NpgsqlParameter("id_sale", NpgsqlDbType.Bigint) { Value = idSale });
                    using (var adapter = new NpgsqlDataAdapter(cmdPag))
                    {
                        adapter.Fill(detalhes.Pagamentos);
                    }
                }
            }

            return detalhes;
        }

        public static OrcamentoParaEfetivar ObterOrcamentoParaEfetivar(long idSale)
        {
            EnsureSchema();
            var resultado = new OrcamentoParaEfetivar { IdVenda = idSale };

            using (var db = new Connection())
            {
                db.IniciarConexao();

                const string sqlCab = @"
                    SELECT COALESCE(id_client, 1), COALESCE(subtotal, 0), COALESCE(discount, 0), COALESCE(total, 0)
                    FROM public.sale
                    WHERE id_sale = @id_sale
                    LIMIT 1;";

                using (var cmd = new NpgsqlCommand(sqlCab, db.Conexao))
                {
                    cmd.Parameters.Add(new NpgsqlParameter("id_sale", NpgsqlDbType.Bigint) { Value = idSale });
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            resultado.IdCliente = r.GetInt64(0);
                            resultado.Subtotal = r.GetDecimal(1);
                            resultado.Desconto = r.GetDecimal(2);
                            resultado.Total = r.GetDecimal(3);
                        }
                    }
                }

                const string sqlItens = @"
                    SELECT
                        sp.id_product,
                        COALESCE(p.name, 'Produto #' || sp.id_product) AS nome,
                        sp.quantity,
                        sp.unit_price
                    FROM public.sale_product sp
                    LEFT JOIN public.product p ON p.id_product = sp.id_product
                    WHERE sp.id_sale = @id_sale
                    ORDER BY sp.id_product ASC;";

                using (var cmdItens = new NpgsqlCommand(sqlItens, db.Conexao))
                {
                    cmdItens.Parameters.Add(new NpgsqlParameter("id_sale", NpgsqlDbType.Bigint) { Value = idSale });
                    using (var r = cmdItens.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            resultado.Itens.Add(new ItemVenda
                            {
                                IdProduto = r.GetInt64(0),
                                Descricao = r.GetString(1),
                                Quantidade = r.GetDecimal(2),
                                PrecoUnitario = r.GetDecimal(3)
                            });
                        }
                    }
                }
            }

            return resultado;
        }

        public static DadosDocumentoVenda ObterDadosDocumento(long idSale)
        {
            EnsureSchema();

            var dados = new DadosDocumentoVenda
            {
                NumeroVenda = (int)idSale,
                NomeCliente = "Consumidor Não Identificado"
            };

            long idCliente = 1;

            using (var db = new Connection())
            {
                db.IniciarConexao();

                const string sqlSale = @"
                    SELECT
                        s.sale_date,
                        COALESCE(s.sale_type, 'VENDA'),
                        COALESCE(s.discount, 0),
                        COALESCE(s.total, 0),
                        COALESCE(s.notes, ''),
                        COALESCE(s.id_client, 1),
                        COALESCE(NULLIF(TRIM(s.seller_name), ''), 'Sistema')
                    FROM public.sale s
                    WHERE s.id_sale = @id_sale
                    LIMIT 1;";

                using (var cmd = new NpgsqlCommand(sqlSale, db.Conexao))
                {
                    cmd.Parameters.Add(new NpgsqlParameter("id_sale", NpgsqlDbType.Bigint) { Value = idSale });
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            dados.DataEmissao = r.IsDBNull(0) ? DateTime.Now : r.GetDateTime(0);
                            dados.EhOrcamento = r.GetString(1).Equals("ORCAMENTO", StringComparison.OrdinalIgnoreCase);
                            dados.Desconto = r.GetDecimal(2);
                            dados.ValorPago = r.GetDecimal(3);
                            dados.Observacoes = r.GetString(4);
                            idCliente = r.GetInt64(5);
                            dados.NomeVendedor = r.GetString(6);
                        }
                    }
                }

                if (idCliente > 1)
                {
                    const string sqlCliente = @"SELECT * FROM public.customer WHERE id_customer = @id_customer LIMIT 1;";
                    using (var cmdCli = new NpgsqlCommand(sqlCliente, db.Conexao))
                    {
                        cmdCli.Parameters.Add(new NpgsqlParameter("id_customer", NpgsqlDbType.Bigint) { Value = idCliente });
                        using (var r = cmdCli.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                dados.NomeCliente = LerTexto(r, "name", "nome");
                                if (string.IsNullOrWhiteSpace(dados.NomeCliente)) dados.NomeCliente = "Cliente Cadastrado";
                                dados.DocumentoCliente = LerTexto(r, "cpf_cnpj", "cpf", "cnpj", "document");
                                dados.TelefoneCliente = LerTexto(r, "phone", "telefone", "cellphone");
                                dados.CidadeCliente = LerTexto(r, "city", "cidade");
                                dados.UfCliente = LerTexto(r, "state", "uf", "estado");
                                dados.CepCliente = LerTexto(r, "zip_code", "cep");

                                string logradouro = LerTexto(r, "address", "street", "logradouro", "endereco");
                                string numero = LerTexto(r, "number", "numero");
                                string bairro = LerTexto(r, "neighborhood", "bairro");

                                var partes = new List<string>();
                                if (!string.IsNullOrWhiteSpace(logradouro))
                                    partes.Add(string.IsNullOrWhiteSpace(numero) ? logradouro : $"{logradouro}, {numero}");
                                if (!string.IsNullOrWhiteSpace(bairro))
                                    partes.Add(bairro);
                                if (!string.IsNullOrWhiteSpace(dados.CidadeCliente))
                                    partes.Add(string.IsNullOrWhiteSpace(dados.UfCliente) ? dados.CidadeCliente : $"{dados.CidadeCliente}/{dados.UfCliente}");

                                dados.EnderecoCliente = string.Join(" - ", partes);
                            }
                        }
                    }
                }

                const string sqlItens = @"
                    SELECT
                        sp.id_product,
                        COALESCE(p.name, 'Produto #' || sp.id_product) AS nome,
                        sp.quantity,
                        sp.unit_price
                    FROM public.sale_product sp
                    LEFT JOIN public.product p ON p.id_product = sp.id_product
                    WHERE sp.id_sale = @id_sale
                    ORDER BY sp.id_product ASC;";

                using (var cmdItens = new NpgsqlCommand(sqlItens, db.Conexao))
                {
                    cmdItens.Parameters.Add(new NpgsqlParameter("id_sale", NpgsqlDbType.Bigint) { Value = idSale });
                    using (var r = cmdItens.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            dados.Itens.Add(new ItemVenda
                            {
                                IdProduto = r.GetInt64(0),
                                Descricao = r.GetString(1),
                                Quantidade = r.GetDecimal(2),
                                PrecoUnitario = r.GetDecimal(3)
                            });
                        }
                    }
                }

                if (dados.EhOrcamento)
                {
                    dados.FormaPagamento = "A definir (Orçamento)";
                    if (string.IsNullOrWhiteSpace(dados.Observacoes))
                        dados.Observacoes = "Orçamento gerado pelo PDV — sujeito à disponibilidade de estoque.";
                }
                else
                {
                    const string sqlPag = @"
                        SELECT payment_method, amount, COALESCE(installments, 1)
                        FROM public.sale_payment
                        WHERE id_sale = @id_sale
                        ORDER BY created_at ASC;";

                    var metodos = new List<string>();
                    decimal somaPagamentos = 0m;

                    using (var cmdPag = new NpgsqlCommand(sqlPag, db.Conexao))
                    {
                        cmdPag.Parameters.Add(new NpgsqlParameter("id_sale", NpgsqlDbType.Bigint) { Value = idSale });
                        using (var r = cmdPag.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                string metodo = r.GetString(0);
                                decimal valor = r.GetDecimal(1);
                                int parcelas = r.GetInt32(2);
                                somaPagamentos += valor;

                                string lower = metodo.ToLowerInvariant();
                                bool ehPrazo = lower.Contains("carnê") || lower.Contains("carne") ||
                                               lower.Contains("prazo") || lower.Contains("fiado");

                                metodos.Add(parcelas > 1 ? $"{metodo} ({parcelas}x)" : metodo);
                                dados.Pagamentos.Add(new PagamentoDocumento
                                {
                                    Metodo = metodo,
                                    Valor = valor,
                                    Parcelas = parcelas,
                                    EhAPrazo = ehPrazo
                                });

                                if (ehPrazo && parcelas > 0)
                                {
                                    decimal valorBase = Math.Floor((valor / parcelas) * 100m) / 100m;
                                    decimal diff = valor - (valorBase * parcelas);
                                    for (int i = 1; i <= parcelas; i++)
                                    {
                                        dados.Parcelas.Add(new ParcelaPromissoria
                                        {
                                            NumeroParcela = i,
                                            TotalParcelas = parcelas,
                                            DataVencimento = dados.DataEmissao.AddMonths(i),
                                            Valor = (i == parcelas) ? valorBase + diff : valorBase
                                        });
                                    }
                                }
                            }
                        }
                    }

                    dados.FormaPagamento = metodos.Count > 0 ? string.Join(" + ", metodos) : "Não informado";
                    if (somaPagamentos > 0m)
                        dados.ValorPago = somaPagamentos;
                }
            }

            return dados;
        }

        public static void AtualizarFlagsDocumentos(long idSale, bool emitiuCupom, bool emitiuPromissoria)
        {
            using (var db = new Connection())
            {
                db.IniciarConexao();
                const string sql = @"
                    UPDATE public.sale
                    SET issued_fiscal_coupon = @cupom,
                        issued_promissory_note = @promissoria
                    WHERE id_sale = @id_sale;";

                using (var cmd = new NpgsqlCommand(sql, db.Conexao))
                {
                    cmd.Parameters.Add(new NpgsqlParameter("cupom", NpgsqlDbType.Boolean) { Value = emitiuCupom });
                    cmd.Parameters.Add(new NpgsqlParameter("promissoria", NpgsqlDbType.Boolean) { Value = emitiuPromissoria });
                    cmd.Parameters.Add(new NpgsqlParameter("id_sale", NpgsqlDbType.Bigint) { Value = idSale });
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static bool QuitarPendencia(long idSale, string usuario)
        {
            using (var db = new Connection())
            {
                db.IniciarConexao();
                const string sql = @"
                    UPDATE public.sale
                    SET payment_status = 'PAGO'
                    WHERE id_sale = @id_sale
                      AND UPPER(COALESCE(payment_status, '')) = 'PENDENTE';";

                using (var cmd = new NpgsqlCommand(sql, db.Conexao))
                {
                    cmd.Parameters.Add(new NpgsqlParameter("id_sale", NpgsqlDbType.Bigint) { Value = idSale });
                    int afetadas = cmd.ExecuteNonQuery();
                    if (afetadas > 0)
                    {
                        AuditLogSQL.Register($"Venda #{idSale} quitada (Carnê/A Prazo)", usuario);
                        return true;
                    }
                    return false;
                }
            }
        }

        public static bool CancelarRegistro(long idSale, string usuario)
        {
            using (var db = new Connection())
            {
                db.IniciarConexao();
                using (var tx = db.Conexao.BeginTransaction())
                {
                    try
                    {
                        string tipo = "VENDA";
                        string status = "";

                        const string sqlSelect = @"
                            SELECT COALESCE(sale_type, 'VENDA'), COALESCE(payment_status, '')
                            FROM public.sale
                            WHERE id_sale = @id_sale
                            FOR UPDATE;";

                        using (var cmdSel = new NpgsqlCommand(sqlSelect, db.Conexao, tx))
                        {
                            cmdSel.Parameters.Add(new NpgsqlParameter("id_sale", NpgsqlDbType.Bigint) { Value = idSale });
                            using (var r = cmdSel.ExecuteReader())
                            {
                                if (!r.Read()) return false;
                                tipo = r.GetString(0).ToUpperInvariant();
                                status = r.GetString(1).ToUpperInvariant();
                            }
                        }

                        if (status == "CANCELADA")
                            return false;

                        const string sqlCancel = @"
                            UPDATE public.sale
                            SET payment_status = 'CANCELADA'
                            WHERE id_sale = @id_sale;";

                        using (var cmdCancel = new NpgsqlCommand(sqlCancel, db.Conexao, tx))
                        {
                            cmdCancel.Parameters.Add(new NpgsqlParameter("id_sale", NpgsqlDbType.Bigint) { Value = idSale });
                            cmdCancel.ExecuteNonQuery();
                        }

                        bool estornouEstoque = false;
                        if (tipo != "ORCAMENTO")
                        {
                            const string sqlEstorno = @"
                                UPDATE public.product p
                                SET stock_quantity = p.stock_quantity + sp.quantity
                                FROM public.sale_product sp
                                WHERE sp.id_sale = @id_sale
                                  AND p.id_product = sp.id_product;";

                            using (var cmdEstorno = new NpgsqlCommand(sqlEstorno, db.Conexao, tx))
                            {
                                cmdEstorno.Parameters.Add(new NpgsqlParameter("id_sale", NpgsqlDbType.Bigint) { Value = idSale });
                                cmdEstorno.ExecuteNonQuery();
                                estornouEstoque = true;
                            }
                        }

                        tx.Commit();

                        string acaoLog = estornouEstoque
                            ? $"Venda #{idSale} cancelada com estorno de estoque"
                            : $"Orçamento #{idSale} cancelado";
                        AuditLogSQL.Register(acaoLog, usuario);
                        return true;
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        private static string LerTexto(NpgsqlDataReader reader, params string[] possiveisColunas)
        {
            foreach (string coluna in possiveisColunas)
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).Equals(coluna, StringComparison.OrdinalIgnoreCase))
                        return reader.IsDBNull(i) ? string.Empty : reader.GetValue(i).ToString();
                }
            }
            return string.Empty;
        }
    }
}
