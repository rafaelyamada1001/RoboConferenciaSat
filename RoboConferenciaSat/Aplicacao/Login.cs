using EasyAutomationFramework;
using OpenQA.Selenium;

namespace RoboConferenciaSat.Aplicacao
{
    public class Login : Web
    {
        public void LoginDfe()
        {
            if(driver == null) driver = new OpenQA.Selenium.Chrome.ChromeDriver();

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

            var escolherEmpresa = driver.FindElement(By.Name("code"));
            escolherEmpresa.SendKeys("40734904000165");

            //var dataInicialDFe = driver.FindElement(By.Id("txtDataInicialDFe"));
            //dataInicialDFe.SendKeys("01/02/2025");

            //var dataFinalDFe = driver.FindElement(By.Id("txtDataFinalDFe"));
            //dataFinalDFe.SendKeys("28/02/2025");


        }
    }
}
