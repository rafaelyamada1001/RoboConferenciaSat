using Aplication.DTO;
using Aplication.Service;

namespace Aplication.UseCase
{
    public class RealizarConferenciaUseCase
    {
        private readonly SeleniumService _selenium;
        private readonly LoginService _loginService;
        private readonly ConferenciaService _conferenciaService;
        private readonly ArquivoService _arquivoService;

        public RealizarConferenciaUseCase(DadosConferencia dados)
        {           
            _selenium = new SeleniumService(dados);
            _loginService = new LoginService(_selenium);
            _arquivoService = new ArquivoService();
            _conferenciaService = new ConferenciaService(_selenium, _arquivoService);
        }

        public async Task ExecuteAsync(DadosConferencia dados)
        {
            string cnpjEmpresa = dados.Cnpj;
            string mesReferencia = dados.MesReferencia;

            try
            {
                var driver = _loginService.FazerLogin(dados);

                if (driver != null)
                {
                    var conferenciaTask = Task.Run(() => _conferenciaService.IniciarConferencia(dados));

                    await conferenciaTask;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
        }
    }
}
