using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    internal static class ExportadorCsv
    {



        public static void ExportarDataGridView(DataGridView dgv, string nomeArquivoSugerido, string tituloDialogo = "Exportar Relatório CSV")
        {
            if (dgv == null || dgv.Rows.Count == 0)
            {
                MessageBox.Show("Não há dados na tabela para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Arquivo CSV (*.csv)|*.csv";
                sfd.Title = tituloDialogo;
                sfd.FileName = nomeArquivoSugerido;

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    var sb = new StringBuilder();


                    var headers = new string[dgv.Columns.Count];
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        headers[i] = EscaparCampoCsv(dgv.Columns[i].HeaderText);
                    }
                    sb.AppendLine(string.Join(";", headers));


                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (row.IsNewRow || !row.Visible) continue;

                        var cells = new string[dgv.Columns.Count];
                        for (int i = 0; i < dgv.Columns.Count; i++)
                        {
                            string texto = row.Cells[i].FormattedValue?.ToString()
                                ?? row.Cells[i].Value?.ToString()
                                ?? string.Empty;

                            texto = texto.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ").Trim();
                            cells[i] = EscaparCampoCsv(texto);
                        }
                        sb.AppendLine(string.Join(";", cells));
                    }


                    File.WriteAllText(sfd.FileName, sb.ToString(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
                    MessageBox.Show("Relatório exportado com sucesso!", "Exportação Concluída", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao salvar o arquivo CSV:\n\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static string EscaparCampoCsv(string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return "\"\"";

            return $"\"{valor.Replace("\"", "\"\"")}\"";
        }
    }
}
