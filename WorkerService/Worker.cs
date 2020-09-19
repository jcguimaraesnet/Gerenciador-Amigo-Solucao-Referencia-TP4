using Dominio;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly IAmigoRepositorio _repositorio;
        private const string pressioneQualquerTecla = "Pressione qualquer tecla para exibir o menu principal ...";

        public Worker(IAmigoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("pt-BR");

            string opcao;
            do
            {
                ExibirMenuPrincipal();

                opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        PesquisarAmigos();
                        break;
                    case "2":
                        AdicionarAmigo();
                        break;
                    case "3":
                        Console.Write("Saindo do programa... ");
                        break;
                    default:
                        Console.Write("Opcao inválida! Escolha uma opção válida. ");
                        break;
                }

                Console.WriteLine(pressioneQualquerTecla);
                Console.ReadKey();
            }
            while (opcao != "3");
        }

        void ExibirMenuPrincipal()
        {
            Console.Clear();
            Console.WriteLine("*** Gerenciador de Amigos *** ");
            Console.WriteLine("1 - Pesquisar Amigos:");
            Console.WriteLine("2 - Adicionar Amigos:");
            Console.WriteLine("3 - Sair:");
            Console.WriteLine("\nEscolha uma das opções acima: ");
        }

        void PesquisarAmigos()
        {
            Console.WriteLine("Informe o nome do amigo que deseja pesquisar:");
            var termoDePesquisa = Console.ReadLine();
            var amigosEncontrados = _repositorio.Pesquisar(termoDePesquisa).ToList();

            if (amigosEncontrados.Count > 0)
            {
                Console.WriteLine("Selecione uma das opções abaixo para visualizar os dados dos amigos encontrados:");
                for (var index = 0; index < amigosEncontrados.Count; index++)
                    Console.WriteLine($"{index} - {amigosEncontrados[index].ObterNomeCompleto()}");

                if (!ushort.TryParse(Console.ReadLine(), out var indexAExibir) || indexAExibir >= amigosEncontrados.Count)
                {
                    Console.Write($"Opção inválida! ");
                    return;
                }

                if (indexAExibir < amigosEncontrados.Count)
                {
                    var amigo = amigosEncontrados[indexAExibir];

                    Console.WriteLine("Dados do amigo:");
                    Console.WriteLine($"Nome: {amigo.ObterNomeCompleto()}");
                    Console.WriteLine($"Data Aniversário: {amigo.Nascimento:dd/MM/yyyy}");

                    var qtdeDiasParaOProximoAniversario = amigo.ObterQtdeDeDiasParaOProximoAniversario();
                    Console.Write(ObterMensagemAniversario(qtdeDiasParaOProximoAniversario));
                }
            }
            else
            {
                Console.Write("Não foi encontrado nenhum amigo! ");
            }
        }

        void AdicionarAmigo()
        {
            Console.WriteLine("Informe o primeiro nome do amigo que deseja adicionar:");
            var primeiroNome = Console.ReadLine();

            Console.WriteLine("Informe o sobrenome do amigo que deseja adicionar:");
            var sobreNome = Console.ReadLine();

            Console.WriteLine("Informe a data de nascimento do amigo (formato dd/MM/yyyy):");
            if (!DateTime.TryParse(Console.ReadLine(), out var dataNascimento))
            {
                Console.Write($"Dado inválido! Dados descartados! ");
                return;
            }

            Console.WriteLine("Informe se o amigo tem parentesco (formato 1-sim ou 2-não):");
            if (!byte.TryParse(Console.ReadLine(), out var parentescoByte) || parentescoByte > 2)
            {
                Console.Write($"Dado inválido! Dados descartados! ");
                return;
            }
            var parentescoEnum = (Parentesco)parentescoByte;

            Console.WriteLine("Dados informados: ");
            Console.WriteLine($"Primeiro Nome: {primeiroNome}");
            Console.WriteLine($"Sobrenome: {sobreNome}");
            Console.WriteLine($"Data nascimento: {dataNascimento:dd/MM/yyyy}");
            Console.WriteLine($"Tem Parentesco: {parentescoEnum}");

            Console.WriteLine("Os dados acima estão corretos?");
            Console.WriteLine("1 - Sim \n2 - Não");

            var opcaoParaAdicionar = Console.ReadLine();

            if (opcaoParaAdicionar == "1")
            {
                var novoAmigo = new Amigo(primeiroNome, sobreNome, dataNascimento, parentescoEnum);
                _repositorio.Adicionar(novoAmigo);
                Console.Write($"Dados adicionados com sucesso! ");
            }
            else if (opcaoParaAdicionar == "2")
            {
                Console.Write($"Dados descartados! ");
            }
            else
            {
                Console.Write($"Opção inválida! ");
            }
        }

        static string ObterMensagemAniversario(double qtdeDias)
        {
            if (double.IsNegative(qtdeDias))
                return $"Este amigo já fez aniversário neste ano! ";
            else if (qtdeDias.Equals(0d))
                return $"Este amigo faz aniversário hoje! ";
            else
                return $"Faltam {qtdeDias:N0} dia(s) para o aniversário deste amigo! ";
        }
    }
}
