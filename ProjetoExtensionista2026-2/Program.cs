using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2
{
    internal static class Program
    {



        private static string token2 = "Pablo é lindo";
        public static string SenhaAtual { get; set; } = "123456";

        public static bool VerificarSenha(string senhaDigitada)
        {
            return senhaDigitada == SenhaAtual;
        }

        public static bool AlterarSenha(string senhaAtualDigitada, string novaSenha)
        {
            if (senhaAtualDigitada != SenhaAtual)
            {
                return false;
            }
            SenhaAtual = novaSenha;
            return true;
        }

        public static bool verificarHash2(string hashSended, string token)
        {
            if (token == token2)
            {
                if (hashSended == SenhaAtual.GetHashCode().ToString())
                {
                    MessageBox.Show("Hash correta!");
                    return true;
                }
                else
                {
                    MessageBox.Show("Hash incorreta!");
                    return false;
                }
            }
            return false;
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            GerenciadorTema.CarregarTemaSalvo();

            Application.Run(new formLogin());
        }
    }
}
