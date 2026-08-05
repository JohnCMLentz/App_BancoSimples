using System;
using System.Globalization;


namespace App_Banco_Simples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bem-vindo ao Banco Simples!");
            // Solicitar informações do usuário
            Console.Write("Digite o nome do titular da conta: ");
            string nomeTitular = Console.ReadLine();
            Console.Write("Digite o número da conta: ");
            int numeroConta = int.Parse(Console.ReadLine());
            Console.Write("Digite o saldo inicial da conta: ");
            double saldoInicial = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
            // Criar uma instância da classe ContaBancaria
            ContaBancaria conta = new ContaBancaria(nomeTitular, numeroConta, saldoInicial);
            // Exibir informações da conta
            Console.WriteLine("\nInformações da Conta:");
            Console.WriteLine($"Titular: {conta.NomeTitular}");
            Console.WriteLine($"Número da Conta: {conta.NumeroConta}");
            Console.WriteLine($"Saldo Inicial: {conta.Saldo.ToString("F2", CultureInfo.InvariantCulture)}");
            // Solicitar depósito
            Console.Write("\nDigite o valor a ser depositado: ");
            double valorDeposito = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
            conta.Depositar(valorDeposito);
            Console.WriteLine($"Novo Saldo após depósito: {conta.Saldo.ToString("F2", CultureInfo.InvariantCulture)}");
            // Solicitar saque
            Console.Write("\nDigite o valor a ser sacado: ");
            double valorSaque = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
            conta.Sacar(valorSaque);
            Console.WriteLine($"Novo Saldo após saque: {conta.Saldo.ToString("F2", CultureInfo.InvariantCulture)}");
        }
    }
}