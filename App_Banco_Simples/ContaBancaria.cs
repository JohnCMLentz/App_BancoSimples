using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace App_Banco_Simples
{
    internal class ContaBancaria
    {
        public string NomeTitular { get; }
        public int NumeroConta { get; }
        public int SenhaConta { get; }
        public double Saldo { get; private set; }

        public ContaBancaria(string nomeTitular, int numeroConta, double saldo)
        {
            NomeTitular = nomeTitular;
            NumeroConta = numeroConta;
            Saldo = saldo;
        }

        public ContaBancaria(string nomeTitular, int numeroConta, int senhaConta, double saldo) : this(nomeTitular, numeroConta, saldo)
        {
            SenhaConta = senhaConta;
        }

        public void Depositar(double valor)
        {
            if (valor <= 0)
                throw new ArgumentException("O valor do depósito deve ser positivo.");

            Saldo += valor;
        }

        public void Sacar(double valor)
        {
            if (valor <= 0)
                throw new ArgumentException("O valor do saque deve ser positivo.");
            if (valor > Saldo)
                throw new InvalidOperationException("Saldo insuficiente para o saque.");

            Saldo -= valor;
        }

        public void Transferir(ContaBancaria destino, double valor)
        {
            if (destino.NumeroConta == NumeroConta)
                throw new InvalidOperationException("Impossivel transferir para a mesma conta de origem.");
            if (valor <= 0)
                throw new ArgumentException("O valor da transferencia deve ser positivo.");
            if (valor > Saldo)
                throw new InvalidOperationException("Saldo insuficiente para a transferencia.");

            Saldo -= valor;
            destino.Depositar(valor);
        }
    }
}
