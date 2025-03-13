using Aplication.DTO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Aplication.Service
{
    public class SeleniumService
    {
        private readonly ChromeDriver _driver;
        private readonly WebDriverWait _wait;

        public SeleniumService(DadosConferencia dados)
        {
            var options = new ChromeOptions();
            string pastaDownload = Path.Combine(@"C:\Conferencias\", dados.NomeEmpresa, dados.MesReferencia);

            options.AddUserProfilePreference("download.default_directory", pastaDownload);
            options.AddUserProfilePreference("download.prompt_for_download", false);
            options.AddUserProfilePreference("download.directory_upgrade", true);

            _driver = new ChromeDriver(options);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        public void NavegarPara(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }
        public SeleniumService()
        {
            _driver = new ChromeDriver();
        }

        public IWebDriver ObterDriver()
        {
            return _driver;
        }

        public IWebElement EsperarElemento(By by)
        {
            return _wait.Until(ExpectedConditions.ElementIsVisible(by));
        }
        public void PreencherCampo(By by, string valor, bool pressionarEnter = false)
        {
            var elemento = EsperarElemento(by);
            elemento.Clear();
            elemento.SendKeys(valor);

            if (pressionarEnter)
            {
                elemento.SendKeys(Keys.Enter);
            }
        }

        public void Clicar(By by)
        {
            var elemento = EsperarElemento(by);
            elemento.Click();
        }
        public void AbrirNovaAba()
        {
            _driver.ExecuteScript("window.open();");
            _driver.SwitchTo().Window(_driver.WindowHandles.Last()); // Muda para a nova aba
        }
        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
