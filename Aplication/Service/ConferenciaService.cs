using Aplication.DTO;
using OpenQA.Selenium;

namespace Aplication.Service
{
    public class ConferenciaService
    {
        private readonly SeleniumService _selenium;
        private readonly ArquivoService _arquivoService;

        public ConferenciaService(SeleniumService service, ArquivoService arquivoService)
        {
            _selenium = service;
            _arquivoService = arquivoService;
        }

        public void IniciarConferencia(DadosConferencia dados)
        {

            try
            {
                _selenium.EsperarElemento(By.ClassName("vs__search"));
                _selenium.PreencherCampo(By.ClassName("vs__search"), dados.Cnpj, true);

                _selenium.Clicar(By.XPath("//button[contains(text(), 'Conferência')]"));

                _selenium.PreencherCampo(By.Id("txtDataInicialDFe"), dados.DataInicialDfe);
                _selenium.PreencherCampo(By.Id("txtDataFinalDFe"), dados.DataFinalDfe);
                _selenium.PreencherCampo(By.Id("txtDataInicialSefaz"), dados.DataInicialSefaz);
                _selenium.PreencherCampo(By.Id("txtDataFinalSefaz"), dados.DataFinalSefaz);
                _selenium.PreencherCampo(By.Id("txtMesDeReferencia"), dados.MesReferencia);
                _selenium.PreencherCampo(By.Id("txtAnoDeReferencia"), dados.AnoReferencia);
                _selenium.Clicar(By.XPath("//button[contains(text(), 'Conferir')]"));


                string pastaDownload = Path.Combine(@"C:\Conferencias\", dados.NomeEmpresa, dados.MesReferencia);
                if (!Directory.Exists(pastaDownload))
                {
                    Directory.CreateDirectory(pastaDownload);
                }
                int arquivosAntes = Directory.GetFiles(pastaDownload).Length;

                var tempoEsperado = 0;
                while (tempoEsperado < 600000)
                {
                    Thread.Sleep(5000);

                    bool sucesso = false;
                    int tentativas = 0;

                    var erroXml = _selenium.ObterDriver()
                        .FindElements(By.XPath("//div[contains(text(), 'Erro interno do servidor ao converter XML!')]")).FirstOrDefault();
                    if (erroXml != null)
                    {
                        while (!sucesso && tentativas < 5)
                        {
                            _selenium.Clicar(By.XPath("//button[contains(text(), 'OK')]"));
                            _selenium.Clicar(By.XPath("//button[contains(text(), 'Conferir')]"));
                            tentativas++;
                            Thread.Sleep(2000);
                        }
                    }

                    var chaveSatInvalida = _selenium.ObterDriver()
                        .FindElements(By.XPath("//div[contains(text(), 'Chave de Segurança do SAT inválda, verifique!')]")).FirstOrDefault();
                    if (chaveSatInvalida != null)
                    {
                        _arquivoService.SalvarEmpresaCsv(dados.Cnpj, dados.NomeEmpresa, "Chave de Segurança do SAT inválda verifique!");
                        _selenium.Clicar(By.XPath("//button[contains(text(), 'OK')]"));
                        sucesso = false;
                        break;
                    }

                    var erroChaveSat = _selenium.ObterDriver()
                        .FindElements(By.XPath("//div[@id='swal2-content' and contains(text(), 'Não foi possível realizar a conferência, verifique se a Chave de Segurança do SAT está correta!')]"))
                        .FirstOrDefault();

                    if (erroChaveSat != null)
                    {
                        _arquivoService.SalvarEmpresaCsv(dados.Cnpj, dados.NomeEmpresa, "Não foi possível realizar a conferência verifique se a Chave de Segurança do SAT está correta!");
                        _selenium.Clicar(By.XPath("//button[contains(text(), 'OK')]"));
                        sucesso = false;
                        break;
                    }

                    var mensagemEquipamentoSat = _selenium.ObterDriver()
                        .FindElements(By.XPath("//div[@id='swal2-content' and contains(text(), 'Nenhum Equipamento SAT encontrado para o período informado, verifique!')]"))
                        .FirstOrDefault();

                    if (mensagemEquipamentoSat != null)
                    {
                        _arquivoService.SalvarEmpresaCsv(dados.Cnpj, dados.NomeEmpresa, "Nenhum Equipamento SAT encontrado para o período informado!");
                        _selenium.Clicar(By.XPath("//button[contains(text(), 'OK')]"));
                        sucesso = false;
                        break;
                    }

                    int arquivosDepois = Directory.GetFiles(pastaDownload).Length;

                    var mensagem = _selenium.ObterDriver().FindElements(By.XPath("//div[@id='swal2-content' and contains(text(), 'Nenhuma divergência encontrada!')]")).FirstOrDefault();
                    if (mensagem != null)
                    {
                        _arquivoService.SalvarEmpresaCsv(dados.Cnpj, dados.NomeEmpresa, "Nenhuma divergência encontrada!");
                        sucesso = true;
                        break;
                    }

                    if (arquivosDepois > arquivosAntes)
                    {
                        var caminhoArquivo = Directory.GetFiles(pastaDownload).FirstOrDefault(f => f.EndsWith(".xlsx"));
                        if (!string.IsNullOrEmpty(caminhoArquivo))
                        {
                            var (datasDivergencia, statusMensagem) = _arquivoService.AnalisarArquivos(caminhoArquivo);
                            string status = datasDivergencia.Any()
                                ? $"Divergências encontradas nas datas: {string.Join("; ", datasDivergencia.Select(d => d.ToString("dd/MM")))}"
                                : "Notas não encontradas na Sefaz";

                            if (statusMensagem != "Nenhuma divergência encontrada!")
                            {
                                status = $"{status} - {statusMensagem}";
                            }

                            _arquivoService.SalvarEmpresaCsv(dados.Cnpj, dados.NomeEmpresa, status);
                        }
                        else
                        {
                            _arquivoService.SalvarEmpresaCsv(dados.Cnpj, dados.NomeEmpresa, "Arquivo de conferência não encontrado");
                        }

                        sucesso = true;
                        break;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao iniciar conferência: " + ex.Message);
            }
            finally
            {
                _selenium.Dispose();
            }
        }
    }
}