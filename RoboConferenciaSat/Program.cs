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
            // Configura��o da inje��o de depend�ncia
            var serviceProvider = new ServiceCollection()               
                .AddSingleton<SeleniumService>() // Registra a inst�ncia �nica de SeleniumService
                .AddSingleton<PostLoginUseCase>() // Registra o PostLoginUseCase
                .AddSingleton<IApiService, ApiService>() // Registra a interface IApiService e sua implementa��o ApiService
                .AddSingleton<HttpClient>()
                .AddSingleton<Form1>() // Registra o Form1 para inje��o
                .AddSingleton<RodarConferenciaTodasEmpresas>()
                .BuildServiceProvider();

            // Inicializa o formul�rio e passa o service provider para a inje��o
            ApplicationConfiguration.Initialize();
            Application.Run(serviceProvider.GetRequiredService<Form1>());
        }
    }
}