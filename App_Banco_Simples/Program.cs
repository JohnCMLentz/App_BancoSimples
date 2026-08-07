using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;


namespace App_Banco_Simples
{
    class Program
    {
        static ContaBancaria ContaLogada;
        private static string? _opcao;


        static void Main(string[] args)
        {
            try
            {
                bool diretorioCriado = ArquivoContas.CriarDiretorio();

                if (diretorioCriado)
                    Console.WriteLine("Arquivo de contas encontrado!\n");
                else
                    Console.WriteLine("Arquivo de contas criado com sucesso!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            PaginaInicial();
        }

        static void PaginaInicial()
        {
            while (true)
            {// Logar ou criar conta
                Console.WriteLine("Bem-vindo ao Banco Simples!");
                Console.WriteLine("Você já possui conta? (s/n)");

                Console.ForegroundColor = ConsoleColor.Red;
                _opcao = Console.ReadLine().ToLower();
                Console.ResetColor();

                if (_opcao == "s")
                {
                    int numConta;
                    int senhaConta;

                    _opcao = null;

                    // Lógica para login
                    while (true)
                    {
                        Console.Write("\nDigite o número da conta (somente números): ");

                        Console.ForegroundColor = ConsoleColor.Red;
                        string entrada = Console.ReadLine();
                        Console.ResetColor();

                        if (int.TryParse(entrada, out numConta))
                            break;

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nDigite apenas números.\n");
                        Console.ResetColor();
                    }
                    while (true)
                    {
                        Console.Write("\nDigite a senha da conta (somente números): ");

                        Console.ForegroundColor = ConsoleColor.Red;
                        string entrada = Console.ReadLine();
                        Console.ResetColor();

                        if (int.TryParse(entrada, out senhaConta))
                            break;

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nDigite apenas números.\n");
                        Console.ResetColor();
                    }

                    // Lógica para verificar se a conta existe no arquivo de contas
                    while (true)
                    {
                        try
                        {
                            ContaLogada = ArquivoContas.Login(numConta, senhaConta);
                            Console.Clear();
                            PaginaContaLogada(); // Mudar para a página de conta logada
                            break;
                        }
                        catch (InvalidOperationException ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"\n{ex.Message}\n");
                            Console.ResetColor();
                        }
                    }
                }
                else if (_opcao == "n")
                {
                    _opcao = null;

                    // Lógica para criação de nova conta
                    Console.Write("\nDigite o nome do titular da conta: ");

                    Console.ForegroundColor = ConsoleColor.Red;
                    string nomeTitular = Console.ReadLine().ToLower();

                    // Tratamento do nome
                    nomeTitular = string.Join(" ",
                        nomeTitular.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Select(palavra =>
                            char.ToUpper(palavra[0]) + palavra.Substring(1).ToLower()));

                    Console.ResetColor();

                    int senhaConta = 0;
                    while (true)
                    {
                        Console.Write("Digite a senha desejada para a nova conta (somente números): ");

                        Console.ForegroundColor = ConsoleColor.Red;
                        string entrada = Console.ReadLine();
                        Console.ResetColor();

                        if (int.TryParse(entrada, out senhaConta))
                            break;

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nO valor deve ser um número inteiro.\n");
                        Console.ResetColor();
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nSenha criada com sucesso!");
                    Console.ResetColor();

                    int numeroConta = 0;
                    // Gerar número de conta aleatório que não exista no arquivo de contas
                    try
                    {
                        numeroConta = ArquivoContas.GerarNumeroConta();
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\n{ex.Message}\n");
                        Console.ResetColor();

                        return;
                    }

                    //Deposito inicial
                    Console.WriteLine("\nDeseja realizar um depósito inicial? (s/n)");

                    Console.ForegroundColor = ConsoleColor.Red;
                    _opcao = Console.ReadLine().ToLower();
                    Console.ResetColor();

                    double saldo = 0;
                    if (_opcao == "s")
                    {
                        _opcao = null;

                        while (true)
                        {
                            Console.Write("\nDigite o valor do depósito (somente números com ponto como separador decimal): ");

                            Console.ForegroundColor = ConsoleColor.Red;
                            string entrada = Console.ReadLine();
                            Console.ResetColor();

                            entrada = entrada.Replace(',', '.');
                            if (double.TryParse(entrada, NumberStyles.Number, CultureInfo.InvariantCulture, out saldo))
                                break;

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\nO valor deve ser um número inteiro ou com separador de decimal utilizando um ponto.");
                            Console.ResetColor();
                        }

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nValor Depositado com sucesso!");
                        Console.ResetColor();
                    }
                    _opcao = null;

                    //Criação da conta no arquivo de contas
                    ContaLogada = new ContaBancaria(nomeTitular, numeroConta, senhaConta, saldo);

                    ArquivoContas.AdicionarConta(ContaLogada);

                    //Conta criada com sucesso
                    Console.WriteLine("\nConta sendo criada aguarde um instante.");

                    Thread.Sleep(2000); // Pausa de 2 segundos para o usuário ler a mensagem

                    Console.Clear();
                    PaginaContaLogada(); // Mudar para a página de conta logada
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nOpção inválida. Tente novamente.\n");
                    Console.ResetColor();
                    continue;
                }

                if (ContaLogada != null)
                    break;
            }
        }

        static void PaginaContaLogada()
        {
            // Tela de conta logada
            while (true)
            {
                Console.WriteLine("Você esta logado!");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Titular: {ContaLogada.NomeTitular} - Número da Conta: {ContaLogada.NumeroConta}\n");
                Console.ResetColor();

                Console.WriteLine("Escolha uma opção:");
                Console.WriteLine("1 - Consultar saldo");
                Console.WriteLine("2 - Depositar");
                Console.WriteLine("3 - Sacar");
                Console.WriteLine("4 - Transferencia");
                Console.WriteLine("5 - Sair da conta");
                Console.ForegroundColor = ConsoleColor.Red;
                _opcao = Console.ReadLine();
                Console.ResetColor();
                switch (_opcao)
                {
                    case "1":
                        // Lógica para consultar saldo
                        _opcao = null;

                        Console.WriteLine($"\nSeu saldo é: R$ {ContaLogada.Saldo.ToString("N2", CultureInfo.InvariantCulture)}");
                        break;
                    case "2":
                        // Lógica para depositar
                        double deposito;
                        while (true)
                        {
                            Console.Write("\nDigite o valor do depósito (somente números com ponto como separador decimal): ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            string entrada = Console.ReadLine();
                            Console.ResetColor();

                            entrada = entrada.Replace(',', '.');
                            if (double.TryParse(entrada, NumberStyles.Number, CultureInfo.InvariantCulture, out deposito))
                                break;

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\nO valor deve ser um número positivo inteiro ou com separador de decimal utilizando um ponto.");
                            Console.ResetColor();
                        }
                        try
                        {
                            ContaLogada.Depositar(deposito);
                            ArquivoContas.Atualizar(ContaLogada);
                            Console.WriteLine($"\nValor depositado: R$ {deposito.ToString("N2", CultureInfo.InvariantCulture)}");
                            Console.WriteLine($"Saldo atual: R$ {ContaLogada.Saldo.ToString("N2", CultureInfo.InvariantCulture)}");
                        }
                        catch (ArgumentException ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"\n{ex.Message}");
                            Console.ResetColor();
                        }
                        break;
                    case "3":
                        // Lógica para sacar
                        double saque;
                        while (true)
                        {
                            Console.Write("\nDigite o valor do saque (somente números com ponto como separador decimal): ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            string entrada = Console.ReadLine();
                            Console.ResetColor();

                            entrada = entrada.Replace(',', '.');
                            if (double.TryParse(entrada, NumberStyles.Number, CultureInfo.InvariantCulture, out saque))
                                break;

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\nO valor deve ser um número positivo inteiro ou com separador de decimal utilizando um ponto.");
                            Console.ResetColor();
                        }
                        try
                        {
                            ContaLogada.Sacar(saque);
                            ArquivoContas.Atualizar(ContaLogada);
                            Console.WriteLine($"\nValor sacado: R$ {saque.ToString("N2", CultureInfo.InvariantCulture)}");
                            Console.WriteLine($"Saldo atual: R$ {ContaLogada.Saldo.ToString("N2", CultureInfo.InvariantCulture)}");
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"\n{ex.Message}");
                            Console.ResetColor();
                        }
                        break;
                    case "4":
                        // Lógica para transferir
                        int contaDestino;
                        double valorTransferencia;
                        while (true)
                        {
                            Console.Write("\nDigite o codigo da conta destino para a transferencia (somente números): ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            string entrada = Console.ReadLine();
                            Console.ResetColor();

                            if (int.TryParse(entrada, out contaDestino))
                                break;

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\nA conta destino deve possuir somente números.");
                            Console.ResetColor();
                        }
                        while (true)
                        {
                            Console.Write($"Quanto deseja enviar? ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            string entrada = Console.ReadLine();
                            Console.ResetColor();

                            entrada = entrada.Replace(',', '.');
                            if (double.TryParse(entrada, NumberStyles.Number, CultureInfo.InvariantCulture, out valorTransferencia))
                                break;

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\nO valor deve ser um número positivo inteiro ou com separador de decimal utilizando um ponto.\n");
                            Console.ResetColor();
                        }

                        // Lógica para verificar se existe a conta destino
                        ContaBancaria? titularDestino = ArquivoContas.BuscarConta(contaDestino);
                        if (titularDestino == null)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\nConta não encontrada!");
                            Console.ResetColor();
                            break;
                        }
                        Console.WriteLine($"\nConta destino encontrada, titular: {titularDestino.NomeTitular}");
                        try
                        {
                            ContaLogada.Transferir(titularDestino, valorTransferencia);
                            ArquivoContas.Atualizar(ContaLogada);
                            ArquivoContas.Atualizar(titularDestino);
                            Console.WriteLine($"Valor transferido: R$ {valorTransferencia.ToString("N2", CultureInfo.InvariantCulture)}");
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"\n{ex.Message}");
                            Console.ResetColor();
                        }
                        break;
                    case "5":
                        // Lógica para sair da conta
                        Console.WriteLine("\nSaindo da conta...\n");
                        _opcao = null;
                        ContaLogada = null;
                        break;
                    default:
                        Console.WriteLine("\nOpção inválida. Tente novamente.");
                        break;
                }

                if (ContaLogada != null)
                {
                    Console.WriteLine("\nRedirecionando para a página de conta logada...\n");

                    Thread.Sleep(2000); // Pausa de 2 segundos para o usuário ler a mensagem

                    continue; // Reinicia a pagina
                }
                else
                {
                    Console.WriteLine("\nRedirecionando para a página inicial...\n");

                    Thread.Sleep(2000); // Pausa de 2 segundos para o usuário ler a mensagem

                    Console.Clear();

                    PaginaInicial();

                    break; // Volta para a pagina de Login/Cadastro
                }
            }
        }
    }
}