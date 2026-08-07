using System;
using System.Collections.Generic;
using System.Text;

namespace App_Banco_Simples
{
    internal static class ArquivoContas
    {
        public static List<ContaBancaria> Carregar();

        public static void Salvar(List<ContaBancaria> contas);

        public static ContaBancaria BuscarConta(int numero);

        public static ContaBancaria Login(int numero, int senha);

        public static bool ExisteConta(int numero);

        public static int GerarNumeroConta();

        public static void AdicionarConta(ContaBancaria conta);
    }
}
