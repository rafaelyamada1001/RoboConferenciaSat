namespace Aplication.Service
{
    public class ArquivoCsvService
    {
        private const string FilePath = @"C:\Conferencias\EmpresasConferencia.csv";
        public void SalvarEmpresaCsv(string cnpjEmpresa, string status)
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
