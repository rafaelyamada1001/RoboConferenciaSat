﻿using OpenQA.Selenium;
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
            string dataIncial = "01/02/2025";
            string dataFinal = "28/02/2025";
            string mesReferencia = "02";
            string anoReferencia = "25";
            string cnpjEmpresa = "40734904000165";
            string pastaDownload = Path.Combine(@"C:\Downloads\", cnpjEmpresa);
            var options = new ChromeOptions();

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

            //---------------Passar Dados Conferência---------------------------

            System.Threading.Thread.Sleep(3000);
            var escolherEmpresa = driver.FindElement(By.ClassName("vs__search"));
            escolherEmpresa.SendKeys(cnpjEmpresa);
            escolherEmpresa.SendKeys(OpenQA.Selenium.Keys.Enter);

            var botaoConferencia = driver.FindElement(By.XPath("//button[contains(text(), 'Conferência')]"));
            botaoConferencia.Click();

            System.Threading.Thread.Sleep(3000);
            var dataInicialDFe = driver.FindElement(By.Id("txtDataInicialDFe"));
            dataInicialDFe.SendKeys(dataIncial);

            var dataFinalDFe = driver.FindElement(By.Id("txtDataFinalDFe"));
            dataFinalDFe.SendKeys(dataFinal);

            var mesDeReferencia = driver.FindElement(By.Id("txtMesDeReferencia"));
            mesDeReferencia.SendKeys(mesReferencia);

            var anoDeReferencia = driver.FindElement(By.Id("txtAnoDeReferencia"));
            anoDeReferencia.SendKeys(anoReferencia);

            //----------------Tentar realizar conferência-------------------------
            bool sucesso = false;
            int tentativas = 0;

            while (!sucesso && tentativas < 3)
            {
                var botaoConferir = driver.FindElement(By.XPath("//button[contains(text(), 'Conferir')]"));
                botaoConferir.Click();

                System.Threading.Thread.Sleep(10000);
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
        }
    }   
}
