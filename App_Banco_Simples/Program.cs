using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;


namespace App_Banco_Simples
{
    class Program
    {
        static ContaBancaria ContaLogada;
        static string _opcao = null;

        public static string Diretorio = Path.Combine(AppContext.BaseDirectory, "Dados");
        public static string ContasPath = Path.Combine(Diretorio, "Contas.txt");

        static void Main(string[] args)
        {

            // Criando Diretório e arquivo de contas, caso não existam
            Directory.CreateDirectory(Diretorio);
            if (File.Exists(ContasPath))
            {
                Console.WriteLine("Arquivo de contas encontrado!\n");
            }
            else
            {
                Console.WriteLine("Arquivo de contas não encontrado. Criando novo arquivo...");
                File.Create(ContasPath).Dispose();
                Console.WriteLine("Arquivo de contas criado com sucesso!\n");
            }

            PaginaInicial();

        }

        static void PaginaInicial()
        {
            // Logar ou criar conta

            Console.WriteLine("Bem-vindo ao Banco Simples!");
            Console.WriteLine("Você já possui conta? (s/n)");


            Console.ForegroundColor = ConsoleColor.Red;
            _opcao = Console.ReadLine().ToLower();
            Console.ResetColor();

            if (_opcao == "s")
            {
                _opcao = null;

                // Lógica para login na conta existente
                Console.Write("\nDigite o número da conta (somente números): ");

                Console.ForegroundColor = ConsoleColor.Red;
                int _numConta = int.Parse(Console.ReadLine());
                Console.ResetColor();

                Console.Write("Digite sua senha (somente números): ");
                Console.ForegroundColor = ConsoleColor.Red;
                int _senhaConta = int.Parse(Console.ReadLine());
                Console.ResetColor();


                // Lógica para verificar se a conta existe no arquivo de contas
                var contas = File.ReadAllLines(ContasPath)
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select(line => line.Split(';'))
                    .Select(parts => new
                    {
                        NomeTitular = parts[0],
                        NumeroConta = int.Parse(parts[1]),
                        SenhaConta = int.Parse(parts[2]),
                        Saldo = double.Parse(parts[3], CultureInfo.InvariantCulture)
                    });

                var contaEncontrada = contas.FirstOrDefault(c => c.NumeroConta == _numConta && c.SenhaConta == _senhaConta);

                if (contaEncontrada != null)
                {
                    ContaLogada = new ContaBancaria(contaEncontrada.NomeTitular, contaEncontrada.NumeroConta, contaEncontrada.Saldo);
                    Console.Clear();
                    PaginaContaLogada(); // Mudar para a página de conta logada
                    return;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nConta não encontrada ou senha incorreta.\n");
                    Console.ResetColor();
                    PaginaInicial();
                }
            }
            else if (_opcao == "n")
            {
                _opcao = null;

                // Lógica para criação de nova conta
                Console.Write("\nDigite o nome do titular da conta: ");

                Console.ForegroundColor = ConsoleColor.Red;
                string _nomeDigitado = Console.ReadLine().ToLower();

                string _nomeTitular = string.Join(" ",
                    _nomeDigitado.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(palavra => palavra.Length > 0
                        ? char.ToUpper(palavra[0]) + palavra.Substring(1).ToLower()
                        : palavra.ToLower()));

                Console.ResetColor();

                Console.Write("Digite a senha desejada para a nova conta (somente números): ");
                Console.ForegroundColor = ConsoleColor.Red;
                int _senhaConta = int.Parse(Console.ReadLine());
                Console.ResetColor();

                // Gerar número de conta aleatório que não exista no arquivo de contas
                Random random = new Random();
                int _numConta;
                do
                {
                    _numConta = random.Next(1000, 9999);
                } while (File.ReadAllLines(ContasPath)
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Any(line => line.Split(';')[1] == _numConta.ToString()));

                //Deposito inicial
                Console.WriteLine("Deseja realizar um depósito inicial? (s/n)");

                Console.ForegroundColor = ConsoleColor.Red;
                _opcao = Console.ReadLine().ToLower();
                Console.ResetColor();

                double _saldo;
                if (_opcao == "s")
                {
                    _opcao = null;
                    Console.Write("\nDigite o valor do depósito (somente números com ponto como separador decimal): ");

                    Console.ForegroundColor = ConsoleColor.Red;
                    _saldo = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nValor Depositado com sucesso!");
                    Console.ResetColor();
                }
                else
                {
                    _opcao = null;
                    _saldo = 0;
                }

                //Criação da conta no arquivo de contas
                string _novaConta = $"{_nomeTitular};{_numConta};{_senhaConta};{_saldo.ToString(CultureInfo.InvariantCulture)}\n";
                ContaLogada = new ContaBancaria(_nomeTitular,_numConta, _saldo);
                File.AppendAllText(ContasPath, _novaConta);

                //Conta criada com sucesso
                Console.WriteLine("\nConta sendo criada aguarde um instante.");
                Console.WriteLine($"Número da Conta: {_numConta}");

                Thread.Sleep(2000); // Pausa de 2 segundos para o usuário ler a mensagem

                Console.Clear();
                PaginaContaLogada(); // Mudar para a página de conta logada
                return;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nOpção inválida. Tente novamente.\n");
                Console.ResetColor();
            }

        }

        static void PaginaContaLogada()
        {
            // Tela de conta logada

            Console.WriteLine($"Você esta logado!");
            Console.ForegroundColor= ConsoleColor.Green;
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
                    Console.Write("\nDigite o valor do depósito (somente números com ponto como separador decimal): ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    double _deposito = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                    Console.ResetColor();

                    ContaLogada.Depositar(_deposito);
                    break;
                case "3":
                    // Lógica para sacar
                    Console.Write("\nDigite o valor do saque (somente números inteiros): ");
                    Console.ForegroundColor= ConsoleColor.Red;
                    double _saque = double.Parse(Console.ReadLine() , CultureInfo.InvariantCulture);
                    Console.ResetColor();

                    ContaLogada.Sacar(_saque);
                    break;
                case "4":
                    // Lógica para transferir
                    Console.Write("\nDigite o codigo da conta destino para a transferencia (somente números): ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    int _contaDestino = int.Parse(Console.ReadLine());

                    // Lógica para verificar se existe a conta destino

                    Console.Write($"Quanto deseja enviar? ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    double _transferencia = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                    Console.ResetColor();

                    ContaLogada.Transferir(_contaDestino, _transferencia);
                    break;
                case "5":
                    // Lógica para sair da conta
                    Console.WriteLine("\nSaindo da conta...\n");
                    _opcao = null;
                    ContaLogada = null;

                    Thread.Sleep(2000); // Pausa de 2 segundos para o usuário ler a mensagem
                    Console.Clear();

                    PaginaInicial();
                    break;
                default:
                    Console.WriteLine("\nOpção inválida. Tente novamente.");
                    break;
            }

            Console.WriteLine("\nRedirecionando para a página de conta logada...\n");

            Thread.Sleep(2000); // Pausa de 2 segundos para o usuário ler a mensagem

            PaginaContaLogada(); // Volta para a página de conta logada
        }
    }
}