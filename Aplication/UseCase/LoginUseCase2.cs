using Aplication.DTO;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace Aplication.UseCase
{
    public class LoginUseCase2
    {
        public void Execute(DadosConferencia dados)
        {
            string dataInicial = dados.DataInicial;
            string dataFinal = dados.DataFinal;
            string mesReferencia = dados.MesReferencia;
            string anoReferencia = dados.AnoReferencia;
            string cnpjEmpresa = dados.Cnpj;
            string pastaDownload = Path.Combine(@"C:\Conferencias\", cnpjEmpresa, mesReferencia);

            if (!Directory.Exists(pastaDownload))
            {
                Directory.CreateDirectory(pastaDownload);
            }

            var options = new ChromeOptions();
            options.AddUserProfilePreference("download.default_directory", pastaDownload);
            options.AddUserProfilePreference("download.prompt_for_download", false);
            options.AddUserProfilePreference("download.directory_upgrade", true);


            var driver = new ChromeDriver(options);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            try
            {
                driver.Navigate().GoToUrl("https://dfe.4lions.tec.br/login.html");

                //----------------Login---------------------------

                Thread.Sleep(3000);
                var usuario = driver.FindElement(By.Id("email"));
                if (string.IsNullOrEmpty(usuario.GetAttribute("value")))
                {
                    usuario.SendKeys("rafael.yamada@fourlions.com.br");
                }

                var senha = driver.FindElement(By.Id("password"));
                senha.SendKeys("raf1713");

                var botaoLogin = driver.FindElement(By.CssSelector("button[type='submit']"));
                botaoLogin.Click();

                //----------------Ir Pagina Sat-------------------------------
                Thread.Sleep(3000);
                var botaoMovimentacao = driver.FindElement(By.Id("dropdown01"));
                botaoMovimentacao.Click();

                var botaoSatCfe = driver.FindElement(By.XPath("//*[@id=\"navbarsExampleDefault\"]/ul/li[2]/div/a[3]"));
                botaoSatCfe.Click();

                // -------------------Iniciar a Conferência---------------------------
                Task.Run(() => IniciarConferencia(driver, wait, cnpjEmpresa, dataInicial, dataFinal, mesReferencia, anoReferencia));

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
        }

        private void IniciarConferencia(ChromeDriver driver, WebDriverWait wait, string cnpjEmpresa, string dataInicial, string dataFinal, string mesReferencia, string anoReferencia)
        {
            //----------------Passar Dados Conferência---------------------------
            Thread.Sleep(3000);
            var escolherEmpresa = driver.FindElement(By.ClassName("vs__search"));
            escolherEmpresa.SendKeys(cnpjEmpresa);
            escolherEmpresa.SendKeys(OpenQA.Selenium.Keys.Enter);

            var botaoConferencia = driver.FindElement(By.XPath("//button[contains(text(), 'Conferência')]"));
            botaoConferencia.Click();

            Thread.Sleep(3000);
            var dataInicialDFe = driver.FindElement(By.Id("txtDataInicialDFe"));
            dataInicialDFe.SendKeys(dataInicial);

            var dataFinalDFe = driver.FindElement(By.Id("txtDataFinalDFe"));
            dataFinalDFe.SendKeys(dataFinal);

            var mesDeReferencia = driver.FindElement(By.Id("txtMesDeReferencia"));
            mesDeReferencia.SendKeys(mesReferencia);

            var anoDeReferencia = driver.FindElement(By.Id("txtAnoDeReferencia"));
            anoDeReferencia.SendKeys(anoReferencia);

            //----------------Tentar realizar conferência-------------------------
            bool sucesso = false;
            int tentativas = 0;

            string pastaDownload = Path.Combine(@"C:\Conferencias\", cnpjEmpresa, mesReferencia);
            int arquivosAntes = Directory.GetFiles(pastaDownload).Length;

            while (!sucesso && tentativas < 3)
            {
                var botaoConferir = driver.FindElement(By.XPath("//button[contains(text(), 'Conferir')]"));
                botaoConferir.Click();

                Thread.Sleep(15000);
                var modalErro = driver.FindElements(By.XPath("//div[contains(text(), 'Erro interno do servidor ao converter XML!')]")).FirstOrDefault();

                if (modalErro != null)
                {
                    var botaoOk = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(), 'OK')]")));
                    botaoOk.Click();

                    tentativas++;
                    Thread.Sleep(2000);
                }
                else
                {
                    Thread.Sleep(500000);
                    var mensagem = driver.FindElements(By.XPath("//div[@id='swal2-content' and contains(text(), 'Nenhuma divergência encontrada!')]")).FirstOrDefault();
                    if (mensagem != null)
                    {
                        SalvarEmpresaNoCSV(cnpjEmpresa, "Nenhuma divergência encontrada");
                        sucesso = true;
                        break;
                    }

                    int arquivosDepois = Directory.GetFiles(pastaDownload).Length;

                    if (arquivosDepois > arquivosAntes)
                    {
                        SalvarEmpresaNoCSV(cnpjEmpresa, "Divergências encontradas");
                        sucesso = true;
                        break;
                    }

                }
            }
        }

        private void SalvarEmpresaNoCSV(string cnpjEmpresa, string status)
        {
            string filePath = $@"C:\Conferencias\EmpresasConferencia.csv";

            try
            {
                if (!File.Exists(filePath))
                {
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine("Data,CNPJ,Status");
                    }
                }

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"{DateTime.Now:yyyy-MM-dd},{cnpjEmpresa},{status}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao salvar no CSV: " + ex.Message);
            }
        }
    }
}
