using Dominio;
using Infraestrutura;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var configRepositorio = hostContext.Configuration.GetValue<string>("AppSettings:configRepositorio");

                    services.AddSingleton<IAmigoRepositorio>(provider => DefinirRepositorioInstancia(configRepositorio));
                    services.AddHostedService<Worker>();
                });

        private static IAmigoRepositorio DefinirRepositorioInstancia(string configRepositorio)
        {
            if (configRepositorio == "AmigoRepositorioLinkedList")
                return new AmigoRepositorioLinkedList();
            else if (configRepositorio == "AmigoRepositorioList")
                return new AmigoRepositorioList();
            else
                throw new NotImplementedException("Não existe implementação de repositório para configuração existente.");
        }
    }
}
