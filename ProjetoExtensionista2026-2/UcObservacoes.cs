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




        public string ObterObservacoes()
        {
            return txtDescricao?.Text?.Trim() ?? string.Empty;
        }




        public void DefinirObservacoes(string texto)
        {
            if (txtDescricao != null)
                txtDescricao.Text = texto ?? string.Empty;
        }




        public void Limpar()
        {
            if (txtDescricao != null)
                txtDescricao.Clear();
        }
    }
}