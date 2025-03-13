using Domain.Models;
using OfficeOpenXml;

namespace Aplication.Service
{
    public class ArquivoService
    {
        public void SalvarEmpresaCsv(string cnpjEmpresa, string nomeEmpresa, string status)
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
        public (List<DateTime> DatasFiltradas, string mensagem) AnalisarArquivos(string caminhoArquivo)
        {
            var datasFiltradas = new List<DateTime>();
            string mensagem = "Nenhuma divergência encontrada!"; // Mensagem padrão

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

                // Filtrando as datas de "Chave não encontrada na DF-e"
                datasFiltradas = registros
                    .Where(r => r.Ocorrencia == "Chave não encontrada na DF-e")
                    .Select(r => r.Data)
                    .Distinct()
                    .OrderBy(d => d)
                    .ToList();

                // Verificando se existem chaves canceladas
                var chavesCanceladas = registros
                    .Where(r => r.Situacao == "C") // Situação "C" para canceladas
                    .Select(r => r.Chave)
                    .Distinct()
                    .ToList();

                // Se chaves canceladas forem encontradas, alterar a mensagem
                if (chavesCanceladas.Any())
                {
                    mensagem = "Notas canceladas encontradas!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao analisar o arquivo Excel: " + ex.Message);
            }

            return (datasFiltradas, mensagem);
        }
    }
}
