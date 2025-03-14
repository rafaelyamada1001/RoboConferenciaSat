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
            var arquivoService = new ArquivoService();

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

                bool sucesso = false;
                int tentativas = 0;

                string pastaDownload = Path.Combine(@"C:\Conferencias\", dados.NomeEmpresa, dados.MesReferencia);
                if (!Directory.Exists(pastaDownload))
                {
                    Directory.CreateDirectory(pastaDownload);
                }
                int arquivosAntes = Directory.GetFiles(pastaDownload).Length;

                while (!sucesso && tentativas < 5)
                {
                    _selenium.Clicar(By.XPath("//button[contains(text(), 'Conferir')]"));

                    Thread.Sleep(15000);

                    var erroXml = _selenium.ObterDriver().FindElements(By.XPath("//div[contains(text(), 'Erro interno do servidor ao converter XML!')]")).FirstOrDefault();
                    if (erroXml != null)
                    {
                        _selenium.Clicar(By.XPath("//button[contains(text(), 'OK')]"));
                        tentativas++;
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        Thread.Sleep(15000);

                        var erroChaveSat = _selenium.ObterDriver()
                            .FindElements(By.XPath("//div[@id='swal2-content' and contains(text(), 'Não foi possível realizar a conferência, verifique se a Chave de Segurança do SAT está correta!')]"))
                            .FirstOrDefault();

                        if (erroChaveSat != null)
                        {
                            arquivoService.SalvarEmpresaCsv(dados.Cnpj, dados.NomeEmpresa, "Erro: Chave de Segurança do SAT inválida!");
                            _selenium.Clicar(By.XPath("//button[contains(text(), 'OK')]"));
                            sucesso = false;
                            break;
                        }

                        var mensagemEquipamentoSat = _selenium.ObterDriver()
                            .FindElements(By.XPath("//div[@id='swal2-content' and contains(text(), 'Nenhum Equipamento SAT encontrado para o período informado, verifique!')]"))
                            .FirstOrDefault();

                        if (mensagemEquipamentoSat != null)
                        {
                            arquivoService.SalvarEmpresaCsv(dados.Cnpj, dados.NomeEmpresa, "Nenhum Equipamento SAT encontrado para o período informado!");
                            _selenium.Clicar(By.XPath("//button[contains(text(), 'OK')]"));
                            sucesso = false;
                            break;
                        }

                        int arquivosDepois = Directory.GetFiles(pastaDownload).Length;
                        var mensagem = _selenium.ObterDriver().FindElements(By.XPath("//div[@id='swal2-content' and contains(text(), 'Nenhuma divergência encontrada!')]")).FirstOrDefault();

                        if (mensagem != null)
                        {
                            arquivoService.SalvarEmpresaCsv(dados.Cnpj, dados.NomeEmpresa, "Nenhuma divergência encontrada!");
                            sucesso = true;
                            break;
                        }

                        if (arquivosDepois > arquivosAntes)
                        {
                            var caminhoArquivo = Directory.GetFiles(pastaDownload).FirstOrDefault(f => f.EndsWith(".xlsx"));
                            if (!string.IsNullOrEmpty(caminhoArquivo))
                            {
                                var (datasDivergencia, Statusmensagem) = arquivoService.AnalisarArquivos(caminhoArquivo);
                                string status = datasDivergencia.Any()
                                    ? $"Divergências encontradas nas datas: {string.Join("; ", datasDivergencia.Select(d => d.ToString("dd/MM")))}"
                                    : "Divergências encontradas";

                                if (Statusmensagem != "Nenhuma divergência encontrada!")
                                {
                                    status = $"{status} - {Statusmensagem}";
                                }

                                arquivoService.SalvarEmpresaCsv(dados.Cnpj, dados.NomeEmpresa, status);
                            }
                            else
                            {
                                arquivoService.SalvarEmpresaCsv(dados.Cnpj, dados.NomeEmpresa, "Arquivo de conferência não encontrado");
                            }

                            sucesso = true;
                            break;
                        }
                    }
                }

                if (tentativas >= 5 && !sucesso)
                {
                    arquivoService.SalvarEmpresaCsv(dados.Cnpj, dados.NomeEmpresa, "Erro interno do servidor ao converter XML!");
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
