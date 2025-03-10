using EasyAutomationFramework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace RoboConferenciaSat.Aplicacao
{
    public class Login
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public void LoginDfe()
        {

            var options = new ChromeOptions();
            string cnpjEmpresa = "40734904000165";  // O CNPJ da empresa informado
            string pastaDownload = Path.Combine(@"C:\Downloads\", cnpjEmpresa); // Definindo o caminho para a pasta do CNPJ

            // Cria a pasta se ela não existir
            if (!Directory.Exists(pastaDownload))
            {
                Directory.CreateDirectory(pastaDownload);
            }

            if (driver == null)
            {
                options.AddUserProfilePreference("download.default_directory", pastaDownload); // Define a pasta de download
                driver = new ChromeDriver(options);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            }

            driver.Navigate().GoToUrl("https://dfe.4lions.tec.br/login.html");

            //----------------Login---------------------------
            System.Threading.Thread.Sleep(1000);
            var usuario = driver.FindElement(By.Id("email"));
            usuario.SendKeys("rafael.yamada@fourlions.com.br");

            System.Threading.Thread.Sleep(1000);
            var senha = driver.FindElement(By.Id("password"));
            senha.SendKeys("raf1713");

            var botaoLogin = driver.FindElement(By.CssSelector("button[type='submit']"));
            botaoLogin.Click();

            //----------------Ir Pagina Sat-------------------------------
            System.Threading.Thread.Sleep(3000);
            var botaoMovimentacao = driver.FindElement(By.Id("dropdown01"));
            botaoMovimentacao.Click();

            var botaoSatCfe = driver.FindElement(By.XPath("//*[@id=\"navbarsExampleDefault\"]/ul/li[2]/div/a[3]"));
            botaoSatCfe.Click();

            //---------------Realizar Conferência---------------------------

            System.Threading.Thread.Sleep(3000);
            var escolherEmpresa = driver.FindElement(By.ClassName("vs__search"));
            escolherEmpresa.SendKeys("40734904000165");
            escolherEmpresa.SendKeys(OpenQA.Selenium.Keys.Enter);

            var botaoConferencia = driver.FindElement(By.XPath("//button[contains(text(), 'Conferência')]"));
            botaoConferencia.Click();

            System.Threading.Thread.Sleep(3000);
            var dataInicialDFe = driver.FindElement(By.Id("txtDataInicialDFe"));
            dataInicialDFe.SendKeys("01/02/2025");

            var dataFinalDFe = driver.FindElement(By.Id("txtDataFinalDFe"));
            dataFinalDFe.SendKeys("28/02/2025");

            var mesDeReferencia = driver.FindElement(By.Id("txtMesDeReferencia"));
            mesDeReferencia.SendKeys("02");

            var anoDeReferencia = driver.FindElement(By.Id("txtAnoDeReferencia"));
            anoDeReferencia.SendKeys("25");

            bool sucesso = false;
            int tentativas = 0;

            while (!sucesso && tentativas < 3)
            {

                var botaoConferir = driver.FindElement(By.XPath("//button[contains(text(), 'Conferir')]"));
                botaoConferir.Click();

                var modalErro = driver.FindElements(By.XPath("//div[contains(text(), 'Erro interno do servidor ao converter XML!')]")).FirstOrDefault();

                if (modalErro != null)
                {

                    var botaoOk = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(), 'OK')]")));
                    botaoOk.Click();

                    tentativas++;
                    System.Threading.Thread.Sleep(2000); 
                }
                else
                {
                    sucesso = true; 
                }
            }

            string caminhoArquivoEsperado = Path.Combine(pastaDownload, "nome_do_arquivo.pdf"); // Defina o nome esperado do arquivo
            int tentativasDeDownload = 0;

            while (!ArquivoBaixado(caminhoArquivoEsperado) && tentativasDeDownload < 10)
            {
                System.Threading.Thread.Sleep(2000); // Aguardar um pouco para o arquivo ser baixado
                tentativasDeDownload++;
            }

            if (ArquivoBaixado(caminhoArquivoEsperado))
            {
                Console.WriteLine("Arquivo baixado com sucesso.");
                sucesso = true;
            }
            else
            {
                Console.WriteLine("Falha ao baixar o arquivo.");
            }
        }

        // Função para verificar se o arquivo foi baixado
        private bool ArquivoBaixado(string caminhoArquivo)
        {
            return File.Exists(caminhoArquivo);
        }
    }

    
}
