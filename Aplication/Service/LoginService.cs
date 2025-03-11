using Aplication.DTO;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Service
{
    public class LoginService
    {
        private readonly SeleniumService _selenium;

        public LoginService(SeleniumService selenium)
        {
            _selenium = selenium;
        }

        public IWebDriver FazerLogin(DadosConferencia dados)
        {
            _selenium.NavegarPara("https://dfe.4lions.tec.br/login.html");

            var usuario = _selenium.EsperarElemento(By.Id("email"));
            if (string.IsNullOrEmpty(usuario.GetAttribute("value")))
            {
                usuario.SendKeys(dados.Email);
            }
            _selenium.PreencherCampo(By.Id("password"), dados.Senha);
            _selenium.Clicar(By.CssSelector("button[type = 'submit']"));

            _selenium.Clicar(By.Id("dropdown01"));
            _selenium.Clicar(By.XPath("//*[@id=\"navbarsExampleDefault\"]/ul/li[2]/div/a[3]"));

            return _selenium.ObterDriver();
        }
    }
}
