using Aplication.DTO;
using Aplication.Interfaces;
using Domain.Models;
using System.Text.Json;

namespace Aplication.UseCase
{
    public class PostLoginUseCase
    {
        private readonly IApiService _apiService;

        public PostLoginUseCase(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<Usuario> ExecuteAsync(DadosConferencia dados)
        {
            var requestData = new
            {
                dados.Email,
                dados.Senha
            };

            var json = JsonSerializer.Serialize(requestData);
            var endpoint = "https://back-dfe.4lions.com.br/dfe/v2/public/PostLogin";
            var response = await _apiService.PostDataAsync(endpoint, json);

            if (response == null) return null;

            var usuario = JsonSerializer.Deserialize<Usuario>(response);

            if (usuario == null) return null;

            return usuario;
        }
    }
}
