using Aplication.DTO;
using Domain.Models;

namespace Aplication.UseCase
{
    public class RodarConferenciaTodasEmpresas
    {
        public async Task ExecuteAsync(List<Empresa> empresas, DadosConferencia dados)
        {
            // Processa as empresas em lotes de 5 simultâneas
            var tasks = new List<Task>();

            foreach (var empresa in empresas)
            {
                var dadosEmpresa = new DadosConferencia()
                {
                    DataInicialDfe = dados.DataInicialDfe,
                    DataFinalDfe = dados.DataFinalDfe,
                    DataInicialSefaz = dados.DataInicialSefaz,
                    DataFinalSefaz = dados.DataFinalSefaz,
                    AnoReferencia = dados.AnoReferencia,
                    MesReferencia = dados.MesReferencia,
                    Cnpj = empresa.CnpjCpf,
                    NomeEmpresa = empresa.Nome,
                    Email = dados.Email,
                    Senha = dados.Senha
                };

                // Instancia nova execução da conferência para cada empresa
                var realizarConferencia = new RealizarConferenciaUseCase(dadosEmpresa);
                tasks.Add(realizarConferencia.ExecuteAsync(dadosEmpresa));

                // Limita para 5 processos simultâneos
                if (tasks.Count >= 5)
                {
                    await Task.WhenAny(tasks); // Aguarda pelo menos uma concluir
                    tasks.RemoveAll(t => t.IsCompleted); // Remove as que já terminaram
                }
            }

            // Aguarda todas as conferências finalizarem
            await Task.WhenAll(tasks);
        }
    }
}
