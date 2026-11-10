using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoExtensionista2026_2
{
    public class ItemVenda
    {
        public long IdProduto { get; set; }
        public string Descricao { get; set; }
        public decimal Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal TotalItem => Quantidade * PrecoUnitario;
    }
}
