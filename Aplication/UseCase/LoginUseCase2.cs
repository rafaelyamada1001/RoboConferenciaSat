using Aplication.DTO;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using Aplication.Service;
using Domain.Models;
using OfficeOpenXml;

namespace Aplication.UseCase
{
    public class LoginUseCase2
    {
        private readonly SeleniumService _selenium;
        private readonly LoginService _login;

        public LoginUseCase2(SeleniumService selenium)
        {
            _selenium = selenium;
            _login = new LoginService(selenium);
        }

        public void Execute(DadosConferencia dados)
        {
            string dataInicial = dados.DataInicial;
            string dataFinal = dados.DataFinal;
            string mesReferencia = dados.MesReferencia;
            string anoReferencia = dados.AnoReferencia;
            string cnpjEmpresa = dados.Cnpj;
            string pastaDownload = Path.Combine($@"C:\Conferencias\", dados.NomeEmpresa, dados.MesReferencia);

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
                    usuario.SendKeys(dados.Email);
                }

                var senha = driver.FindElement(By.Id("password"));
                senha.SendKeys(dados.Senha);

                var botaoLogin = driver.FindElement(By.CssSelector("button[type='submit']"));
                botaoLogin.Click();

                //----------------Ir Pagina Sat-------------------------------
                Thread.Sleep(3000);
                var botaoMovimentacao = driver.FindElement(By.Id("dropdown01"));
                botaoMovimentacao.Click();

                var botaoSatCfe = driver.FindElement(By.XPath("//*[@id=\"navbarsExampleDefault\"]/ul/li[2]/div/a[3]"));
                botaoSatCfe.Click();
                // -------------------Iniciar a Conferência---------------------------
                Task.Run(() => IniciarConferencia(driver, wait, dados));

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
        }

        private void IniciarConferencia(ChromeDriver driver, WebDriverWait wait, DadosConferencia dados)
        {
            //----------------Passar Dados Conferência---------------------------
            Thread.Sleep(3000);
            var escolherEmpresa = driver.FindElement(By.ClassName("vs__search"));
            escolherEmpresa.SendKeys(dados.Cnpj);
            escolherEmpresa.SendKeys(OpenQA.Selenium.Keys.Enter);

            var botaoConferencia = driver.FindElement(By.XPath("//button[contains(text(), 'Conferência')]"));
            botaoConferencia.Click();

            Thread.Sleep(3000);
            var dataInicialDFe = driver.FindElement(By.Id("txtDataInicialDFe"));
            dataInicialDFe.SendKeys(dados.DataInicial);

            var dataFinalDFe = driver.FindElement(By.Id("txtDataFinalDFe"));
            dataFinalDFe.SendKeys(dados.DataFinal);

            var mesDeReferencia = driver.FindElement(By.Id("txtMesDeReferencia"));
            mesDeReferencia.SendKeys(dados.MesReferencia);

            var anoDeReferencia = driver.FindElement(By.Id("txtAnoDeReferencia"));
            anoDeReferencia.SendKeys(dados.AnoReferencia);

            //----------------Tentar realizar conferência-------------------------
            bool sucesso = false;
            int tentativas = 0;

            
            string pastaDownload = Path.Combine(@"C:\Conferencias\",dados.NomeEmpresa, dados.MesReferencia);
            int arquivosAntes = Directory.GetFiles(pastaDownload).Length;

            Thread.Sleep(1000);
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
                    Thread.Sleep(100000);
                    var mensagem = driver.FindElements(By.XPath("//div[@id='swal2-content' and contains(text(), 'Nenhuma divergência encontrada!')]")).FirstOrDefault();
                    if (mensagem != null)
                    {
                        SalvarEmpresaNoCSV(dados.Cnpj, dados.NomeEmpresa, "Nenhuma divergência encontrada");
                        sucesso = true;
                        break;
                    }

                    int arquivosDepois = Directory.GetFiles(pastaDownload).Length;

                    if (arquivosDepois > arquivosAntes)
                    {
                        // Integrar a análise do arquivo baixado
                        var caminhoArquivo = Directory.GetFiles(pastaDownload).FirstOrDefault(f => f.EndsWith(".xlsx")); // Aqui, escolhemos o primeiro arquivo .xlsx encontrado
                        if (!string.IsNullOrEmpty(caminhoArquivo))
                        {
                            var datasDivergencia = AnalisarArquivos(caminhoArquivo);
                            string status = datasDivergencia.Any() 
                                ? $"Divergências encontradas nas datas: {string.Join(", ", datasDivergencia.Select(d => d.ToString("dd/MM")))}" 
                                : "Divergências encontradas";
                            SalvarEmpresaNoCSV(dados.Cnpj, dados.NomeEmpresa, status);
                        }
                        else
                        {
                            SalvarEmpresaNoCSV(dados.Cnpj, dados.NomeEmpresa, "Arquivo de conferência não encontrado");
                        }

                        sucesso = true;
                        break;
                    }
                }
            }
        }

        private List<DateTime> AnalisarArquivos(string caminhoArquivo)
        {
            var datasFiltradas = new List<DateTime>();
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Necessário para uso gratuito

                var registros = new List<RegistroXlsx>();

                using (var package = new ExcelPackage(new FileInfo(caminhoArquivo)))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var registro = new RegistroXlsx
                        {
                            Chave = worksheet.Cells[row, 1].Text,
                            Valor = decimal.TryParse(worksheet.Cells[row, 2].Text, out var valor) ? valor : 0,
                            Situacao = worksheet.Cells[row, 3].Text,
                            Ocorrencia = worksheet.Cells[row, 4].Text,
                            Data = DateTime.TryParse(worksheet.Cells[row, 5].Text, out var data) ? data : DateTime.MinValue
                        };
                        registros.Add(registro);
                    }
                }

                datasFiltradas = registros
                    .Where(r => r.Ocorrencia == "Chave não encontrada na DF-e")
                    .Select(r => r.Data)
                    .Distinct()
                    .OrderBy(d => d)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao analisar o arquivo Excel: " + ex.Message);
            }

            return datasFiltradas;
        }

        private void SalvarEmpresaNoCSV(string cnpjEmpresa, string nomeEmpresa, string status)
        {
            string filePath = $@"C:\Conferencias\EmpresasConferencia.csv";

            try
            {
                if (!File.Exists(filePath))
                {
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine("CNPJ, Razão Social, Status");
                    }
                }

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"{cnpjEmpresa},{nomeEmpresa},{status}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao salvar no CSV: " + ex.Message);
            }
        }

    }
}