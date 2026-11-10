using System;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    public partial class UcObservacoes : UserControl
    {
        public UcObservacoes()
        {
            InitializeComponent();
            ConfigurarCampoObservacoes();
        }

        private void ConfigurarCampoObservacoes()
        {
            if (txtDescricao == null) return;
        }

        /// <summary>
        /// Retorna o texto digitado em txtDescricao para ser gravado em sale.notes
        /// </summary>
        public string ObterObservacoes()
        {
            return txtDescricao?.Text?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Define um texto prévio caso esteja editando uma venda já aberta
        /// </summary>
        public void DefinirObservacoes(string texto)
        {
            if (txtDescricao != null)
                txtDescricao.Text = texto ?? string.Empty;
        }

        /// <summary>
        /// Limpa o campo de observações para a próxima venda
        /// </summary>
        public void Limpar()
        {
            if (txtDescricao != null)
                txtDescricao.Clear();
        }
    }
}