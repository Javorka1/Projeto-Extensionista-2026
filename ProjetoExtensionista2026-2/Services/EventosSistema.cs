using System;

namespace ProjetoExtensionista2026_2
{
    public static class EventosSistema
    {
        public static event Action VendaConcluida;
        public static event Action EstoqueAtualizado;
        public static event Action ClienteAtualizado;

        public static void NotificarVendaConcluida() => VendaConcluida?.Invoke();
        public static void NotificarEstoqueAtualizado() => EstoqueAtualizado?.Invoke();
        public static void NotificarClienteAtualizado() => ClienteAtualizado?.Invoke();
    }
}