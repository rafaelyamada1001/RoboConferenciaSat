using Aplication.Interfaces;
using Aplication.Service;
using Aplication.UseCase;
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
                .AddSingleton<SeleniumService>() // Registra a instância única de SeleniumService
                .AddSingleton<PostLoginUseCase>() // Registra o PostLoginUseCase
                .AddSingleton<IApiService, ApiService>() // Registra a interface IApiService e sua implementação ApiService
                .AddSingleton<HttpClient>()
                .AddSingleton<Form1>() // Registra o Form1 para injeção
                .AddSingleton<RodarConferenciaTodasEmpresas>()
                .BuildServiceProvider();

            // Inicializa o formulário e passa o service provider para a injeção
            ApplicationConfiguration.Initialize();
            Application.Run(serviceProvider.GetRequiredService<Form1>());
        }
    }
}