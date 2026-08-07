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
            if (valor > 0)
            {
                Console.WriteLine($"\nValor depositado: R$ {valor.ToString("N2", CultureInfo.InvariantCulture)}");
                Saldo += valor;
                Console.WriteLine($"Saldo atual: R$ {Saldo.ToString("N2", CultureInfo.InvariantCulture)}");
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
                    Console.WriteLine($"\nValor sacado: R$ {valor.ToString("N2", CultureInfo.InvariantCulture)}");
                    Saldo -= valor;
                    Console.WriteLine($"Saldo atual: R$ {Saldo.ToString("N2", CultureInfo.InvariantCulture)}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nValor de saque indisponivel ou saldo insuficiente na conta!\n");
                    Console.ResetColor();

                    throw new InvalidOperationException("Saldo insuficiente para o saque.");
                }
            }
            else
            {
                throw new ArgumentException("O valor do saque deve ser positivo.");
            }
        }

        public void Transferir(int destino, double valor)
        {
            if (destino == NumeroConta)
            {
                throw new InvalidOperationException("Impossivel transferir para a mesma conta de origem.");
            }
            else
            {
                if (valor > 0)
                {
                    if (valor <= Saldo)
                    {

                        var contas = File.ReadAllLines(Program.ContasPath)
                            .Where(line => !string.IsNullOrWhiteSpace(line))
                            .Select(line => line.Split(';'))
                            .Select(parts => new
                            {
                                NomeTitulo = parts[0],
                                NumeroConta = int.Parse(parts[1]),
                                Saldo = double.Parse(parts[3], CultureInfo.InvariantCulture)
                            });

                        var contaEncontrada = contas.FirstOrDefault(c => c.NumeroConta == destino);

                        if (contaEncontrada != null)
                        {
                            ContaBancaria contaDestino = new ContaBancaria(contaEncontrada.NomeTitulo, contaEncontrada.NumeroConta, contaEncontrada.Saldo);

                            Saldo -= valor;
                            contaDestino.Depositar(valor);
                            Console.WriteLine($"\nValor transferido: R$ {valor.ToString("N2", CultureInfo.InvariantCulture)}");
                            Console.WriteLine($"Para: {contaDestino.NomeTitular}");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\nConta não encontrada!\n");
                            Console.ResetColor();

                            throw new InvalidOperationException("Conta não encontrada!");
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nValor de saque indisponivel ou saldo insuficiente na conta!\n");
                        Console.ResetColor();

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
}
