using Aplication.DTO;
using OpenQA.Selenium;

namespace Aplication.Service
{
    public class ConferenciaService
    {
        private readonly SeleniumService _selenium;
        private readonly ArquivoCsvService _arquivoService;

        public ConferenciaService(SeleniumService service, ArquivoCsvService arquivoService)
        {
            _selenium = service;
            _arquivoService = arquivoService;
        }

        public void IniciarConferencia(DadosConferencia dados)
        {
            _selenium.EsperarElemento(By.ClassName("vs__search"));
            _selenium.PreencherCampo(By.ClassName("vs__search"), dados.Cnpj, true);

            _selenium.Clicar(By.XPath("//button[contains(text(), 'Conferência')]"));


            _selenium.PreencherCampo(By.Id("txtDataInicialDFe"), dados.DataInicial);
            _selenium.PreencherCampo(By.Id("txtDataFinalDFe"), dados.DataFinal);
            _selenium.PreencherCampo(By.Id("txtMesDeReferencia"), dados.MesReferencia);
            _selenium.PreencherCampo(By.Id("txtAnoDeReferencia"), dados.AnoReferencia);

            bool sucesso = false;
            int tentativas = 0;

            string pastaDownload = Path.Combine(@"C:\Conferencias\", dados.Cnpj, dados.MesReferencia);
            int arquivosAntes = Directory.GetFiles(pastaDownload).Length;

            while (!sucesso && tentativas < 3)
            {
                _selenium.Clicar(By.XPath("//button[contains(text(), 'Conferir')]"));
                
                Thread.Sleep(15000);
                var modalErro = _selenium.EsperarElemento(By.XPath("//div[contains(text(), 'Erro interno do servidor ao converter XML!')]"));

                if (modalErro != null)
                {
                    _selenium.Clicar(By.XPath("//button[contains(text(), 'OK')]"));
                    tentativas++;
                    Thread.Sleep(2000);
                }
                else
                {
                    Thread.Sleep(500000);
                    int arquivosDepois = Directory.GetFiles(pastaDownload).Length;

                    if (arquivosDepois > arquivosAntes)
                    {
                        _arquivoService.SalvarEmpresaCsv(dados.Cnpj, "Divergências encontradas");
                        sucesso = true;
                        break;
                    }
                    else
                    {
                        _arquivoService.SalvarEmpresaCsv(dados.Cnpj, "Nenhuma divergência encontrada");
                        sucesso = true;
                        break;
                    }
                }
            }
        }
    }
}
