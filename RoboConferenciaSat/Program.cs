using Aplication.Interfaces;
using Aplication.Service;
using Aplication.UseCase;
using Infra.Repository;
using Infra;
using Microsoft.Extensions.DependencyInjection;

namespace RoboConferenciaSat
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configuração da injeção de dependência
            var serviceProvider = new ServiceCollection()
                .AddSingleton<EmpresasRepository>() // Registra a instância única de EmpresasRepository
                .AddSingleton<SeleniumService>() // Registra a instância única de SeleniumService
                .AddSingleton<LoginUseCase2>() // Registra o LoginUseCase2
                .AddSingleton<PostLoginUseCase>() // Registra o PostLoginUseCase
                .AddSingleton<IApiService, ApiService>() // Registra a interface IApiService e sua implementação ApiService
                .AddSingleton<HttpClient>()
                .AddSingleton<Form1>() // Registra o Form1 para injeção
                .BuildServiceProvider();

            // Inicializa o formulário e passa o service provider para a injeção
            ApplicationConfiguration.Initialize();
            Application.Run(serviceProvider.GetRequiredService<Form1>());
        }
    }
}