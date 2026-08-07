using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace App_Banco_Simples
{
    internal static class ArquivoContas
    {
        private static string Diretorio = Path.Combine(AppContext.BaseDirectory, "Dados");
        private static string ContasPath = Path.Combine(Diretorio, "Contas.txt");
        private static readonly Random Random = new();

        public static bool CriarDiretorio()
        {
            // Criando Diretório e arquivo de contas, caso não existam
            Directory.CreateDirectory(Diretorio);

            if (File.Exists(ContasPath))
                return true;

            File.Create(ContasPath).Dispose();
            return false;
        }

        public static List<ContaBancaria> Carregar()
        {
            // Puxar lista de contas do diretorio
            List<ContaBancaria> lista = File.ReadAllLines(ContasPath)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Split(';'))
                .Select(parts => new ContaBancaria
                (
                    parts[0],
                    int.Parse(parts[1]),
                    int.Parse(parts[2]),
                    double.Parse(parts[3], CultureInfo.InvariantCulture)
                ))
                .ToList();

            return lista;
        }

        private static void Salvar(List<ContaBancaria> contas)
        {
            // Salvar lista de contas
            List<string> linhas = contas
                .Select(ConverterParaLinha)
                .ToList();

            File.WriteAllLines(ContasPath, linhas);
        }

        public static void Atualizar(ContaBancaria conta)
        {
            // Atualizar uma conta
            List<ContaBancaria> contas = Carregar();

            int indice = contas.FindIndex(c => c.NumeroConta == conta.NumeroConta);

            if (indice == -1)
                throw new InvalidOperationException("Conta não encontrada.");

            contas[indice] = conta;
            Salvar(contas);
        }


        public static ContaBancaria Login(int numero, int senha)
        {
            // Verificar credenciais de login
            ContaBancaria? conta = BuscarConta(numero);

            if (conta == null)
                throw new InvalidOperationException("Conta ou senha incorreta.");
            
            if(conta.SenhaConta != senha)
                throw new InvalidOperationException("Conta ou senha incorreta.");

            return conta;
        }

        // Gera um numero ainda não utilizado entre 1000 e 9999
        public static int GerarNumeroConta()
        {
            // Verificar se já não foi atingido o número maximo de contas
            List<ContaBancaria> contas = Carregar();

            if (contas.Count >= 9000)
                throw new InvalidOperationException("Número máximo de contas alcançado.");

            // Gerar número de conta aleatório que não exista no arquivo de contas
            int numero;
            do
            {
                numero = Random.Next(1000, 10000);
            } while (ExisteConta(numero));

            return numero;
        }

        // Adiciona uma nova conta ao arquivo
        public static void AdicionarConta(ContaBancaria conta)
        {
            File.AppendAllText(ContasPath, ConverterParaLinha(conta) + Environment.NewLine);
        }

        // Expression-bodied members //

        // Buscar uma conta a partir do numero da mesma
        public static ContaBancaria? BuscarConta(int numero) => Carregar().FirstOrDefault(c => c.NumeroConta == numero);

        // Retorna o cliente em forma de linha string para o documento txt
        private static string ConverterParaLinha(ContaBancaria conta) =>
            $"{conta.NomeTitular};{conta.NumeroConta};{conta.SenhaConta};{conta.Saldo.ToString(CultureInfo.InvariantCulture)}";

        // Retorno de bool se existe ou não uma conta a partir do numero
        private static bool ExisteConta(int numero) =>
            BuscarConta(numero) != null;
    }
}
