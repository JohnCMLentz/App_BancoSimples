using System;
using System.Collections.Generic;
using System.Text;

namespace App_Banco_Simples
{
    internal class ContaBancaria
    {
        public string NomeTitular { get; }
        public int NumeroConta { get; }
        public double Saldo { get; private set; }

        public ContaBancaria(string nomeTitular, int numeroConta, double saldoInicial)
        {
            NomeTitular = nomeTitular;
            NumeroConta = numeroConta;
            Saldo = saldoInicial;
        }

        public void Depositar(double valor)
        {
            if (valor > 0)
            {
                Saldo += valor;
            }
            else
            {
                throw new ArgumentException("O valor do depósito deve ser positivo.");
            }
        }

        public void Sacar(double valor)
        {
            if (valor > 0)
            {
                if (valor <= Saldo)
                {
                    Saldo -= valor;
                }
                else
                {
                    throw new InvalidOperationException("Saldo insuficiente para o saque.");
                }
            }
            else
            {
                throw new ArgumentException("O valor do saque deve ser positivo.");
            }
        }
    }
}
